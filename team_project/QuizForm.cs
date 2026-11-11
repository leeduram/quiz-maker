using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.Json; // System.Text.Json이 사용되지는 않았지만, 코드 상단에 포함되어 있습니다.

namespace team_project
{
    // ListExtensions 클래스
    // IList 인터페이스를 확장하는 정적(static) 클래스로, 리스트를 무작위로 섞는 기능을 제공합니다.
    public static class ListExtensions
    {
        private static readonly Random Rng = new Random(); // 무작위 인덱스 생성을 위한 Random 객체

        // Fisher-Yates 셔플 알고리즘을 사용하여 리스트의 요소를 무작위로 섞습니다.
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rng.Next(n + 1); // 0부터 n까지의 무작위 인덱스 선택
                T value = list[k];       // k번째 요소와 n번째 요소를 교환
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    // QuizForm 클래스
    // 문제 풀이 화면을 담당하는 폼입니다.
    public partial class QuizForm : Form
    {
        private QuestionSetData _questionSet; // 원본 문제집 데이터
        private List<BaseQuestionData> _randomizedQuestions; // 순서가 섞인 실제 풀이용 문제 목록
        private List<BaseUserAnswer> _userAnswers; // 각 문제에 대한 사용자의 답안 목록
        private int _currentIndex = 0; // 현재 풀이 중인 문제의 인덱스 (0부터 시작)
        private Timer _timer; // 시간 측정을 위한 타이머
        private DateTime _startTime; // 문제 풀이 시작 시간

        private SolveListForm _parentSolveListForm; // 부모 폼(문제 목록) 참조

        // 생성자
        public QuizForm(SolveListForm parent, QuestionSetData set)
        {
            InitializeComponent();

            _parentSolveListForm = parent;
            _questionSet = set;

            // 퀴즈의 핵심 로직 초기화 및 시작
            InitializeQuizLogic();

            // UI 이벤트 핸들러 연결
            this.Before.Click += (sender, e) => NavigateQuestion(-1); // 이전 문제로 이동
            this.Next.Click += (sender, e) => NavigateQuestion(1); // 다음 문제로 이동
            this.QuizList.SelectedIndexChanged += ListBox_QuizList_SelectedIndexChanged; // 문제 목록 클릭 시 이동
            this.Stop.Click += Button_Abort_Click; // 중단 버튼
            this.Submit.Click += Submit_Click; // 제출 버튼
        }

        // 퀴즈 풀이의 초기 설정 로직
        private void InitializeQuizLogic()
        {
            this.Problem.Text = _questionSet.title; // 문제집 제목 설정

            // 원본 문제 목록을 복사한 후 셔플합니다. (문제 순서 무작위화)
            _randomizedQuestions = new List<BaseQuestionData>(_questionSet.questions);
            _randomizedQuestions.Shuffle();

            // 사용자 답안 목록을 초기화하고, 문제 수만큼 Answer 객체를 생성합니다.
            _userAnswers = new List<BaseUserAnswer>();
            for (int i = 0; i < _randomizedQuestions.Count; i++)
            {
                var question = _randomizedQuestions[i];
                // 문제 유형에 맞는 BaseUserAnswer의 하위 클래스 인스턴스를 생성
                BaseUserAnswer userAnswer = CreateUserAnswerInstance(question);
                userAnswer.Question = question;
                _userAnswers.Add(userAnswer);

                // 문제 목록 (ListBox)에 초기 텍스트 추가
                this.QuizList.Items.Add($"{i + 1}번");
            }

            StartTimer(); // 시간 측정 시작

            // 첫 번째 문제를 표시합니다.
            if (_randomizedQuestions.Any())
            {
                _currentIndex = 0;
                DisplayCurrentQuestion();
                this.QuizList.SelectedIndex = 0; // 목록에서 첫 번째 문제 선택
            }
        }

        // 문제 유형에 따라 적절한 BaseUserAnswer 객체를 생성하여 반환합니다.
        private BaseUserAnswer CreateUserAnswerInstance(BaseQuestionData question)
        {
            switch (question.Type)
            {
                case QuestionType.Short:
                    return new ShortUserAnswer();
                case QuestionType.Ox:
                    return new OxUserAnswer();
                case QuestionType.Multiple: // 객관식(단일/복수) 모두 MultipleUserAnswer 사용
                case QuestionType.Single:
                    return new MultipleUserAnswer();
                default:
                    throw new InvalidOperationException($"알 수 없는 문제 유형: {question.Type}");
            }
        }

        // 현재 인덱스(_currentIndex)에 해당하는 문제를 화면에 표시합니다.
        private void DisplayCurrentQuestion()
        {
            if (_currentIndex < 0 || _currentIndex >= _randomizedQuestions.Count) return;

            this.Quiz.Controls.Clear(); // 이전 문제 컨트롤 제거

            BaseQuestionData currentQuestion = _randomizedQuestions[_currentIndex];
            BaseUserAnswer currentUserAnswer = _userAnswers[_currentIndex];

            // 문제 유형에 맞는 BaseAnswerControl의 하위 컨트롤을 생성
            BaseAnswerControl newControl = CreateAnswerControl(currentQuestion);

            // 컨트롤에 필요한 데이터(문제 번호, 문제 데이터, 답안 데이터)를 할당
            newControl.QuizNumber = _currentIndex + 1;
            newControl.QuestionData = currentQuestion;
            newControl.CurrentUserAnswer = currentUserAnswer;

            newControl.Dock = DockStyle.Fill;
            newControl.DisplayQuestion(); // 문제 텍스트 및 보기 표시

            newControl.LoadAnswerFromData(); // 저장된 답안을 컨트롤에 로드 (이전에 풀었던 경우)

            // 답안 변경 이벤트를 연결하여, 답안이 변경될 때마다 답안 요약이 업데이트되도록 합니다.
            newControl.AnswerUpdated += AnswerControl_AnswerUpdated;

            this.Quiz.Controls.Add(newControl);

            UpdateNavigationButtons(); // 네비게이션 버튼 상태 업데이트
        }

        // 문제 유형에 따라 적절한 BaseAnswerControl 객체를 생성하여 반환합니다.
        private BaseAnswerControl CreateAnswerControl(BaseQuestionData question)
        {
            switch (question.Type)
            {
                case QuestionType.Short:
                    return new ShortAnswerControl();
                case QuestionType.Ox:
                    return new OxAnswerControl();
                case QuestionType.Multiple:
                case QuestionType.Single:
                    return new MultipleAnswerControl();
                default:
                    throw new InvalidOperationException($"알 수 없는 문제 유형: {question.Type}");
            }
        }

        // 문제 컨트롤에서 답안이 업데이트될 때 호출되는 이벤트 핸들러
        private void AnswerControl_AnswerUpdated(object sender, EventArgs e)
        {
            BaseAnswerControl control = sender as BaseAnswerControl;
            if (control == null) return;

            control.SaveAnswerToData(); // UI의 답안을 AnswerData 객체에 저장

            int index = control.QuizNumber - 1;
            string summary = control.CurrentUserAnswer.GetSummaryText(); // 답안 요약 텍스트 가져오기

            // 문제 목록(QuizList)의 텍스트를 비동기적으로 업데이트하여 사용자가 답안을 했음을 표시
            if (index >= 0 && index < this.QuizList.Items.Count)
            {
                this.BeginInvoke(new Action(() =>
                {
                    this.QuizList.Items[index] = $"{control.QuizNumber}번 {summary}";
                }));
            }
        }

        // 문제 목록(ListBox) 항목을 선택했을 때 호출되는 이벤트 핸들러
        private void ListBox_QuizList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.QuizList.SelectedIndex == -1) return;
            if (this.QuizList.SelectedIndex == _currentIndex) return; // 이미 선택된 문제면 무시

            SaveCurrentAnswer(); // 현재 화면의 답안을 먼저 저장

            _currentIndex = this.QuizList.SelectedIndex; // 새 인덱스로 업데이트

            DisplayCurrentQuestion(); // 새 문제를 화면에 표시
        }


        // 이전/다음 버튼 클릭 또는 코드를 통한 문제 이동
        private void NavigateQuestion(int delta)
        {
            SaveCurrentAnswer(); // 현재 문제의 답안 저장

            _currentIndex += delta; // 인덱스 변경

            // 인덱스 범위 체크
            if (_currentIndex < 0) _currentIndex = 0;
            if (_currentIndex >= _randomizedQuestions.Count) _currentIndex = _randomizedQuestions.Count - 1;

            DisplayCurrentQuestion(); // 문제 표시

            this.QuizList.SelectedIndex = _currentIndex; // 목록에서 선택된 문제 업데이트

            UpdateNavigationButtons(); // 버튼 상태 업데이트
        }

        // 현재 화면에 표시된 컨트롤로부터 답안을 가져와 Answer 객체에 저장합니다.
        private void SaveCurrentAnswer()
        {
            if (this.Quiz.Controls.Count > 0 && this.Quiz.Controls[0] is BaseAnswerControl currentControl)
            {
                currentControl.SaveAnswerToData(); // 답안을 AnswerData 객체에 저장

                int index = currentControl.QuizNumber - 1;
                string summary = currentControl.CurrentUserAnswer.GetSummaryText(); // 답안 요약 가져오기

                // 문제 목록(QuizList) 업데이트
                if (index >= 0 && index < this.QuizList.Items.Count)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        this.QuizList.Items[index] = $"{currentControl.QuizNumber}번 {summary}";
                    }));
                }
            }
        }

        // 이전/다음 버튼의 활성화 상태를 현재 인덱스에 맞게 업데이트합니다.
        private void UpdateNavigationButtons()
        {
            this.Before.Enabled = _currentIndex > 0; // 첫 번째 문제에서는 이전 버튼 비활성화
            this.Next.Enabled = _currentIndex < _randomizedQuestions.Count - 1; // 마지막 문제에서는 다음 버튼 비활성화
        }

        // 시간 측정을 시작합니다.
        private void StartTimer()
        {
            _startTime = DateTime.Now; // 시작 시간 기록

            _timer = new Timer();
            _timer.Interval = 1000; // 1초 간격
            _timer.Tick += Timer_Tick; // 1초마다 Timer_Tick 호출
            _timer.Start();
        }

        // 1초마다 호출되어 경과 시간을 업데이트합니다.
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - _startTime;
            // 경과 시간을 시:분:초 형식으로 표시 (D2는 두 자리로 표시)
            this.Time.Text = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
        }

        // '중단' 버튼 클릭 이벤트 핸들러
        private void Button_Abort_Click(object sender, EventArgs e)
        {
            _timer.Stop(); // 타이머 일시 정지

            DialogResult result = MessageBox.Show(
              "문제 풀이를 중단하고 목록으로 돌아가시겠습니까? 현재 답안은 저장되지 않습니다.",
              "중단 확인",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                this.Close(); // 폼 닫기 (문제 목록으로 돌아감)
            }
            else
            {
                _timer.Start(); // '아니오' 선택 시 타이머 재시작
            }
        }

        // '제출' 버튼 클릭 이벤트 핸들러
        private void Submit_Click(object sender, EventArgs e)
        {
            SaveCurrentAnswer(); // 마지막으로 보고 있던 문제의 답안 저장
            _timer.Stop(); // 타이머 최종 정지

            TimeSpan elapsedTime = DateTime.Now - _startTime; // 최종 경과 시간 계산

            DialogResult result = MessageBox.Show(
              "문제 풀이를 제출하고 결과를 확인하시겠습니까?",
              "제출 확인",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // 1. 모든 문제에 대해 채점을 수행합니다.
                foreach (var answer in _userAnswers)
                {
                    answer.ScoreAnswer(); // BaseUserAnswer의 ScoreAnswer() 메서드 호출
                }

                try
                {
                    // 2. 결과 폼(EndForm)을 열어 결과를 표시합니다.
                    EndForm endForm = new EndForm(_questionSet.title, _randomizedQuestions, _userAnswers, elapsedTime);
                    endForm.ShowDialog();

                    // 3. 결과 폼이 닫히면 현재 퀴즈 폼도 닫습니다.
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"폼을 여는 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                _timer.Start(); // '아니오' 선택 시 타이머 재시작
            }
        }
    }
}
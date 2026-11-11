using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace team_project
{
    // GradingForm 클래스
    // 문제 풀이 결과를 표시하고, 사용자가 개별 문제의 정답, 오답, 해설을 검토할 수 있도록 하는 폼입니다.
    public partial class GradingForm : Form
    {
        // 문제집 제목
        private string _questionSetTitle;
        // 문제집의 전체 문제 데이터 리스트
        private List<BaseQuestionData> _questionDataList;
        // 사용자가 제출한 정답 데이터 리스트 (채점 결과 포함)
        private List<BaseUserAnswer> _userAnswers;
        // 문제 풀이에 소요된 총 시간
        private TimeSpan _elapsedTime;

        // 현재 화면에 표시되고 있는 문제의 인덱스 (0부터 시작)
        private int _currentIndex = -1;

        // 생성자: 채점 결과를 표시하는 데 필요한 모든 데이터를 전달받습니다.
        public GradingForm(string questionSetTitle, List<BaseQuestionData> questions, List<BaseUserAnswer> userAnswers, TimeSpan elapsedTime)
        {
            InitializeComponent(); // 폼의 구성 요소 초기화

            // 전달받은 데이터를 내부 필드에 저장합니다.
            _questionSetTitle = questionSetTitle;
            _questionDataList = questions;
            _userAnswers = userAnswers;
            _elapsedTime = elapsedTime;

            // prev (TextBox/Label) 컨트롤을 읽기 전용으로 설정합니다. (여기에 결과 요약 텍스트를 표시)
            this.prev.ReadOnly = true;
            // Quiz 패널(컨트롤이 추가될 영역)에 스크롤 기능을 활성화합니다.
            this.Quiz.AutoScroll = true;

            // 문제 리스트 (ListBox: QuizList)에 모든 문제 항목을 추가합니다.
            for (int i = 0; i < _questionDataList.Count; i++)
            {
                this.QuizList.Items.Add(FormatQuizListItem(i));
            }

            // 이벤트 핸들러를 연결합니다.
            this.QuizList.SelectedIndexChanged += ListBox_QuizList_SelectedIndexChanged; // 리스트 선택 변경 시
            this.Stop.Click += Stop_Click; // 종료 버튼 클릭 시
            this.Before.Click += Before_Click; // 이전 버튼 클릭 시
            this.Next.Click += Next_Click; // 다음 버튼 클릭 시

            // 상단에 전체 채점 결과를 요약하여 표시합니다.
            DisplayGradingResult();

            // 문제집에 문제가 하나라도 있다면, 첫 번째 문제를 선택하여 화면에 표시합니다.
            if (_questionDataList.Any())
            {
                _currentIndex = 0;
                this.QuizList.SelectedIndex = 0; // ListBox 선택을 트리거하여 DisplayCurrentGradedQuestion() 호출
            }
            else
            {
                // 문제가 없는 경우 탐색 버튼을 비활성화합니다.
                UpdateNavigationButtons();
            }
        }

        // ListBox(QuizList) 항목이 변경될 때 호출되는 이벤트 핸들러
        private void ListBox_QuizList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 선택된 항목이 없으면(초기화 등) 무시합니다.
            if (this.QuizList.SelectedIndex == -1) return;

            // 현재 인덱스를 업데이트합니다.
            _currentIndex = this.QuizList.SelectedIndex;

            // 선택된 문제의 상세 채점 내용을 화면에 표시합니다.
            DisplayCurrentGradedQuestion();

            // 탐색 버튼 (이전/다음)의 활성화 상태를 업데이트합니다.
            UpdateNavigationButtons();
        }

        // 현재 선택된 문제의 상세 채점 내용을 Quiz 패널에 표시하는 메서드
        private void DisplayCurrentGradedQuestion()
        {
            // 이전에 표시된 컨트롤을 모두 제거합니다.
            this.Quiz.Controls.Clear();

            // 유효하지 않은 인덱스인 경우 종료합니다.
            if (_currentIndex < 0 || _currentIndex >= _questionDataList.Count) return;

            // 해당 인덱스의 문제 데이터와 사용자 답변 데이터를 가져옵니다.
            BaseQuestionData question = _questionDataList[_currentIndex];
            BaseUserAnswer userAnswer = _userAnswers[_currentIndex];

            // 문제 유형에 맞는 정답 표시 컨트롤(BaseAnswerControl의 상속 클래스)을 생성합니다.
            BaseAnswerControl answerControl = CreateAnswerControl(question);

            // 컨트롤에 필요한 데이터를 설정합니다.
            answerControl.QuizNumber = _currentIndex + 1; // 문제 번호 설정
            answerControl.QuestionData = question; // 문제 데이터 연결
            answerControl.CurrentUserAnswer = userAnswer; // 사용자 응답 데이터 연결

            // 문제 내용을 화면에 표시합니다.
            answerControl.DisplayQuestion();
            // 사용자의 응답 내용을 로드하여 표시합니다.
            answerControl.LoadAnswerFromData();

            // 채점 화면이므로 사용자가 입력을 변경할 수 없도록 읽기 전용으로 설정합니다.
            answerControl.SetReadOnly(true);
            // 정답을 화면에 표시합니다. (채점 기능을 위해 BaseAnswerControl에 구현되어 있어야 함)
            answerControl.LoadCorrectAnswer();

            // 컨트롤을 Quiz 패널 전체에 채우도록 설정하고 추가합니다.
            answerControl.Dock = DockStyle.Fill;
            this.Quiz.Controls.Add(answerControl);
        }

        // 문제 유형에 따라 적절한 BaseAnswerControl의 하위 클래스 인스턴스를 생성하여 반환합니다.
        private BaseAnswerControl CreateAnswerControl(BaseQuestionData question)
        {
            switch (question.Type)
            {
                case QuestionType.Short:
                    return new ShortAnswerControl(); // 단답형 컨트롤 생성
                case QuestionType.Ox:
                    return new OxAnswerControl(); // OX형 컨트롤 생성
                case QuestionType.Multiple:
                case QuestionType.Single:
                    return new MultipleAnswerControl(); // 객관형 컨트롤 생성 (단일/복수 정답 모두 사용)
                default:
                    // 정의되지 않은 문제 유형에 대한 예외 처리
                    throw new InvalidOperationException($"알 수 없는 문제 유형: {question.Type}");
            }
        }

        // 상단 텍스트 박스에 전체 채점 결과를 요약하여 표시합니다.
        private void DisplayGradingResult()
        {
            int correctCount = 0;
            int totalQuestions = _questionDataList.Count;

            // 정답 개수를 계산합니다.
            foreach (var answer in _userAnswers)
            {
                if (answer.IsCorrect)
                {
                    correctCount++;
                }
            }

            // 소요 시간을 "시:분:초" 형식의 문자열로 변환합니다.
            string timeString = _elapsedTime.ToString(@"hh\:mm\:ss");
            // 총점 문자열을 만듭니다. (예: 5/10)
            string scoreText = $"{correctCount}/{totalQuestions}";

            // 최종 결과 요약 문자열을 구성하여 prev 컨트롤에 표시합니다.
            string resultText = $"문제제목: {_questionSetTitle} | 소요시간: {timeString} | 총점: {scoreText}";
            this.prev.Text = resultText;
        }

        // '종료' 버튼 (Stop) 클릭 이벤트 핸들러
        private void Stop_Click(object sender, EventArgs e)
        {
            this.Close(); // 현재 채점 폼을 닫습니다.
        }

        // '이전' 버튼 (Before) 클릭 이벤트 핸들러
        private void Before_Click(object sender, EventArgs e)
        {
            // 현재 선택된 인덱스에서 1을 뺀 다음 인덱스를 계산합니다.
            int previousIndex = this.QuizList.SelectedIndex - 1;

            // 인덱스가 유효한 범위(0 이상)에 있으면 해당 항목을 선택합니다.
            if (previousIndex >= 0)
            {
                this.QuizList.SelectedIndex = previousIndex;
            }
        }

        // '다음' 버튼 (Next) 클릭 이벤트 핸들러
        private void Next_Click(object sender, EventArgs e)
        {
            // 현재 선택된 인덱스에서 1을 더한 다음 인덱스를 계산합니다.
            int nextIndex = this.QuizList.SelectedIndex + 1;

            // 인덱스가 전체 문제 수 미만이면 해당 항목을 선택합니다.
            if (nextIndex < _questionDataList.Count)
            {
                this.QuizList.SelectedIndex = nextIndex;
            }
        }

        // '이전/다음' 버튼의 활성화 상태를 현재 선택된 인덱스에 맞춰 업데이트합니다.
        private void UpdateNavigationButtons()
        {
            int totalQuestions = _questionDataList.Count;
            int currentSelectedIndex = this.QuizList.SelectedIndex;

            // 문제가 없거나 선택된 항목이 없으면 두 버튼 모두 비활성화합니다.
            if (totalQuestions == 0 || currentSelectedIndex == -1)
            {
                this.Before.Enabled = false;
                this.Next.Enabled = false;
                return;
            }

            // 이전 버튼: 현재 인덱스가 0보다 클 때만 활성화
            this.Before.Enabled = (currentSelectedIndex > 0);
            // 다음 버튼: 현재 인덱스가 마지막 문제 인덱스보다 작을 때만 활성화
            this.Next.Enabled = (currentSelectedIndex < totalQuestions - 1);
        }

        // ListBox에 표시할 문제 항목의 텍스트 포맷을 생성하는 메서드
        private string FormatQuizListItem(int index)
        {
            // 유효성 검사
            if (index < 0 || index >= _questionDataList.Count) return $"{index + 1}번";

            var answer = _userAnswers[index];

            // 채점 결과 (O 또는 X)
            string result = answer.IsCorrect ? "O" : "X";

            // 답변 요약 텍스트를 가져옵니다.
            string answerSummary = GetAnswerSummary(answer);

            // "N번 (사용자답변 요약) [O/X]" 형식으로 반환합니다.
            return $"{index + 1}번 {answerSummary} [{result}]";
        }

        // BaseUserAnswer 객체 타입에 따라 답변 내용을 요약하여 문자열로 반환합니다.
        private string GetAnswerSummary(BaseUserAnswer answer)
        {
            // OX형 답변인 경우
            if (answer is OxUserAnswer oxAnswer)
            {
                return oxAnswer.AnswerIsO.HasValue ?
                       (oxAnswer.AnswerIsO.Value ? "(O)" : "(X)") :
                       "(미선택)";
            }
            // 객관형 답변인 경우 (단일/복수 정답)
            else if (answer is MultipleUserAnswer mcAnswer)
            {
                if (mcAnswer.SelectedOptions != null && mcAnswer.SelectedOptions.Any())
                {
                    // 선택된 인덱스 리스트를 1을 더해 보기 번호로 변환하여 쉼표로 연결합니다.
                    return $"({string.Join(",", mcAnswer.SelectedOptions.OrderBy(i => i).Select(i => (i + 1).ToString()))}번)";
                }
                return "(미선택)";
            }
            // 단답형 답변인 경우
            else if (answer is ShortUserAnswer shortAnswer)
            {
                if (!string.IsNullOrWhiteSpace(shortAnswer.AnswerText))
                {
                    string text = shortAnswer.AnswerText.Trim();
                    // 답변 텍스트가 긴 경우 (10자 초과) 줄여서 표시합니다.
                    return text.Length > 10 ? $"('{text.Substring(0, 10)}...')" : $"('{text}')";
                }
                return "(미입력)";
            }
            // 알 수 없는 답변 유형인 경우
            return "(답변 오류)";
        }
    }
}
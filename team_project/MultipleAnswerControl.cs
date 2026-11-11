using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace team_project
{
    // MultipleAnswerControl 클래스
    // BaseAnswerControl을 상속받아 객관식(단일 선택) 및 복수 정답 문제의 풀이 영역을 구현합니다.
    public partial class MultipleAnswerControl : BaseAnswerControl
    {
        // 데이터 로딩 중인지 여부를 나타내는 플래그.
        // 이 값이 true일 때는 컨트롤의 CheckedChanged 이벤트 발생 시 로직 처리를 건너뛰어 무한 루프나 불필요한 이벤트 호출을 방지합니다.
        private bool _isDataLoading = false;

        // 동적으로 생성된 보기 컨트롤(RadioButton 또는 CheckBox) 리스트
        private List<Control> optionControls = new List<Control>();

        // 채점 결과를 표시할 레이블 컨트롤
        private Label _gradingStatusLabel;

        // 현재 컨트롤이 읽기 전용 상태(채점 모드)인지 여부
        private bool _isReadOnlyState = false;

        // 생성자
        public MultipleAnswerControl()
        {
            InitializeComponent(); // 디자이너 컴포넌트 초기화
            InitializeGradingComponents(); // 채점 상태 표시를 위한 컨트롤 초기화
        }

        // 채점 상태 표시 레이블을 동적으로 생성하고 초기화하는 메서드
        private void InitializeGradingComponents()
        {
            _gradingStatusLabel = new Label();
            _gradingStatusLabel.Name = "GradingStatusLabel";
            _gradingStatusLabel.Text = "";
            // 글꼴 설정 (굵게, 크기 12)
            _gradingStatusLabel.Font = new Font(_gradingStatusLabel.Font.FontFamily, 12F, FontStyle.Bold);
            _gradingStatusLabel.AutoSize = true;

            // 위치 설정 (문제 레이블 왼쪽, 폼 하단에서 30 픽셀 위)
            _gradingStatusLabel.Location = new Point(this.label_Problem.Left, this.Height - 30);
            // 폼 크기 변경 시 위치가 하단 왼쪽에 고정되도록 설정
            _gradingStatusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _gradingStatusLabel.Visible = false; // 기본적으로 숨김 상태
            this.Controls.Add(_gradingStatusLabel);
            // Z-Order를 최상위로 설정 (다른 컨트롤 위에 표시되도록)
            this.Controls.SetChildIndex(_gradingStatusLabel, 0);
        }

        // 보기 컨트롤(RadioButton/CheckBox)의 선택 상태가 변경될 때 호출되는 이벤트 핸들러
        private void Option_CheckedChanged(object sender, EventArgs e)
        {
            // 데이터 로딩 중이면 이벤트 처리를 건너뜁니다.
            if (_isDataLoading) return;

            // 읽기 전용 상태(채점 모드)에서 선택이 변경되면, 변경을 무시하고 원래 사용자 답 및 정답을 다시 로드하여
            // 사용자의 조작을 막고 화면 상태를 유지합니다.
            if (_isReadOnlyState)
            {
                LoadAnswerFromData();
                LoadCorrectAnswer();
                return;
            }

            // 읽기 전용 상태가 아니면 사용자 입력이 변경되었음을 부모 폼에 알립니다.
            OnAnswerUpdated();
        }

        // BaseAnswerControl의 추상 메서드 구현: 문제 내용과 보기를 화면에 표시합니다.
        public override void DisplayQuestion()
        {
            // 문제 번호와 문제 텍스트를 레이블에 표시
            label_Problem.Text = $"{QuizNumber}. {QuestionData.ProblemText}";

            // 문제 텍스트 레이블의 최대 너비를 계산하여 자동 줄 바꿈이 되도록 설정
            int horizontalPadding = this.label_Problem.Left * 2;
            this.label_Problem.MaximumSize = new Size(this.Width - horizontalPadding, 0);

            // 기존의 보기 컨트롤들을 모두 제거하고 리스트도 초기화합니다.
            optionsContainer.Controls.Clear();
            optionControls.Clear();
            this._gradingStatusLabel.Visible = false; // 채점 모드 해제 시 상태 레이블 숨김
            _isReadOnlyState = false; // 읽기 전용 상태 해제

            // QuestionData를 MultipleQuestionData로 형변환하여 보기를 처리합니다.
            if (QuestionData is MultipleQuestionData mcData)
            {
                // 문제 유형이 단일 선택(Single)인지 확인합니다.
                bool isSingleChoice = mcData.IsSingleChoice;

                // 문제 데이터의 보기 수만큼 반복하여 컨트롤을 생성합니다.
                for (int i = 0; i < mcData.Options.Count; i++)
                {
                    Control optionControl;
                    string optionText = mcData.Options[i].Text;

                    // 단일 선택이면 RadioButton을 생성합니다.
                    if (isSingleChoice)
                    {
                        RadioButton rb = new RadioButton();
                        rb.CheckedChanged += Option_CheckedChanged;
                        optionControl = rb;
                    }
                    // 복수 정답이면 CheckBox를 생성합니다.
                    else
                    {
                        CheckBox cb = new CheckBox();
                        cb.CheckedChanged += Option_CheckedChanged;
                        optionControl = cb;
                    }

                    // 보기 텍스트 설정 (예: "1. 보기 내용")
                    optionControl.Text = $"{i + 1}. {optionText}";
                    // Control의 Tag 속성에 해당 보기의 **인덱스(0부터 시작)**를 저장하여 나중에 식별합니다.
                    optionControl.Tag = i;

                    // 기본 텍스트 색상 설정
                    optionControl.ForeColor = SystemColors.ControlText;

                    // 레이아웃 및 스타일 설정
                    optionControl.AutoSize = true;
                    optionControl.Font = new Font("굴림", 14F);
                    optionControl.Margin = new Padding(0, 5, 0, 5);

                    // 보기 텍스트 영역의 최대 너비를 설정하여 줄 바꿈을 유도합니다.
                    int optionMargin = 30;
                    optionControl.MaximumSize = new Size(optionsContainer.Width - optionMargin, 0);

                    // 생성된 컨트롤을 리스트에 추가하고, 화면의 컨테이너(optionsContainer)에 추가합니다.
                    optionControls.Add(optionControl);
                    optionsContainer.Controls.Add(optionControl);
                }
            }
        }

        // BaseAnswerControl의 추상 메서드 구현: 저장된 사용자 응답을 컨트롤에 로드하여 표시합니다.
        public override void LoadAnswerFromData()
        {
            if (CurrentUserAnswer is MultipleUserAnswer mcAnswer)
            {
                _isDataLoading = true; // 데이터 로딩 시작 플래그 설정 (이벤트 방지)

                try
                {
                    // 기존 선택 상태 및 색상을 초기화합니다.
                    foreach (Control control in optionControls)
                    {
                        if (control is RadioButton rb) rb.Checked = false;
                        else if (control is CheckBox cb) cb.Checked = false;
                        control.ForeColor = SystemColors.ControlText;
                    }

                    // 사용자 응답 데이터(SelectedOptions)에 저장된 인덱스에 해당하는 보기를 체크합니다.
                    foreach (int index in mcAnswer.SelectedOptions)
                    {
                        if (index >= 0 && index < optionControls.Count)
                        {
                            Control control = optionControls[index];
                            if (control is RadioButton rb) rb.Checked = true;
                            else if (control is CheckBox cb) cb.Checked = true;
                        }
                    }
                }
                finally
                {
                    _isDataLoading = false; // 데이터 로딩 완료 플래그 해제
                }
            }
        }

        // BaseAnswerControl의 추상 메서드 구현: 컨트롤의 현재 선택 상태를 사용자 응답 객체에 저장합니다.
        public override void SaveAnswerToData()
        {
            if (CurrentUserAnswer is MultipleUserAnswer mcAnswer)
            {
                mcAnswer.SelectedOptions.Clear(); // 기존 선택 리스트를 비웁니다.

                // 모든 보기 컨트롤을 순회하며 선택된 항목을 찾습니다.
                foreach (Control control in optionControls)
                {
                    // 라디오 버튼이나 체크 박스가 선택되었고, Tag(인덱스)가 설정되어 있는지 확인합니다.
                    if (((control is RadioButton rb && rb.Checked) ||
                         (control is CheckBox cb && cb.Checked)) && control.Tag != null)
                    {
                        // 선택된 보기의 인덱스를 사용자 응답 데이터에 추가합니다.
                        mcAnswer.SelectedOptions.Add((int)control.Tag);
                    }
                }
            }
        }

        // BaseAnswerControl의 추상 메서드 구현: 컨트롤의 입력 상태를 읽기 전용으로 설정합니다.
        public override void SetReadOnly(bool readOnly)
        {
            _isReadOnlyState = readOnly;

            // 모든 보기 컨트롤의 Enabled 속성을 readOnly의 반대로 설정합니다.
            // true: 읽기 전용(비활성화), false: 입력 가능(활성화)
            foreach (Control control in optionControls)
            {
                // 현재 코드는 'readOnly' 값과 관계없이 항상 true로 설정하고 있습니다.
                control.Enabled = !readOnly;
            }
        }

        // BaseAnswerControl의 추상 메서드 구현: 정답과 오답을 표시하고 채점 결과를 보여줍니다.
        public override void LoadCorrectAnswer()
        {
            // 문제 데이터와 사용자 응답 데이터가 모두 객관형/복수 정답 유형인지 확인합니다.
            if (QuestionData is MultipleQuestionData question && CurrentUserAnswer is MultipleUserAnswer userAnswer)
            {
                // 1. 문제 데이터에서 정답인 보기들의 **인덱스 리스트**를 추출합니다.
                var correctIndices = question.Options
                    .Select((option, index) => new { option, index })
                    .Where(x => x.option.IsCorrect)
                    .Select(x => x.index)
                    .ToList();

                // 2. 사용자 응답에서 선택된 보기들의 인덱스 리스트를 가져옵니다.
                var userSelectedIndices = userAnswer.SelectedOptions;

                // 3. 정답 보기 번호 문자열을 생성합니다. (예: "1번, 3번")
                string correctAnswerOptionNumbers = string.Join(", ", correctIndices.Select(i => (i + 1).ToString() + "번"));
                string correctAnswerText = $"정답: {correctAnswerOptionNumbers}";

                // 4. 채점 결과 텍스트와 색상을 설정합니다.
                string resultText = userAnswer.IsCorrect ? "O 정답" : "X 오답";
                Color resultColor = userAnswer.IsCorrect ? Color.DarkGreen : Color.Red;

                // 5. 채점 상태 레이블에 결과를 표시하고 보이도록 설정합니다.
                this._gradingStatusLabel.Text = $"채점 결과: {resultText} ({correctAnswerText})";
                this._gradingStatusLabel.ForeColor = resultColor;
                this._gradingStatusLabel.Visible = true;


                // 6. 각 보기 컨트롤에 대해 정답 및 오답 표시를 위한 색상 처리를 수행합니다.
                foreach (Control control in optionControls)
                {
                    if (control.Tag == null) continue;

                    int index = (int)control.Tag;
                    bool isCorrectAnswer = correctIndices.Contains(index); // 이 보기가 정답인가?
                    bool userSelectedThis = userSelectedIndices.Contains(index); // 사용자가 이 보기를 선택했는가?

                    if (isCorrectAnswer && userSelectedThis)
                    {
                        // 정답 보기를 선택함: 진한 녹색 (사용자 정답 표시)
                        control.ForeColor = Color.DarkGreen;
                    }
                    else if (isCorrectAnswer && !userSelectedThis)
                    {
                        // 정답 보기를 선택하지 않음: 연두색 (사용자 오답 시 정답 표시)
                        control.ForeColor = Color.LimeGreen;
                    }
                    else if (!isCorrectAnswer && userSelectedThis)
                    {
                        // 오답 보기를 선택함: 빨간색 (사용자 오답 표시)
                        control.ForeColor = Color.Red;
                    }
                    else
                    {
                        // 오답 보기를 선택하지 않음: 어두운 회색 (선택하지 않은 항목)
                        control.ForeColor = SystemColors.ControlDarkDark;
                    }
                }
            }
        }
    }
}
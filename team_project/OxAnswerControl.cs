using System;
using System.Drawing;
using System.Windows.Forms;

namespace team_project
{
    // OxAnswerControl 클래스
    // BaseAnswerControl을 상속받아 O/X형 문제의 풀이 영역을 구현합니다.
    public partial class OxAnswerControl : BaseAnswerControl
    {
        // 채점 결과를 표시할 레이블 컨트롤
        private Label _gradingStatusLabel;

        // 현재 컨트롤이 읽기 전용 상태(채점 모드)인지 여부
        private bool _isReadOnlyState = false;

        // 생성자
        public OxAnswerControl()
        {
            InitializeComponent(); // 디자이너 컴포넌트 초기화
            InitializeGradingComponents(); // 채점 상태 표시를 위한 컨트롤 초기화

            // RadioButton의 클릭 이벤트를 연결합니다.
            // (RadioButton은 CheckedChanged 이벤트 대신 Click 이벤트를 사용하여 즉각적인 응답을 처리)
            this.radioButton1.Click += RadioButton_Click; // 'O' 버튼
            this.radioButton2.Click += RadioButton_Click; // 'X' 버튼
        }

        // RadioButton 클릭 이벤트 핸들러
        private void RadioButton_Click(object sender, EventArgs e)
        {
            // 읽기 전용 상태(채점 모드)인 경우
            if (_isReadOnlyState)
            {
                // 클릭을 무시하고, 저장된 답과 정답을 다시 로드하여 
                // 사용자가 채점된 상태에서 답을 변경하는 것을 방지합니다.
                LoadAnswerFromData();
                LoadCorrectAnswer();
                return;
            }

            // 읽기 전용 상태가 아니면 사용자 입력이 변경되었음을 부모 폼에 알립니다.
            OnAnswerUpdated();
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

        // BaseAnswerControl의 추상 메서드 구현: 문제 내용을 화면에 표시합니다.
        public override void DisplayQuestion()
        {
            // 문제 번호와 문제 텍스트를 레이블에 표시
            label_Problem.Text = $"{QuizNumber}. {QuestionData.ProblemText}";

            // 문제 텍스트 레이블의 최대 너비를 계산하여 자동 줄 바꿈이 되도록 설정
            int horizontalPadding = this.label_Problem.Left * 2;
            this.label_Problem.MaximumSize = new System.Drawing.Size(this.Width - horizontalPadding, 0);

            // 상태 초기화
            this._gradingStatusLabel.Visible = false; // 채점 상태 숨김
            radioButton1.ForeColor = SystemColors.ControlText; // 텍스트 색상 초기화
            radioButton2.ForeColor = SystemColors.ControlText; // 텍스트 색상 초기화
            _isReadOnlyState = false; // 읽기 전용 상태 해제
        }

        // BaseAnswerControl의 추상 메서드 구현: 저장된 사용자 응답을 컨트롤에 로드하여 표시합니다.
        public override void LoadAnswerFromData()
        {
            if (CurrentUserAnswer is OxUserAnswer oxAnswer)
            {
                // AnswerIsO가 값을 가질 경우 (즉, 사용자가 O 또는 X를 선택했을 경우)
                if (oxAnswer.AnswerIsO.HasValue)
                {
                    // true면 radioButton1(O) 체크, false면 radioButton2(X) 체크
                    this.radioButton1.Checked = oxAnswer.AnswerIsO.Value;
                    this.radioButton2.Checked = !oxAnswer.AnswerIsO.Value;
                }
                // AnswerIsO가 null일 경우 (즉, 사용자가 아무것도 선택하지 않았을 경우)
                else
                {
                    this.radioButton1.Checked = false;
                    this.radioButton2.Checked = false;
                }
            }
        }

        // BaseAnswerControl의 추상 메서드 구현: 컨트롤의 현재 선택 상태를 사용자 응답 객체에 저장합니다.
        public override void SaveAnswerToData()
        {
            if (CurrentUserAnswer is OxUserAnswer oxAnswer)
            {
                if (this.radioButton1.Checked)
                {
                    oxAnswer.AnswerIsO = true; // O 선택
                }
                else if (this.radioButton2.Checked)
                {
                    oxAnswer.AnswerIsO = false; // X 선택
                }
                else
                {
                    oxAnswer.AnswerIsO = null; // 아무것도 선택하지 않음
                }
            }
        }

        // BaseAnswerControl의 추상 메서드 구현: 컨트롤의 입력 상태를 읽기 전용으로 설정합니다.
        public override void SetReadOnly(bool readOnly)
        {
            _isReadOnlyState = readOnly;

            // 'readOnly' 값과 관계없이 항상 Enabled를 true로 설정.
            this.radioButton1.Enabled = true;
            this.radioButton2.Enabled = true;
        }

        // BaseAnswerControl의 추상 메서드 구현: 정답과 오답을 표시하고 채점 결과를 보여줍니다.
        public override void LoadCorrectAnswer()
        {
            // 문제 데이터(OxQuestionData)와 사용자 응답 데이터(OxUserAnswer)가 모두 존재하는지 확인합니다.
            if (QuestionData is OxQuestionData question && CurrentUserAnswer is OxUserAnswer userAnswer)
            {
                // 1. 채점 결과 텍스트와 색상을 설정합니다.
                string resultText = userAnswer.IsCorrect ? "O 정답" : "X 오답";
                Color resultColor = userAnswer.IsCorrect ? Color.DarkGreen : Color.Red;

                // 2. 정답 표시 텍스트를 구성합니다.
                string correctAnswerText = question.CorrectAnswerIsO ? "O" : "X";
                this._gradingStatusLabel.Text = $"채점 결과: {resultText} (정답: {correctAnswerText})";

                // 3. 채점 상태 레이블에 결과를 표시하고 보이도록 설정합니다.
                this._gradingStatusLabel.ForeColor = resultColor;
                this._gradingStatusLabel.Visible = true;

                // 4. RadioButton의 텍스트 색상을 기본값으로 초기화합니다.
                this.radioButton1.ForeColor = SystemColors.ControlText;
                this.radioButton2.ForeColor = SystemColors.ControlText;

                // 5. 정답 RadioButton을 찾아 녹색으로 표시합니다.
                bool correctAnswerIsO = question.CorrectAnswerIsO;
                RadioButton correctButton = correctAnswerIsO ? this.radioButton1 : this.radioButton2;

                correctButton.ForeColor = Color.DarkGreen; // 정답은 진한 녹색으로 표시

                // 6. 사용자가 오답을 선택했고, 선택된 답이 있을 경우 해당 버튼을 빨간색으로 표시합니다.
                if (!userAnswer.IsCorrect && userAnswer.AnswerIsO.HasValue)
                {
                    RadioButton userButton = userAnswer.AnswerIsO.Value ? this.radioButton1 : this.radioButton2;

                    // 사용자가 선택한 버튼이 정답 버튼과 다를 경우 (즉, 오답을 선택한 경우)
                    if (userButton != correctButton)
                    {
                        userButton.ForeColor = Color.Red; // 사용자의 오답 선택은 빨간색으로 표시
                    }
                }
            }
        }
    }
}
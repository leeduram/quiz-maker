using System;
using System.Drawing;
using System.Windows.Forms;
using team_project;

namespace team_project
{
    // ShortAnswerControl 클래스
    // 단답형 문제의 풀이 화면을 담당하는 UserControl입니다.
    // 사용자가 텍스트 박스에 답을 입력하고, 채점 후 결과를 표시하는 로직을 포함합니다.
    public partial class ShortAnswerControl : BaseAnswerControl
    {
        // 채점 결과를 표시하기 위해 동적으로 생성하는 Label 컨트롤
        private Label _correctAnswerLabel;

        // 생성자
        public ShortAnswerControl()
        {
            InitializeComponent();
            InitializeGradingComponents(); // 채점 관련 UI 요소를 초기화

            // 답안 텍스트 박스 내용이 변경될 때마다 OnAnswerUpdated 이벤트를 발생시킵니다.
            // 이 이벤트는 QuizForm에서 답안 요약 업데이트 및 데이터 저장을 위해 사용됩니다.
            this.textBox_Answer.TextChanged += (sender, e) => OnAnswerUpdated();
        }

        // 채점 결과를 표시할 Label 컨트롤을 초기화하고 배치합니다.
        private void InitializeGradingComponents()
        {
            _correctAnswerLabel = new Label();
            _correctAnswerLabel.Name = "CorrectAnswerLabel";
            _correctAnswerLabel.Text = "";
            _correctAnswerLabel.Font = new Font(_correctAnswerLabel.Font.FontFamily, 12F, FontStyle.Bold);
            _correctAnswerLabel.AutoSize = true; // 내용에 따라 크기 자동 조절

            // 위치 설정: 문제 레이블의 왼쪽과 답안 텍스트 박스의 아래쪽에 배치
            _correctAnswerLabel.Location = new Point(this.label_Problem.Left, this.textBox_Answer.Bottom + 10);
            _correctAnswerLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _correctAnswerLabel.Visible = false; // 기본적으로 숨김

            this.Controls.Add(_correctAnswerLabel);
            // 다른 컨트롤 위에 겹치지 않도록 z-index를 조정할 수 있습니다.
            this.Controls.SetChildIndex(_correctAnswerLabel, 0);
        }

        // BaseAnswerControl의 추상 메서드를 오버라이드하여 문제를 화면에 표시합니다.
        public override void DisplayQuestion()
        {
            if (QuestionData == null) return;

            // 문제 번호와 문제 텍스트를 레이블에 설정
            this.label_Problem.Text = $"{QuizNumber}. {QuestionData.ProblemText}";

            // 레이블의 MaximumSize를 설정하여 텍스트가 컨트롤 너비에 맞게 자동으로 줄 바꿈되도록 합니다.
            int horizontalPadding = this.label_Problem.Left * 2;
            this.label_Problem.MaximumSize = new System.Drawing.Size(this.Width - horizontalPadding, 0);

            // 채점 결과를 숨기고 텍스트 박스 배경색을 기본값으로 초기화
            this._correctAnswerLabel.Visible = false;
            this.textBox_Answer.BackColor = SystemColors.Window;
        }

        // BaseUserAnswer 객체에서 저장된 답안을 UI(텍스트 박스)에 로드합니다.
        public override void LoadAnswerFromData()
        {
            var shortAnswer = (ShortUserAnswer)CurrentUserAnswer;
            // null 체크 후 답안 텍스트를 로드
            this.textBox_Answer.Text = shortAnswer.AnswerText ?? string.Empty;
        }

        // UI(텍스트 박스)의 현재 답안을 BaseUserAnswer 객체에 저장합니다.
        public override void SaveAnswerToData()
        {
            var shortAnswer = (ShortUserAnswer)CurrentUserAnswer;
            // 텍스트를 Trim()하여 앞뒤 공백을 제거한 후 저장
            shortAnswer.AnswerText = this.textBox_Answer.Text.Trim();
        }

        // 텍스트 박스의 ReadOnly 상태를 설정하여 답안 수정 가능/불가능 상태를 제어합니다. (채점 후 사용)
        public override void SetReadOnly(bool readOnly)
        {
            this.textBox_Answer.ReadOnly = readOnly;
        }

        // 채점 후 정답과 오답 여부, 그리고 실제 정답을 화면에 표시합니다.
        public override void LoadCorrectAnswer()
        {
            // 데이터 유형 캐스팅 및 null 체크
            if (QuestionData is ShortQuestionData question && CurrentUserAnswer is ShortUserAnswer userAnswer)
            {
                bool isCorrect = userAnswer.IsCorrect;

                string statusIndicator;
                string resultText;
                Color resultColor; // 결과 텍스트 색상
                Color backColor; // 답안 텍스트 박스 배경색

                // 정답/오답에 따라 표시할 내용과 색상을 결정
                if (isCorrect)
                {
                    statusIndicator = "O";
                    resultText = "정답";
                    resultColor = Color.DarkGreen;
                    backColor = Color.LightGreen;
                }
                else
                {
                    statusIndicator = "X";
                    resultText = "오답";
                    resultColor = Color.Red;
                    backColor = Color.LightPink;
                }

                string correctAnswer = question.CorrectAnswerText;

                // 채점 결과 레이블의 텍스트와 색상 설정
                this._correctAnswerLabel.Text = $"채점 결과: {statusIndicator} {resultText} (정답: {correctAnswer})";
                this._correctAnswerLabel.ForeColor = resultColor;

                // 답안 텍스트 박스의 배경색 설정
                this.textBox_Answer.BackColor = backColor;

                // 채점 결과 레이블을 보이도록 설정
                this._correctAnswerLabel.Visible = true;
            }
        }
    }
}
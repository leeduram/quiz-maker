using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace team_project
{
    // PreviewForm 클래스
    // 문제집 목록에서 선택된 문제집의 상세 정보와 첫 번째 문제의 미리보기를 보여주는 폼입니다.
    public partial class PreviewForm : Form
    {
        private BaseQuestionData _firstQuestion; // 문제집의 첫 번째 문제 데이터
        private QuestionSetData _questionSetData; // 문제집 전체 데이터
        private SolveListForm _parentSolveListForm; // 부모 폼(SolveListForm)에 대한 참조 (문제 풀이 시작 후 처리를 위해 필요)

        // 생성자
        public PreviewForm(SolveListForm parent, string title, int totalCount, string types, QuestionSetData setData)
        {
            InitializeComponent();
            // 미리보기가 표시되는 패널에 경계선을 추가합니다.
            this.PreviewPanel.BorderStyle = BorderStyle.FixedSingle;

            this._parentSolveListForm = parent;
            this._questionSetData = setData;

            // 문제집 데이터에서 첫 번째 문제만 미리보기에 사용합니다.
            this._firstQuestion = setData.questions.FirstOrDefault();

            // 문제집 제목을 표시합니다.
            this.Problem.Text = title;

            // 총 문항 수와 유형 정보를 표시합니다.
            this.PreviewLabel.Text = $"총 {totalCount}문항 | 유형 : {types}";

            // 첫 번째 문제의 내용을 동적으로 표시합니다.
            DisplayQuestionContent();
        }


        // 첫 번째 문제의 유형에 맞춰 미리보기를 동적으로 생성하여 표시하는 메서드
        private void DisplayQuestionContent()
        {
            if (_firstQuestion == null) return;

            this.PreviewPanel.Controls.Clear();

            // 문제를 담을 FlowLayoutPanel을 생성하여 PreviewPanel에 추가합니다.
            // FlowLayoutPanel을 사용하여 컨트롤을 수직으로 자동 배치하고 스크롤 기능을 제공합니다.
            FlowLayoutPanel contentContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown, // 수직으로 배치
                WrapContents = false,
                AutoScroll = true, // 내용이 길어지면 스크롤바 생성
                Padding = new Padding(10),
                BorderStyle = BorderStyle.None
            };
            this.PreviewPanel.Controls.Add(contentContainer);

            // 문제 텍스트를 표시할 Label을 생성합니다. (글꼴 크기를 두 배로 하여 미리보기 효과)
            int questionFontSize = 12 * 2;
            Label questionTextDisplay = new Label
            {
                Text = $"1. {_firstQuestion.ProblemText}", // 문제 번호와 텍스트
                AutoSize = true,
                Font = new Font("굴림", questionFontSize),
                BorderStyle = BorderStyle.None,

                // 레이아웃을 위해 너비를 명시적으로 설정하여 자동 줄 바꿈을 유도합니다.
                Width = this.PreviewPanel.ClientSize.Width - 40,
                Margin = new Padding(0, 0, 0, 20)
            };
            contentContainer.Controls.Add(questionTextDisplay);

            // 보기 컨트롤들의 너비 설정
            int controlWidth = this.PreviewPanel.ClientSize.Width - 40;

            // 문제 유형에 따라 보기 영역을 다르게 구성합니다. (C# 7.0 이상의 패턴 매칭 사용)
            switch (_firstQuestion)
            {
                // 1. O/X형 문제 (OxQuestionData)
                case OxQuestionData oxData:
                    int oxFontSize = 14 * 2;
                    // RadioButton의 높이를 계산하여 패널 높이를 결정
                    int radioOxHeight = TextRenderer.MeasureText("O", new Font("굴림", oxFontSize)).Height + 10;

                    // O/X 버튼을 담을 Panel 생성 및 추가
                    Panel oxPanel = new Panel { Height = radioOxHeight + 10, Width = controlWidth, Margin = new Padding(0) };
                    contentContainer.Controls.Add(oxPanel);

                    // O RadioButton 생성 (실제 체크 기능을 비활성화: AutoCheck = false)
                    RadioButton radioO = new RadioButton
                    {
                        Text = "O",
                        Location = new Point(5, 5),
                        AutoSize = true,
                        Font = new Font("굴림", oxFontSize),
                        AutoCheck = false,
                        ForeColor = SystemColors.ControlText
                    };

                    // X RadioButton 생성 (실제 체크 기능을 비활성화: AutoCheck = false)
                    RadioButton radioX = new RadioButton
                    {
                        Text = "X",
                        Location = new Point(95, 5), // O 버튼 옆에 배치
                        AutoSize = true,
                        Font = new Font("굴림", oxFontSize),
                        AutoCheck = false,
                        ForeColor = SystemColors.ControlText
                    };

                    oxPanel.Controls.Add(radioO);
                    oxPanel.Controls.Add(radioX);

                    break;

                // 2. 단답형 문제 (ShortQuestionData)
                case ShortQuestionData shortData:
                    // 답안 입력 상자의 높이 계산
                    int answerBoxHeight = TextRenderer.MeasureText("답안 입력", new Font("굴림", questionFontSize)).Height + 10;

                    // 답안 입력 TextBox 생성 (미리보기이므로 읽기 전용)
                    TextBox answerBox = new TextBox
                    {
                        Text = "답안 입력",
                        Width = controlWidth,
                        Height = answerBoxHeight,
                        ReadOnly = true,
                        BackColor = SystemColors.Window, // 배경색을 흰색으로 유지
                        Font = new Font("굴림", questionFontSize),
                        Margin = new Padding(0, 10, 0, 10)
                    };
                    contentContainer.Controls.Add(answerBox);
                    break;

                // 3. 객관식 문제 (MultipleQuestionData - 단일 또는 복수 정답)
                case MultipleQuestionData multipleData:
                    int optionFontSize = 11 * 2;
                    // 보기 컨트롤 하나의 높이 계산
                    int optionControlHeight = TextRenderer.MeasureText("1. 보기 텍스트", new Font("굴림", optionFontSize)).Height + 10;

                    // 문제 유형에 따라 RadioButton(단일 선택) 또는 CheckBox(복수 정답)의 타입을 결정
                    Type controlType = multipleData.IsSingleChoice ? typeof(RadioButton) : typeof(CheckBox);
                    int optionIndex = 1;

                    // 각 보기에 대해 컨트롤을 동적으로 생성
                    foreach (var option in multipleData.Options)
                    {
                        // Activator.CreateInstance를 사용하여 동적으로 인스턴스 생성
                        Control control = (Control)Activator.CreateInstance(controlType);

                        control.Text = $"{optionIndex++}. {option.Text}";

                        // 크기 및 스타일 설정
                        control.AutoSize = false;
                        control.Width = controlWidth;
                        control.Height = optionControlHeight;

                        control.Font = new Font("굴림", optionFontSize, FontStyle.Regular);
                        control.ForeColor = SystemColors.ControlText;

                        // CheckBox 또는 RadioButton의 체크 기능 비활성화 (미리보기)
                        if (control is CheckBox checkBox)
                        {
                            checkBox.AutoCheck = false;
                        }
                        else if (control is RadioButton radioButton)
                        {
                            radioButton.AutoCheck = false;
                        }

                        control.Margin = new Padding(0, 5, 0, 5); // 보기 간 간격

                        contentContainer.Controls.Add(control);
                    }
                    break;
            }
        }

        // '수정' 버튼 클릭 이벤트 핸들러
        private void EditButton_Click(object sender, EventArgs e)
        {
            // 문제 데이터가 없으면 오류 메시지 표시
            if (_questionSetData == null || _questionSetData.questions == null)
            {
                MessageBox.Show("수정할 문제 데이터가 로드되지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 문제 편집 폼(ProblemForm)을 열어 문제집 데이터를 넘겨줍니다.
            ProblemForm problemForm = new ProblemForm(
                this._questionSetData.questions,
                this._questionSetData.title
            );

            // 모달(Modal)로 폼을 표시하고 결과를 기다립니다.
            DialogResult result = problemForm.ShowDialog();

            // ProblemForm에서 '저장'을 눌렀을 경우
            if (result == DialogResult.OK)
            {
                MessageBox.Show("문제 수정 및 저장 완료. 메인 화면으로 돌아갑니다.", "알림");

                // 부모 폼에게 문제 목록을 새로고침해야 함을 알리기 위해 Yes를 반환
                this.DialogResult = DialogResult.Yes;

                this.Close();
            }
        }

        // '취소' 버튼 클릭 이벤트 핸들러
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close(); // 폼 닫기
        }

        // '풀기' 버튼 클릭 이벤트 핸들러
        private void SolveBtn_Click(object sender, EventArgs e)
        {
            // 데이터 유효성 검사
            if (_questionSetData == null || _parentSolveListForm == null)
            {
                MessageBox.Show("문제집 데이터 또는 부모 폼 참조가 준비되지 않았습니다. PreviewForm이 잘못된 방식으로 호출되었습니다.", "오류");
                return;
            }

            // 문제 풀이 폼(QuizForm)을 생성하고 현재 문제집 데이터를 넘겨줍니다.
            // 부모 폼 참조도 함께 넘겨주어 QuizForm에서 필요한 작업을 수행할 수 있도록 합니다.
            QuizForm quizForm = new QuizForm(this._parentSolveListForm, this._questionSetData);

            // 문제 풀이 폼을 현재 폼을 소유자(Owner)로 하여 모달로 표시합니다.
            DialogResult result = quizForm.ShowDialog(this);

            // QuizForm에서 돌아온 후, PreviewForm은 그대로 유지됩니다.
        }
    }
}
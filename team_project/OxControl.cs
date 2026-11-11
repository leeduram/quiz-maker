using System;
using System.Windows.Forms;

namespace team_project
{
    // OxControl 클래스
    // O/X형 문제의 출제/편집 영역을 담당하는 사용자 정의 컨트롤입니다.
    // BaseQuestionControl을 상속받아 문제 생성 폼(ProblemForm)에서 사용됩니다.
    public partial class OxControl : BaseQuestionControl
    {

        // 생성자
        public OxControl()
        {
            InitializeComponent(); // 디자이너 컴포넌트 초기화
        }

        // IQuestionControl 인터페이스 구현: 문제 번호 텍스트 접근자
        public override string NumText
        {
            get => Num.Text;
            set => Num.Text = value;
        }

        // '삭제' 버튼 (Del) 클릭 이벤트 핸들러
        private void Del_Click(object sender, EventArgs e)
        {
            // 부모 컨트롤에게 이 문제가 삭제되어야 함을 알립니다.
            OnDeleteRequested(); // BaseQuestionControl에서 정의된 이벤트 발생
        }

        // IQuestionControl 인터페이스 구현: 현재 컨트롤의 상태를 OxQuestionData 객체로 추출하여 반환합니다.
        public override BaseQuestionData GetQuestionData()
        {
            var data = new OxQuestionData
            {
                ProblemText = Problem.Text, // 문제 텍스트 필드의 내용 저장

                // RadioO.Checked가 true면 O가 정답, false면 X가 정답
                CorrectAnswerIsO = RadioO.Checked
            };
            return data;
        }

        // IQuestionControl 인터페이스 구현: 전달받은 BaseQuestionData 객체를 컨트롤에 로드합니다.
        public override void SetQuestionData(BaseQuestionData data)
        {
            if (data is OxQuestionData oxData)
            {
                // 문제 텍스트를 로드합니다.
                this.Problem.Text = oxData.ProblemText;

                // 정답이 O인지 여부에 따라 RadioButton 상태를 설정합니다.
                this.RadioO.Checked = oxData.CorrectAnswerIsO;
                // RadioX는 RadioO의 반대 상태를 가집니다.
                this.RadioX.Checked = !oxData.CorrectAnswerIsO;
            }
        }
    }
}
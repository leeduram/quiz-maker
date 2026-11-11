using System;
using System.Windows.Forms;

namespace team_project
{
    // ShortControl 클래스
    // 문제집 편집 화면(ProblemForm)에서 단답형 문제를 생성하고 편집하는 데 사용되는 UserControl입니다.
    // BaseQuestionControl을 상속받아 공통적인 문제 관리 기능을 구현합니다.
    public partial class ShortControl : BaseQuestionControl
    {

        // 생성자
        public ShortControl()
        {
            InitializeComponent(); // 디자이너에서 정의된 UI 요소 초기화
        }

        // IQuestionControl 인터페이스 구현: 문제 번호(NumText)를 관리하는 속성
        public override string NumText
        {
            // Num 컨트롤(Label 또는 TextBox)에서 텍스트를 가져옵니다.
            get => Num.Text;
            // Num 컨트롤에 텍스트(문제 번호)를 설정합니다.
            set => Num.Text = value;
        }

        // '삭제' 버튼 (Del) 클릭 이벤트 핸들러
        private void Del_Click(object sender, EventArgs e)
        {
            // BaseQuestionControl에 정의된 삭제 요청 이벤트를 발생시켜 부모 폼(ProblemForm)이 이 컨트롤을 제거하도록 알립니다.
            OnDeleteRequested();
        }

        // IQuestionControl 인터페이스 구현: UI의 데이터를 수집하여 BaseQuestionData 객체를 생성하고 반환합니다.
        public override BaseQuestionData GetQuestionData()
        {
            // ShortQuestionData 객체를 생성하고 현재 UI의 값을 할당합니다.
            return new ShortQuestionData
            {
                // Problem UI 요소(TextBox)의 텍스트를 문제 텍스트로 저장
                ProblemText = Problem.Text,
                // Answer UI 요소(TextBox)의 텍스트를 정답 텍스트로 저장
                CorrectAnswerText = Answer.Text
            };
        }

        // IQuestionControl 인터페이스 구현: BaseQuestionData 객체의 내용을 UI에 로드합니다.
        public override void SetQuestionData(BaseQuestionData data)
        {
            // 전달받은 데이터를 ShortQuestionData로 캐스팅하여 유효성을 확인합니다.
            if (data is ShortQuestionData shortData)
            {
                // 문제 텍스트를 UI에 표시
                this.Problem.Text = shortData.ProblemText;
                // 정답 텍스트를 UI에 표시
                this.Answer.Text = shortData.CorrectAnswerText;
            }
        }
    }
}
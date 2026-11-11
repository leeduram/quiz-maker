using System;
using System.Windows.Forms;

namespace team_project
{
    // BaseQuestionControl 추상 클래스
    // 문제집을 만들거나 편집하는 화면에서 개별 문제 항목을 나타내는 컨트롤의 기반 클래스입니다.
    // Windows Forms의 UserControl과 IQuestionControl 인터페이스를 구현합니다.
    public abstract partial class BaseQuestionControl : UserControl, IQuestionControl
    {
        // 문제 번호 텍스트(예: "1.", "2." 등)를 관리하는 추상 속성
        // 이 속성을 통해 컨트롤 외부에서 문제 번호를 설정하거나 가져올 수 있습니다.
        public abstract string NumText { get; set; }

        // 현재 컨트롤에 입력된 문제 데이터(문제 내용, 보기, 정답 등)를 
        // BaseQuestionData 객체로 추출하여 반환하는 추상 메서드입니다.
        public abstract BaseQuestionData GetQuestionData();

        // 외부에서 전달된 BaseQuestionData 객체의 내용을 컨트롤에 로드하여 표시하는 추상 메서드입니다.
        // 문제 편집 시 데이터 로딩에 사용됩니다.
        public abstract void SetQuestionData(BaseQuestionData data);

        // 사용자가 현재 문제 항목의 삭제를 요청했을 때 발생하는 이벤트
        // 이벤트를 구독하는 부모 폼(예: 문제 생성 폼)에서 실제 삭제 처리를 수행합니다.
        public event EventHandler DeleteRequested;

        // DeleteRequested 이벤트를 발생시키는 보호된(protected) 헬퍼 메서드
        // 상속받은 구체적인 컨트롤 내에서 삭제 버튼 클릭 시 이 메서드를 호출합니다.
        protected void OnDeleteRequested()
        {
            // 이벤트 핸들러가 등록되어 있는 경우에만 이벤트 발생
            DeleteRequested?.Invoke(this, EventArgs.Empty);
        }

        // 기본 생성자
        public BaseQuestionControl()
        {
            // 컨트롤 초기화 로직 (필요 시 추가)
        }
    }
}
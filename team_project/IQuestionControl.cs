using System;
using System.Windows.Forms;

namespace team_project
{
    // IQuestionControl 인터페이스
    // 문제집 생성 또는 편집 폼에서 사용되는 개별 문제 컨트롤(UserControl)이 반드시 구현해야 하는 기능을 정의합니다.
    // 이 인터페이스를 통해 다양한 문제 유형의 컨트롤을 통일된 방식으로 처리할 수 있습니다.
    public interface IQuestionControl
    {
        // 문제 번호 텍스트를 설정하거나 가져오는 속성입니다.
        // 예: "1.", "2." 등을 표시하고 업데이트하는 데 사용됩니다.
        string NumText { get; set; }

        // 현재 컨트롤에 입력된 **문제 데이터 전체**를 BaseQuestionData 객체 형태로 반환하는 메서드입니다.
        // (문제 내용, 보기, 정답 등)
        BaseQuestionData GetQuestionData();

        // 외부에서 전달된 BaseQuestionData 객체를 컨트롤에 로드하여 표시하는 메서드입니다.
        // 기존 문제를 편집할 때 데이터를 불러오는 데 사용됩니다.
        void SetQuestionData(BaseQuestionData data);

        // 사용자가 이 문제 항목의 삭제를 요청했을 때 발생하는 이벤트입니다.
        // 주로 컨트롤 내의 '삭제' 버튼 클릭 시 부모 폼에 알리는 용도로 사용됩니다.
        event EventHandler DeleteRequested;
    }
}
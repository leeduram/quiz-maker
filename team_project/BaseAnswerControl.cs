using System;
using System.Windows.Forms;

namespace team_project
{
    // IGradingControl 인터페이스
    // 문제 풀이 화면에서 정답 표시 및 입력 제한 기능을 공통으로 제공하기 위한 인터페이스 정의
    public interface IGradingControl
    {
        // 컨트롤의 입력 상태를 읽기 전용으로 설정하거나 해제하는 메서드
        // 문제 풀이가 완료되거나 채점 결과를 볼 때 사용자 입력을 막는 데 사용됨
        void SetReadOnly(bool readOnly);

        // 컨트롤에 문제의 정답을 표시하도록 로드하는 메서드
        // 채점 후 정답을 사용자에게 보여줄 때 사용됨
        void LoadCorrectAnswer();
    }

    // BaseAnswerControl 추상 클래스
    // 개별 문제 풀이 영역(정답 입력 및 표시)을 위한 기반 클래스
    // 모든 구체적인 문제 유형(단답형, 객관식 등)의 정답 컨트롤은 이 클래스를 상속받아야 함
    public abstract partial class BaseAnswerControl : UserControl, IGradingControl
    {
        // 현재 문제의 번호
        public int QuizNumber { get; set; }

        // 이 컨트롤이 다루는 문제 자체의 데이터 (문제 내용, 정답 등)
        public BaseQuestionData QuestionData { get; set; }

        // 사용자가 현재 입력한 정답 데이터 (사용자 입력 값)
        public BaseUserAnswer CurrentUserAnswer { get; set; }

        // 사용자 정답이 업데이트되었을 때 발생하는 이벤트
        // 외부(예: 부모 폼)에서 사용자 입력 변경을 감지하는 데 사용됨
        public event EventHandler AnswerUpdated;

        // AnswerUpdated 이벤트를 발생시키는 헬퍼 메서드
        // 상속받은 클래스에서 사용자 입력 변경 시 이 메서드를 호출하여 알림
        protected void OnAnswerUpdated()
        {
            // 이벤트 핸들러가 등록되어 있는 경우에만 이벤트 발생
            AnswerUpdated?.Invoke(this, EventArgs.Empty);
        }

        // 사용자가 컨트롤에 입력한 내용을 CurrentUserAnswer 객체에 저장하는 추상 메서드
        // 예: 텍스트 박스의 내용을 UserAnswer 객체에 복사
        public abstract void SaveAnswerToData();

        // CurrentUserAnswer 객체의 내용을 컨트롤에 로드하여 표시하는 추상 메서드
        // 예: 저장된 답을 텍스트 박스에 채우거나 라디오 버튼을 선택
        public abstract void LoadAnswerFromData();

        // QuestionData의 내용을 화면에 표시하는 추상 메서드
        // 예: 문제 내용 텍스트, 보기 항목들을 컨트롤에 렌더링
        public abstract void DisplayQuestion();

        // IGradingControl 인터페이스의 구현: 입력 상태를 설정 (읽기 전용/활성화)
        public abstract void SetReadOnly(bool readOnly);

        // IGradingControl 인터페이스의 구현: 컨트롤에 정답을 로드하여 표시
        public abstract void LoadCorrectAnswer();
    }
}
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
    // EndForm 클래스
    // 문제 풀이 세션이 종료된 후 사용자에게 최종 완료를 알리고,
    // 채점 결과 보기 옵션을 제공하는 폼입니다.
    public partial class EndForm : Form
    {
        // 문제집 제목을 저장하는 필드
        private string _questionSetTitle;
        // 풀이에 사용된 전체 문제 데이터 리스트
        private List<BaseQuestionData> _questions;
        // 사용자가 제출한 정답 데이터 리스트
        private List<BaseUserAnswer> _userAnswers;
        // 문제 풀이에 소요된 총 시간
        private TimeSpan _elapsedTime;

        // 생성자: 필요한 모든 풀이 결과 데이터를 전달받아 저장합니다.
        public EndForm(string title, List<BaseQuestionData> questions, List<BaseUserAnswer> answers, TimeSpan elapsedTime)
        {
            InitializeComponent(); // 폼의 구성 요소 초기화 (디자이너 생성 코드)

            // 전달받은 데이터를 내부 필드에 저장합니다.
            _questionSetTitle = title;
            _questions = questions;
            _userAnswers = answers;
            _elapsedTime = elapsedTime;

            // 참고: 보통 이 단계에서 풀이 요약 정보(점수, 시간 등)를 화면에 표시하는 로직이 추가됩니다.
        }

        // '종료' 버튼 (EndBtn) 클릭 이벤트 핸들러
        // 애플리케이션의 메인 화면 등으로 돌아가기 위해 현재 폼을 닫습니다.
        private void EndBtn_Click(object sender, EventArgs e)
        {
            this.Close(); // 현재 폼 닫기
        }

        // '결과 보기' 버튼 (ResultBtn) 클릭 이벤트 핸들러
        // 채점 결과 및 상세 풀이를 볼 수 있는 GradingForm을 엽니다.
        private void ResultBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // 채점 폼(GradingForm)의 새 인스턴스를 생성하고, 모든 풀이 데이터를 전달합니다.
                GradingForm gradingForm = new GradingForm(_questionSetTitle, _questions, _userAnswers, _elapsedTime);

                // 모달 다이얼로그로 폼을 표시합니다. (이 폼이 닫힐 때까지 EndForm은 대기)
                gradingForm.ShowDialog();

                // 채점 폼이 닫히면, EndForm도 닫고 다음 단계로 넘어갑니다 (보통 메인 폼으로 복귀).
                this.Close();
            }
            catch (Exception ex)
            {
                // 폼 실행 중 예외 발생 시 오류 메시지를 사용자에게 표시합니다.
                MessageBox.Show($"채점 폼을 여는 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
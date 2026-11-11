using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace team_project
{
    // ProblemForm 클래스
    // 문제집을 생성하거나 기존 문제를 편집하는 메인 폼입니다.
    public partial class ProblemForm : Form
    {
        // 현재 폼에서 편집 중인 문제 데이터 리스트 (기존 문제 로드 또는 새 문제 생성 시 사용)
        private List<BaseQuestionData> _editingQuestions;

        // 문제집의 초기 제목 (수정 취소 시 되돌리거나, 저장 시 제목 변경 여부 확인 등에 사용 가능)
        private string _initialTitle;

        // UI 레이아웃에 사용되는 상수
        private const int INITIAL_TOP = 13; // 첫 번째 컨트롤의 상단 위치
        private const int INITIAL_LEFT = 18; // 컨트롤의 왼쪽 위치
        private const int SPACING = 10; // 컨트롤 간의 수직 간격

        // 기본 생성자 (새 문제집 생성 시 호출)
        public ProblemForm()
        {
            InitializeComponent();
            this.Type.SelectedIndex = 0; // 문제 유형 선택 ComboBox의 기본값을 첫 번째 항목으로 설정
            this._editingQuestions = new List<BaseQuestionData>();
            this._initialTitle = string.Empty;
        }

        // 오버로드된 생성자 (기존 문제집 수정 시 호출)
        public ProblemForm(List<BaseQuestionData> existingQuestions, string title) : this()
        {
            this._editingQuestions = existingQuestions;
            this._initialTitle = title;

            this.textBox1.Text = title; // 문제집 제목 설정

            LoadExistingQuestionsToUI(); // 기존 문제를 UI에 로드
        }

        // 기존 문제 데이터를 읽어와서 해당 문제 유형에 맞는 UserControl을 동적으로 생성하고 배치합니다.
        private void LoadExistingQuestionsToUI()
        {
            if (_editingQuestions == null || _editingQuestions.Count == 0) return;

            Panel targetPanel = this.panel1;
            targetPanel.Controls.Clear(); // 패널 초기화

            foreach (var data in _editingQuestions)
            {
                // 데이터 로드 시, 문제 텍스트에 붙어있던 " (복수정답)" 접미사를 제거합니다.
                if (data is MultipleQuestionData)
                {
                    const string suffix = " (복수정답)";
                    if (data.ProblemText.EndsWith(suffix))
                    {
                        data.ProblemText = data.ProblemText.Substring(0, data.ProblemText.Length - suffix.Length).TrimEnd();
                    }
                }

                UserControl newControl = null;

                // 문제 데이터 유형에 따라 적절한 Question Control을 생성합니다.
                if (data is ShortQuestionData)
                {
                    newControl = new ShortControl();
                }
                else if (data is MultipleQuestionData mcData)
                {
                    MultipleControl multipleControl = new MultipleControl();
                    // MultipleControl은 높이가 가변적이므로 HeightChanged 이벤트를 연결합니다.
                    multipleControl.HeightChanged += UserControl_OnHeightChanged;
                    newControl = multipleControl;
                }
                else if (data is OxQuestionData)
                {
                    newControl = new OxControl();
                }

                if (newControl == null) continue;

                // 새 컨트롤이 배치될 Y 좌표를 계산합니다.
                int nextY = INITIAL_TOP;
                if (targetPanel.Controls.Count > 0)
                {
                    Control lastControl = targetPanel.Controls[targetPanel.Controls.Count - 1];
                    nextY = lastControl.Bottom + SPACING;
                }
                newControl.Location = new Point(INITIAL_LEFT, nextY);

                // IQuestionControl 인터페이스를 통해 데이터 로드 및 이벤트 핸들러를 연결합니다.
                if (newControl is IQuestionControl questionControl)
                {
                    questionControl.SetQuestionData(data); // 데이터 로드
                    questionControl.NumText = $"{targetPanel.Controls.Count + 1}."; // 문제 번호 설정 (추가 후 번호)

                    // 문제 삭제 요청 이벤트 핸들러 연결
                    questionControl.DeleteRequested += UserControl_OnDeleteRequested;
                }

                targetPanel.Controls.Add(newControl);
            }

            // 모든 컨트롤의 위치와 번호를 재배치합니다.
            RelayoutControls(targetPanel);
        }

        // '취소' 버튼 클릭 이벤트 핸들러
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close(); // 폼 닫기
        }

        // '문제 추가' 버튼 (Create) 클릭 이벤트 핸들러
        private void Create_Click(object sender, EventArgs e)
        {
            Panel targetPanel = this.panel1;
            UserControl newControl = null;

            if (this.Type.SelectedItem == null) return;
            string selectedType = this.Type.SelectedItem.ToString();

            // 선택된 유형에 따라 새로운 Question Control을 생성합니다.
            switch (selectedType)
            {
                case "단답형":
                    newControl = new ShortControl();
                    break;
                case "객관형":
                    newControl = new MultipleControl();
                    // HeightChanged 이벤트 연결 (객관식은 보기 추가/삭제 시 높이가 변함)
                    ((MultipleControl)newControl).HeightChanged += UserControl_OnHeightChanged;
                    break;
                case "O / X":
                    newControl = new OxControl();
                    break;
                default:
                    return;
            }

            // 새 컨트롤이 배치될 Y 좌표를 계산합니다.
            int nextY = INITIAL_TOP;
            if (targetPanel.Controls.Count > 0)
            {
                Control lastControl = targetPanel.Controls[targetPanel.Controls.Count - 1];
                nextY = lastControl.Bottom + SPACING;
            }

            newControl.Location = new Point(INITIAL_LEFT, nextY);

            // 문제 번호를 설정합니다.
            if (newControl is IQuestionControl questionControl)
            {
                int questionNumber = targetPanel.Controls.Count + 1;
                questionControl.NumText = $"{questionNumber}.";
            }

            targetPanel.Controls.Add(newControl);

            // 문제 삭제 요청 이벤트 핸들러 연결
            if (newControl is MultipleControl mc)
            {
                mc.DeleteRequested += UserControl_OnDeleteRequested;
            }
            else if (newControl is ShortControl sc)
            {
                sc.DeleteRequested += UserControl_OnDeleteRequested;
            }
            else if (newControl is OxControl oc)
            {
                oc.DeleteRequested += UserControl_OnDeleteRequested;
            }

            // 새로 추가된 컨트롤로 스크롤 이동
            targetPanel.ScrollControlIntoView(newControl);
        }


        // 자식 컨트롤(특히 MultipleControl)의 높이가 변경되었을 때 전체 레이아웃을 재배치합니다.
        private void UserControl_OnHeightChanged(object sender, EventArgs e)
        {
            RelayoutControls(this.panel1);
        }

        // 컨트롤들의 위치와 문제 번호를 재설정합니다.
        private void RelayoutControls(Panel targetPanel)
        {
            int currentY = INITIAL_TOP;
            int spacing = SPACING;

            Point savedScrollPosition;
            Panel targetPanel1 = this.panel1;

            // 현재 스크롤 위치를 저장합니다.
            savedScrollPosition = new Point(Math.Abs(targetPanel1.AutoScrollPosition.X), Math.Abs(targetPanel1.AutoScrollPosition.Y));

            // 레이아웃 변경 시 깜빡임을 방지하기 위해 스크롤을 일시적으로 0으로 설정합니다.
            targetPanel1.AutoScrollPosition = new Point(0, 0);

            // 모든 컨트롤의 위치와 문제 번호를 업데이트합니다.
            for (int i = 0; i < targetPanel.Controls.Count; i++)
            {
                Control control = targetPanel.Controls[i];

                // 컨트롤의 Y 좌표를 업데이트합니다.
                control.Location = new Point(control.Left, currentY);

                // 문제 번호를 업데이트합니다.
                if (control is IQuestionControl questionControl)
                {
                    int questionNumber = i + 1;
                    questionControl.NumText = $"{questionNumber}.";
                }

                // 다음 컨트롤의 시작 Y 좌표를 계산합니다.
                currentY = control.Bottom + spacing;
            }

            targetPanel.PerformLayout();
            // 저장된 스크롤 위치로 되돌립니다.
            targetPanel.AutoScrollPosition = savedScrollPosition;
        }

        // 자식 컨트롤에서 문제 삭제를 요청했을 때 처리하는 이벤트 핸들러
        private void UserControl_OnDeleteRequested(object sender, EventArgs e)
        {
            UserControl controlToDelete = sender as UserControl;

            if (controlToDelete != null && controlToDelete.Parent != null)
            {
                Panel targetPanel = controlToDelete.Parent as Panel;

                // 부모 컬렉션에서 컨트롤을 제거합니다.
                controlToDelete.Parent.Controls.Remove(controlToDelete);

                // 리소스를 해제합니다.
                controlToDelete.Dispose();

                if (targetPanel != null)
                {
                    // 삭제 후 남은 컨트롤들의 위치와 번호를 재배치합니다.
                    RelayoutControls(targetPanel);
                }
            }
        }

        // '저장' 버튼 (Save) 클릭 이벤트 핸들러
        private void Save_Click(object sender, EventArgs e)
        {
            string rawName = textBox1.Text.Trim();

            // 1. 파일 제목 유효성 검사 (빈 값 체크)
            if (string.IsNullOrWhiteSpace(rawName))
            {
                MessageBox.Show("파일 제목을 입력하세요.", "경고",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. 파일 이름 유효성 검사 (시스템에서 사용할 수 없는 문자 체크)
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (rawName.Any(c => invalidChars.Contains(c)))
            {
                MessageBox.Show("파일 이름에 사용할 수 없는 문자가 포함되어 있습니다.\n" +
                                "사용할 수 없는 문자:  \\ / : * ? \" < > |",
                                "잘못된 파일 이름",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            string fileName = rawName + ".json"; // 저장할 파일 이름

            List<BaseQuestionData> allQuestions = new List<BaseQuestionData>();

            // 3. UI의 모든 Question Control에서 데이터를 추출합니다.
            foreach (Control control in panel1.Controls.OfType<BaseQuestionControl>())
            {
                BaseQuestionControl questionControl = (BaseQuestionControl)control;
                BaseQuestionData data = questionControl.GetQuestionData(); // 각 컨트롤에서 데이터 객체를 가져옵니다.

                // 문제 번호(NumText)를 정수로 파싱하여 저장 데이터에 할당합니다.
                if (int.TryParse(questionControl.NumText.TrimEnd('.'), out int number))
                {
                    data.Number = number;
                }

                // MultipleQuestionData (객관형) 처리
                if (data is MultipleQuestionData mcData)
                {
                    // 복수정답 문제일 경우, 문제 텍스트에 " (복수정답)" 접미사를 다시 붙여서 저장합니다.
                    if (!mcData.IsSingleChoice)
                    {
                        const string suffix = " (복수정답)";

                        if (!data.ProblemText.EndsWith(suffix))
                        {
                            data.ProblemText += suffix;
                        }
                    }
                }

                allQuestions.Add(data);
            }

            // 저장할 문제가 없는 경우 경고
            if (allQuestions.Count == 0)
            {
                MessageBox.Show("저장할 문제가 없습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. JSON 직렬화 옵션 설정
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // 가독성을 위해 들여쓰기 적용
                // JSON에 한글이 깨지지 않고 안전하게 인코딩되도록 설정
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };


            // 5. 문제집의 유형 목록을 수집합니다. (메타데이터용)
            List<string> types = new List<string>();

            foreach (var q in allQuestions)
            {
                if (q is MultipleQuestionData m)
                {
                    string multipleType = m.IsSingleChoice ? "Single" : "Multiple";
                    if (!types.Contains(multipleType))
                        types.Add(multipleType);
                }
                else
                {
                    string typeName = q.Type.ToString();
                    if (!types.Contains(typeName))
                        types.Add(typeName);
                }
            }

            // 6. JSON으로 직렬화할 최종 데이터 구조를 익명 객체로 생성합니다.
            var package = new
            {
                title = rawName, // 문제집 제목
                totalCount = allQuestions.Count, // 총 문제 수
                types = types, // 포함된 문제 유형 목록
                questions = allQuestions // 실제 문제 데이터 리스트
            };


            // 7. 객체를 JSON 문자열로 변환합니다.
            string jsonString = JsonSerializer.Serialize(package, options);


            // 8. SaveFileDialog를 사용하여 파일 저장 경로를 사용자에게 묻습니다.
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON 파일 (*.json)|*.json"; // 파일 필터 설정
                saveFileDialog.Title = "문제 파일 저장";
                saveFileDialog.FileName = fileName; // 기본 파일 이름 설정

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    try
                    {
                        // 9. JSON 문자열을 파일에 씁니다.
                        File.WriteAllText(filePath, jsonString, Encoding.UTF8);

                        // 저장 성공 시 DialogResult를 OK로 설정하고 폼을 닫습니다.
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        // 파일 저장 중 오류 발생 시 메시지 표시
                        MessageBox.Show($"파일 저장 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
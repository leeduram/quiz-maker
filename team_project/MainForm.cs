using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace team_project
{
    // MainForm 클래스
    // 애플리케이션의 시작점(메인 화면)을 나타내는 폼입니다.
    // 문제 생성, 문제 풀이 폴더 불러오기, 도움말 기능을 제공합니다.
    public partial class MainForm : Form
    {
        // 생성자
        public MainForm()
        {
            InitializeComponent(); // 폼의 구성 요소 초기화 (디자이너 생성 코드)

            // 폼이 화면에 표시된 후 (Shown) 이벤트 핸들러를 연결합니다.
            this.Shown += MainForm_Shown;

        }

        // 폼이 처음 화면에 표시될 때 호출되는 이벤트 핸들러입니다.
        // 프로그램 최초 실행 시 튜토리얼 메시지를 표시하는 역할을 합니다.
        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Properties.Settings.Default.TutorialShown 값(프로그램 설정)을 확인합니다.
            // 이 값이 false (튜토리얼이 아직 표시되지 않음)이면 튜토리얼을 표시합니다.
            if (!Properties.Settings.Default.TutorialShown)
            {
                // 사용자에게 프로그램 사용법을 안내하는 메시지 박스를 표시합니다.
                MessageBox.Show(
                    "📘 프로그램 사용법\n\n" +
                    "1. 문제를 생성하기 전에 문제들을 저장할 새로운 폴더를 만들어주세요.\n" +
                    "2. [새로운 문제 생성] 버튼으로 문제를 생성하세요.\n" +
                    "3. 처음에 만들었던 새로운 폴더의 위치에 저장하세요.\n" +
                    "4. [문제 불러오기] 버튼으로 처음에 만들었던 폴더를 선택하세요.\n" +
                    "5. JSON 문제 목록이 자동으로 로드됩니다.\n" +
                    "6. 문제를 선택해서 풀이를 진행하세요.\n\n" +
                    "이 창은 최초 1회만 표시됩니다.",
                    "튜토리얼",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // 튜토리얼이 표시되었음을 설정에 저장하고, 다음 실행부터는 표시되지 않도록 합니다.
                Properties.Settings.Default.TutorialShown = true;
                Properties.Settings.Default.Save();
            }
        }

        // '새로운 문제 생성' 버튼 (button1) 클릭 이벤트 핸들러
        private void button1_Click(object sender, EventArgs e)
        {
            // 문제 생성 및 편집을 위한 ProblemForm 인스턴스를 생성하고 표시합니다.
            ProblemForm problemForm = new ProblemForm();
            problemForm.ShowDialog(); // 모달(Modal)로 표시하여 ProblemForm이 닫힐 때까지 MainForm은 대기합니다.
        }

        // '문제 불러오기' 버튼 (button2) 클릭 이벤트 핸들러
        // 문제 파일(.json)이 저장된 폴더를 선택하고 문제 목록 폼을 로드합니다.
        private void button2_Click(object sender, EventArgs e)
        {
            // FolderBrowserDialog를 using 문으로 선언하여 사용 후 자동으로 리소스를 해제합니다.
            using (var fbd = new FolderBrowserDialog())
            {
                // 폴더 선택 대화상자의 초기 경로를 애플리케이션 실행 디렉토리로 설정합니다.
                fbd.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;

                // 사용자가 폴더를 선택하고 '확인'을 눌렀으며, 선택된 경로가 비어있지 않은지 확인합니다.
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string selectedFolderPath = fbd.SelectedPath; // 사용자가 선택한 폴더 경로

                    List<string> jsonFileNames = new List<string>(); // 발견된 JSON 파일 이름(확장자 제외)을 저장할 리스트

                    try
                    {
                        // 선택된 폴더 내에서 "*.json" 확장자를 가진 모든 파일을 찾습니다.
                        string[] files = Directory.GetFiles(selectedFolderPath, "*.json");

                        // 찾은 파일들을 반복 처리합니다.
                        foreach (string filePath in files)
                        {
                            // 파일의 전체 경로에서 파일 이름(확장자 포함)을 가져옵니다.
                            string fileNameWithExt = Path.GetFileName(filePath);
                            // 파일 이름에서 확장자를 제외한 부분만 가져옵니다.
                            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                            jsonFileNames.Add(fileNameWithoutExt); // 리스트에 추가
                        }

                        // JSON 파일이 하나 이상 발견된 경우
                        if (jsonFileNames.Count > 0)
                        {
                            // 로드된 파일 목록을 사용자에게 알립니다.
                            MessageBox.Show($"선택된 폴더: {selectedFolderPath}\n\n발견된 JSON 파일 목록:\n{string.Join("\n", jsonFileNames)}", "문제 목록 로드 성공");

                            // 문제 풀이 목록 폼(SolveListForm)을 생성하고 표시합니다.
                            // 선택된 폴더 경로와 파일 이름 리스트를 전달하여 폼에서 문제 정보를 로드하도록 합니다.
                            SolveListForm solveListForm = new SolveListForm(selectedFolderPath, jsonFileNames);
                            solveListForm.ShowDialog(); // 모달로 표시
                        }
                        else
                        {
                            // JSON 파일이 발견되지 않은 경우 알림 메시지를 표시합니다.
                            MessageBox.Show("선택된 폴더에 *.json 확장자를 가진 파일이 없습니다.", "알림");
                        }
                    }
                    catch (Exception ex)
                    {
                        // 파일 시스템 접근 중 오류 발생 시 사용자에게 알립니다.
                        MessageBox.Show($"파일을 읽는 도중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // '도움말' 버튼 (button3) 클릭 이벤트 핸들러
        private void button3_Click(object sender, EventArgs e)
        {
            // 튜토리얼과 동일한 내용의 도움말 메시지를 표시합니다.
            MessageBox.Show(
                "📘 프로그램 사용법\n\n" +
                "1. 문제를 생성하기 전에 문제들을 저장할 새로운 폴더를 만들어주세요.\n" +
                "2. [새로운 문제 생성] 버튼으로 문제를 생성하세요.\n" +
                "3. 처음에 만들었던 새로운 폴더의 위치에 저장하세요.\n" +
                "4. [문제 불러오기] 버튼으로 처음에 만들었던 폴더를 선택하세요.\n" +
                "5. JSON 문제 목록이 자동으로 로드됩니다.\n" +
                "6. 문제를 선택해서 풀이를 진행하세요.\n\n",
                "도움말",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
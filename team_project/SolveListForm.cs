using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // 파일 시스템 접근 (읽기)
using System.Linq; // LINQ 확장 메서드 사용 (Where, Select, ToList)
using System.Text;
using System.Text.Json; // JSON 직렬화/역직렬화
using System.Windows.Forms;

namespace team_project
{
    // SolveListForm 클래스
    // 문제집 풀이 목록을 표시하고, 선택된 문제집을 로드하여 풀이 화면을 시작하는 폼입니다.
    public partial class SolveListForm : Form
    {
        private string _questionFolderPath; // 문제집 파일이 저장된 폴더 경로
        private List<QuestionSetInfo> _allQuestionSets; // 로드된 모든 문제집의 메타데이터 목록

        // 생성자
        public SolveListForm(string folderPath, List<string> fileNames)
        {
            InitializeComponent();
            this.listView1.View = View.Details; // ListView를 상세 정보 보기 모드로 설정

            _questionFolderPath = folderPath; // 폴더 경로 저장

            // 파일 이름 목록을 기반으로 문제집 메타데이터를 로드
            _allQuestionSets = LoadQuestionSetInfos(folderPath, fileNames);

            // 로드된 문제집 목록을 화면에 표시
            DisplayFileList(_allQuestionSets);
        }

        // 문제 유형 문자열을 한국어로 변환합니다. (정적 메서드)
        private static string ConvertTypeToKorean(string type)
        {
            string trimmedType = type.Trim().ToLower();

            if (trimmedType == "short")
            {
                return "단답형";
            }
            else if (trimmedType == "single")
            {
                return "객관형";
            }
            else if (trimmedType == "ox")
            {
                return "OX";
            }
            else if (trimmedType == "multiple")
            {
                return "객관형(복수정답)";
            }
            return type; // 매핑되지 않는 유형은 원본 그대로 반환
        }

        // 문제 유형 목록(List<string>)을 쉼표로 연결된 한국어 문자열로 변환합니다. (정적 메서드)
        private static string GetProblemTypesString(List<string> types)
        {
            if (types == null || types.Count == 0)
            {
                return "유형 없음";
            }
            // LINQ를 사용하여 각 유형을 한국어로 변환한 후 Join으로 연결
            var koreanTypes = types.Select(ConvertTypeToKorean).ToList();
            return string.Join(", ", koreanTypes);
        }


        // QuestionSetInfo 클래스
        // 파일 로드 시 QuestionSetData 전체가 아닌, 목록 표시에 필요한 메타데이터(제목, 개수, 유형)만 담는 구조체입니다.
        public class QuestionSetInfo
        {
            public string title { get; set; } // 문제집 제목
            public int totalCount { get; set; } // 총 문제 수
            public List<string> types { get; set; } // 포함된 문제 유형 목록 (예: "Short", "Ox")
            public string FileName { get; set; } // 원본 파일 이름 (확장자 제외)

            public override string ToString()
            {
                string typesString = types != null ? string.Join(", ", types) : "유형 없음";
                return $"{title} | {totalCount}개 | {typesString}";
            }
        }

        // 지정된 폴더에서 파일 이름 목록을 기반으로 문제집 메타데이터를 로드합니다.
        private List<QuestionSetInfo> LoadQuestionSetInfos(string folderPath, List<string> fileNames)
        {
            var questionSets = new List<QuestionSetInfo>();

            // 각 파일에 대해 반복
            foreach (string fileName in fileNames)
            {
                string fullPath = Path.Combine(folderPath, fileName + ".json");
                if (File.Exists(fullPath))
                {
                    try
                    {
                        string jsonString = File.ReadAllText(fullPath);
                        // 파일에서 QuestionSetInfo(메타데이터)만 역직렬화
                        var info = JsonSerializer.Deserialize<QuestionSetInfo>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (info != null)
                        {
                            info.FileName = fileName;
                            questionSets.Add(info);
                        }
                    }
                    catch (Exception ex)
                    {
                        // JSON 파일 읽기 또는 역직렬화 오류 발생 시 메시지 표시
                        MessageBox.Show($"파일 로드 오류 ({fileName}.json): {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return questionSets;
        }

        // QuestionSetInfo 목록을 ListView에 표시합니다.
        private void DisplayFileList(List<QuestionSetInfo> questionSets)
        {
            this.listView1.Items.Clear(); // 기존 목록 초기화

            foreach (QuestionSetInfo info in questionSets)
            {
                // 문제 유형을 한국어 문자열로 변환
                string typesString = GetProblemTypesString(info.types);

                // ListViewItem 생성 (첫 번째 칼럼: 제목)
                ListViewItem item = new ListViewItem(info.title);

                // 서브 아이템 추가 (총 문제 수, 유형)
                item.SubItems.Add($"{info.totalCount}개");
                item.SubItems.Add(typesString);

                // QuestionSetInfo 객체 자체를 Tag 속성에 저장하여 나중에 쉽게 접근할 수 있도록 합니다.
                item.Tag = info;

                this.listView1.Items.Add(item);
            }
        }

        // 검색어 입력 필드(SearchText)의 텍스트가 변경될 때 호출됩니다.
        private void SearchText_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = SearchText.Text.Trim(); // 검색어 가져오기

            if (string.IsNullOrEmpty(searchTerm))
            {
                // 검색어가 없으면 전체 목록 표시
                DisplayFileList(_allQuestionSets);
            }
            else
            {
                // 제목에 검색어가 포함된 문제집만 필터링 (대소문자 구분 없이)
                List<QuestionSetInfo> filteredList = _allQuestionSets
                    .Where(info => info.title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                // 필터링된 목록 표시
                DisplayFileList(filteredList);
            }
        }

        // ListView 항목을 더블클릭했을 때 호출되는 이벤트 핸들러입니다. (문제 풀이 시작 로직)
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = this.listView1.SelectedItems[0];

                // Tag에 저장된 QuestionSetInfo 객체 가져오기
                if (selectedItem.Tag is QuestionSetInfo selectedInfo)
                {
                    string problemTitle = selectedInfo.title;
                    string selectedFile = selectedInfo.FileName;
                    string fullPath = Path.Combine(_questionFolderPath, selectedFile + ".json");

                    QuestionSetData data = null; // 문제집 전체 데이터

                    // 파일이 실제로 존재하는지 확인
                    if (File.Exists(fullPath))
                    {
                        try
                        {
                            string jsonString = File.ReadAllText(fullPath);

                            var options = new JsonSerializerOptions
                            {
                                // JSON 속성 이름의 대소문자 구분을 무시하여 역직렬화
                                PropertyNameCaseInsensitive = true
                            };

                            // QuestionSetData 전체를 역직렬화 
                            data = JsonSerializer.Deserialize<QuestionSetData>(jsonString, options);

                            if (data != null)
                            {
                                int totalProblems = data.totalCount;
                                string problemTypes = (data.types != null && data.types.Count > 0) ? GetProblemTypesString(data.types) : "유형 없음";

                                // 1. 풀이 전 미리보기 폼(PreviewForm)을 생성하고 문제집 데이터를 전달
                                PreviewForm previewForm = new PreviewForm(
                                    this, // 부모 폼
                                    problemTitle,
                                    totalProblems,
                                    problemTypes,
                                    data // 전체 문제 데이터
                                );

                                this.Hide(); // 현재 폼 숨기기

                                // 2. 미리보기 폼을 모달로 표시 (사용자가 닫을 때까지 대기)
                                DialogResult result = previewForm.ShowDialog();

                                // 3. 미리보기 폼의 결과 처리 (풀이 시작 여부)
                                if (result == DialogResult.Yes) // 사용자가 '풀이 시작'을 선택한 경우
                                {
                                    this.Close(); // 현재 문제 목록 폼을 닫고, QuizForm이 열리도록 합니다.
                                }
                                else // 사용자가 '취소'를 선택한 경우
                                {
                                    this.Show(); // 숨겨진 현재 폼을 다시 표시
                                }
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"문제 로드 중 오류 발생: {ex.Message}", "오류");
                            return;
                        }
                    }

                    MessageBox.Show("문제집 데이터 로드에 실패했습니다. 파일을 찾을 수 없거나 형식이 잘못되었습니다.", "오류");
                }
            }
        }

        // '취소' 또는 '닫기' 버튼 클릭 이벤트 핸들러
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close(); // 폼 닫기
        }
    }
}
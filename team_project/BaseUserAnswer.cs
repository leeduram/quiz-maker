using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace team_project
{
    // BaseUserAnswer 추상 클래스
    // 모든 문제 유형에 대한 **사용자의 응답 데이터를 저장하고 관리**하는 기반 클래스입니다.
    public abstract class BaseUserAnswer
    {
        // 사용자가 풀고 있는 문제의 원본 데이터를 참조합니다.
        // [JsonIgnore] 속성을 사용하여, 이 객체가 JSON 파일로 저장될 때 Question 데이터는 제외됩니다.
        // Question 데이터는 문제집 데이터에 이미 포함되어 있으므로 중복 저장을 방지합니다.
        [JsonIgnore]
        public BaseQuestionData Question { get; set; }

        // 사용자의 응답이 정답인지 여부를 나타냅니다. (채점 결과)
        public bool IsCorrect { get; set; }

        // 사용자의 응답 내용을 요약된 텍스트(예: 단답형 답, 선택한 보기 번호)로 반환하는 추상 메서드입니다.
        // 결과 요약 화면 등에 사용됩니다.
        public abstract string GetSummaryText();

        // 사용자의 응답을 채점하고 IsCorrect 속성을 업데이트하는 추상 메서드입니다.
        // 채점 결과를 bool 값으로 반환합니다.
        public abstract bool ScoreAnswer();

        // 연결된 문제 객체(Question)로부터 문제 유형을 가져오는 헬퍼 메서드입니다.
        public QuestionType GetQuestionType() => Question.Type;
    }


    // ShortUserAnswer 클래스
    // **단답형(Short) 문제**에 대한 사용자의 응답 데이터를 나타냅니다.
    public class ShortUserAnswer : BaseUserAnswer
    {
        // 사용자가 입력한 단답형 텍스트 응답
        public string AnswerText { get; set; } = ""; // 기본값: 빈 문자열

        // 응답 요약 텍스트를 반환합니다. 입력이 없으면 "미제출"로 표시합니다.
        public override string GetSummaryText() => string.IsNullOrEmpty(AnswerText) ? "미제출" : AnswerText;

        // 단답형 응답을 채점합니다.
        public override bool ScoreAnswer()
        {
            // Question 속성을 단답형 문제 데이터(ShortQuestionData)로 형변환합니다.
            var shortQuestion = Question as ShortQuestionData;

            // 문제 데이터가 없거나 정답이 설정되어 있지 않으면 오답 처리합니다.
            if (shortQuestion == null || string.IsNullOrEmpty(shortQuestion.CorrectAnswerText))
                return IsCorrect = false;

            // 1. 사용자 응답과 정답을 양쪽 공백 제거 후 비교합니다.
            // 2. StringComparison.OrdinalIgnoreCase를 사용하여 **대소문자를 구분하지 않고** 비교합니다.
            bool result = shortQuestion.CorrectAnswerText.Trim().Equals(
                AnswerText.Trim(),
                StringComparison.OrdinalIgnoreCase
            );

            // 채점 결과를 IsCorrect에 저장하고 반환합니다.
            return IsCorrect = result;
        }
    }

    // OxUserAnswer 클래스
    // **OX형(Ox) 문제**에 대한 사용자의 응답 데이터를 나타냅니다.
    public class OxUserAnswer : BaseUserAnswer
    {
        // 사용자의 O/X 선택. true: O, false: X, null: 미선택
        public bool? AnswerIsO { get; set; } = null;

        // 응답 요약 텍스트를 반환합니다. 선택 값이 있으면 "O" 또는 "X", 없으면 "미제출"입니다.
        public override string GetSummaryText() => AnswerIsO.HasValue ? (AnswerIsO.Value ? "O" : "X") : "미제출";

        // OX형 응답을 채점합니다.
        public override bool ScoreAnswer()
        {
            // Question 속성을 OX형 문제 데이터(OxQuestionData)로 형변환합니다.
            var oxQuestion = Question as OxQuestionData;

            // 문제 데이터가 없거나 사용자가 선택하지 않았으면 오답 처리합니다.
            if (oxQuestion == null || !AnswerIsO.HasValue)
                return IsCorrect = false;

            // 사용자의 선택 값과 문제의 정답 값(CorrectAnswerIsO)을 비교합니다.
            bool result = AnswerIsO.Value == oxQuestion.CorrectAnswerIsO;

            // 채점 결과를 IsCorrect에 저장하고 반환합니다.
            return IsCorrect = result;
        }
    }

    // MultipleUserAnswer 클래스
    // **객관형(Single) 또는 복수정답 객관형(Multiple) 문제**에 대한 사용자의 응답 데이터를 나타냅니다.
    public class MultipleUserAnswer : BaseUserAnswer
    {
        // 사용자가 선택한 **보기의 인덱스 리스트** (0부터 시작하는 인덱스).
        public List<int> SelectedOptions { get; set; } = new List<int>();

        // 응답 요약 텍스트를 반환합니다.
        // 선택된 인덱스에 1을 더하여 (1번 보기, 2번 보기...) 쉼표로 연결하고 오름차순으로 정렬합니다.
        public override string GetSummaryText()
        {
            if (SelectedOptions.Count == 0) return "미제출";
            // 인덱스(0부터)를 보기 번호(1부터)로 변환하여 정렬 후 쉼표로 연결
            return string.Join(",", SelectedOptions.Select(i => i + 1).OrderBy(x => x));
        }

        // 객관형 응답을 채점합니다. (단일/복수 정답 모두 처리)
        public override bool ScoreAnswer()
        {
            // Question 속성을 객관형 문제 데이터(MultipleQuestionData)로 형변환합니다.
            var multipleQuestion = Question as MultipleQuestionData;
            if (multipleQuestion == null) return IsCorrect = false;

            // 1. 문제 데이터에서 **정답인 보기의 인덱스 리스트**를 추출하고 정렬합니다.
            List<int> correctIndices = multipleQuestion.Options
                // 보기 항목을 인덱스와 함께 익명 객체로 만듭니다.
                .Select((option, index) => new { option, index })
                // IsCorrect가 true인 항목만 필터링합니다.
                .Where(x => x.option.IsCorrect)
                // 인덱스만 추출합니다.
                .Select(x => x.index)
                // 오름차순으로 정렬합니다.
                .OrderBy(x => x)
                .ToList();

            // 2. 사용자가 선택한 인덱스 리스트를 오름차순으로 정렬합니다.
            List<int> userSelections = SelectedOptions.OrderBy(x => x).ToList();

            // 3. 사용자의 선택 리스트와 정답 인덱스 리스트가 **순서와 내용이 완전히 일치**하는지 확인합니다.
            // SequenceEqual은 복수정답 문제에서 모든 정답을 정확히 선택했는지 확인하는 데 적합합니다.
            bool result = userSelections.SequenceEqual(correctIndices);

            // 채점 결과를 IsCorrect에 저장하고 반환합니다.
            return IsCorrect = result;
        }
    }
}
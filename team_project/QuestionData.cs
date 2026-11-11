using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace team_project
{
    // QuestionType 열거형
    // 문제의 유형을 정의하며, JSON 직렬화 시 문자열로 처리되도록 설정합니다.
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuestionType { Short, Ox, Multiple, Single }


    // BaseQuestionData 추상 클래스
    // 모든 문제 데이터 클래스의 기본이 되는 추상 클래스입니다.
    // JSON 직렬화 시 다형성(Polymorphism)을 지원하도록 설정합니다.
    [JsonPolymorphic(
         TypeDiscriminatorPropertyName = "$type")] // 직렬화 시 문제의 실제 타입을 저장할 속성 이름을 정의
    [JsonDerivedType(typeof(OxQuestionData), typeDiscriminator: "Ox")] // 하위 타입: OxQuestionData (식별자: "Ox")
    [JsonDerivedType(typeof(ShortQuestionData), typeDiscriminator: "Short")] // 하위 타입: ShortQuestionData (식별자: "Short")
    [JsonDerivedType(typeof(MultipleQuestionData), typeDiscriminator: "Multiple")] // 하위 타입: MultipleQuestionData (식별자: "Multiple")
    [JsonDerivedType(typeof(SingleQuestionData), typeDiscriminator: "Single")] // 하위 타입: SingleQuestionData (식별자: "Single")

    public abstract class BaseQuestionData
    {
        // 문제 유형 (열거형)
        public QuestionType Type { get; protected set; }
        // 문제 번호
        public int Number { get; set; }
        // 문제 텍스트
        public string ProblemText { get; set; }

        // 생성자
        public BaseQuestionData()
        {
            // 리플렉션(Reflection)을 사용하여 클래스 이름을 기반으로 Type 속성을 자동으로 설정합니다.
            // 예: OxQuestionData -> "OxQuestionData" -> "Ox" -> QuestionType.Ox
            Type = (QuestionType)Enum.Parse(typeof(QuestionType), this.GetType().Name.Replace("QuestionData", ""));
        }
    }

    // OptionData 클래스
    // 객관식 문제의 보기 하나에 대한 데이터를 정의합니다.
    public class OptionData
    {
        public string Text { get; set; } // 보기 텍스트 내용
        public bool IsCorrect { get; set; } // 해당 보기가 정답인지 여부
    }

    // MultipleQuestionData 클래스
    // 객관식 (복수 정답 포함) 문제의 데이터를 정의합니다.
    public class MultipleQuestionData : BaseQuestionData
    {
        public MultipleQuestionData() { }
        // 단일 선택형인지 (true) 복수 선택형인지 (false) 여부
        public bool IsSingleChoice { get; set; }
        // 보기 목록
        public List<OptionData> Options { get; set; }
        // 정답인 보기의 인덱스 목록 (0부터 시작)
        public List<int> CorrectAnswerIndices { get; set; } = new List<int>();
    }

    // SingleQuestionData 클래스
    // 객관식 단일 정답 문제의 데이터 클래스입니다. (MultipleQuestionData를 상속받아 사용)
    public class SingleQuestionData : MultipleQuestionData
    {
        public SingleQuestionData() { }
    }

    // ShortQuestionData 클래스
    // 단답형 문제의 데이터를 정의합니다.
    public class ShortQuestionData : BaseQuestionData
    {
        public ShortQuestionData() { }
        public string CorrectAnswerText { get; set; } // 단답형 정답 텍스트
    }

    // OxQuestionData 클래스
    // O/X형 문제의 데이터를 정의합니다.
    public class OxQuestionData : BaseQuestionData
    {
        public OxQuestionData() { }
        public bool CorrectAnswerIsO { get; set; } // 정답이 O인지 (true) X인지 (false) 여부
    }

    // QuestionSetData 클래스
    // 전체 문제집 파일의 메타데이터와 문제 목록을 포함하는 최종 데이터 구조입니다.
    public class QuestionSetData
    {
        public string title { get; set; } // 문제집 제목
        public int totalCount { get; set; } // 총 문제 수
        public List<string> types { get; set; } // 포함된 문제 유형 목록 (메타데이터)
        // BaseQuestionData 타입으로 선언하여 다형성을 통해 모든 문제 유형을 포함할 수 있습니다.
        public List<BaseQuestionData> questions { get; set; }
    }
}
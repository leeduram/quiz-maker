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
    // MultipleControl 클래스
    // 객관식 (단일 선택 또는 복수 정답) 문제를 정의하고 편집하는 컨트롤입니다.
    // BaseQuestionControl을 상속받아 문제 생성 폼(ProblemForm)에서 사용됩니다.
    public partial class MultipleControl : BaseQuestionControl
    {
        // 동적으로 추가된 개별 보기 컨트롤(Multiple 또는 Single UserControl) 리스트
        private List<UserControl> optionControls = new List<UserControl>();
        // 현재 문제의 보기 유형을 추적 (None, Single, Multiple 중 하나)
        private OptionType currentOptionType = OptionType.None;

        // 보기 유형을 나타내는 열거형
        private enum OptionType { None, Single, Multiple }

        // 상수 정의
        private const int MAX_OPTIONS = 6;              // 최대 보기 개수
        private const int OPTION_SPACING = 5;           // 보기 간의 수직 간격
        private const int INITIAL_OPTION_Y = 50;        // 첫 번째 보기 컨트롤의 초기 Y 좌표
        private const int INITIAL_BUTTON_Y = 64;        // 보기 추가 버튼들의 초기 Y 좌표 (보기가 없을 때)

        // 단일 선택 문제일 경우, 정답 선택을 위해 별도로 관리하는 RadioButton 리스트
        private List<RadioButton> singleRadioButtons = new List<RadioButton>();


        // 컨트롤의 높이가 변경되었음을 부모에게 알리는 이벤트 (부모 폼의 스크롤 또는 레이아웃 조정에 사용)
        public event EventHandler HeightChanged;

        // 생성자
        public MultipleControl()
        {
            InitializeComponent(); // 디자이너 컴포넌트 초기화
        }

        // IQuestionControl 인터페이스 구현: 문제 번호 텍스트 접근자
        public override string NumText
        {
            get { return Num.Text; }
            set { Num.Text = value; }
        }


        // '단일 선택 보기 추가' 버튼 클릭 이벤트 핸들러
        private void SingleOptionAddButton_Click(object sender, EventArgs e)
        {
            // 현재 유형이 설정되지 않았거나 단일 선택 유형일 경우에만 추가를 허용합니다.
            if (currentOptionType == OptionType.None || currentOptionType == OptionType.Single)
            {
                currentOptionType = OptionType.Single;
                AddOptionControl(currentOptionType);
            }
        }

        // '복수 정답 보기 추가' 버튼 클릭 이벤트 핸들러
        private void OptionAddButton_Click(object sender, EventArgs e)
        {
            // 현재 유형이 설정되지 않았거나 복수 정답 유형일 경우에만 추가를 허용합니다.
            if (currentOptionType == OptionType.None || currentOptionType == OptionType.Multiple)
            {
                currentOptionType = OptionType.Multiple;
                AddOptionControl(currentOptionType);
            }
        }


        // 보기 컨트롤을 데이터 없이 추가하는 오버로드 메서드
        private void AddOptionControl(OptionType type)
        {
            AddOptionControl(type, null);
        }

        // 보기 데이터(OptionData)와 함께 보기 컨트롤을 생성하고 추가하는 핵심 메서드
        private void AddOptionControl(OptionType type, OptionData data)
        {
            // 최대 보기 개수 제한을 초과하면 추가하지 않습니다.
            if (optionControls.Count >= MAX_OPTIONS)
            {
                return;
            }

            // 문제 유형에 따라 적절한 UserControl(Multiple 또는 Single)을 생성합니다.
            UserControl newOptionControl = (type == OptionType.Multiple) ?
                (UserControl)new Multiple() : (UserControl)new Single();

            if (newOptionControl == null) return;

            RadioButton newRadio = null;

            // 단일 선택 유형인 경우, 정답 선택을 위한 RadioButton을 별도로 생성하고 관리합니다.
            if (type == OptionType.Single)
            {
                newRadio = new RadioButton();
                newRadio.AutoSize = true;

                // RadioButton의 Tag에 연결된 보기 컨트롤을 저장하여 나중에 찾을 수 있도록 합니다.
                newRadio.Tag = newOptionControl;

                this.Controls.Add(newRadio);
                singleRadioButtons.Add(newRadio);
            }


            // 첫 번째 보기가 추가될 때, 다른 유형의 '보기 추가' 버튼을 숨깁니다.
            if (optionControls.Count == 0)
            {
                if (type == OptionType.Multiple) SingleBtn.Visible = false;
                else MultipleBtn.Visible = false;
            }

            // data가 null이 아니면(문제를 로드하는 경우), 컨트롤에 데이터를 설정합니다.
            if (data != null)
            {
                // 복수 정답 컨트롤(Multiple)인 경우
                if (newOptionControl is Multiple)
                {
                    // UserControl을 Multiple 타입으로 명시적 형변환 후, targetControl 변수에 할당합니다.
                    Multiple targetControl = (Multiple)newOptionControl;

                    // 내부 컨트롤(CheckBox, TextBox)을 찾아 데이터 로드
                    CheckBox answerCheckbox = targetControl.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Name == "Answer");
                    TextBox answerTextBox = targetControl.Controls.OfType<TextBox>().FirstOrDefault(c => c.Name == "AnswerTxt");

                    if (answerCheckbox != null) answerCheckbox.Checked = data.IsCorrect;
                    if (answerTextBox != null) answerTextBox.Text = data.Text;
                }
                // 단일 선택 컨트롤(Single)인 경우
                else if (newOptionControl is Single)
                {
                    // UserControl을 Single 타입으로 명시적 형변환 후, targetControl 변수에 할당합니다.
                    Single targetControl = (Single)newOptionControl;

                    // 내부 컨트롤(TextBox)을 찾아 데이터 로드
                    TextBox answerTextBox = targetControl.Controls.OfType<TextBox>().FirstOrDefault(c => c.Name == "AnswerTxt");

                    if (answerTextBox != null) answerTextBox.Text = data.Text;

                    // 별도로 생성한 RadioButton의 상태를 설정합니다.
                    if (newRadio != null)
                    {
                        newRadio.Checked = data.IsCorrect;
                    }
                }
            }


            // 보기 컨트롤의 '삭제' 버튼 클릭 이벤트 핸들러를 연결합니다.
            if (newOptionControl is Multiple multiOption)
            {
                multiOption.Del.Click += OptionDel_Click;
            }
            else if (newOptionControl is Single singleOption)
            {
                singleOption.Del.Click += OptionDel_Click;
            }

            // 생성된 보기 컨트롤을 리스트에 추가하고, 화면에 표시합니다.
            optionControls.Add(newOptionControl);
            this.Controls.Add(newOptionControl);

            // 컨트롤들의 위치를 재배치합니다.
            RelayoutOptions();

            // 최대 보기 개수에 도달하면 '보기 추가' 버튼들을 모두 숨깁니다.
            if (optionControls.Count >= MAX_OPTIONS)
            {
                MultipleBtn.Visible = false;
                SingleBtn.Visible = false;
            }

            // 높이 변경 이벤트를 발생시켜 부모 폼에 레이아웃 업데이트를 알립니다.
            HeightChanged?.Invoke(this, EventArgs.Empty);
        }

        // 보기 컨트롤 내부의 '삭제' 버튼 클릭 이벤트 핸들러
        private void OptionDel_Click(object sender, EventArgs e)
        {
            Button delButton = sender as Button;
            if (delButton == null) return;

            // '삭제' 버튼의 부모 컨트롤(UserControl)을 찾아 삭제할 대상을 식별합니다.
            UserControl controlToDelete = delButton.Parent as UserControl;
            if (controlToDelete == null) return;

            // 단일 선택 컨트롤인 경우, 연결된 RadioButton도 찾아 제거합니다.
            if (controlToDelete is Single)
            {
                RadioButton radioToDelete = singleRadioButtons.FirstOrDefault(r => r.Tag == controlToDelete);

                if (radioToDelete != null)
                {
                    this.Controls.Remove(radioToDelete);
                    singleRadioButtons.Remove(radioToDelete);
                    radioToDelete.Dispose();
                }
            }

            // 보기 컨트롤 자체를 제거하고 리스트에서 삭제 후 리소스를 해제합니다.
            this.Controls.Remove(controlToDelete);
            optionControls.Remove(controlToDelete);
            controlToDelete.Dispose();

            // 남은 컨트롤들의 위치를 재배치하고 번호를 업데이트합니다.
            RelayoutOptions();

            // 보기가 모두 삭제된 경우, 문제 유형 상태를 초기화하고 버튼들을 다시 보이게 합니다.
            if (optionControls.Count == 0)
            {
                MultipleBtn.Visible = true;
                SingleBtn.Visible = true;
                currentOptionType = OptionType.None;
            }
            // 최대 개수 미만이면 현재 유형에 해당하는 '보기 추가' 버튼만 다시 보이게 합니다.
            else if (optionControls.Count < MAX_OPTIONS)
            {
                if (currentOptionType == OptionType.Multiple) MultipleBtn.Visible = true;
                else if (currentOptionType == OptionType.Single) SingleBtn.Visible = true;
            }

            // 높이 변경 이벤트를 발생시켜 부모 폼에 레이아웃 업데이트를 알립니다.
            HeightChanged?.Invoke(this, EventArgs.Empty);
        }


        // 문제 전체의 '삭제' 버튼 클릭 이벤트 핸들러
        private void Del_Click(object sender, EventArgs e)
        {
            OnDeleteRequested(); // IQuestionControl 인터페이스의 DeleteRequested 이벤트 발생
        }


        // 보기 컨트롤들과 버튼의 위치를 계산하고 업데이트합니다.
        private void RelayoutOptions()
        {
            int currentY = INITIAL_OPTION_Y; // 첫 번째 보기의 시작 Y 좌표

            // 1. 모든 보기 컨트롤의 위치를 조정하고 번호를 업데이트합니다.
            for (int i = 0; i < optionControls.Count; i++)
            {
                UserControl control = optionControls[i];

                // 보기 컨트롤의 새로운 위치 설정
                control.Location = new Point(70, currentY);

                // 단일 선택(Single)인 경우, 연결된 RadioButton의 위치도 조정합니다.
                if (control is Single singleOption)
                {
                    RadioButton radio = singleRadioButtons.FirstOrDefault(r => r.Tag == singleOption);

                    if (radio != null)
                    {
                        // RadioButton을 보기 컨트롤의 중앙에 배치하고 앞으로 가져옵니다.
                        radio.Location = new Point(72, currentY + (singleOption.Height / 2) - (radio.Height / 2));
                        radio.BringToFront();
                    }
                }

                // 보기 번호 텍스트를 업데이트합니다.
                string numText = $"{i + 1}.";
                if (control is Multiple multiple)
                {
                    multiple.Num.Text = numText;
                }
                else if (control is Single single)
                {
                    single.Num.Text = numText;
                }

                // 다음 컨트롤의 Y 좌표를 계산합니다.
                currentY = control.Bottom + OPTION_SPACING;
            }

            // 2. '보기 추가' 버튼들의 Y 좌표를 조정합니다.
            int buttonY;
            if (optionControls.Count == 0)
            {
                // 보기가 없으면 미리 정해진 초기 위치에 배치
                buttonY = INITIAL_BUTTON_Y;
            }
            else
            {
                // 보기가 있으면 마지막 보기 아래에 배치
                buttonY = currentY + 5;
            }

            MultipleBtn.Location = new Point(MultipleBtn.Location.X, buttonY);
            SingleBtn.Location = new Point(SingleBtn.Location.X, buttonY);

            // 3. 컨트롤 자체의 높이를 조정합니다.
            int newHeight = SingleBtn.Bottom + 10; // 마지막 버튼의 하단에 약간의 여백 추가

            if (this.Height != newHeight)
            {
                this.Height = newHeight;
            }

            // 부모 컨트롤에 레이아웃 변경을 알립니다.
            this.PerformLayout();
        }

        // IQuestionControl 인터페이스 구현: 현재 컨트롤의 상태를 MultipleQuestionData 객체로 추출하여 반환합니다.
        public override BaseQuestionData GetQuestionData()
        {
            var data = new MultipleQuestionData
            {
                ProblemText = Problem.Text, // 문제 텍스트
                IsSingleChoice = (currentOptionType == OptionType.Single), // 단일 선택 여부
                Options = new List<OptionData>() // 보기 리스트 초기화
            };

            // 각 보기 컨트롤에서 데이터를 추출하여 OptionData를 생성합니다.
            foreach (UserControl control in optionControls)
            {
                OptionData option = new OptionData();

                if (currentOptionType == OptionType.Multiple)
                {
                    if (control is Multiple multipleControl)
                    {
                        // 복수 정답 컨트롤에서 보기 내용과 정답 체크 여부를 가져옵니다.
                        CheckBox answerCheckbox = multipleControl.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Name == "Answer");
                        TextBox answerTextBox = multipleControl.Controls.OfType<TextBox>().FirstOrDefault(c => c.Name == "AnswerTxt");

                        if (answerTextBox != null) option.Text = answerTextBox.Text;
                        if (answerCheckbox != null) option.IsCorrect = answerCheckbox.Checked;

                        data.Options.Add(option);
                    }
                }
                else if (currentOptionType == OptionType.Single)
                {
                    if (control is Single singleControl)
                    {
                        // 단일 선택 컨트롤에서 보기 내용을 가져옵니다.
                        TextBox answerTextBox = singleControl.Controls.OfType<TextBox>().FirstOrDefault(c => c.Name == "AnswerTxt");

                        int index = optionControls.IndexOf(singleControl); // 현재 컨트롤의 인덱스

                        if (answerTextBox != null) option.Text = answerTextBox.Text;

                        // 해당 인덱스의 RadioButton 상태를 통해 정답 여부를 판단합니다.
                        if (index >= 0 && index < singleRadioButtons.Count)
                        {
                            option.IsCorrect = singleRadioButtons[index].Checked;
                        }

                        data.Options.Add(option);
                    }
                }
            }

            return data;
        }


        // 모든 보기 컨트롤과 RadioButton을 제거하고 상태를 초기화합니다.
        private void ClearOptions()
        {
            // 모든 컨트롤 Dispose
            foreach (var control in optionControls) control.Dispose();
            foreach (var radio in singleRadioButtons) radio.Dispose();

            // 부모 컨트롤에서 RadioButton 제거
            this.Controls.OfType<RadioButton>().Where(r => singleRadioButtons.Contains(r)).ToList().ForEach(r => this.Controls.Remove(r));

            // 리스트 초기화
            optionControls.Clear();
            singleRadioButtons.Clear();

            // 상태 초기화
            currentOptionType = OptionType.None;
            if (SingleBtn != null) SingleBtn.Visible = true;
            if (MultipleBtn != null) MultipleBtn.Visible = true;

            // 레이아웃 재조정
            RelayoutOptions();
        }

        // IQuestionControl 인터페이스 구현: 전달받은 MultipleQuestionData 객체를 컨트롤에 로드합니다.
        public override void SetQuestionData(BaseQuestionData data)
        {
            if (data is MultipleQuestionData multiData)
            {
                // 기존의 보기들을 모두 지우고 초기화합니다.
                ClearOptions();

                // 문제 텍스트를 설정합니다.
                this.Problem.Text = multiData.ProblemText;

                // 문제 유형을 설정합니다.
                OptionType type = multiData.IsSingleChoice ? OptionType.Single : OptionType.Multiple;
                this.currentOptionType = type;

                // 반대 유형의 '보기 추가' 버튼을 숨깁니다.
                if (type == OptionType.Multiple) SingleBtn.Visible = false;
                else MultipleBtn.Visible = false;

                // 저장된 각 보기 데이터에 대해 보기 컨트롤을 생성하고 데이터를 로드합니다.
                foreach (var optionData in multiData.Options)
                {
                    AddOptionControl(type, optionData);
                }
            }
        }
    }
}
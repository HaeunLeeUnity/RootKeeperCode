using UnityEngine;
using UnityEngine.UI;

namespace LevelDesign
{
    // 레벨 디자이너에서 생성된 패턴 라인.
    public class PattenLineScript : MonoBehaviour
    {
        public int PattenIndex;

        [SerializeField] GameObject SelectGroup;
        [SerializeField] GameObject DelayGroup;
        [SerializeField] GameObject MonsterGroup;
        [SerializeField] GameObject GoalGroup;

        public Patten LinePatten;

        public InputField DelayField;
        public InputField[] MonsterField = new InputField[8];

        [SerializeField] Text timeText;

        [SerializeField] Text totalMoneyText;

        [SerializeField] GameObject startCheckButtonOff;
        [SerializeField] GameObject startCheckButtonOn;

        public void Setting(float time, int totalMoney, bool isStart)
        {
            SelectGroup.SetActive(false);

            if (isStart)
            {
                startCheckButtonOff.SetActive(true);
                startCheckButtonOn.SetActive(false);
            }

            switch (LinePatten.Type)
            {
                case PatternType.Empty:
                    SelectGroup.SetActive(true);
                    break;
                case PatternType.Delay:
                    timeText.text = $"해당 딜레이 종료 시점\n{(int)time / 60}:{(time % 60).ToString("00")}";
                    DelayField.text = LinePatten.Delay.ToString();
                    DelayGroup.SetActive(true);
                    break;

                case PatternType.MonsterSpawn:
                    totalMoneyText.text = $"해당 패턴 종료 후 소지 금액 : {string.Format("{0:n0}", Mathf.RoundToInt(totalMoney))}";
                    MonsterGroup.SetActive(true);

                    for (int i = 0; i < 8; i++)
                    {
                        MonsterField[i].text = LinePatten.Monsters[i].ToString();
                    }

                    break;
                case PatternType.Clear:
                    GoalGroup.SetActive(true);
                    break;
            }
        }

        public void StartCheckOn()
        {
            LevelDesigner.instance.StartPattenSelect(PattenIndex);
        }

        public void StartCheckOff()
        {
            LevelDesigner.instance.StartPattenSelect(0);
        }


        public void DestroyLine()
        {
            LevelDesigner.instance.DestroyPatten(PattenIndex);
        }

        public void AddNewLine()
        {
            LevelDesigner.instance.AddNewPattenLine(PattenIndex + 1);
        }


        public void TypeSelect(int Type)
        {
            SelectGroup.SetActive(false);
            switch (Type)
            {
                case 1:
                    DelayGroup.SetActive(true);
                    break;
                case 2:
                    MonsterGroup.SetActive(true);
                    break;
                case 3:
                    GoalGroup.SetActive(true);
                    break;
            }

            LinePatten.Type = (PatternType)Type;

            LevelDesigner.instance.ChangePatten(PattenIndex, LinePatten);
        }

        public void SelectPatten()
        {
            LevelDesigner.instance.SelectPatten(PattenIndex);
        }

        public void ChangeMonsterNumber()
        {
            for (int i = 0; i < 8; i++)
            {

                int result;
                if (int.TryParse(MonsterField[i].text, out result))
                {
                    LinePatten.Monsters[i] = result;
                }
                else
                {
                    Debug.Log(i + "에서 문제가 발생 했습니다. 스트링 값은 " + MonsterField[i].text + "입니다.");
                }
            }
            LevelDesigner.instance.ChangePatten(PattenIndex, LinePatten);
        }

        public void ChangeDelay()
        {
            LinePatten.Delay = float.Parse(DelayField.text);
            LevelDesigner.instance.ChangePatten(PattenIndex, LinePatten);
        }
    }
}
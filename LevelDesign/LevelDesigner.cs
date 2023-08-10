using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

namespace LevelDesign
{
    // 레벨 디자인을 구현한다.
    public class LevelDesigner : MonoBehaviour
    {
        public static LevelDesigner instance;

        [SerializeField] GameObject _difficultyStampButtonPrefab;
        [SerializeField] GameObject _difficultyStampButtonParent;
        [SerializeField] Sprite[] feelSprites = new Sprite[3];
        [SerializeField] GameObject pattenLineParent;
        [SerializeField] GameObject pattenLinePrifab;
        [SerializeField] RectTransform scrollContent;
        [SerializeField] Text[] pazeButtonTexts = new Text[5];
        [SerializeField] InputField fileNameInputField;
        [SerializeField] InputField[] monsterMoveSpeedInputField = new InputField[8];
        [SerializeField] InputField[] monsterHpInputField = new InputField[8];
        [SerializeField] InputField[] monsterMoneyInputField = new InputField[8];
        [SerializeField] InputField[] itemPriceMoneyInputField = new InputField[11];


        float totalTime;
        int totalMoney;
        int Page = 0;
        int selectedPattenIndex = 0;

        Patten _copiedPattern;


        void Start()
        {
            instance = this;

            MonsterBalance monsterBalance = new MonsterBalance();

            // 몬스터 밸런스 배열 초기화.
            for (int i = 0; i < LevelDataReader.instance.LevelData.MonsterBalances.Length; i++)
            {
                LevelDataReader.instance.LevelData.MonsterBalances[i] = monsterBalance;
            }

            MonsterBalanceUIInitalize();
            ItemBalanceUIInitalize();
            PazeButtonTextsInitalize();
            DifficultyStampButtonsSpawn();
        }


        // 레벨 데이터를 저장한다.
        public void SaveLevelData()
        {
            string levelDataToString = JsonConvert.SerializeObject(LevelDataReader.instance.LevelData, Formatting.Indented);
            File.WriteAllText($"{Application.persistentDataPath}/{fileNameInputField.text}.json", levelDataToString);
        }

        // 레벨 데이터를 불러온다.
        public void LoadLevelData()
        {
            string levelDataToString = File.ReadAllText($"{Application.persistentDataPath}/{fileNameInputField.text}.json");
            LevelDataReader.instance.LevelData = JsonConvert.DeserializeObject<LevelData>(levelDataToString);

            // 첫 페이지로 되돌린다.
            Page = 0;
            LevelDataReader.instance.FileName = fileNameInputField.text;

            PattenLineUIInitalize();
            MonsterBalanceUIInitalize();
            ItemBalanceUIInitalize();
            PazeButtonTextsInitalize();
            DifficultyStampButtonsSpawn();

            // 스크롤을 맨 위로 되돌린다.
            scrollContent.position = new Vector3(scrollContent.position.x, -586.6616f, 0);
        }






        // 새로운 라인 생성. (패턴 버튼 및 + 버튼 클릭 시 실행. 인덱스는 해당 패턴 버튼에 인덱스로 호출).
        public void AddNewPattenLine(int index)
        {
            var newPatten = new Patten();
            LevelDataReader.instance.LevelData.Pattens.Insert(index, newPatten);
            PattenLineUIInitalize();
        }

        // 패턴을 삭제한다.
        public void DestroyPatten(int Index)
        {
            LevelDataReader.instance.LevelData.Pattens.RemoveAt(Index);
            PattenLineUIInitalize();
        }

        // 패턴 인스턴스를 매개변수로 변경한다.
        public void ChangePatten(int pattenIndex, Patten patten)
        {
            LevelDataReader.instance.LevelData.Pattens[pattenIndex] = patten;
            PattenLineUIInitalize();
        }

        // 컨트롤 + C , V 로 복사 붙혀넣기를 수행한다.
        void Update()
        {
            if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.LeftCommand))
            {
                CopyPatten();
            }

            if (Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.LeftCommand))
            {
                PastePatten();
            }
        }

        // 선택된 패턴을 복사한다.
        public void CopyPatten()
        {
            _copiedPattern = LevelDataReader.instance.LevelData.Pattens[selectedPattenIndex];
        }

        // 패턴을 선택한다.
        public void SelectPatten(int pattenIndex)
        {
            selectedPattenIndex = pattenIndex;
        }

        // 패턴을 붙여넣는다.
        public void PastePatten()
        {
            LevelDataReader.instance.LevelData.Pattens[selectedPattenIndex] = _copiedPattern;
            PattenLineUIInitalize();
        }

        // 보고 있는 페이지의 패턴 라인을 재정의한다.
        public void PattenLineUIInitalize()
        {

            totalTime = 0;
            totalMoney = 0;

            foreach (Transform child in pattenLineParent.transform)
            {
                if (child.gameObject.name != "NewPattenButton")
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < LevelDataReader.instance.LevelData.Pattens.Count; i++)
            {

                Debug.Log("Spawns");

                totalTime += LevelDataReader.instance.LevelData.Pattens[i].Delay;

                if (LevelDataReader.instance.LevelData.Pattens[i].Type == PatternType.MonsterSpawn)
                {
                    for (int ii = 0; ii < 8; ii++)
                    {
                        totalMoney += LevelDataReader.instance.LevelData.Pattens[i].Monsters[ii] * LevelDataReader.instance.LevelData.MonsterBalances[ii].Money;
                    }
                }

                var min = Page * 10;
                var max = min + 10;


                if (i >= min && i < max)
                {
                    GameObject newButton = Instantiate(pattenLinePrifab);
                    newButton.GetComponent<RectTransform>().SetParent(pattenLineParent.transform);
                    newButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    newButton.GetComponent<PattenLineScript>().PattenIndex = i;
                    newButton.GetComponent<PattenLineScript>().LinePatten = LevelDataReader.instance.LevelData.Pattens[i];

                    if (i == LevelDataReader.instance.StartPattenIndex)
                    {
                        newButton.GetComponent<PattenLineScript>().Setting(totalTime, totalMoney, true);
                    }
                    else
                    {
                        newButton.GetComponent<PattenLineScript>().Setting(totalTime, totalMoney, false);
                    }

                }
            }
            _difficultyStampButtonParent.GetComponent<RectTransform>().sizeDelta = new Vector2(totalTime * 20, 70);
        }

        // 몬스터 밸런스를 조정하는 패널의 인풋 필드 텍스트를 데이터로 초기화한다.
        public void MonsterBalanceUIInitalize()
        {
            for (int i = 0; i < 8; i++)
            {
                monsterHpInputField[i].text = LevelDataReader.instance.LevelData.MonsterBalances[i].MaxHp.ToString();
                monsterMoveSpeedInputField[i].text = LevelDataReader.instance.LevelData.MonsterBalances[i].Speed.ToString();
                monsterMoneyInputField[i].text = LevelDataReader.instance.LevelData.MonsterBalances[i].Money.ToString();
            }
        }

        // 아이템 밸런스를 조정하는 패널의 인풋 필드 텍스트를 데이터로 초기화한다.
        public void ItemBalanceUIInitalize()
        {
            for (int i = 0; i < 11; i++)
            {
                itemPriceMoneyInputField[i].text = LevelDataReader.instance.LevelData.ItemPrices[i].ToString();
            }
        }

        // 느낌을 표시하는 버튼을 생성한다.
        public void DifficultyStampButtonsSpawn()
        {
            foreach (Transform child in _difficultyStampButtonParent.transform)
            {
                Destroy(child.gameObject);
            }

            float sum = 0;
            for (int i = 0; i < LevelDataReader.instance.LevelData.DifficultyStamp.Count; i++)
            {
                GameObject newButton = Instantiate(_difficultyStampButtonPrefab);
                newButton.GetComponent<Image>().sprite = feelSprites[LevelDataReader.instance.LevelData.DifficultyStamp[i].Difficulty];
                newButton.GetComponent<RectTransform>().SetParent(_difficultyStampButtonParent.transform);
                newButton.GetComponent<difficultyStampButton>().StampIndex = i;
                newButton.GetComponent<difficultyStampButton>().ShowTime(LevelDataReader.instance.LevelData.DifficultyStamp[i].TimeStamp);
                sum = LevelDataReader.instance.LevelData.DifficultyStamp[i].TimeStamp / totalTime;
                newButton.GetComponent<RectTransform>().localPosition = new Vector3((sum * _difficultyStampButtonParent.GetComponent<RectTransform>().rect.width) - (_difficultyStampButtonParent.GetComponent<RectTransform>().rect.width / 2), 0, 0);
            }
        }

        // 느낌을 제거한다.
        public void DestroyStamp(int Index)
        {
            LevelDataReader.instance.LevelData.DifficultyStamp.RemoveAt(Index);
            DifficultyStampButtonsSpawn();
        }

        // 느낌을 전체 제거한다.
        public void ClearStamp()
        {
            LevelDataReader.instance.LevelData.DifficultyStamp = new List<DifficultyStamp>();
            DifficultyStampButtonsSpawn();
        }


        // 몬스터 밸런스와 아이템 밸런스를 적용한다.
        public void ApplyBalance()
        {
            // 몬스터 밸런스 적용.
            for (int i = 0; i < 8; i++)
            {

                float result;
                if (float.TryParse(monsterHpInputField[i].text, out result))
                {
                    LevelDataReader.instance.LevelData.MonsterBalances[i].MaxHp = (int)result;
                }
                else
                {
                    Debug.Log(i + "에서 문제가 발생 했습니다. 스트링 값은 " + monsterHpInputField[i].text + "입니다.");
                }
                if (float.TryParse(monsterMoneyInputField[i].text, out result))
                {
                    LevelDataReader.instance.LevelData.MonsterBalances[i].Money = (int)result;
                }
                else
                {
                    Debug.Log(i + "에서 문제가 발생 했습니다. 스트링 값은 " + monsterHpInputField[i].text + "입니다.");
                }
                if (float.TryParse(monsterMoveSpeedInputField[i].text, out result))
                {
                    LevelDataReader.instance.LevelData.MonsterBalances[i].Speed = result;
                }
                else
                {
                    Debug.Log(i + "에서 문제가 발생 했습니다. 스트링 값은 " + monsterMoveSpeedInputField[i].text + "입니다.");
                }
            }

            // 아이템 밸런스 적용.
            for (int i = 0; i < 11; i++)
            {

                int result;
                if (int.TryParse(itemPriceMoneyInputField[i].text, out result))
                {
                    LevelDataReader.instance.LevelData.ItemPrices[i] = result;
                }
                else
                {
                    Debug.Log(i + "에서 문제가 발생 했습니다. 스트링 값은 " + itemPriceMoneyInputField[i].text + "입니다.");
                }
            }

            PattenLineUIInitalize();
        }

        // 페이지 버튼 텍스트 초기화.
        public void PazeButtonTextsInitalize()
        {
            pazeButtonTexts[0].text = "";
            pazeButtonTexts[1].text = "";

            if (Page - 1 >= 1)
            {
                pazeButtonTexts[0].text = (Page - 1).ToString();
            }

            if (Page >= 1)
            {
                pazeButtonTexts[1].text = (Page).ToString();
            }

            pazeButtonTexts[2].text = (Page + 1).ToString();
            pazeButtonTexts[3].text = (Page + 2).ToString();
            pazeButtonTexts[4].text = (Page + 3).ToString();
        }

        // 페이지 변경.
        public void MovePage(int pazeOffset)
        {
            if (Page + pazeOffset < 0)
            {
                return;
            }

            Page += pazeOffset;


            scrollContent.position = new Vector3(scrollContent.position.x, -586.6616f, 0);
            PazeButtonTextsInitalize();
            PattenLineUIInitalize();
        }

        // 테스트 시작지점을 선택한다.
        public void StartPattenSelect(int pattenIndex)
        {
            LevelDataReader.instance.StartPattenIndex = pattenIndex;
            PattenLineUIInitalize();
        }

        // 테스트를 시작한다.
        public void GoToTest()
        {
            SceneManager.LoadScene("GameScene");
        }

        // 타이틀 씬으로 돌아간다.
        public void GotoTitle()
        {
            SceneManager.LoadScene("TitleScene");
        }

    }
}
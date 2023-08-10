using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Firebase.Analytics;

namespace UserData
{
    //선택된 캐릭터, 보유 캐릭터, 점수 등 유저의 데이터 제어를 구현.
    public class UserDataManager : MonoBehaviour
    {
        public static UserDataManager instance;
        public int BestScore = 0;
        public int SelectedCharacter = 0;
        public bool[] PossessedCharacters = { true, false, false, false, false, false, false, false, false };
        public bool[] GetCharacter = { false, false, false, false, false, false, false, false, false };

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }

            LoadCharactersData();
            BestScoreLoad();
        }

        // 소지 캐릭터 파일을 저장.
        public void SaveCharactersData()
        {
            string GetCharactersData = JsonConvert.SerializeObject(GetCharacter, Formatting.Indented);
            File.WriteAllText($"{Application.persistentDataPath}/GetCharacters.json", GetCharactersData);
            string possessedCharactersData = JsonConvert.SerializeObject(PossessedCharacters, Formatting.Indented);
            File.WriteAllText($"{Application.persistentDataPath}/PossessedCharacters.json", possessedCharactersData);
        }

        // 소지 캐릭터 파일을 로드.
        public void LoadCharactersData()
        {
            SelectedCharacter = PlayerPrefs.GetInt("SelectedCharacter");
            FileInfo fi = new FileInfo($"{Application.persistentDataPath}/PossessedCharacters.json");

            // 파일이 없는 경우 신규 파일 생성.
            if (!fi.Exists)
            {
                string possessedCharactersDataTemp = JsonConvert.SerializeObject(PossessedCharacters, Formatting.Indented);
                File.WriteAllText($"{Application.persistentDataPath}/PossessedCharacters.json", possessedCharactersDataTemp);
            }


            string possessedCharactersData = File.ReadAllText($"{Application.persistentDataPath}/PossessedCharacters.json");
            PossessedCharacters = JsonConvert.DeserializeObject<bool[]>(possessedCharactersData);


            FileInfo fi2 = new FileInfo($"{Application.persistentDataPath}/GetCharacters.json");

            // 파일이 없는 경우 신규 파일 생성.
            if (!fi2.Exists)
            {
                string GetCharactersDataTemp = JsonConvert.SerializeObject(GetCharacter, Formatting.Indented);
                File.WriteAllText($"{Application.persistentDataPath}/GetCharacters.json", GetCharactersDataTemp);
                return;
            }

            string GetCharactersData = File.ReadAllText($"{Application.persistentDataPath}/GetCharacters.json");
            GetCharacter = JsonConvert.DeserializeObject<bool[]>(GetCharactersData);

        }

        // 캐릭터를 선택하여 선택된 캐릭터에 적용하고 저장.
        public void SelectCharacter(int characterNumber)
        {
            SelectedCharacter = characterNumber;
            PlayerPrefs.SetInt("SelectedCharacter", SelectedCharacter);
            PlayerPrefs.Save();
        }

        // 캐릭터 해금.
        public void UnLockCharacter(int UnLockCharacterNumber)
        {
            if (PossessedCharacters[UnLockCharacterNumber]) return;

            FirebaseAnalytics.LogEvent("Character Unlock", "CharacterType", UnLockCharacterNumber);

            GetCharacter[UnLockCharacterNumber] = true;
            PossessedCharacters[UnLockCharacterNumber] = true;

            // 4번을 제외한 전체 캐릭터가 해금된 경우 4번 캐릭터 해금.
            if (PossessedCharacters[1]
            && PossessedCharacters[2]
            && PossessedCharacters[3]
            && PossessedCharacters[5]
            && !PossessedCharacters[4])
            {
                UnLockCharacter(4);
            }

            // 캐릭터 정보를 저장.
            SaveCharactersData();
        }

        // 최고 기록 불러오기.
        public void BestScoreLoad()
        {
            BestScore = PlayerPrefs.GetInt("BestScore");
        }

        // 최고기록 저장.
        public void BestScoreSave()
        {
            int PrevBestScore = PlayerPrefs.GetInt("BestScore");
            if (ScoreManager.instance.Score > PrevBestScore)
            {
                PrevBestScore = ScoreManager.instance.Score;
                BestScore = ScoreManager.instance.Score;
            }

            PlayerPrefs.SetInt("BestScore", PrevBestScore);
            PlayerPrefs.Save();
        }

        // 노말모드 클리어 정보 저장.
        public void ClearNormalModeSave()
        {
            // 1번 캐릭터가 없는 경우 1번 캐릭터를 획득.
            if (!PossessedCharacters[1])
            {
                UnLockCharacter(1);
            }

            // 클리어 횟수 저장.
            int ClearNumber = PlayerPrefs.GetInt("ClearNumber");

            PlayerPrefs.SetInt("ClearNumber", ClearNumber + 1);
            PlayerPrefs.Save();
        }

        // 하드모드 클리어 정보 저장.
        public void ClearHardModeSave()
        {
            // 2번 캐릭터가 없는 경우 1번 캐릭터를 획득.
            if (!PossessedCharacters[2])
            {
                UnLockCharacter(2);
            }

            // 클리어 횟수 저장.
            int ClearHardNumber = PlayerPrefs.GetInt("ClearHardNumber");

            PlayerPrefs.SetInt("ClearHardNumber", ClearHardNumber + 1);
            PlayerPrefs.Save();
        }

        // 신규 캐릭터 획득 정보를 표시하지않게 변경.
        public void ClearGetCharacterData()
        {
            GetCharacter[0] = false;
            GetCharacter[1] = false;
            GetCharacter[2] = false;
            GetCharacter[3] = false;
            GetCharacter[4] = false;
            GetCharacter[5] = false;
            GetCharacter[6] = false;
            GetCharacter[7] = false;
            GetCharacter[8] = false;
            string GetCharactersData = JsonConvert.SerializeObject(GetCharacter, Formatting.Indented);
            File.WriteAllText($"{Application.persistentDataPath}/GetCharacters.json", GetCharactersData);
        }
    }

}

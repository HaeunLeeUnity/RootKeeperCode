using UnityEngine;

namespace UserData
{
    // 게임에 처음 접속했을 때 튜토리얼을 표시함.
    public class FirstTimeTipManager : MonoBehaviour
    {
        [SerializeField]
        GameObject tipPanel;
        void Start()
        {
            // 게임 플레이 횟수 확인.
            int GameNumber = PlayerPrefs.GetInt("GameNumber");

            // 플레이 횟수가 0 번이면 튜토리얼을 표시.
            if (GameNumber == 0)
            {
                Time.timeScale = 0;
                tipPanel.SetActive(true);
            }

            PlayerPrefs.SetInt("GameNumber", GameNumber + 1);

            // 플레이 횟수가 29 번 이상이라면 캐릭터 획득.
            if (GameNumber >= 29 && !UserDataManager.instance.PossessedCharacters[3])
            {
                UserDataManager.instance.UnLockCharacter(3);
            }

            PlayerPrefs.Save();

        }
    }
}

using System.Collections;
using UnityEngine;
using UserData;
using GameNTT;

// 리워드 광고를 보고 획득할 수 있는 보상 획득 (부활, 초기 지원금)을 구현.
namespace Item
{
    public class RewardManager : MonoBehaviour
    {
        bool isReborned = false;
        bool isGetRewardOnStart = false;
        [SerializeField] GameObject slotMachinePopup;

        [SerializeField] GameObject rebornPopup;
        [SerializeField] private GameObject gameoverPopup;

        public bool IsReborned
        {
            get { return isReborned; }
        }
        public bool IsGetRewardOnStart
        {
            get { return isGetRewardOnStart; }
        }

        private void Start()
        {
            StartCoroutine(SlotPopupOpen());
        }

        // 게임 시작 후 광고를 보고 초기 지원금을 받을 것인지를 묻는 팝업을 생성.
        IEnumerator SlotPopupOpen()
        {
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 0;
            slotMachinePopup.SetActive(true);
        }

        // 게임 시작 후 리워드 광고를 볼 경우 실행.
        public void OnClickShowReword()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GoogleAdsManager.instance.ShowADReward();
            }
            else
            {
                Time.timeScale = 1;
            }
            isGetRewardOnStart = true;
            InventoryManager.instance.CalculateMoney(300);
            HidePopup();
        }

        // 게임 시작 후 리워드 광고를 시청했거나 거절한 경우 실행. 팝업을 닫음.
        public void HidePopup()
        {
            slotMachinePopup.SetActive(false);
        }

        // 인삼이 죽었을 시 실행. 부활 기회가 있는 지 확인하고 있으면 부활할 것 인지 묻는 팝업을 활성화.
        public void DeadGinseng()
        {
            if (!isReborned)
            {
                rebornPopup.SetActive(true);
            }
            else
            {
                GameEnd();
            }
        }

        // 부활을 선택했을 시 실행. 리워드 광고를 표시하고 인삼을 부활시킴.
        public void OnClickReborn()
        {
            isReborned = true;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                GoogleAdsManager.instance.ShowADReward();
            else
            {
                Time.timeScale = 1;
            }
            GinsengScript.instance.Reborn();
            rebornPopup.SetActive(false);
        }

        // 부활을 거절했거나 부활 기회가 없을 때 사망한 경우 실행.
        public void GameEnd()
        {
            rebornPopup.SetActive(false);
            gameoverPopup.SetActive(true);
            UserDataManager.instance.BestScoreSave();
            StartCoroutine(GameOverAndNextGame());
        }

        IEnumerator GameOverAndNextGame()
        {
            yield return new WaitForSecondsRealtime(1.5f);
            GameResultManager.instance.OnShowResult();
        }
    }
}

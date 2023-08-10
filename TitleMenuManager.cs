using UnityEngine;
using UnityEngine.UI;
using UserData;

// 게임 타이틀 화면에 애니메이션 제어를 구현.
// 게임 모드 선택, 앱 추적 구현.
namespace Title
{
    public class TitleMenuManager : MonoBehaviour
    {
        [SerializeField] Text bestScoreText;

        Animator animator;
        [SerializeField] AudioSource bgmManager;

        bool isNormalClear = false;


        [SerializeField] GameObject newbieGameStart;
        [SerializeField] GameObject modeSelectParent;
        private void Start()
        {
            animator = GetComponent<Animator>();
            BestScoreLoad();
            Application.targetFrameRate = 120;
        }

        public void AnimationSkip()
        {
            ShowGameStart();
            BGMStart();
            animator.SetTrigger("Skip");
        }

        public void AnimationSoundPlay()
        {
            SoundManager.instance.PlayWeaponChange();
        }

        // BGM 이 시작되는 타이밍에 앱 추적을 허용할 것인지 묻는 모달 표시.
        public void BGMStart()
        {
#if UNITY_IOS
        IDFARequest.instance.PleaseGiveMeIDFA();
#endif
            bgmManager.Play();

        }

        // 노말 모드를 클리어 하면 하드모드를 선택할 수 있음.
        public void ShowGameStart()
        {
            if (isNormalClear)
            {
                modeSelectParent.SetActive(true);
            }
            else
            {
                newbieGameStart.SetActive(true);
            }
        }

        // 최고 점수를 표시.
        public void BestScoreLoad()
        {
            bestScoreText.text = $"BEST: {string.Format("{0:n0}", UserDataManager.instance.BestScore)}";


            if (0 < PlayerPrefs.GetInt("ClearNumber"))
            {
                bestScoreText.text = $"BEST: {string.Format("{0:n0}", UserDataManager.instance.BestScore)} <color=yellow>CLEAR</color>";
                isNormalClear = true;
            }

            if (0 < PlayerPrefs.GetInt("ClearHardNumber"))
            {
                bestScoreText.text = $"BEST: {string.Format("{0:n0}", UserDataManager.instance.BestScore)} <color=orange>CLEAR</color>";
                isNormalClear = true;
            }
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Item;
using UserData;
using GameNTT;
using Firebase.Analytics;

// 게임 결과를 표시함. 게임 결과에는 진행률, 게임모드, 점수, 아이템 상황 등이 표시됨.
// 게임 결과를 SNS 등의 네이티브 공유할 수 있음.
public class GameResultManager : MonoBehaviour
{

    public static GameResultManager instance;
    [SerializeField] Text clearPercentText;
    [SerializeField] Text pointText;
    [SerializeField] Text attackText;
    [SerializeField] Text attackSpeedText;
    [SerializeField] Image specialWeaponImage;
    [SerializeField] Image[] catImages;
    [SerializeField] Text potionText;
    [SerializeField] Text beerText;
    [SerializeField] Text gameModeText;
    [SerializeField] Sprite[] weaponIcons;

    [SerializeField] Canvas normalCanvas;
    [SerializeField] Canvas resultCanvas;
    [SerializeField] Animator animator;

    [SerializeField] EnemySpawnManager _enemySpawnManager;
    [SerializeField] WeaponController _weaponController;
    [SerializeField] RewardManager _rewardManager;

    Texture2D screenTex;


    string shareString = "";
    private void Start()
    {
        instance = this;
    }

    // 게임 결과 화면을 출력함.
    public void OnShowResult()
    {
        // 기존의 UI 캔버스를 비활성화 하고 Result 전용 캔버스를 활성화함.
        normalCanvas.enabled = false;
        resultCanvas.enabled = true;

        //광고 애니메이터를 활성화함.
        animator.enabled = true;

        // 진행도를 계산하여 표시함. 
        // (CurrentTime) 15 / (ClearTime) 50 = 0.3 * 100 = (진행도) 30
        float Persent = _enemySpawnManager.CurrentTime;
        Persent /= _enemySpawnManager.ClearTime;
        Persent *= 100;

        // 게임 모드 출력. 및 공유 메세지에 게임 모드 더함.
        if (LevelDataReader.instance.nowGameMode == GameMode.Hard)
        {
            gameModeText.text = "<color=red>HARD</color>";
            shareString = "Hard Mode ";
        }
        else if (LevelDataReader.instance.nowGameMode == GameMode.Normal)
        {
            gameModeText.text = "NORMAL";
            shareString = "Normal Mode ";
        }
        else
        {
            gameModeText.text = "QUICK";
            shareString = "Quick Mode ";
        }

        // 진행도 소수점 한 자릿수 까지 출력.
        clearPercentText.text = $"{Persent.ToString("N1")}%";

        // 공유 메세지에 진행도 더함.
        shareString += clearPercentText.text;

        // 진행도가 100% 일 경우 소수점을 제외하기 위해 따로 표기를 변경.
        if (Persent == 100)
        {
            clearPercentText.text = "100%";
            // 클리어 시 진행율 텍스트를 초록색으로 표시
            clearPercentText.color = new Vector4(0.2783019f, 1, 0.4521211f, 1);
            // 공유 메세지에 클리어 여부를 더함.
            shareString += " Clear!";
        }
        else
        {
            shareString += " Fail...";
        }

        // 스코어 표시.
        pointText.text = $"{string.Format("{0:n0}", Mathf.RoundToInt(ScoreManager.instance.Score))}";

        // 공격력 및 공격속도 표시
        attackText.text = $"{100 + PlayerScript.instance.AttackDamageLevel * 10}%";
        attackSpeedText.text = $"{100 + PlayerScript.instance.AttackSpeedLevel * 10}%";

        // 고양이 갯수 표시
        for (int i = 0; i < StoreManager.instance.CatNumber; i++)
        {
            catImages[i].color = new Vector4(1, 1, 1, 1);
        }

        // 히든 무기가 없는 경우
        if (InventoryManager.instance.SpecialWeaponNumber == 0)
        {
            specialWeaponImage.color = new Vector4(0, 0, 0, 0);
        }
        // 히든 무기가 있는 경우.
        else
        {
            // 히든 무기 아이콘 초기화.
            specialWeaponImage.sprite = weaponIcons[InventoryManager.instance.SpecialWeaponNumber - 6];
        }

        // 소비 아이템 사용 횟수 표시.
        potionText.text = StoreManager.instance.PotionUseNumber.ToString();
        beerText.text = StoreManager.instance.BeerUseNumber.ToString();

        // 파이어베이스 애널리틱스에 로그를 보냄.
        var parameterPersent = new Parameter("progress", Persent);
        var parameterGameMode = new Parameter("GameMode", LevelDataReader.instance.nowGameMode.ToString());
        var parameterStartReward = new Parameter("StartReward", _rewardManager.IsGetRewardOnStart.ToString());
        var parameterReborned = new Parameter("Reborned", _rewardManager.IsReborned.ToString());
        var parameterWeapon = new Parameter("Weapon", InventoryManager.instance.SpecialWeaponNumber);

        FirebaseAnalytics.LogEvent("Game Result", new Parameter[] { parameterPersent, parameterGameMode, parameterStartReward, parameterReborned, parameterWeapon });
    }

    // 네이티브 공유.
    public void Share()
    {
        new NativeShare().AddFile(screenTex, "GameResult.png")
                .SetText(shareString + "\n#RootKeeper\n").SetUrl("https://rootkeeperapp.page.link/RtQw")
                .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                .Share();
    }

    public void OnClickExit()
    {
        // 나가기 전 광고 표시.        
        if (GoogleAdsManager.instance != null && Application.platform == RuntimePlatform.Android ||
        GoogleAdsManager.instance != null && Application.platform == RuntimePlatform.IPhonePlayer)
            GoogleAdsManager.instance.ShowADInterstitial();
        else
        {
            Time.timeScale = 1;
        }
        // 광고 종료 후 나가기.
        StartCoroutine(Exit());
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.02f);
        SceneManager.LoadScene("TitleScene");
    }

    // 공유 이미지 캡쳐
    public void ScreenShot()
    {
        StartCoroutine("ScreenShotCaptureCo");
    }

    // 광고 이미지 캠쳐 코루틴.
    IEnumerator ScreenShotCaptureCo()
    {
        yield return new WaitForEndOfFrame();
        screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);
        screenTex.ReadPixels(area, 0, 0);
        screenTex.Apply();
        Sprite sprite = Sprite.Create(screenTex, area, Vector2.one * 0.5f);
    }
}

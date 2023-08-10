using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;

// 구글 애드몹 전면광고 및 리워드 광고 구현.
public class GoogleAdsManager : MonoBehaviour
{
    public static GoogleAdsManager instance;

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    [SerializeField] string IOSUnitId = "ca-app-pub-9073117645123680/3981749058";
    [SerializeField] string AndroidUnitId = "ca-app-pub-9073117645123680/4162590761";
    [SerializeField] string EditerUnitId = "unexpected_platform";



    [SerializeField] string IOSRewordUnitId = "ca-app-pub-9073117645123680/4287449931";
    [SerializeField] string AndroidRewordUnitId = "ca-app-pub-9073117645123680/8274510997";
    [SerializeField] string EditerRewordUnitId = "unused";

    string adUnitId;
    string adRewardUnitId;

    private void Start()
    {
        if (instance == null)
        {
            Debug.Log("싱글톤 초기화.");
            instance = this;
            DontDestroyOnLoad(gameObject);

            MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
        }
        else
        {
            Destroy(gameObject);
            return;
        }

#if UNITY_ANDROID

        Debug.Log("안드로이드 광고 유닛 ID로 변경.");
        adUnitId = AndroidUnitId;
        adRewardUnitId = AndroidRewordUnitId;
#elif UNITY_IPHONE
        Debug.Log("애플 광고 유닛 ID로 변경.");
        adUnitId = IOSUnitId;
        adRewardUnitId = IOSRewordUnitId;
#else
        Debug.Log("에디터 광고 유닛 ID 로 변경");
        adUnitId = EditerUnitId;
        adRewardUnitId = EditerRewordUnitId;
#endif

        StartCoroutine(ADRequestCO());
    }

    IEnumerator ADRequestCO()
    {
        yield return new WaitForSecondsRealtime(5);

#if UNITY_IOS
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            if (IDFARequest.instance.IsDetermined())
            {
                Debug.Log("IDFA 요청 여부 확인됨.");
            break;
            }
        }
#endif
        RequestInterstitial();
        RequestReward();
    }

    IEnumerator RequestInterstitialFailedCo()
    {
        yield return new WaitForSecondsRealtime(30);
        RequestInterstitial();
    }

    IEnumerator RequestRewardFailedCo()
    {
        yield return new WaitForSecondsRealtime(30);
        RequestReward();
    }




    public void RequestInterstitial()
    {
        if (PremiumMemberManager.instance != null)
        {
            if (PremiumMemberManager.instance.IsPremium) return;
        }

        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
            this.interstitial = null;
        }

        this.interstitial = new InterstitialAd(adUnitId);

        this.interstitial.OnAdClosed += OnExitInierstitialAd;
        this.interstitial.OnAdFailedToShow += OnFailedInterstitialShow;
        this.interstitial.OnAdFailedToLoad += OnFailedInterstitialRequest;

        AdRequest request = new AdRequest.Builder().Build();

        this.interstitial.LoadAd(request);
    }


    public void OnFailedInterstitialRequest(object sender, EventArgs args)
    {
        StartCoroutine(RequestInterstitialFailedCo());
    }
    public void OnFailedInterstitialShow(object sender, EventArgs args)
    {
        Debug.Log("전면 광고 표시 실패");
    }


    public void ShowADInterstitial()
    {
        if (PremiumMemberManager.instance != null)
        {
            if (this.interstitial == null)
            {
                Debug.Log("로드된 전면 광고가 없음 광고를 요청함");
                BGMManager.instance.Unpause();
                Time.timeScale = 1;
                interstitial.Destroy();
                RequestInterstitial();
            }
            if (this.interstitial.IsLoaded() && !PremiumMemberManager.instance.IsPremium && 2 < PlayerPrefs.GetInt("GameNumber"))
            {
                Debug.Log("로드된 광고 표시.");

                BGMManager.instance.Pause();
                Time.timeScale = 0;
                this.interstitial.Show();
            }
            else
            {
                Debug.Log("로드된 광고 없음 또는 광고 제거 판을 구매한 유저이거나 게임 횟수가 2회 미만임. ");
                BGMManager.instance.Unpause();
                Time.timeScale = 1;
                interstitial.Destroy();
                RequestInterstitial();
            }
        }
        else
        {
            BGMManager.instance.Unpause();
            Time.timeScale = 1;
            interstitial.Destroy();
            RequestInterstitial();
        }

    }

    public void OnExitInierstitialAd(object sender, EventArgs args)
    {
        BGMManager.instance.Unpause();
        Time.timeScale = 1;
        interstitial.Destroy();
        RequestInterstitial();
        Debug.Log("광고 종료됨.");
    }




    public void RequestReward()
    {
        if (PremiumMemberManager.instance != null)
        {
            if (PremiumMemberManager.instance.IsPremium) return;
        }

        if (this.rewardedAd != null)
        {
            this.rewardedAd.Destroy();
            this.rewardedAd = null;
        }

        Debug.Log("리워드 광고 리퀘스트.");
        this.rewardedAd = new RewardedAd(adRewardUnitId);

        this.rewardedAd.OnAdClosed += OnExitReward;
        this.rewardedAd.OnAdFailedToShow += OnFailedRewardShow;
        this.rewardedAd.OnAdFailedToLoad += OnFailedRewardRequest;

        AdRequest request = new AdRequest.Builder().Build();
        // Load the Reward with the request.
        this.rewardedAd.LoadAd(request);
    }

    public bool IsRewordLoaded()
    {
        if (this.rewardedAd == null || !this.rewardedAd.IsLoaded())
        {
            return false;
        }
        return true;
    }
    public void OnFailedRewardRequest(object sender, EventArgs args)
    {
        StartCoroutine(RequestRewardFailedCo());
    }
    public void OnFailedRewardShow(object sender, EventArgs args)
    {
        Debug.Log("리워드 광고 표시 실패.");
    }


    public void ShowADReward()
    {
        if (PremiumMemberManager.instance != null || Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
        {
            if (this.rewardedAd == null)
            {
                Debug.Log("로드된 리워드 광고가 없음 광고를 요청함");
                BGMManager.instance.Unpause();
                Time.timeScale = 1;
                rewardedAd.Destroy();
                RequestReward();
            }
            if (this.rewardedAd.IsLoaded() && !PremiumMemberManager.instance.IsPremium)
            {
                Debug.Log("리워드 광고 표시.");

                BGMManager.instance.Pause();
                Time.timeScale = 0;
                this.rewardedAd.Show();
            }
            else
            {
                Debug.Log("리워드 광고 로드 안됨 또는 광고 제거판 유저 이거나 게임 횟수가 2회 미만임. ");
                BGMManager.instance.Unpause();
                Time.timeScale = 1;
                rewardedAd.Destroy();
                RequestReward();
            }
        }
        else
        {
            BGMManager.instance.Unpause();
            Time.timeScale = 1;
            rewardedAd.Destroy();
            RequestReward();
        }

    }

    public void OnExitReward(object sender, EventArgs args)
    {

        BGMManager.instance.Unpause();
        Time.timeScale = 1;
        rewardedAd.Destroy();
        RequestReward();
        Debug.Log("리워드 광고 종료");
    }


}

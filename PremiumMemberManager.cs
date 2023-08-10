using UnityEngine;

// 광고 제거 판을 구매한 경우에 대한 표시.
public class PremiumMemberManager : MonoBehaviour
{
    public static PremiumMemberManager instance;

    public bool _isPremium = false;

    public bool IsPremium
    {
        get { return _isPremium; }
    }

    void Start()
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
    }

}

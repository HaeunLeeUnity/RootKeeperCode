using UnityEngine;
using UnityEngine.Pool;

public class DamageTextEffectManager : MonoBehaviour
{
    // 데미지를 표기하는 텍스트를 생성함.
    // 게임 중 생성된 몬스터와 상호작용 하기 때문에 스태틱 인스턴스화.
    public static DamageTextEffectManager instance;

    [SerializeField] GameObject DamageText;
    [SerializeField] GameObject moneyText;


    IObjectPool<DamageText> _damageTextPool;
    IObjectPool<DamageText> _moneyTextPool;

    private void Start()
    {
        instance = this;

        _damageTextPool = new ObjectPool<DamageText>(CreatedDamageText, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
        _moneyTextPool = new ObjectPool<DamageText>(CreatedMoneyText, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
    }

    DamageText CreatedDamageText()
    {
        var newDamageTextGameObject = Instantiate(DamageText);
        var newDamageText = newDamageTextGameObject.GetComponent<DamageText>();
        newDamageText.Initalize(_damageTextPool);
        return newDamageText;
    }

    DamageText CreatedMoneyText()
    {
        var newMoneyTextGameObject = Instantiate(moneyText);
        var newMoneyText = newMoneyTextGameObject.GetComponent<DamageText>();
        newMoneyText.Initalize(_moneyTextPool);
        return newMoneyText;
    }

    void OnReturnedToPool(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
    }

    void OnTakeFromPool(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    // 데미지를 텍스트를 생성하고 위치를 초기화함.
    public void SpawnDamageText(Vector3 Position, string content)
    {
        var newText = _damageTextPool.Get();
        newText.transform.position = new Vector3(Position.x + Random.Range(-0.8f, 0.8f), Position.y + Random.Range(-0.8f, 0.8f), 0);
        newText.text.text = content;
    }

    // 획득한 돈을 표시하는 텍스트를 생성하고 위치를 초기화함.
    // 돈 텍스트를 생성하고 위치를 초기화함.
    public void SpawnMoneyText(Vector3 Position, string content)
    {
        var newText = _moneyTextPool.Get();
        newText.transform.position = new Vector3(Position.x, Position.y + 1.2f, 0);
        newText.text.text = $"+{content}";
    }
}
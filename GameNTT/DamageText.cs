using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

// 데미지를 표기하는 텍스트.
public class DamageText : MonoBehaviour
{
    public Text text;
    [SerializeField] float delectTime = 0.25f;

    IObjectPool<DamageText> _pool;

    public void Initalize(IObjectPool<DamageText> textPool)
    {
        _pool = textPool;
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(delectTime);
        _pool.Release(this);
    }
}

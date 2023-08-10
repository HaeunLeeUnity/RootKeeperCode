using System.Collections;
using UnityEngine;

namespace GameNTT
{
    // 몬스터 또는 고양이의 공격 콜라이더가 자동으로 활성화됨.
    public class AutoAttack : MonoBehaviour
    {
        [SerializeField] GameObject _attackCollider;
        [SerializeField] float _interval = 2;

        private void OnEnable()
        {
            StartCoroutine("AutoAttackLoop");
        }

        private void OnDisable()
        {
            _attackCollider.SetActive(false);
            StopCoroutine("AutoAttackLoop");
        }

        IEnumerator AutoAttackLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_interval);
                _attackCollider.SetActive(true);
                yield return new WaitForSeconds(0.03f);
                _attackCollider.SetActive(false);
            }
        }
    }
}

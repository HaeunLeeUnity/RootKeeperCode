using System.Collections;
using UnityEngine;
using Item;

namespace GameNTT
{
    // 인삼의 피격, 회복, 부활을 구현
    public class GinsengScript : MonoBehaviour
    {
        // 생성된 몬스터가 추적해야하기 때문에 스태틱 인스턴스화 함.
        public static GinsengScript instance;

        [SerializeField] private GameObject rebornAttackRange;

        int _currentHp = 15;
        bool _isUnbreakable = false;

        [SerializeField] RewardManager _rewardManager;

        [SerializeField] GinsengHPBar _ginsengHpBar;
        [SerializeField] ScreenRedEvent _screenRedEvent;

        void Start()
        {
            instance = this;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_isUnbreakable) return;
            if (col.CompareTag("DamageCollider"))
            {
                if (col.GetComponent<DamageColliderScript>().isPlayer) return;

                _screenRedEvent.ShowRed();
                CameraManager.instance.ShakeLevel2();
                DamageTextEffectManager.instance.SpawnDamageText(transform.position, $"<color=red>1</color>");
                _currentHp--;
                _ginsengHpBar.HpSimbolsInitalize(_currentHp);
                SoundManager.instance.PlayGinseng();

                if (_currentHp <= 0)
                {
                    _isUnbreakable = true;
                    Time.timeScale = 0;
                    _rewardManager.DeadGinseng();
                }
            }
        }

        public void Heal()
        {
            _currentHp = 15;
            _ginsengHpBar.HpSimbolsInitalize(_currentHp);
        }

        public void Reborn()
        {
            _currentHp = 15;
            _ginsengHpBar.HpSimbolsInitalize(_currentHp);
            StartCoroutine(RebornAttack());
        }

        IEnumerator RebornAttack()
        {
            rebornAttackRange.SetActive(true);
            _isUnbreakable = true;
            yield return new WaitForSeconds(0.2f);
            rebornAttackRange.SetActive(false);
            _isUnbreakable = false;
        }
    }
}

using System.Collections;
using UnityEngine;
using UserData;
using Item;

namespace GameNTT
{
    public class MonsterHit : MonoBehaviour
    {
        [SerializeField] Knockback Knockback;
        [SerializeField] bool _isBoss;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Material hitMaterial;
        [SerializeField] Material normalMaterial;
        [SerializeField] Animator animator;
        [SerializeField] AutoAttack _autoAttack;
        IMonster _monsterController;
        BoxCollider2D _boxCollider2D;
        [SerializeField] ParticleSystem _bloodParticle;

        // 인터페이스를 상속받는 클래스이기 때문에 프로퍼티를 통해 초기화 한다. 
        public IMonster MonsterController
        {
            set
            {
                _monsterController = value;
                Knockback.MonsterController = _monsterController;
                _boxCollider2D = GetComponent<BoxCollider2D>();
            }
        }

        private void OnEnable()
        {
            _autoAttack.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {

            if (col.CompareTag("DamageCollider") && _monsterController.IsAlive)
            {
                if (!col.GetComponent<DamageColliderScript>().isPlayer) return;


                // 데미지 계산. 플레이어 데미지 계수 * 무기 공격력 * 0.8 ~ 1.2.
                float realDamage = col.GetComponent<DamageColliderScript>().Damage * PlayerScript.instance.AttackDamage;
                realDamage *= Random.Range(0.8f, 1.2f);
                // hp 삭감.
                _monsterController.NowHP -= Mathf.RoundToInt(realDamage);

                // 데미지 텍스트 표시.
                DamageTextEffectManager.instance.SpawnDamageText(transform.position, Mathf.RoundToInt(realDamage).ToString());
                // // 피격 소리 출력.
                SoundManager.instance.PlayEnemyHit();

                // // 스코어에 데미지 추가.
                ScoreManager.instance.Score += Mathf.RoundToInt(realDamage);

                // 몬스터 사망 판정.
                if (_monsterController.NowHP <= 0 && _monsterController.IsAlive)
                {
                    Die();
                }

                int KnockbackLevel = col.GetComponent<DamageColliderScript>().AttackType;

                // 전기톱에 해당하는 넉백 특수 넉백 레벨. 넉백 하지 않는다.
                if (KnockbackLevel == 5)
                {
                    CameraManager.instance.ShakeLevel2();
                }
                else
                {
                    // 보스 몬스터의 경우 넉백 레벨이 1 낮게 적용됨.
                    if (_isBoss) KnockbackLevel--;
                    // 넉백 실행.
                    Knockback.KnockBackStart(KnockbackLevel);
                }


                StopCoroutine("HitLightOn");
                StartCoroutine("HitLightOn");
            }
        }

        // 몬스터가 공격 당했을 때 점등하는 효과. 메테리얼을 통해 구현함.
        IEnumerator HitLightOn()
        {
            spriteRenderer.material = hitMaterial;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.material = normalMaterial;
        }


        void Die()
        {
            // 죽을 때 피 효과 재생.
            _bloodParticle.Play();
            // 몬스터 컨트롤러에 상태를 죽었음으로 변경하여 더 움직일 수 없게함.
            _monsterController.IsAlive = false;
            // 더 이상 피격할 수 없도록 콜라이더를 끔.
            _boxCollider2D.enabled = false;
            // 애니메이션 트리거 실행.
            animator.SetBool("Dead", true);
            _autoAttack.enabled = false;
            // 1초 뒤 객체를 삭제하는 코루틴.
            StartCoroutine(DieCoroutine());
            // 돈이 추가 되는 소리를 재생.
            SoundManager.instance.PlayMoneySound();
            // 돈을 추가하고 돈 표시 텍스트를 업데이트함.
            InventoryManager.instance.CalculateMoney(_monsterController.RewordAmount);
            // 돈 텍스트 효과 표시.
            DamageTextEffectManager.instance.SpawnMoneyText(transform.position, _monsterController.RewordAmount.ToString());
        }

        // 1초 뒤 풀로 돌려보냄.
        IEnumerator DieCoroutine()
        {
            yield return new WaitForSeconds(1);
            // 다시 소환됐을 때 피격당할 수 있도록 콜라이더를 켬.
            _boxCollider2D.enabled = true;
            _monsterController.Pool.Release(_monsterController);
        }
    }
}
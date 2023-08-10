using UnityEngine;
using System.Collections;
namespace GameNTT
{
    // 몬스터의 넉백을 구현한다.
    public class Knockback : MonoBehaviour
    {
        IMonster _monsterController;
        Coroutine _nowKnockBackCo;
        Rigidbody2D _rigidbody2D;

        public IMonster MonsterController
        {
            set
            {
                _monsterController = value;
                _rigidbody2D = GetComponent<Rigidbody2D>();
            }
        }

        // 넉백 레벨로 해당하는 넉백을 실행시킴.
        public void KnockBackStart(int knockBackLevel)
        {
            // 상급 몬스터는 넉백 레벨을 감소하여 적용된다.
            if (knockBackLevel <= 0)
            {
                Debug.Log("isReturn");
                return;
            }
            // 넉백이 실행중인 경우 진행중인 넉백을 종료한다.
            else if (_nowKnockBackCo != null)
            {
                Debug.Log("isStop");
                StopCoroutine(_nowKnockBackCo);
            }

            CameraManager.instance.Shake(knockBackLevel);
            Vector2 pushedForce = transform.position;
            pushedForce -= new Vector2(PlayerScript.instance.transform.position.x, PlayerScript.instance.transform.position.y);
            switch (knockBackLevel)
            {
                case 1:
                    _nowKnockBackCo = StartCoroutine(KnockBackLevel1(pushedForce.normalized));
                    break;
                case 2:
                    _nowKnockBackCo = StartCoroutine(KnockBackLevel2(pushedForce.normalized));
                    break;
                case 3:
                    _nowKnockBackCo = StartCoroutine(KnockBackLevel3(pushedForce.normalized));
                    break;
                case 4:
                    _nowKnockBackCo = StartCoroutine(KnockBackLevel4(pushedForce.normalized));
                    break;
            }
        }

        IEnumerator KnockBackLevel1(Vector2 pushedDirection)
        {
            _monsterController.IsKnockBack = true;
            _rigidbody2D.velocity = pushedDirection * 8;
            yield return new WaitForSeconds(0.2f);
            _rigidbody2D.velocity = pushedDirection * 1;
            yield return new WaitForSeconds(0.2f);
            _rigidbody2D.velocity = pushedDirection * 0.3f;
            yield return new WaitForSeconds(0.4f);
            _rigidbody2D.velocity = Vector2.zero; ;
            yield return new WaitForSeconds(0.2f);
            _monsterController.IsKnockBack = false;
        }

        IEnumerator KnockBackLevel2(Vector2 pushedDirection)
        {
            _monsterController.IsKnockBack = true;
            _rigidbody2D.velocity = pushedDirection * 12;
            yield return new WaitForSeconds(0.4f);
            _rigidbody2D.velocity = pushedDirection * 1;
            yield return new WaitForSeconds(0.3f);
            _rigidbody2D.velocity = pushedDirection * 0.3f;
            yield return new WaitForSeconds(0.4f);
            _rigidbody2D.velocity = Vector2.zero; ;
            yield return new WaitForSeconds(0.2f);
            _monsterController.IsKnockBack = false;
        }

        IEnumerator KnockBackLevel3(Vector2 pushedDirection)
        {
            _monsterController.IsKnockBack = true;
            _rigidbody2D.velocity = pushedDirection * 15;
            yield return new WaitForSeconds(0.35f);
            _rigidbody2D.velocity = pushedDirection * 1;
            yield return new WaitForSeconds(0.6f);
            _rigidbody2D.velocity = pushedDirection * 0.3f;
            yield return new WaitForSeconds(0.4f);
            _rigidbody2D.velocity = Vector2.zero; ;
            yield return new WaitForSeconds(0.2f);
            _monsterController.IsKnockBack = false;
        }

        IEnumerator KnockBackLevel4(Vector2 pushedDirection)
        {
            _monsterController.IsKnockBack = true;
            _rigidbody2D.velocity = pushedDirection * 30;
            yield return new WaitForSeconds(0.35f);
            _rigidbody2D.velocity = pushedDirection * 1;
            yield return new WaitForSeconds(0.6f);
            _rigidbody2D.velocity = pushedDirection * 0.3f;
            yield return new WaitForSeconds(0.4f);
            _rigidbody2D.velocity = Vector2.zero; ;
            yield return new WaitForSeconds(0.2f);
            _monsterController.IsKnockBack = false;
        }
    }
}
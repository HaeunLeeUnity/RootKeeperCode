using UnityEngine;

namespace GameNTT
{
    public class ChaseGinseng : MonoBehaviour, MonsterState
    {
        // 인삼을 추격함.
        // 몬스터를 추격하는 것과 플레이어를 추격하는 것을 string 으로 추격 대상의 이름을 받는 등 Find 를 통해 할 수 있지만
        // Find 는 자원 소모가 크기 때문에 ChaseGinseng, ChasePlayer는 스태틱 인스턴스를 통해 접근함.   
        public void Handle()
        {
            Vector2 pushedForce = transform.position;
            pushedForce -= new Vector2(GinsengScript.instance.transform.position.x, GinsengScript.instance.transform.position.y);
            var angle = Mathf.Atan2(pushedForce.normalized.x, pushedForce.normalized.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angle + 180);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace GameNTT
{
    // 고양이 추적 반경을 구현한다. 적이 충돌하면 리스트에 넣고 고양이 컨트롤러에 적이 있음 신호를 보낸다.
    // 고양이가 적을 추격할 때 사용할 수 있도록 0 번째 적을 반환한다.
    public class TargetFindRange : MonoBehaviour
    {
        MonsterTracer _monsterTracer;

        List<GameObject> _enemyInRange = new List<GameObject>();

        public MonsterTracer MonsterTracer
        {
            set { _monsterTracer = value; }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                _enemyInRange.Add(col.gameObject);
                TargetInitalize();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                _enemyInRange.Remove(other.gameObject);
                TargetInitalize();
            }
        }

        void TargetInitalize()
        {
            // 사망한 몬스터 리스트에서 제거.
            while (_enemyInRange.Count > 0)
            {
                if (!_enemyInRange[0].activeInHierarchy)
                {
                    _enemyInRange.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }

            if (_enemyInRange.Count > 0)
            {
                _monsterTracer.Target = _enemyInRange[0];
            }
            else
            {
                _monsterTracer.Target = null;
            }
        }
    }
}
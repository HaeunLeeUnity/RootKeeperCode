using System.Collections;
using UnityEngine;

namespace GameNTT
{
    // 벌의 움직임 및 상태를 구현.
    public class BeeController : AntController
    {
        // AntController 을 상속한다.
        MonsterState _turnAround;

        // 몬스터의 컴포넌트 및 능력치를 초기화한다.
        public override void Initalize(int monsterType)
        {
            base.Initalize(monsterType);
            _turnAround = GetComponent<TurnAround>();
        }

        // 오브젝트 풀에서 초기 생성 되었을 때 몬스터의 상태를 초기화 한다.
        public override void Reset()
        {
            StartCoroutine(BeeDance());
            base.Reset();
        }

        // 스폰되고 2초 뒤 원으로 도는 패턴을 실행한다.
        // 4초 뒤 인삼을 향해 빠르게 돌진한다.
        IEnumerator BeeDance()
        {
            yield return new WaitForSeconds(2);
            _currentState = _turnAround;
            yield return new WaitForSeconds(4);
            _currentState = _chaseGinseng;
            _currentSpeed *= 4;
        }
    }
}
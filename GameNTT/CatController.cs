using UnityEngine;

namespace GameNTT
{
    public class CatController : MonoBehaviour, MonsterTracer
    {
        // 고양이 컨트롤러.
        // 고양이는 플레이어가 거리가 가까워질 때 까지 플레이어를 쫒아가고
        // 몬스터가 주변에 있으면 몬스터를 추적하여 공격한다.
        [SerializeField]
        float _moveSpeed;
        [SerializeField]
        Animator _animator;
        GameObject _target;

        MoveVector _moveVector;
        ChangeAnimatorDirection _changeAnimatorDirection;
        [SerializeField] TargetFindRange _targetFindRange;

        public GameObject Target
        {
            set { _target = value; }
        }

        void Start()
        {
            _moveVector = GetComponent<MoveVector>();
            _changeAnimatorDirection = GetComponent<ChangeAnimatorDirection>();
            _targetFindRange.MonsterTracer = this;
        }

        void Update()
        {
            // 적이 없을 때.
            if (_target == null || _target == PlayerScript.instance.gameObject)
            {
                // 캐릭터를 타깃으로 변경..
                _target = PlayerScript.instance.gameObject;
                // 플레이어 근처에 있을 때.
                if (5 > Vector2.Distance(_target.transform.position, transform.position))
                {
                    _moveVector.Move(_target.transform.position - transform.position, 0);
                }
                // 플레이어가 근처에 없을 때.
                else
                {
                    _moveVector.Move(_target.transform.position - transform.position, _moveSpeed);
                }
            }
            // 적이 있을 때.
            else
            {
                _moveVector.Move(_target.transform.position - transform.position, _moveSpeed);
            }

            _changeAnimatorDirection.ApplyDirection(_target.transform.position - transform.position);

        }
    }
}
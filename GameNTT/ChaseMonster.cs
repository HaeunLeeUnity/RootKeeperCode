using UnityEngine;

namespace GameNTT
{
    // 몬스터를 추격한다. 고양이 총알에서 사용.
    public class ChaseMonster : MonoBehaviour, MonsterTracer
    {
        [SerializeField]
        float _rotationSpeed;

        GameObject _target;
        [SerializeField] TargetFindRange _targetFindRange;

        public GameObject Target
        {
            set
            {
                if (_target == null)
                {
                    _target = value;
                }
            }
        }

        private void OnEnable()
        {
            _target = null;
            _targetFindRange.MonsterTracer = this;
        }

        void Update()
        {
            if (_target != null)
            {
                Vector2 pushedForce = transform.position;
                pushedForce -= new Vector2(_target.transform.position.x, _target.transform.position.y);

                var angle = Mathf.Atan2(pushedForce.normalized.x, pushedForce.normalized.y) * Mathf.Rad2Deg;
                var rotTarget = Quaternion.AngleAxis(-angle + 270, Vector3.forward);

                // 각도를 몬스터 쪽으로 천천히 돌림
                transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, _rotationSpeed * Time.deltaTime);
            }
        }
    }
}
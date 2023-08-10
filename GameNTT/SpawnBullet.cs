using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace GameNTT
{
    // 몬스터와 충돌하면 풀로 돌아가며 오브젝트를 생성하는 총알.
    public class SpawnBullet : MonoBehaviour, IBullet
    {

        [SerializeField]
        float _deadTime = 2;

        [SerializeField]
        float bulletSpeed = 5;

        IObjectPool<IBullet> _pool;

        [SerializeField]
        GameObject _spawnObjectPrefab;
        MoveDirection _moveDirection;

        bool _isHit = false;

        public GameObject GameObjectProperty
        {
            get { return gameObject; }
        }

        private void OnEnable()
        {
            _isHit = false;
            StopCoroutine("DestroyTimer");
            StartCoroutine("DestroyTimer");
        }

        public void Initalize(IObjectPool<IBullet> bulletPool)
        {
            _pool = bulletPool;
            _moveDirection = gameObject.AddComponent<MoveDirection>();
            _moveDirection.Direction = Direction.Right;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy") && !_isHit)
            {
                Instantiate(_spawnObjectPrefab, transform.position, Quaternion.identity);
                _pool.Release(this);
                // 온트리거엔터가 한 프레임에 다수의 몬스터에서 호출되는 것을 방지.
                _isHit = true;
            }
        }

        // 시간이 지나면 충돌하지 않아도 풀로 돌아간다.
        IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(_deadTime);
            _pool.Release(this);
        }

        private void Update()
        {
            _moveDirection.Move(bulletSpeed);
        }
    }
}

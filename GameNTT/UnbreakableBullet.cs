using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace GameNTT
{
    // 시간이 지나야 풀로 돌아가는 총알.
    public class UnbreakableBullet : MonoBehaviour, IBullet
    {
        [SerializeField]
        float _deadTime = 2;
        [SerializeField]
        float bulletSpeed = 5;
        IObjectPool<IBullet> _pool;

        MoveDirection _moveDirection;
        public GameObject GameObjectProperty
        {
            get { return gameObject; }
        }

        private void OnEnable()
        {
            StopCoroutine("DestroyTimer");
            StartCoroutine("DestroyTimer");
        }

        public void Initalize(IObjectPool<IBullet> bulletPool)
        {
            _pool = bulletPool;
            _moveDirection = gameObject.AddComponent<MoveDirection>();
            _moveDirection.Direction = Direction.Right;
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

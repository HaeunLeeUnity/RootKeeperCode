using UnityEngine;
using UnityEngine.Pool;

namespace GameNTT
{
    // 총알 오브젝트 풀링을 구현.
    public class BulletPool : MonoBehaviour
    {
        IObjectPool<IBullet>[] _bulletPools = new IObjectPool<IBullet>[16];

        [SerializeField] GameObject[] _bulletPrefabs = new GameObject[16];

        void Start()
        {
            _bulletPools[1] = new ObjectPool<IBullet>(CreatedNormalSprayBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _bulletPools[2] = new ObjectPool<IBullet>(CreatedNormalSlingShotBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _bulletPools[4] = new ObjectPool<IBullet>(CreatedGoldSprayBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _bulletPools[5] = new ObjectPool<IBullet>(CreatedGoldlSlingShotBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _bulletPools[6] = new ObjectPool<IBullet>(CreatedCatBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _bulletPools[10] = new ObjectPool<IBullet>(CreatedSickleBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _bulletPools[11] = new ObjectPool<IBullet>(CreatedPistolBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _bulletPools[12] = new ObjectPool<IBullet>(CreatedBazukaBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _bulletPools[13] = new ObjectPool<IBullet>(CreatedFireShoesBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _bulletPools[15] = new ObjectPool<IBullet>(CreatedPerfumeBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
        }

        // 해당 코드는 UntiyObject.Pool 에서 생성 콜백에 매개 변수를 넣을 수 없기 때문에 함수를 분리함.
        IBullet CreatedNormalSprayBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[1]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[1]);
            return newBullet;
        }

        IBullet CreatedNormalSlingShotBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[2]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[2]);
            return newBullet;
        }

        IBullet CreatedGoldSprayBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[4]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[4]);
            return newBullet;
        }
        IBullet CreatedGoldlSlingShotBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[5]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[5]);
            return newBullet;
        }

        IBullet CreatedCatBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[6]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[6]);
            return newBullet;
        }
        IBullet CreatedSickleBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[10]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[10]);
            return newBullet;
        }

        IBullet CreatedPistolBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[11]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[11]);
            return newBullet;
        }
        IBullet CreatedBazukaBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[12]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[12]);
            return newBullet;
        }
        IBullet CreatedFireShoesBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[13]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[13]);
            return newBullet;
        }

        IBullet CreatedPerfumeBullet()
        {
            var newBulletGameObject = Instantiate(_bulletPrefabs[15]);
            var newBullet = newBulletGameObject.GetComponent<IBullet>();
            newBullet.Initalize(_bulletPools[15]);
            return newBullet;
        }


        void OnReturnedToPool(IBullet bullet)
        {
            bullet.GameObjectProperty.SetActive(false);
        }
        void OnTakeFromPool(IBullet bullet)
        {
            bullet.GameObjectProperty.SetActive(true);
        }

        void OnDestroyPoolObject(IBullet bullet)
        {
            Destroy(bullet.GameObjectProperty);
        }

        public GameObject SpawnBullet(int weaponType)
        {
            var newbullet = _bulletPools[weaponType].Get();
            newbullet.GameObjectProperty.transform.position = transform.position;
            newbullet.GameObjectProperty.transform.rotation = transform.rotation;
            return newbullet.GameObjectProperty;
        }
    }
}
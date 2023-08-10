using UnityEngine;
using UnityEngine.Pool;

namespace GameNTT
{
    // 몬스터 오브젝트 풀.
    public class MonsterGenerator : MonoBehaviour
    {
        IObjectPool<IMonster>[] _monsterPool = new IObjectPool<IMonster>[8];

        [SerializeField]
        GameObject[] _monsterPrefabs = new GameObject[8];

        void Start()
        {
            _monsterPool[0] = new ObjectPool<IMonster>(CreatedNormalAnt, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _monsterPool[1] = new ObjectPool<IMonster>(CreatedNormalBee, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 1000);
            _monsterPool[2] = new ObjectPool<IMonster>(CreatedRedAnt, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 30, 1000);
            _monsterPool[3] = new ObjectPool<IMonster>(CreatedRedBee, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 30, 1000);
            _monsterPool[4] = new ObjectPool<IMonster>(CreatedGoldAnt, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, 1000);
            _monsterPool[5] = new ObjectPool<IMonster>(CreatedGoldBee, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, 1000);
            _monsterPool[6] = new ObjectPool<IMonster>(CreatedPerpleAnt, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 5, 1000);
            _monsterPool[7] = new ObjectPool<IMonster>(CreatedPerpleBee, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 5, 1000);
        }

        // 해당 코드는 UntiyObject.Pool 에서 생성 콜백에 매개 변수를 넣을 수 없기 때문에 함수를 분리함.
        IMonster CreatedNormalAnt()
        {
            GameObject newMons = Instantiate(_monsterPrefabs[0]);
            var monsterController = newMons.GetComponent<IMonster>();
            monsterController.Pool = _monsterPool[0];
            monsterController.Initalize(0);
            return monsterController;
        }

        IMonster CreatedNormalBee()
        {
            GameObject newMons = Instantiate(_monsterPrefabs[1]);
            var monsterController = newMons.GetComponent<IMonster>();
            monsterController.Pool = _monsterPool[1];
            monsterController.Initalize(1);
            return monsterController;
        }

        IMonster CreatedRedAnt()
        {
            GameObject newMons = Instantiate(_monsterPrefabs[2]);
            var monsterController = newMons.GetComponent<IMonster>();
            monsterController.Pool = _monsterPool[2];
            monsterController.Initalize(2);
            return monsterController;
        }

        IMonster CreatedRedBee()
        {
            GameObject newMons = Instantiate(_monsterPrefabs[3]);
            var monsterController = newMons.GetComponent<IMonster>();
            monsterController.Pool = _monsterPool[3];
            monsterController.Initalize(3);
            return monsterController;
        }

        IMonster CreatedGoldAnt()
        {
            GameObject newMons = Instantiate(_monsterPrefabs[4]);
            var monsterController = newMons.GetComponent<IMonster>();
            monsterController.Pool = _monsterPool[4];
            monsterController.Initalize(4);
            return monsterController;
        }

        IMonster CreatedGoldBee()
        {
            GameObject newMons = Instantiate(_monsterPrefabs[5]);
            var monsterController = newMons.GetComponent<IMonster>();
            monsterController.Pool = _monsterPool[5];
            monsterController.Initalize(5);
            return monsterController;
        }

        IMonster CreatedPerpleAnt()
        {
            GameObject newMons = Instantiate(_monsterPrefabs[6]);
            var monsterController = newMons.GetComponent<IMonster>();
            monsterController.Pool = _monsterPool[6];
            monsterController.Initalize(6);
            return monsterController;
        }

        IMonster CreatedPerpleBee()
        {
            GameObject newMons = Instantiate(_monsterPrefabs[7]);
            var monsterController = newMons.GetComponent<IMonster>();
            monsterController.Pool = _monsterPool[7];
            monsterController.Initalize(7);
            return monsterController;
        }

        void OnReturnedToPool(IMonster monsterController)
        {
            monsterController.GameObjectProperty.SetActive(false);
        }
        void OnTakeFromPool(IMonster monsterController)
        {
            monsterController.GameObjectProperty.SetActive(true);
            monsterController.Reset();
        }

        void OnDestroyPoolObject(IMonster monsterController)
        {
            Destroy(monsterController.GameObjectProperty);
        }

        public void Spawn(int monsterType, Vector2 position)
        {
            var newMonster = _monsterPool[monsterType].Get();
            newMonster.GameObjectProperty.transform.position = position;
        }
    }
}

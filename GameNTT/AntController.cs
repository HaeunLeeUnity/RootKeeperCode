using UnityEngine;
using UnityEngine.Pool;
namespace GameNTT
{
    // 개미의 움직임 및 상태를 구현.
    public class AntController : MonoBehaviour, IMonster
    {
        bool _isKnockBack;
        protected MonsterBalance _monsterBalance;
        [SerializeField] protected MoveDirection _moveDirection;
        [SerializeField] MonsterHit _monsterHit;
        [SerializeField] Animator _animator;
        protected MonsterState _chaseGinseng;
        protected MonsterState _currentState;
        Rigidbody2D _rigidbody2D;
        int hp = 0;
        bool _isAlive = true;

        IObjectPool<IMonster> _pool;

        protected float _currentSpeed;

        public bool IsKnockBack
        {
            set { _isKnockBack = value; }
        }

        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        public int NowHP
        {
            get { return hp; }
            set { hp = value; }
        }

        public int RewordAmount
        {
            get { return _monsterBalance.Money; }
        }

        public GameObject GameObjectProperty
        {
            get { return gameObject; }
        }

        public IObjectPool<IMonster> Pool
        {
            get { return _pool; }
            set { _pool = value; }
        }

        // 몬스터의 컴포넌트 및 능력치를 초기화한다.
        public virtual void Initalize(int monsterType)
        {
            // 인스펙터에서 인터페이스를 상속하는 클래스를 필드로 초기화 할 수 없기 때문에
            // 스크립트에서 생성하고 초기화함.
            _chaseGinseng = gameObject.AddComponent<ChaseGinseng>();
            _currentState = _chaseGinseng;
            _monsterBalance = LevelDataReader.instance.LevelData.MonsterBalances[monsterType];
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _monsterHit.MonsterController = this;
        }

        // 오브젝트 풀에서 초기 생성 되었을 때 몬스터의 상태를 초기화 한다.
        public virtual void Reset()
        {
            hp = _monsterBalance.MaxHp;
            _animator.SetBool("Dead", false);
            _isAlive = true;
            _isKnockBack = false;
            _currentSpeed = _monsterBalance.Speed;
        }

        private void FixedUpdate()
        {

            // 넉백 상태의 경우 움직임 실행 X.
            if (_isKnockBack) return;

            // 사망 상태의 경우 움직임 실행 X. 넉백 중이 아니라면 속도도 0
            if (!_isAlive)
            {
                _rigidbody2D.velocity = Vector2.zero;
                return;
            }

            // Ant 의 경우 다른 조건 없이 무조건 인삼따라가기 상태 실행.
            _currentState.Handle();
            _moveDirection.Move(_currentSpeed);
        }
    }
}
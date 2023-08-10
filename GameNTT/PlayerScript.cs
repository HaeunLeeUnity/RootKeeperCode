using System.Collections;
using UnityEngine;
using UserData;

namespace GameNTT
{
    // 플레이어의 이동, 넉백, 스탯을 구현.
    public class PlayerScript : MonoBehaviour
    {
        int _attackDamageLevel = 0;
        int _attackSpeedLevel = 0;
        float _attackDamage = 1;
        float _attackSpeed = 1;

        public static PlayerScript instance;

        bool _isDirectionFix = false;
        int randomDirectionNumber = 4;


        [SerializeField] private float _slowMoveSpeed = 2;
        [SerializeField] private float _normalMoveSpeed = 5;
        [SerializeField] private float _fastMoveSpeed = 12;

        [SerializeField] private float _moveSpeed = 5;

        [SerializeField] private Animator animator;
        [SerializeField] RuntimeAnimatorController[] characterRuntimeAnimator;


        [SerializeField] GhostTrail _ghostTrail;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] VariableJoystick _variableJoystick;

        [SerializeField] Transform _knockBackPoint;

        [SerializeField] MoveVector _moveVector;
        [SerializeField] ChangeAnimatorDirection _changeAnimatorDirection;
        [SerializeField] WeaponController _weaponController;
        [SerializeField] GameObject _weaponGameObject;


        bool _isDrunk = false;
        bool _isKnockBack = false;

        Vector2 _randomVector;

        public bool IsDirectionFix
        {
            set { _isDirectionFix = value; }
        }

        public float AttackDamage
        {
            get { return _attackDamage; }
        }

        public float AttackSpeed
        {
            get { return _attackSpeed; }
        }

        public int AttackDamageLevel
        {
            get { return _attackDamageLevel; }
        }
        public int AttackSpeedLevel
        {
            get { return _attackSpeedLevel; }
        }

        void Start()
        {
            instance = this;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            animator.runtimeAnimatorController = characterRuntimeAnimator[UserDataManager.instance.SelectedCharacter];

            // 3번 캐릭터를 착용했을 경우 이동속도가 10 % 증가함.
            if (UserDataManager.instance.SelectedCharacter == 3)
            {
                _moveSpeed *= 1.1f;
                _fastMoveSpeed *= 1.1f;
                _normalMoveSpeed *= 1.1f;
                _slowMoveSpeed *= 1.1f;
            }
        }

        // 플레이어의 방향이 돌아가는 구조가 아니라 스프라이트를 변경하기 때문에 위치 이동시 상속된 무기의 각도를 변경해야함. 
        void HandleWeaponDirection(Vector2 directionPosition)
        {
            var angle = Mathf.Atan2(directionPosition.normalized.x, directionPosition.normalized.y) * Mathf.Rad2Deg;
            angle *= -1;
            angle += 180;

            // angle = 0 -> *-1 = 0 + 180 = -> 180
            // angle = 180 -> *-1 = -180 + 180 = -> 0

            _weaponGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // 이동함.
        void Update()
        {
            if (_isKnockBack) return;

            // 가상 조이스틱이 조작 되었을 경우 실행.
            if (_variableJoystick.Vertical != 0 || _variableJoystick.Horizontal != 0)
            {
                // 캐릭터를 움직이고 각도를 적용함.
                _moveVector.Move(new Vector2(_variableJoystick.Horizontal, _variableJoystick.Vertical), _moveSpeed);
                _changeAnimatorDirection.ApplyDirection(new Vector2(_variableJoystick.Horizontal, _variableJoystick.Vertical));
                HandleWeaponDirection(new Vector2(_variableJoystick.Horizontal, _variableJoystick.Vertical));
            }
            else
            {
                _moveVector.Move(new Vector2(_variableJoystick.Horizontal, _variableJoystick.Vertical), 0);
            }
        }

        // 스폐셜 무기 불 신말 공격 시 이동속도 증가 효과 및 트레일 효과. 
        public void ApplyFastSpeed()
        {
            _moveSpeed = _fastMoveSpeed;
            _ghostTrail.IsOn = true;
        }

        // 스폐셜 무기 전기톱, 레이저 런처 공격 시 이동속도 저하.
        public void ApplySlowSpeed()
        {
            _moveSpeed = _slowMoveSpeed;
            _ghostTrail.IsOn = false;
        }

        // 공격 종료 또는 무기 변경 시 이동속도 초기화.
        public void ApplyNormalSpeed()
        {
            _moveSpeed = _normalMoveSpeed;
            _ghostTrail.IsOn = false;
        }

        public void KnockBackStart()
        {
            SoundManager.instance.PlayAttackSound(9);
            Vector2 pushedForce = transform.position;
            pushedForce -= new Vector2(_knockBackPoint.transform.position.x, _knockBackPoint.transform.position.y);
            CameraManager.instance.ShakeLevel2();
            StartCoroutine(KnockBackCo(pushedForce.normalized));
        }

        IEnumerator KnockBackCo(Vector2 pushedDirection)
        {
            _isKnockBack = true;
            _moveVector.Move(new Vector2(_variableJoystick.Horizontal, _variableJoystick.Vertical), 0);
            _rigidbody2D.velocity = pushedDirection * 8;
            yield return new WaitForSeconds(0.2f);
            _rigidbody2D.velocity = pushedDirection * 1;
            yield return new WaitForSeconds(0.2f);
            _rigidbody2D.velocity = pushedDirection * 0.3f;
            yield return new WaitForSeconds(0.6f);
            _rigidbody2D.velocity = Vector2.zero; ;
            yield return new WaitForSeconds(0.2f);
            _isKnockBack = false;
        }


        // 맥주 아이템을 사용함. (15 초간 공격속도 100% 증가).
        public void DrunkBeer()
        {
            if (_isDrunk)
            {
                _attackDamage -= 1;
                _attackSpeed -= 1;
            }
            StopCoroutine("DrunkBeerCoroutine");
            StartCoroutine("DrunkBeerCoroutine");
        }

        IEnumerator DrunkBeerCoroutine()
        {
            _isDrunk = true;
            _attackDamage += 1;
            _attackSpeed += 1;
            _weaponController.ChangeAttackSpeed();
            yield return new WaitForSeconds(15);
            _isDrunk = false;
            _attackDamage -= 1;
            _attackSpeed -= 1;
            _weaponController.ChangeAttackSpeed();
        }

        public void UpgradeAttackDamage()
        {
            _attackDamage += 0.1f;
            _attackDamageLevel++;
        }
        public void UpgradeAttackSpeed()
        {
            _attackSpeed += 0.1f;
            _attackSpeedLevel++;
        }
    }
}

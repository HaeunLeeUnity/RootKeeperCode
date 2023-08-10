using System.Collections;
using UnityEngine;
using Item;

namespace GameNTT
{
    // 플레이어의 공격 구현.
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] ParticleSystem _catMuzzleFire;
        [SerializeField] ParticleSystem _bazukaMuzzleFire;

        Animator _animator;

        [SerializeField] BulletPool _bulletPool;
        bool _isAttacking;
        bool _isToggleAttackMode = false;
        public bool IsToggleAttackMode
        {
            set { _isToggleAttackMode = value; }
        }



        private void Start()
        {
            _animator = GetComponent<Animator>();
            StartCoroutine(AttackCo());
        }

        IEnumerator AttackCo()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                PlayerScript.instance.ApplyNormalSpeed();
                if (_isAttacking)
                {
                    // 불 신발로 공격 시 속도 상승 효과가 있음.
                    if (InventoryManager.instance.CurrentWeapon == 13)
                    {
                        PlayerScript.instance.ApplyFastSpeed();
                    }
                    // 레이저 런처 또는 전기톱으로 공격할 시 속도를 감속함.
                    else if (InventoryManager.instance.CurrentWeapon == 14 || InventoryManager.instance.CurrentWeapon == 16)
                    {
                        PlayerScript.instance.ApplySlowSpeed();
                    }
                }

                _animator.SetInteger("AttackType", InventoryManager.instance.CurrentWeapon);
                _animator.SetBool("isAttacking", _isAttacking);
            }
        }

        // 공격 속도가 변경 되었을 때 알림을 받음. (공격 속도 업그레이드 / 맥주 아이템 사용).
        public void ChangeAttackSpeed()
        {
            _animator.SetFloat("AttackSpeed", PlayerScript.instance.AttackSpeed);
        }

        // 총알을 발사함. 무기 애니메이션 이벤트로 호출
        // 무기를 변경함.
        public void ChangeWeapon(int SlotNumber)
        {
            InventoryManager.instance.CurrentWeapon = SlotNumber;
            _animator.SetInteger("AttackType", InventoryManager.instance.CurrentWeapon);
        }

        // 해당 이벤트는 삭제되었음.
        public void EndToAnimation()
        {

        }

        // 공격 시작. 공격 버튼을 클릭했을 때 실행.
        public void OnAttack()
        {
            // 자동 공격 모드가 활성화 되어 있을 때에는 공격 상태를 바꾸는 방식으로 작동함.
            if (_isToggleAttackMode)
            {
                _isAttacking = !_isAttacking;
            }
            else
            {
                _isAttacking = true;
            }
        }

        // 공격 종료. 공격 버튼에서 손을 땠을 때 실행.
        public void OffAttack()
        {
            if (!_isToggleAttackMode) _isAttacking = false;
        }


        public void ShootEvent()
        {
            // 무기 타입이 근접 무기일 경우 발사하지않음.
            if (InventoryManager.instance.CurrentWeapon == 0 ||
            InventoryManager.instance.CurrentWeapon == 3 ||
            InventoryManager.instance.CurrentWeapon == 7 ||
            InventoryManager.instance.CurrentWeapon == 8 ||
            InventoryManager.instance.CurrentWeapon == 9 ||
            InventoryManager.instance.CurrentWeapon == 14 ||
            InventoryManager.instance.CurrentWeapon == 16) return;

            var newBullet = _bulletPool.SpawnBullet(InventoryManager.instance.CurrentWeapon);

            switch (InventoryManager.instance.CurrentWeapon)
            {
                // 스프레이.
                case 1:
                    SoundManager.instance.PlayAttackSound(1);
                    break;
                // 새총.
                case 2:
                    SoundManager.instance.PlayAttackSound(2);
                    break;
                // 골드 스프레이.
                case 4:
                    SoundManager.instance.PlayAttackSound(1);
                    break;
                // 골드 새총 (세갈레).
                case 5:
                    SoundManager.instance.PlayAttackSound(2);
                    newBullet = _bulletPool.SpawnBullet(InventoryManager.instance.CurrentWeapon);
                    newBullet.transform.Rotate(0, 0, 20);
                    newBullet = _bulletPool.SpawnBullet(InventoryManager.instance.CurrentWeapon);
                    newBullet.transform.Rotate(0, 0, -20);
                    break;
                // 고양이 총 (세갈레).
                case 6:
                    SoundManager.instance.PlayAttackSound(3);
                    _catMuzzleFire.Play();
                    newBullet = _bulletPool.SpawnBullet(InventoryManager.instance.CurrentWeapon);
                    newBullet.transform.Rotate(0, 0, 20);
                    newBullet = _bulletPool.SpawnBullet(InventoryManager.instance.CurrentWeapon);
                    newBullet.transform.Rotate(0, 0, -20); break;

                // 낫.
                case 10:
                    SoundManager.instance.PlayAttackSound(0);
                    break;

                // 권총 (발사 후 화면 흔들림).
                case 11:
                    SoundManager.instance.PlayAttackSound(3);
                    CameraManager.instance.ShakeLevel1();
                    break;

                // 로켓 런처 (발사 후 화면 흔들림).
                case 12:
                    _bazukaMuzzleFire.Play();
                    SoundManager.instance.PlayAttackSound(5);
                    CameraManager.instance.ShakeLevel2();
                    break;

                // 불 신발 (나가는 방향 고정).
                case 13:
                    SoundManager.instance.PlayAttackSound(7);
                    newBullet.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                // 향수
                case 15:
                    SoundManager.instance.PlayAttackSound(1);
                    break;
            }
        }

        // 근접 무기 사용 시 공격 중 각도를 변경하여 더 많은 몬스터를 공격할 수 없도록 각도를 제한함.
        public void FixDirection()
        {
            PlayerScript.instance.IsDirectionFix = true;
            SoundManager.instance.PlayAttackSound(0);
        }

        // 근접 무기 사용 시 공격 중 각도를 변경하여 더 많은 몬스터를 공격할 수 없도록 각도를 제한함.
        // 광선검은 특수한 소리가 적용됨.
        public void LightSaverSoundPlay()
        {
            PlayerScript.instance.IsDirectionFix = true;
            SoundManager.instance.PlayAttackSound(4);
        }

        // 근접 무기 공격이 끝났을 때 각도 변경 제한을 해제함.
        public void UnFixDirection()
        {
            PlayerScript.instance.IsDirectionFix = false;
        }

        // 레이저 런처를 발사했을 시 반동으로 뒤로 밀려남.
        public void KnockBack()
        {
            PlayerScript.instance.KnockBackStart();
        }

        // 레이저 런처 충전 소리.
        public void Charge()
        {
            SoundManager.instance.PlayAttackSound(8);
        }

        // 전기톱 돌아가는 소리.
        public void ChainSaw()
        {
            SoundManager.instance.PlayAttackSound(10);
        }
    }
}

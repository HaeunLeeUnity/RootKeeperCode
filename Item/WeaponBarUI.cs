using UnityEngine;
using UnityEngine.UI;

namespace Item
{
    // 가지고 있는 무기, 사용하는 무기의 버튼 이미지 관리를 구현.
    public class WeaponBarUI : MonoBehaviour
    {
        private Animator animator;

        [SerializeField] Image[] _weaponButtonImages = new Image[4];
        [SerializeField] Sprite[] _goldWeaponSprites = new Sprite[3];

        [SerializeField] Sprite[] _specialSprite = new Sprite[11];


        void Start()
        {
            animator = GetComponent<Animator>();
            Notify();
        }

        // 사용중인 무기가 변경되었거나 새로운 무기를 획득했을 때 실행.
        // 버튼의 이미지를 초기화.
        public void Notify()
        {
            // 선택된 버튼 이미지를 크게 표시.
            animator.SetInteger("WeaponType", InventoryManager.instance.CurrentWeapon);

            // 스프레이와 새총을 획득한 경우 무기 변경 버튼 활성화.
            if (InventoryManager.instance.PosseseWeapon[1])
            {
                _weaponButtonImages[1].gameObject.SetActive(true);
            }

            if (InventoryManager.instance.PosseseWeapon[2])
            {
                _weaponButtonImages[2].gameObject.SetActive(true);
            }

            // 업그레이드 버전인 황금 무기를 가지고 있는 경우 무기 버튼 스프라이트 변경.
            for (int i = 3; i < 6; i++)
            {
                if (InventoryManager.instance.PosseseWeapon[i])
                {
                    _weaponButtonImages[i - 3].sprite = _goldWeaponSprites[i - 3];
                }
            }

            // 랜덤 무기의 스프라이트 변경 및 활성화.
            for (int ii = 6; ii < 17; ii++)
            {
                if (InventoryManager.instance.PosseseWeapon[ii])
                {
                    _weaponButtonImages[3].gameObject.SetActive(true);
                    _weaponButtonImages[3].sprite = _specialSprite[ii - 6];
                }
            }
        }
    }
}
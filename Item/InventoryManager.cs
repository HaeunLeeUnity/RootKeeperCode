using UnityEngine;
using UnityEngine.UI;
using UserData;

// 인벤토리의 기능 아이템 선택, 아이템 소지 확인, 돈 획득을 구현함.
namespace Item
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager instance;

        bool[] _possessWeapon = new bool[17];

        int _currentMoney = 0;

        [SerializeField] Text moneyText;
        [SerializeField] WeaponBarUI _weaponBarUI;
        int _currentWeapon = 0;


        public int SpecialWeaponNumber
        {
            get
            {
                int weaponNumber = 0;
                for (int i = 6; i < PosseseWeapon.Length; i++)
                {
                    if (_possessWeapon[i] == true)
                    {
                        weaponNumber = i;
                    }
                }
                return weaponNumber;
            }
        }

        public bool CollectedAllNormalWeapons
        {
            get
            {
                if (_possessWeapon[3] && _possessWeapon[4] && _possessWeapon[5])
                {
                    return true;
                }
                return false;
            }
        }

        public int CurrentMoney
        {
            get { return _currentMoney; }
        }

        public bool[] PosseseWeapon
        {
            get { return _possessWeapon; }
        }
        void Awake()
        {
            instance = this;
            PosseseWeapon[0] = true;
            if (UserDataManager.instance.SelectedCharacter == 3)
            {
                _currentMoney += 300;
            }

            moneyText.text = string.Format("{0:n0}", _currentMoney);
        }

        public int CurrentWeapon
        {
            set
            {
                switch (value)
                {
                    case 0:
                        if (PosseseWeapon[value + 3]) value += 3;
                        break;

                    case 1:
                        if (PosseseWeapon[value + 3]) value += 3;
                        break;

                    case 2:
                        if (PosseseWeapon[value + 3]) value += 3;
                        break;
                    case 4:
                        for (int i = 6; i < 17; i++)
                        {
                            if (_possessWeapon[i])
                            {
                                value = i;
                            }
                        }
                        break;
                }

                if (_possessWeapon[value])
                {
                    _currentWeapon = value;
                    SoundManager.instance.PlayWeaponChange();
                    _weaponBarUI.Notify();
                };
            }
            get
            {
                return _currentWeapon;
            }
        }

        public void GetWeapon(int weaponNumber)
        {
            _possessWeapon[weaponNumber] = true;
            _currentWeapon = weaponNumber;
            _weaponBarUI.Notify();
        }

        public void CalculateMoney(int amount)
        {
            _currentMoney += amount;
            moneyText.text = string.Format("{0:n0}", _currentMoney);
        }

        public bool CompareMoney(int amount)
        {
            if (_currentMoney >= amount)
            {
                return true;
            }
            return false;
        }

    }

}


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UserData;
using GameNTT;

namespace Item
{
    // 상점 아이템의 현황(가격, 이름, 구매 가능 여부)와 구매를 구현.
    public class StoreManager : MonoBehaviour
    {

        public static StoreManager instance;
        [Header("구매하시겠습니까 ? 팝업. 구매하겟다 클릭 시 CheckBuy 인가 함수 실행 해주면 됨.")]
        [SerializeField] GameObject buyPopup;


        [SerializeField] Image buyPopupItemIcon;
        [SerializeField] Text buyPopupNameText;
        [SerializeField] Text buyPopupContentsText;
        [SerializeField] Sprite[] itemIcons;


        [Header("고양이 프리팹 등록")]
        [SerializeField] GameObject catPrefab;
        public int CatNumber = 0;

        [Header("고양이 총 버튼이었으나 현제는 랜덤 상자 버튼으로 변경.")]
        [SerializeField] GameObject RandomWeaponButton;

        [Header("랜덤 아이템 패널")]
        [SerializeField] GameObject RandomWeaponPanel;

        [Header("아이템 가격 표시 부.")]
        [SerializeField] Text[] itemPriceText;

        [Header("아이템을 더 살 수 없음을 표시하는 이미지")]
        [SerializeField] GameObject[] itemButtonLocks;

        [Header("경고 or 구매 성공. 위로 슬슬 올라가게 끔 애니메이터 짜고 트리거 이름 On")]
        [SerializeField] Text noticeText;
        [SerializeField] Animator noticeAnimator;

        [Header("스탯을 표시할 텍스트")]
        [SerializeField] Text[] statusTexts;

        [SerializeField] GameObject[] goldWeaponLock = new GameObject[2];
        int[] itemPrices = { 80, 200, 800, 1000, 1200, 9999, 250, 250, 10, 10, 1500 };

        [SerializeField] WeaponController _weaponController;
        int _selectedItem = 0;

        public int PotionUseNumber = 0;
        public int BeerUseNumber = 0;
        [SerializeField] Button[] ItemButtons;

        void Start()
        {
            instance = this;

            if (LevelDataReader.instance != null)
            {
                itemPrices = LevelDataReader.instance.LevelData.ItemPrices;
            }

            // 체력 물약의 가격을 할인하는 캐릭터.
            if (UserDataManager.instance.SelectedCharacter == 1)
            {
                itemPrices[6] = 125;
            }

            // 랜덤 무기의 가격을 할인하는 캐릭터.
            if (UserDataManager.instance.SelectedCharacter == 4)
            {
                itemPrices[5] = 8999;
            }

            InitalizeStatusText();
            InitalizeItemPriceText();
        }

        [SerializeField] GameObject storePopup;


        public void InitalizeItemPriceText()
        {
            for (int i = 0; i < itemPrices.Length; i++)
            {
                itemPriceText[i].text = string.Format("{0:n0}", itemPrices[i]);
            }
        }


        public void InitalizeStatusText()
        {
            statusTexts[0].text = $"{100 + PlayerScript.instance.AttackDamageLevel * 10}%";
            statusTexts[1].text = $"{100 + PlayerScript.instance.AttackSpeedLevel * 10}%";
            statusTexts[2].text = $"{CatNumber}";
        }

        // 아이템을 구매하기 위해 클릭함. 버튼에서 온클릭 이벤트에 등록하여 실행
        public void OnClickItemBuy(int nowSelectItem)
        {
            // 선택된 아이템을 매개변수로 초기화함.
            _selectedItem = nowSelectItem;

            // 구매를 확인하는 팝업을 뛰움.
            buyPopup.SetActive(true);
            // 구매 확인 팝업에 아이템 아이콘을 초기화함.
            buyPopupItemIcon.sprite = itemIcons[nowSelectItem];

            // 구매 확인 팝업에 아이템 이름,정보,구매하시겠습니까? 를 언어에 맞게 초기화함.
            var localizedItemName = new LocalizedString("New Table", $"ItemName{nowSelectItem}_Key");
            var localizedItemInformation = new LocalizedString("New Table", $"ItemInformation_{nowSelectItem}_Key");
            var localizedDoYouWannaBuy = new LocalizedString("New Table", "DoYouWantBuy_Key");

            var itemNameString = localizedItemName.GetLocalizedString();
            var itemInformationString = localizedItemInformation.GetLocalizedString();

            var dict = new Dictionary<string, string>
        {{"itemName", itemNameString},
        {"itemPrice", string.Format("{0:n0}", itemPrices[nowSelectItem])},
        {"hasMoney", string.Format("{0:n0}", InventoryManager.instance.CurrentMoney)}};

            localizedDoYouWannaBuy.Arguments = new object[] { dict };

            buyPopupNameText.text = localizedItemName.GetLocalizedString();
            buyPopupContentsText.text = $"{itemInformationString}\n\n{localizedDoYouWannaBuy.GetLocalizedString()}";
        }

        // 상점 UI 를 활성화함. 버튼에서 온클릭으로 실행.
        public void ShowStore()
        {
            Time.timeScale = 0;
            storePopup.SetActive(true);
        }

        // 아이템 구매 확인창에서 구매를 눌러 최종적으로 구매함.
        public void CheckBuy()
        {
            // 인벤토리에 소지금액을 아이템 가격과 비교.
            if (InventoryManager.instance.CompareMoney(itemPrices[_selectedItem]))
            {
                // 인게임 재화 계산.
                InventoryManager.instance.CalculateMoney(-itemPrices[_selectedItem]);

                // 구매에 성공했습니다. 알림 텍스트로 출력함.
                var localizedBuyComplete = new LocalizedString("New Table", "PurchaseCompleted_Key");
                noticeText.text = localizedBuyComplete.GetLocalizedString();
                noticeAnimator.SetTrigger("On");
            }
            else
            {
                // 구매에 실패했습니다. 알림 텍스트로 출력함.
                var localizedBuyFailed = new LocalizedString("New Table", "PurchaseFailed_Key");
                noticeText.text = localizedBuyFailed.GetLocalizedString();
                noticeAnimator.SetTrigger("On");
                return;
            }

            switch (_selectedItem)
            {
                case 0:
                    BuyWeapon(0);
                    break;
                case 1:
                    BuyWeapon(1);
                    break;
                case 2:
                    BuyWeapon(2);
                    break;
                case 3:
                    BuyWeapon(3);
                    break;
                case 4:
                    BuyWeapon(4);
                    break;
                case 5:
                    BuyRandomWeapon();
                    break;
                case 6:
                    BuyHeal();
                    break;
                case 7:
                    BuyBeer();
                    break;
                case 8:
                    BuyUpgradeAttackDamage();
                    break;
                case 9:
                    BuyUpgradeAttackSpeed();
                    break;
                case 10:
                    BuyCat();
                    break;
            }
        }

        void BuyWeapon(int weaponNumber)
        {
            // 더이상 아이템을 구매할 수 없음을 표시하고 아이템 버튼의 상태를 상호작용할 수 없음으로 변경.
            itemButtonLocks[_selectedItem].SetActive(true);
            ItemButtons[_selectedItem].interactable = false;

            // 무기를 획득.
            InventoryManager.instance.GetWeapon(_selectedItem + 1);

            // 아이템이 일반 무기 구매시 황금 무기 구매 언락. 
            if (_selectedItem == 0)
            {
                ItemButtons[3].interactable = true;
                goldWeaponLock[0].SetActive(false);
            }

            if (_selectedItem == 1)
            {
                ItemButtons[4].interactable = true;
                goldWeaponLock[1].SetActive(false);
            }

            // 만약 모든 일반 무기를 구매했다면 히든 무기를 구매 버튼 활성화.
            if (InventoryManager.instance.CollectedAllNormalWeapons)
            {
                RandomWeaponButton.SetActive(true);
            }
        }

        void BuyRandomWeapon()
        {
            // 랜덤 박스 팝업 표시
            RandomWeaponPanel.SetActive(true);
            itemButtonLocks[5].SetActive(true);
            ItemButtons[5].interactable = false;
            // 상점 팝업을 닫음.
            storePopup.SetActive(false);
        }

        void BuyHeal()
        {
            // 체력 회복 아이템 가격을 2배로 올림.
            itemPrices[6] *= 2;
            // 인삼 체력 회복
            GinsengScript.instance.Heal();
            // 포션 사용 횟수 증가.
            PotionUseNumber++;
            // 아이템 가격 초기화.
            InitalizeItemPriceText();
        }
        //7
        void BuyBeer()
        {
            itemPrices[7] *= 2;
            BeerUseNumber++;
            PlayerScript.instance.DrunkBeer();
            InitalizeItemPriceText();
        }
        //8
        void BuyUpgradeAttackDamage()
        {
            PlayerScript.instance.UpgradeAttackDamage();
            itemPrices[_selectedItem] += 10 * PlayerScript.instance.AttackDamageLevel;

            if (PlayerScript.instance.AttackDamageLevel == 20)
            {
                itemButtonLocks[_selectedItem].SetActive(true);
                ItemButtons[_selectedItem].interactable = false;

                if (UserDataManager.instance.SelectedCharacter == 5)
                {
                    PlayerScript.instance.UpgradeAttackDamage();
                }
            }


            InitalizeItemPriceText();
            InitalizeStatusText();
        }
        //9
        void BuyUpgradeAttackSpeed()
        {
            PlayerScript.instance.UpgradeAttackSpeed();
            itemPrices[_selectedItem] += 10 * PlayerScript.instance.AttackSpeedLevel;
            _weaponController.ChangeAttackSpeed();

            if (PlayerScript.instance.AttackSpeedLevel == 20)
            {
                itemButtonLocks[_selectedItem].SetActive(true);
                ItemButtons[_selectedItem].interactable = false;
            }

            InitalizeItemPriceText();
            InitalizeStatusText();
        }
        //10
        void BuyCat()
        {
            var newCat = Instantiate(catPrefab);
            newCat.transform.position = PlayerScript.instance.transform.position;
            newCat.transform.Translate(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            CatNumber++;
            if (CatNumber == 10)
            {
                itemButtonLocks[_selectedItem].SetActive(true);
                ItemButtons[_selectedItem].interactable = false;
            }
            InitalizeStatusText();
        }
    }
}
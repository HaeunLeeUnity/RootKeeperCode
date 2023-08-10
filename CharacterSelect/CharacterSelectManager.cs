using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UserData;

namespace CharacterSelect
{
    // 캐릭터를 선택하는 과정을 구현한다. 슬라이드 해서 캐릭터를 선택하고 선택된 캐릭터를 소지했는지 확인한다.
    // 캐릭터의 이름을 표시한다. 효과 또는 획득 조건을 표시한다.
    // 선택이 눌리면 UserDataManager 에 저장한다.

    public class CharacterSelectManager : MonoBehaviour
    {
        // 싱글톤인 UserDataManager 와 상호작용 해야하기 때문에 스태틱 인스턴스를 생성한다. 
        public static CharacterSelectManager instance;

        float maxDistance = 15;
        Vector3 MousePos;
        float FirstPointX = 0;
        float SecondPointX = 0;

        public CharacterSelectUnit Target;

        public int CharacterNumber;

        [SerializeField] GameObject lineGameObject;
        float TargetPosX = 0;

        bool isOn = false;

        [SerializeField] Text characterNameText;
        [SerializeField] Text characterInformationText;
        [SerializeField] Text characterSelectText;
        [SerializeField] Button characterSelectButton;
        [SerializeField] CharacterSelectUnit[] characterSelectUnits;
        [SerializeField] GameObject characterSelectGameObject;
        [SerializeField] Canvas mainCanvas;
        [SerializeField] Canvas characterSelect;

        [SerializeField] Sprite[] characterButtonSprites;
        [SerializeField] Image characterButtonImage;



        void Start()
        {
            instance = this;
            characterButtonImage.sprite = characterButtonSprites[UserDataManager.instance.SelectedCharacter];
        }

        // 해금된 캐릭터를 표시한다. 해금된 캐릭터는 텍스쳐가 표시되고 해금되지 않은 캐릭터의 경우 전체가 검은색으로 표시된다.
        public void InitalizeCharacterLockState()
        {
            for (int i = 0; i < UserDataManager.instance.PossessedCharacters.Length; i++)
            {
                if (UserDataManager.instance.PossessedCharacters[i])
                {
                    characterSelectUnits[i].Unlock();
                }
                else
                {
                    characterSelectUnits[i].Lock();

                }
            }
        }

        // 버튼에서 실행. 캐릭터를 선택한다.
        public void SelectCharacter()
        {
            // UserDataManager 에 캐릭터 선택을 실행한다.
            UserDataManager.instance.SelectCharacter(CharacterNumber);
            // 타이틀 화면에서 표시되는 캐릭터 버튼의 스프라이트를 선택한 캐릭터로 바꾼다.
            characterButtonImage.sprite = characterButtonSprites[UserDataManager.instance.SelectedCharacter];
            // 버튼의 텍스트를 선택됨으로 변경한다.
            CharacterInformationLoad();
        }

        // 캐릭터 선택창을 연다.
        public void ShowCharacterSelectScene()
        {
            CharacterInformationLoad();
            InitalizeCharacterLockState();
            isOn = true;
            mainCanvas.enabled = false;
            characterSelect.enabled = true;
            characterSelectGameObject.SetActive(true);
        }

        // 캐릭터 선택창을 닫는다.
        public void HideCharacterSelectScene()
        {
            Target.OffTarget();
            isOn = false;
            mainCanvas.enabled = true;
            characterSelect.enabled = false;
            characterSelectGameObject.SetActive(false);
        }

        void Update()
        {
            if (!isOn) return;

            // 슬라이드 하여 화면을 이동시킴.
            if (Input.GetMouseButton(0))
            {
                MousePos = Input.mousePosition;
                MousePos = Camera.main.ScreenToWorldPoint(MousePos);

                RaycastHit2D hit2D = Physics2D.Raycast(MousePos, transform.forward, maxDistance);

                if (hit2D && hit2D.transform.gameObject == lineGameObject)
                {
                    if (FirstPointX != 0)
                    {
                        lineGameObject.transform.Translate(-(FirstPointX - hit2D.point.x), 0, 0);
                    }
                    FirstPointX = hit2D.point.x;
                }
            }
            // 손을 때면 마지막으로 선택된 캐릭터의 위치를 저장하고 저장된 캐릭터 쪽으로 위치를 이동.
            else if (Input.GetMouseButtonUp(0))
            {
                TargetPosX = lineGameObject.transform.position.x - Target.transform.position.x;
                FirstPointX = 0;
            }
            else
            {
                lineGameObject.transform.position = new Vector3(Mathf.Lerp(lineGameObject.transform.position.x, TargetPosX, 15 * Time.deltaTime), lineGameObject.transform.position.y, 0);
            }
        }

        // 화면 중간에 놓인 캐릭터의 정보를 표기함.
        public void TargetingCharacter(CharacterSelectUnit target, int targetNumber)
        {
            if (target != Target && FirstPointX != 0)
            {
                if (Target != null)
                {
                    Target.OffTarget();
                }
                Target = target;
                CharacterNumber = targetNumber;
                CharacterInformationLoad();
            }
        }

        // 캐릭터의 이름, 설명, 해금 조건을 표시. 선택 여부, 보유 여부 표시.
        public void CharacterInformationLoad()
        {
            var localizedItemName = new LocalizedString("New Table", $"CharacterName{CharacterNumber}");
            var localizedItemInformation = new LocalizedString("New Table", $"CharacterInformation{CharacterNumber}");
            var localizedItemUnLockCondition = new LocalizedString("New Table", $"CharacterUnlockCondition{CharacterNumber}");


            characterNameText.text = localizedItemName.GetLocalizedString();
            characterInformationText.text = localizedItemInformation.GetLocalizedString();

            // 캐릭터가 선택되었거나 캐릭터를 보유하고 있지 않을 경우 버튼을 비활성화 함.
            characterSelectButton.interactable = false;

            if (!UserDataManager.instance.PossessedCharacters[CharacterNumber])
            {
                characterInformationText.text = localizedItemUnLockCondition.GetLocalizedString();



                var localizedUnavailable = new LocalizedString("New Table", "Unavailable_Key");
                characterSelectText.text = localizedUnavailable.GetLocalizedString();
            }
            else
            {
                if (UserDataManager.instance.SelectedCharacter == CharacterNumber)
                {
                    var localizedSelected = new LocalizedString("New Table", "Selected_Key");
                    characterSelectText.text = localizedSelected.GetLocalizedString();
                }
                else
                {
                    var localizedSelect = new LocalizedString("New Table", "Select_Key");
                    characterSelectButton.interactable = true;
                    characterSelectText.text = localizedSelect.GetLocalizedString();
                }
            }
        }
    }
}

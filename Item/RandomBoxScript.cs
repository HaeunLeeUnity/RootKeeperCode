using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

namespace Item
{
    public class RandomBoxScript : MonoBehaviour
    {
        [SerializeField] Sprite[] itemIcons;

        [SerializeField] Image itemIconImage;

        [SerializeField] Animator ChestAnimator;


        [SerializeField] Text itemNameText;

        [SerializeField] Animator itemNameTextAnimator;
        [SerializeField] Text itemInformationText;
        [SerializeField] GameObject rootingButton;

        [SerializeField] ParticleSystem screenParticle;


        void OnEnable()
        {
            // 아이템 이름 표시부를 '랜덤 스폐셜 무기' 로 변경하여 표시.
            var localizedItemName = new LocalizedString("New Table", "ItemName5_Key");
            itemNameText.text = localizedItemName.GetLocalizedString();
            StartCoroutine("RandomIconChangeCo");
        }






        // 아이템 스프라이트를 일정 주기마다 랜덤하게 바꿈.
        IEnumerator RandomIconChangeCo()
        {
            while (true)
            {
                ChangeIcon();
                yield return new WaitForSecondsRealtime(0.4f);
            }
        }

        // 아이템 아이콘 스프라이트를 변경.
        public void ChangeIcon()
        {
            var randomItemNumber = (int)Random.Range(0, itemIcons.Length);
            itemIconImage.sprite = itemIcons[randomItemNumber];
        }

        // 온클릭 이벤트로 버튼에서 실행. 기존의 아이콘을 변경하던 코루틴을 종료하고 Open코루틴을 실행.
        public void Open()
        {
            StopCoroutine("RandomIconChangeCo");
            StartCoroutine(RandomBoxOn());
        }


        // 상자를 여는 연출을 진행하고 아이템을 획득함.
        IEnumerator RandomBoxOn()
        {
            // 긴장감을 고조시키기 위해 BGM의 불륨을 낮춤.
            BGMManager.instance.VolumeDown();
            // 상자가 흔들 거리는 애니메이션 시작.
            ChestAnimator.SetTrigger("Start");

            // 최종적으로 획득할 아이템을 랜덤으로 정함.
            var randomItemNumber = (int)Random.Range(0, itemIcons.Length);

            yield return new WaitForSecondsRealtime(5);
            // 아이템을 획득하는 소리 재생.
            SoundManager.instance.PlayAttackSound(3);

            // 애니메이션 파라미터 변경.
            ChestAnimator.SetTrigger("Open");
            itemNameTextAnimator.SetTrigger("On");

            // 아이템 아이콘을 최종 획득 아이템으로 변경.
            itemIconImage.sprite = itemIcons[randomItemNumber];
            // 화면을 덮는 빛 파티클 재생.
            screenParticle.Play();
            yield return new WaitForSecondsRealtime(1f);

            // BGM 을 처음으로 초기화.
            BGMManager.instance.VolumeUp();

            // 아이템 이름과 설명 텍스트를 초기화.
            var localizedItemName = new LocalizedString("New Table", $"SpecialWeapon{randomItemNumber}Name_Key");
            var localizedItemInformation = new LocalizedString("New Table", $"SpecialWeapon{randomItemNumber}Information_Key");

            itemNameText.text = localizedItemName.GetLocalizedString();
            itemInformationText.text = localizedItemInformation.GetLocalizedString();
            yield return new WaitForSecondsRealtime(1.2f);

            // 팝업을 나갈 수 있는 버튼 활성화.
            rootingButton.SetActive(true);
            // 인벤토리에 아이템을 획득.
            InventoryManager.instance.GetWeapon(randomItemNumber + 6);
        }
    }
}

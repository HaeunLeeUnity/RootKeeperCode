using UnityEngine;
namespace Item
{
    // 랜덤 박스 애니메이션 재생 시 애니메이션 이벤트를 수신하여 소리를 내거나 아이템 이미지를 바꿈. 
    public class ChestEventReciver : MonoBehaviour
    {
        [SerializeField] RandomBoxScript randomBoxScript;

        // 아이템 아이콘을 변경한다.
        public void ChangeIconEvent()
        {
            randomBoxScript.ChangeIcon();
            SoundManager.instance.PlayShakeBoxSound();
        }

        // 상자가 열리는 효과음을 실행한다.
        public void OpenEvent()
        {
            SoundManager.instance.PlayOpenBoxSound();
        }

        // 긴장감을 고조시키는 효과음을 낸다.
        public void TentionEvent()
        {
            SoundManager.instance.PlaytentionBoxSound();
        }

    }
}
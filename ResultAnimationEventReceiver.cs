using UnityEngine;

// 게임 종료 후 표시되는 결과 화면에서 애니메이션 이벤트로 소리 재생, 스크린 샷을 표시할 수 있게 해주는 컴포넌트.
public class ResultAnimationEventReceiver : MonoBehaviour
{
    [SerializeField] GameResultManager gameResultManager;
    public void PlaySound()
    {
        SoundManager.instance.PlayAttackSound(0);
    }
    public void ScreenShot()
    {
        gameResultManager.ScreenShot();
    }
}

using UnityEngine;

// 오브젝트가 활성화 되었을 때 공격 소리를 재생한다.
public class AttackSoundPlayOnEnable : MonoBehaviour
{
    [SerializeField]
    int _attackSoundNunber;
    private void OnEnable()
    {
        SoundManager.instance.PlayAttackSound(_attackSoundNunber);
    }
}

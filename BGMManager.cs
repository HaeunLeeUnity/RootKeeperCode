using UnityEngine;

// BGM 을 멈추거나 재생하고 불륨을 조절한다. 
public class BGMManager : MonoBehaviour
{
    AudioSource audioSource;

    // 스태틱 인스턴스으로 만드는 이유는 구글 애즈 싱글톤에서 접근해야하기 때문이다.  
    public static BGMManager instance;
    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // 불륨 레벨을 변경한다. 설정 적용시 실행된다.
    public void ChangeVolumeValue(int BGMVolume)
    {

        float realvolume = BGMVolume;
        realvolume /= 100;
        audioSource.volume = realvolume * 0.3f;
    }

    // BGM 을 멈춘다. 광고를 표시할 때 실행된다.
    public void Pause()
    {
        audioSource.Pause();
    }

    // BGM 을 재생한다. 광고가 끝났을 때에 실행된다.
    public void Unpause()
    {
        audioSource.UnPause();
    }

    // 불륨을 잠시 낮춘다. 랜덤 박스를 열 때 실행된다.
    public void VolumeDown()
    {
        audioSource.volume = audioSource.volume * 0.3333f;
    }

    // 불륨을 다시 키운다. 랜덤 박스를 다 열었을 때에 실행된다.
    public void VolumeUp()
    {
        audioSource.volume = audioSource.volume * 3;
    }

}

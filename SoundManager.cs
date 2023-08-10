using System.Collections;
using UnityEngine;

// SFX 효과 재생을 구현.
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource audioSource;

    [SerializeField] private AudioClip swordClip;
    [SerializeField] private AudioClip sprayClip;
    [SerializeField] private AudioClip ginsengClip;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip currentButton;
    [SerializeField] private AudioClip cancelButton;
    [SerializeField] private AudioClip moneySound;
    [SerializeField] private AudioClip catSound;
    [SerializeField] private AudioClip clearSound;

    [SerializeField] AudioClip shakeBoxSound;

    [SerializeField] AudioClip openBoxSound0;
    [SerializeField] AudioClip openBoxSound1;

    [SerializeField] AudioClip tentionBoxSound;



    [Header("공격 휘두르기 or 격발시 날 소리")]
    [SerializeField] AudioClip[] attackSounds;

    [Header("무기 변경시 날 소리.")]
    [SerializeField] AudioClip changeWeapon;


    int _enemyHitsNumber = 0;

    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeVolumeValue(int SFXVolume)
    {
        float realvolume = SFXVolume;
        realvolume /= 100;
        audioSource.volume = realvolume * 0.75f;
    }

    public void PlayShakeBoxSound()
    {
        audioSource.PlayOneShot(shakeBoxSound);
    }

    public void PlayOpenBoxSound()
    {
        audioSource.PlayOneShot(openBoxSound0);
    }

    public void PlaytentionBoxSound()
    {
        audioSource.PlayOneShot(tentionBoxSound);
    }

    public void PlayAttackSound(int attackIndex)
    {
        audioSource.PlayOneShot(attackSounds[attackIndex]);
    }

    public void PlayWeaponChange()
    {
        audioSource.PlayOneShot(changeWeapon);
    }

    public void PlaySprayClip()
    {
        audioSource.PlayOneShot(sprayClip);
    }

    public void PlayGinseng()
    {
        audioSource.PlayOneShot(ginsengClip);
    }

    public void PlayEnemyHit()
    {
        if (_enemyHitsNumber < 30)
        {
            _enemyHitsNumber++;
        }
        if (_enemyHitsNumber == 1)
        {
            StopCoroutine("EnemyHitCo");
            StartCoroutine("EnemyHitCo");
        }
    }

    // 적이 공격 당하는 소리가 뭉쳐서 너무 크게 들리거나 이상하게 들리는 현상을 시간차를 통해 해결.
    IEnumerator EnemyHitCo()
    {
        while (_enemyHitsNumber > 0)
        {
            audioSource.PlayOneShot(enemyHit);
            if (_enemyHitsNumber == 0)
            {
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.003f);
            _enemyHitsNumber--;
        }
    }
    
    public void PlayCurrentButton()
    {
        audioSource.PlayOneShot(currentButton);
    }

    public void PlayCancelButton()
    {
        audioSource.PlayOneShot(cancelButton);
    }

    public void PlayMoneySound()
    {
        audioSource.PlayOneShot(moneySound);
    }

    public void PlayCatSound()
    {
        audioSource.PlayOneShot(catSound);
    }
    public void PlayClearSound()
    {
        audioSource.PlayOneShot(clearSound);
    }


}

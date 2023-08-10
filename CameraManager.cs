using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 카메라가를 흔드는 효과를 재생하거나 해상도에 따라 카메라 및 캔버스 비율을 조정한다.
public class CameraManager : MonoBehaviour
{
    private Animator animator;

    public static CameraManager instance;

    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] GameObject storePanel;
    [SerializeField] GameObject pauseMenu;

    public bool CameraShakeOn = true;

    private void Start()
    {
        instance = this;
        animator = GetComponent<Animator>();
        StartCoroutine(CamSettingLoop());

    }

    // 카메라의 비율을 조정하는 코루틴을 일정 시간 마다 호출한다. (폴더블 디스플레이 대응).
    IEnumerator CamSettingLoop()
    {
        while (true)
        {
            CamSetting();
            yield return new WaitForSecondsRealtime(0.25f);
        }
    }

    public void CamSetting()
    {
        float width = Screen.width;
        float height = Screen.height;

        float sum = width / height;

        // 가로가 짧은 해상도의 경우 태블릿으로 인식하고 카메라를 줌 아웃한다.
        if (sum < 1.5f)
        {
            canvasScaler.matchWidthOrHeight = 0;
            GetComponent<Camera>().orthographicSize = 11;
        }

        // 가로가 긴 해상도의 경우 바형 스마트폰으로 인식하고 카메라를 줌 인 한다.
        else
        {
            canvasScaler.matchWidthOrHeight = 1;
            GetComponent<Camera>().orthographicSize = 7.5f;
        }
    }

    // 카메라를 흔든다. 피격 시 실행한다. 무기 종류에 따라 잠깐 시간을 멈추는 효과를 갖는다.
    public void Shake(int level)
    {
        // 설정에서 카메라 흔들림이 켜져있는지 여부 확인.
        if (CameraShakeOn)
        {
            switch (level)
            {
                case 1:
                    animator.SetTrigger("SmallShake");
                    break;
                case 2:
                    animator.SetTrigger("BigShake");
                    break;
                case 3:
                    animator.SetTrigger("BigShake");
                    StartCoroutine(TimeStopTimer(0.05f));
                    break;
                case 4:
                    animator.SetTrigger("BigShake");
                    StartCoroutine(TimeStopTimer(0.1f));
                    break;
            }
        }

    }
    public void ShakeLevel1()
    {
        if (CameraShakeOn)
            animator.SetTrigger("SmallShake");
    }

    public void ShakeLevel2()
    {
        if (CameraShakeOn)
            animator.SetTrigger("BigShake");
    }

    public void ShakeLevel3()
    {
        if (CameraShakeOn)
        {
            animator.SetTrigger("BigShake");
            StartCoroutine(TimeStopTimer(0.05f));
        }
    }

    public void ShakeLevel4()
    {
        if (CameraShakeOn)
        {
            animator.SetTrigger("BigShake");
            StartCoroutine(TimeStopTimer(0.1f));
        }
    }

    // 잠깐동안 게임을 멈춘다. (타격감을 위하여)
    IEnumerator TimeStopTimer(float stoptime)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(stoptime);

        // 만약 시간 정지 도중 정지메뉴 또는 상점이 열려있을 때에는 시간 정지를 해제 하지 않는다. 
        if (!pauseMenu.activeInHierarchy && !storePanel.activeInHierarchy)
        {
            Time.timeScale = 1;
        }

    }
}
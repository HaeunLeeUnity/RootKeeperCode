using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 타이틀 화면에서 캔버스의 비율을 조정한다.
public class CanvasReScaler : MonoBehaviour
{
    // 타이틀 화면에는 일반 캔버스와 캐릭터 선택 캔버스 두 종류가 있다.  
    CanvasScaler canvasScaler;
    [SerializeField] CanvasScaler canvasScaler2;

    private void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        StartCoroutine(CamSettingLoop());
    }


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


        if (sum < 1.5f)
        {
            canvasScaler.matchWidthOrHeight = 0;
            canvasScaler2.matchWidthOrHeight = 0;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 1;
            canvasScaler2.matchWidthOrHeight = 1;
        }

    }
}

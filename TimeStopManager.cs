using UnityEngine;

// 일시 정지를 구현.
public class TimeStopManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private void OnApplicationPause(bool pauseStatus)
    {
        if (Time.timeScale != 0 && pauseStatus)
        {
            pauseMenu.SetActive(true);
            TimeStop();
        }
    }

    public void TimeStop()
    {
        Time.timeScale = 0;
    }

    public void TimeGo()
    {
        Time.timeScale = 1;
    }
}

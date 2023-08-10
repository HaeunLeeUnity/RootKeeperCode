using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // 씬 이동 및 레벨 파일 관리.
    private string savedScenename = "";

    [SerializeField] TextAsset quickModeJsonFile;
    [SerializeField] TextAsset normalModeJsonFile;
    [SerializeField] TextAsset hardModeJsonFile;

    // 씬으로 이동하는 버튼을 눌렀을 때 실행. 해당 함수가 실행되고 정말 이동할 것인지를 묻는 팝업이 동시에 표시.
    public void SaveSceneName(string SceneName)
    {
        savedScenename = SceneName;
    }

    // 정말 이동할 것인지 묻는 팝업에서 확인을 누를 경우 저장한 씬으로 이동.
    public void GoToSavedScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(savedScenename);
    }

    public void ChangeScene(string SceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName);
    }



    // 선택된 레벨 데이터 파일을 등록함.
    public void GoToQuickMode()
    {
        LevelDataReader.instance.ReadLevelData(quickModeJsonFile);
        LevelDataReader.instance.nowGameMode = GameMode.Quick;
        savedScenename = "TitleScene";
        StartCoroutine(newLevelLoad());
    }

    public void GoToNormalMode()
    {
        LevelDataReader.instance.ReadLevelData(normalModeJsonFile);
        LevelDataReader.instance.nowGameMode = GameMode.Normal;
        savedScenename = "TitleScene";
        StartCoroutine(newLevelLoad());
    }

    public void GoToHardMode()
    {
        LevelDataReader.instance.ReadLevelData(hardModeJsonFile);
        LevelDataReader.instance.nowGameMode = GameMode.Hard;
        savedScenename = "TitleScene";
        StartCoroutine(newLevelLoad());
    }

    public void GoToLevelDesign()
    {
        LevelDataReader.instance.nowGameMode = GameMode.LevelDesign;
        SceneManager.LoadScene("LevelDesign");
    }

    IEnumerator newLevelLoad()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene("GameScene");
    }
}

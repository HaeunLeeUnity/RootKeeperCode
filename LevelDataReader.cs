using UnityEngine;
using System.IO;
using Newtonsoft.Json;

// 레벨 데이터 파일을 저장하거나 불러오는 기능을 구현한다.
public class LevelDataReader : MonoBehaviour
{
    public int StartPattenIndex = 0;
    public LevelData LevelData = new LevelData();
    public static LevelDataReader instance;
    public string FileName;
    public GameMode nowGameMode = GameMode.Normal;
    [SerializeField] TextAsset _defaultJsonFile;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            ReadLevelData(_defaultJsonFile);
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // 레벨 데이터를 저장한다.
    public void SaveLevelData()
    {
        string levelDataToString = JsonConvert.SerializeObject(LevelDataReader.instance.LevelData, Formatting.Indented);
        File.WriteAllText($"{Application.persistentDataPath}/{FileName}.json", levelDataToString);
    }

    // 레벨 데이터를 불러온다.
    public void ReadLevelData(TextAsset levelData)
    {
        LevelData = JsonConvert.DeserializeObject<LevelData>(levelData.text);
    }
}

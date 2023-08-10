using System.Collections;
using UnityEngine;

// 레벨 디자인 시 사용하는 코드. 지금 느끼는 난이도를 기록할 수 있다.
public class DifficultyWriter : MonoBehaviour
{
    [SerializeField] Animator[] buttonAnimators = new Animator[4];
    [SerializeField] EnemySpawnManager _enemySpawnManager;

    private void Start()
    {
        if (LevelDataReader.instance.nowGameMode != GameMode.LevelDesign)
        {
            Destroy(this.gameObject);
            return;
        }

        StartCoroutine(SaveTimer());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AddNewDifficulty(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            AddNewDifficulty(1);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            AddNewDifficulty(2);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            AddNewDifficulty(3);
        }
    }

    public void AddNewDifficulty(int DifficultyNumber)
    {
        DifficultyStamp newStamp = new DifficultyStamp();

        newStamp.TimeStamp = _enemySpawnManager.CurrentTime;
        newStamp.Difficulty = (short)DifficultyNumber;

        LevelDataReader.instance.LevelData.DifficultyStamp.Add(newStamp);
        buttonAnimators[DifficultyNumber].SetTrigger("On");
    }

    IEnumerator SaveTimer()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(5);
            LevelDataReader.instance.SaveLevelData();
        }
    }
}

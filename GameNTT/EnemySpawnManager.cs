using System.Collections;
using UnityEngine;
using UserData;
using Item;
using GameNTT;

// 레벨 디자인 파일에 맞게 몬스터를 생성하고 경과 시간을 체크함.
public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    MonsterGenerator _monsterGenerator;

    [SerializeField]
    HUDTimeController _HUDTimeController;


    int pattenIndex = 0;

    int _clearTime = 30;
    int _currentTime = 0;

    // 게임 종료 후 결과화면에서 진행도를 표시할 때에 사용함.
    public int CurrentTime
    {
        get { return _currentTime; }
    }

    public int ClearTime
    {
        get { return _clearTime; }
    }


    void Start()
    {

        if (LevelDataReader.instance != null)
        {
            // 레벨 디자인 시 시작지점을 설정할 수 있음.
            // 게임 시작 지점에 대한 계산 및 레벨 총 시간 계산.

            var totalMoney = 0;
            var totalTime = 0f;

            for (int i = 0; i < LevelDataReader.instance.LevelData.Pattens.Count; i++)
            {
                // 시작지점 까지의 Money 와 Time 을 적용.
                if (i == LevelDataReader.instance.StartPattenIndex)
                {
                    InventoryManager.instance.CalculateMoney(totalMoney);
                    _currentTime = Mathf.RoundToInt(totalTime);
                }

                // 전체 시간 계산
                totalTime += LevelDataReader.instance.LevelData.Pattens[i].Delay;

                // 전체 돈 계산
                if (LevelDataReader.instance.LevelData.Pattens[i].Type == PatternType.MonsterSpawn)
                {
                    for (int ii = 0; ii < 8; ii++)
                    {
                        totalMoney += LevelDataReader.instance.LevelData.Pattens[i].Monsters[ii] * LevelDataReader.instance.LevelData.MonsterBalances[ii].Money;
                    }
                }
            }

            // 클리어 시간 적용
            _clearTime = Mathf.RoundToInt(totalTime);

            // 패턴 시작 지점 적용
            pattenIndex = LevelDataReader.instance.StartPattenIndex;
        }

        // 게임 시작.
        StartCoroutine(LevelDataPattenStart());

        //타이머 시작.
        StartCoroutine(TimeOn());
    }

    // 타이머 코루틴.
    IEnumerator TimeOn()
    {
        while (_clearTime != _currentTime)
        {
            _HUDTimeController.Notify();
            yield return new WaitForSeconds(1);
            _currentTime++;
        }

        // 시간이 종료시 실행.
        // 시간 정지
        Time.timeScale = 0;

        // 베스트 스코어 저장
        UserDataManager.instance.BestScoreSave();

        // 클리어 소리 재생.
        SoundManager.instance.PlayClearSound();

        //  클리어 데이터 저장.
        if (LevelDataReader.instance.nowGameMode == GameMode.Hard)
        {
            UserDataManager.instance.ClearHardModeSave();
        }
        else if ((LevelDataReader.instance.nowGameMode == GameMode.Normal))
        {
            UserDataManager.instance.ClearNormalModeSave();
        }

        // 게임 결과 출력
        GameResultManager.instance.OnShowResult();
    }

    // 레벨 시작.
    IEnumerator LevelDataPattenStart()
    {
        // 시작 시 3초 딜레이 후 인삼이 움직이기 시작함.
        yield return new WaitForSeconds(3);
        // GinsengScript.instance.gameObject.GetComponent<AIMoveScript>().MoveSpeed = 2;

        while (LevelDataReader.instance != null)
        {
            // 패턴 타입에 따라 실행.
            switch (LevelDataReader.instance.LevelData.Pattens[pattenIndex].Type)
            {
                // 딜레이.
                case PatternType.Delay:
                    yield return new WaitForSeconds(LevelDataReader.instance.LevelData.Pattens[pattenIndex].Delay);
                    break;
                // 몬스터 생성.
                case PatternType.MonsterSpawn:
                    // 몬스터 종류 탐색 후 몬스터 수 만큼 생성.
                    for (int i = 0; i < 8; i++)
                    {
                        for (int ii = 0; ii < LevelDataReader.instance.LevelData.Pattens[pattenIndex].Monsters[i]; ii++)
                        {
                            SpawnMonster(i);
                            yield return new WaitForEndOfFrame();
                        }
                    }
                    break;
                // 클리어시 코루틴을 종료함.
                case PatternType.Clear:
                    yield break;
            }

            pattenIndex++;
        }
    }

    // 오브젝트 풀에서 몬스터 Spawn.
    public void SpawnMonster(int monsterType)
    {
        // 한 사이드를 정해 몬스터를 소환.
        var spawnPivot = (int)Random.Range(0, 4);
        var spawnPoint = new Vector2();
        switch (spawnPivot)
        {
            case 0:
                spawnPoint = new Vector2(-18, Random.Range(-12, 12));
                break;
            case 1:
                spawnPoint = new Vector2(18, Random.Range(-12, 12));
                break;
            case 2:
                spawnPoint = new Vector2(Random.Range(-18, 18), 12);
                break;
            case 3:
                spawnPoint = new Vector2(Random.Range(-18, 18), -12);
                break;
        }

        _monsterGenerator.Spawn(monsterType, spawnPoint);
    }

}

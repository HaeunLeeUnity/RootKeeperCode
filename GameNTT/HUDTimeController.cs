using UnityEngine;
using UnityEngine.UI;

namespace GameNTT
{
    public class HUDTimeController : MonoBehaviour
    {
        [SerializeField]
        Text _timeText;
        [SerializeField]
        EnemySpawnManager _enemySpawnManager;

        // 시간을 표시하는 텍스트의 내용을 변경한다.
        public void Notify()
        {
            _timeText.text = $"{(_enemySpawnManager.ClearTime - _enemySpawnManager.CurrentTime) / 60}:{((_enemySpawnManager.ClearTime - _enemySpawnManager.CurrentTime) % 60).ToString("00")}";
        }
    }
}

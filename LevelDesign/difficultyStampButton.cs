using UnityEngine;
using UnityEngine.UI;

namespace LevelDesign
{
    // 레벨 디자이너에서 기록된 느낌 버튼.
    public class difficultyStampButton : MonoBehaviour
    {
        public int StampIndex = 0;
        [SerializeField] Text timeText;

        public void DestroyThisStamp()
        {
            LevelDesigner.instance.DestroyStamp(StampIndex);
        }

        public void ShowTime(float time)
        {
            timeText.text = $"{Mathf.RoundToInt(time) / 60}:{(time % 60).ToString("00")}";
        }
    }
}
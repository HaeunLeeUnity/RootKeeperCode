using UnityEngine;
using UnityEngine.UI;

namespace UserData
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        public int Score = 0;
        float stateScore = 0;
        [SerializeField] Text scoreText;

        [SerializeField] float TextSpeed = 0.1f;

        public int MyBest = 0;

        void Awake()
        {
            instance = this;

            // 최고 점수를 불러옴.
            if (UserDataManager.instance != null)
            {
                MyBest = UserDataManager.instance.BestScore;
            }
        }

        // 획득한 점수가 바로 반영되는 것이 아니라 숫자가 서서히 바뀌는 효과를 구현.
        // 최고 점수를 기록했을 시 점수를 노란색으로 표시하는 효과를 구현.
        private void Update()
        {
            stateScore = Mathf.Lerp(stateScore, Score, TextSpeed * Time.deltaTime);

            var stateScoreString = string.Format("{0:n0}", Mathf.RoundToInt(stateScore));

            if (stateScore > MyBest)
            {
                scoreText.text = $"<color=yellow>{stateScoreString}</color>";
            }
            else
            {
                scoreText.text = stateScoreString;
            }
        }
    }
}

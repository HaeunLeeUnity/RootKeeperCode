using UnityEngine;

namespace GameNTT
{
    // 화면 가장자리를 붉게 하는 효과를 실행.
    public class ScreenRedEvent : MonoBehaviour
    {
        Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void ShowRed()
        {
            animator.SetTrigger("Go");
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSelect
{
    // 캐릭터 선택 화면에서 표시되는 각각의 캐릭터.
    public class CharacterSelectUnit : MonoBehaviour
    {
        [SerializeField] int characterNumber = 0;
        bool isTarget = false;

        public int direction = 4;
        [SerializeField] Animator animator;
        [SerializeField] SpriteRenderer spriteRenderer;

        // 화면 중간에 캐릭터가 놓일 경우 선택된 것으로 간주함.
        void Update()
        {
            float x = transform.position.x;
            if (x > -2f && x < 2f)
            {
                OnTarget();
            }
        }

        // 캐릭터가 잠겨있는 경우 색을 검은 색으로 표시함.
        public void Lock()
        {
            spriteRenderer.color = new Vector4(0, 0, 0, 1);
        }

        // 캐릭터가 해금된 경우 원래 색을 표시함.
        public void Unlock()
        {
            spriteRenderer.color = new Vector4(1, 1, 1, 1);
        }

        // 캐릭터가 선택 됐을 때 크기를 키우고 8방향으로 회전하며 달리는 애니메이션을 실행함.
        public void OnTarget()
        {
            if (isTarget) return;
            isTarget = true;
            direction = 3;
            StartCoroutine("OnAnimationStart");
            transform.localScale = new Vector3(10, 10, 10);
            CharacterSelectManager.instance.TargetingCharacter(this, characterNumber);
        }

        // 캐릭터가 선택 해제 됐을 때 크기를 키우고 8방향으로 회전하며 달리는 애니메이션을 실행함.
        public void OffTarget()
        {
            if (!isTarget) return;
            isTarget = false;
            StopCoroutine("OnAnimationStart");
            animator.SetInteger("Direction", 4);
            animator.SetBool("Move", false);
            transform.localScale = new Vector3(8, 8, 8);
        }

        IEnumerator OnAnimationStart()
        {
            animator.SetBool("Move", true);
            animator.SetInteger("Direction", direction);

            while (true)
            {
                direction++;
                if (direction == 8)
                {
                    direction = 0;
                }

                animator.SetInteger("Direction", direction);
                yield return new WaitForSecondsRealtime(0.8f);
            }
        }
    }
}

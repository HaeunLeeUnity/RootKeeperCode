using UnityEngine;

namespace GameNTT
{
    // 오브젝트의 각도를 그대로 두고 목표지점으로 위치를 이동 시킨다.
    public class MoveVector : MonoBehaviour
    {
        Rigidbody2D _rigidBody2D;
        [SerializeField] Animator _animator;
        private void Start()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 targetPos, float moveSpeed)
        {
            // 이동속도가 0 이상이라면 애니메이터 파타미터를 변경한다.
            if (moveSpeed > 0)
            {
                _animator.SetBool("Move", true);
            }
            else
            {
                _animator.SetBool("Move", false);
            }

            _rigidBody2D.velocity = targetPos.normalized * moveSpeed;
        }
    }
}
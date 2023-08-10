using UnityEngine;

namespace GameNTT
{
    public class ChangeAnimatorDirection : MonoBehaviour
    {
        // 벡터에 따라 애니메이터에 Direction 값을 변경함. 

        [SerializeField] Animator _animator;

        public void ApplyDirection(Vector2 directionVector)
        {
            var angle = Mathf.Atan2(directionVector.x, directionVector.y) * Mathf.Rad2Deg;

            if (-22.5f < angle && 22.5f > angle)
            {
                _animator.SetInteger("Direction", 0);
            }
            if (22.5f < angle && 67.5f > angle)
            {
                _animator.SetInteger("Direction", 1);
            }
            if (67.5f < angle && 112.5f > angle)
            {
                _animator.SetInteger("Direction", 2);
            }
            if (112.5f < angle && 157.5f > angle)
            {
                _animator.SetInteger("Direction", 3);
            }
            if (157.5f < angle || -157.5f > angle)
            {
                _animator.SetInteger("Direction", 4);
            }
            if (-157.5f < angle && -112.5f > angle)
            {
                _animator.SetInteger("Direction", 5);
            }
            if (-112.5f < angle && -67.5f > angle)
            {
                _animator.SetInteger("Direction", 6);
            }
            if (-67.5f < angle && -22.5f > angle)
            {
                _animator.SetInteger("Direction", 7);
            }
        }
    }
}
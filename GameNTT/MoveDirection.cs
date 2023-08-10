using UnityEngine;

namespace GameNTT
{
    // 설정한 방향으로 오브젝트를 이동시킴.
    public class MoveDirection : MonoBehaviour
    {
        Rigidbody2D _rigidbody2D;

        [SerializeField] Direction _direction;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public Direction Direction
        {
            set { _direction = value; }
        }

        public void Move(float moveSpeed)
        {
            switch (_direction)
            {
                case Direction.Up:
                    _rigidbody2D.velocity = transform.up * moveSpeed;
                    break;
                case Direction.Right:
                    _rigidbody2D.velocity = transform.right * moveSpeed;
                    break;
                case Direction.Left:
                    _rigidbody2D.velocity = -transform.right * moveSpeed;
                    break;
                case Direction.Down:
                    _rigidbody2D.velocity = -transform.up * moveSpeed;
                    break;
            }
        }
    }

    public enum Direction
    {
        Up,
        Right,
        Left,
        Down
    }
}
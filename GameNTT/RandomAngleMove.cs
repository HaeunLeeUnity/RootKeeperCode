using System.Collections;
using UnityEngine;

namespace GameNTT
{
    // 타이틀 화면에서 개미, 벌의 움직임을 구현.
    public class RandomAngleMove : MonoBehaviour
    {
        [SerializeField] float _moveSpeed = 1;
        [SerializeField] float _rotSpeed = 30;

        Quaternion angle;
        MoveDirection _moveDirection;

        void Start()
        {
            _moveDirection = gameObject.AddComponent<MoveDirection>();
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            StartCoroutine(ChangeDirectionToRandom());
        }

        void Update()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, _rotSpeed * Time.deltaTime);
            _moveDirection.Move(_moveSpeed);
        }

        IEnumerator ChangeDirectionToRandom()
        {
            while (true)
            {
                ChangeDirection();
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
            }
        }

        void ChangeDirection()
        {
            angle = Quaternion.Euler(0, 0, Random.Range(0, 360));

            if (transform.position.x < -10.5)
            {
                angle = Quaternion.Euler(0, 0, 270);
            }

            if (transform.position.x > 10.5)
            {
                angle = Quaternion.Euler(0, 0, 90);
            }

            if (transform.position.y < -7)
            {
                angle = Quaternion.Euler(0, 0, 0);
            }

            if (transform.position.y > 4.5)
            {
                angle = Quaternion.Euler(0, 0, 180);
            }
        }

    }
}

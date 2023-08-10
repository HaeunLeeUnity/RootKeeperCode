using System.Collections;
using UnityEngine;

namespace GameNTT
{
    // 캐릭터를 일정 시간 간격으로 랜덤 방향으로 돌아다니게함.
    public class RandomDirectionMove : MonoBehaviour
    {
        [SerializeField] float _startDelay = 0;
        [SerializeField] float _intervalMin = 0.5f;
        [SerializeField] float _intervalMax = 3;
        [SerializeField] float _moveSpeed = 1.2f;
        [SerializeField] float _boundaryX = 11;
        [SerializeField] float _boundaryY = 5;
        [SerializeField] MoveVector _moveVector;
        [SerializeField] ChangeAnimatorDirection _changeAnimatorDirection;

        float _currentSpeed = 0;
        Vector2 _randomVector = new Vector2(0, -1);



        private void Start()
        {
            StartCoroutine(ChangeDirectionToRandom());
        }

        void Update()
        {
            _moveVector.Move(_randomVector, _currentSpeed);
            _changeAnimatorDirection.ApplyDirection(_randomVector);
        }

        // 일정 주기로 방향을 변경함.
        IEnumerator ChangeDirectionToRandom()
        {
            yield return new WaitForSeconds(_startDelay);
            _currentSpeed = _moveSpeed;

            while (true)
            {
                _randomVector = new Vector2((Random.Range(-1f, 1f)), (Random.Range(-1f, 1f)));

                // 바운더리를 벗어날 경우 맵 중심으로 이동.
                if (_boundaryX < transform.position.x)
                {
                    _randomVector = new Vector2(-1, 0);
                }
                if (-_boundaryX > transform.position.x)
                {
                    _randomVector = new Vector2(1, 0);
                }
                if (_boundaryY < transform.position.y)
                {
                    _randomVector = new Vector2(0, -1);
                }
                if (-_boundaryY > transform.position.y)
                {
                    _randomVector = new Vector2(0, 1);
                }

                yield return new WaitForSeconds(Random.Range(_intervalMin, _intervalMax));
            }
        }
    }
}

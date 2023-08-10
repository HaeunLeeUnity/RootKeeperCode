using UnityEngine;

namespace GameNTT
{
    public class TurnAround : MonoBehaviour, MonsterState
    {
        [SerializeField]
        float _angle = 45;

        public void Handle()
        {
            transform.Rotate(0, 0, _angle * Time.deltaTime);
        }
    }
}
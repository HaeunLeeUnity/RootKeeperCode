using UnityEngine;

namespace GameNTT
{
    public class ParticlePlayOnEnable : MonoBehaviour
    {
        [SerializeField] ParticleSystem _particleSystem;
        private void OnEnable()
        {
            _particleSystem.Play();
        }
    }
}

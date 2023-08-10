#if UNITY_EDITOR
using UnityEngine;

namespace Title
{
    public class SetActiveOnUnityEditer : MonoBehaviour
    {
        [SerializeField] GameObject _gameObject;
        void Start()
        {
            _gameObject.SetActive(true);
        }
    }
}
#endif
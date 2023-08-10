using UnityEngine;

namespace GameNTT
{
    // 벌에 그림자를 생성하고 벌이 죽었을 때 그림자를 비활성화 한다.
    public class BeeShadowGenerator : MonoBehaviour
    {
        [SerializeField] GameObject beeShadows;
        GameObject _myShadow;

        void OnEnable()
        {
            if (_myShadow == null)
            {
                _myShadow = Instantiate(beeShadows);
                _myShadow.GetComponent<BeeShadow>().Target = this.gameObject;
            }
            _myShadow.SetActive(true);
        }

        private void OnDisable()
        {
            try
            {
                _myShadow.SetActive(false);
            }
            catch
            {

            }
        }
    }
}

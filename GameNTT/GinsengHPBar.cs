using UnityEngine;

namespace GameNTT
{
    public class GinsengHPBar : MonoBehaviour
    {
        [SerializeField] private GameObject[] hpSimbols;

        public void HpSimbolsInitalize(int hp)
        {
            foreach (var VARIABLE in hpSimbols)
            {
                VARIABLE.SetActive(false);
            }

            for (int i = 0; i < hp; i++)
            {
                hpSimbols[i].SetActive(true);
            }
        }
    }
}


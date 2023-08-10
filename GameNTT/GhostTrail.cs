using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameNTT
{
    // 캐릭터 속도가 빨라졌을 때 캐릭터를 따라오는 트레일 효과.
    // 일반적인 트레일 효과가 아닌 크레이지 아케이드에 샤샤샥 효과.

    public class GhostTrail : MonoBehaviour
    {

        List<GameObject> trailObject = new List<GameObject>();
        [SerializeField] GameObject trailOriginalObject;
        [SerializeField] float _trailCool = 0.05f;

        bool _isOn = false;

        public bool IsOn
        {
            set { _isOn = value; }
        }

        void Start()
        {
            StartCoroutine(TrailSpawn());
        }

        IEnumerator TrailSpawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                if (_isOn)
                {
                    var newTrail = Instantiate(trailOriginalObject, transform.position, Quaternion.identity);
                    newTrail.transform.localScale = new Vector3(6.5f, 6.5f, 1);
                    newTrail.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                    newTrail.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
                    newTrail.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0.4f);
                    trailObject.Add(newTrail);
                }

                if (trailObject.Count == 5 || !_isOn && trailObject.Count != 0)
                {
                    Destroy(trailObject[0]);
                    trailObject.RemoveAt(0);
                }

                yield return new WaitForSeconds(_trailCool);
            }
        }
    }
}
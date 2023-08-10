using UnityEngine;
namespace GameNTT
{
    // 생성된 고양이 총알의 색과 트레일의 색을 랜덤하게 초기화 한다.
    public class CatBulletHack : MonoBehaviour
    {
        [SerializeField] Sprite[] bulletSprites;
        [SerializeField] GameObject[] trails;
        void OnEnable()
        {
            foreach (GameObject trail in trails)
            {
                trail.SetActive(false);
            }
            var colorIndex = (int)Random.Range(0, bulletSprites.Length);
            GetComponent<SpriteRenderer>().sprite = bulletSprites[colorIndex];
            trails[colorIndex].SetActive(true);
        }
    }
}

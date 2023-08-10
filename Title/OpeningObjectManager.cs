using UnityEngine;
using UserData;

// 게임 타이틀 씬에서 가지고 있는 캐릭터들이 뛰어다니는 모습을 보여줌. 가지고 있는 캐릭터에 따라 표시.
namespace Title
{
    public class OpeningObjectManager : MonoBehaviour
    {
        [SerializeField] GameObject[] openingObjects;


        void Start()
        {
            foreach (GameObject openingObject in openingObjects)
            {
                openingObject.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-7, 4.5f), 0);
            }
            CharactersOn();
        }

        public void CharactersOn()
        {
            if (UserDataManager.instance != null)
            {
                for (int i = 0; i < UserDataManager.instance.PossessedCharacters.Length; i++)
                {
                    if (UserDataManager.instance.PossessedCharacters[i])
                    {
                        openingObjects[i].SetActive(true);
                    }
                }
            }
            else
            {
                openingObjects[0].SetActive(true);
            }
        }
    }
}
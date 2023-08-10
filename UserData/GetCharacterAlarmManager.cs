using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UserData
{
    // 타이틀 화면에서 획득한 캐릭터를 확인할 수 있는 알림을 표시함.
    public class GetCharacterAlarmManager : MonoBehaviour
    {
        [SerializeField] Text[] characterAlarmTexts;
        [SerializeField] GameObject[] characterAlarmGameObjects;

        void Start()
        {
            StartCoroutine(ShowAlarmDelay());
        }

        IEnumerator ShowAlarmDelay()
        {
            yield return new WaitForSeconds(0.25f);
            ShowAlarm();
        }

        public void ShowAlarm()
        {
            for (int i = 0; i < UserDataManager.instance.GetCharacter.Length; i++)
            {
                if (UserDataManager.instance.GetCharacter[i])
                {
                    var ItemGetAlarm = new LocalizedString("New Table", "UnlockedCharacter_Key");

                    var localizedCharacterName = new LocalizedString("New Table", $"CharacterName{i}");
                    var CharacterNameString = localizedCharacterName.GetLocalizedString();


                    var dict = new Dictionary<string, string>
                {{"CharacterName", CharacterNameString}};

                    ItemGetAlarm.Arguments = new object[] { dict };

                    characterAlarmGameObjects[i].SetActive(true);
                    characterAlarmTexts[i].text = ItemGetAlarm.GetLocalizedString();
                }
            }
        }

        public void ClearAlarm()
        {
            for (int i = 0; i < characterAlarmGameObjects.Length; i++)
            {
                UserDataManager.instance.ClearGetCharacterData();
                characterAlarmGameObjects[i].SetActive(false);
            }
        }
    }

}

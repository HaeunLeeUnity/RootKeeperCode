using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using GameNTT;

namespace Setting
{
    // 소리, 화면 흔들림, 자동공격, 언어의 환경 설정을 구현.
    public class SettingManager : MonoBehaviour
    {
        [SerializeField] int MasterVolume = 100;
        [SerializeField] int SFXVolume = 100;
        [SerializeField] int BGMVolume = 100;

        [SerializeField] bool CameraShakeOn = true;
        [SerializeField] bool AttackToggle = true;

        [SerializeField] Toggle cameraShakeToggle;
        [SerializeField] Toggle toggleAttackToggle;
        [SerializeField] Slider masterVolumeSlider;
        [SerializeField] Slider SFXVolumeSlider;
        [SerializeField] Slider BGMVolumeSlider;
        [SerializeField] Text masterVolumeText;
        [SerializeField] Text SFXVolumeText;
        [SerializeField] Text BGMVolumeText;

        bool localSettingActive = false;

        bool isUIInit = true;

        [SerializeField] WeaponController _weaponController;


        void Start()
        {
            Application.targetFrameRate = 120;

            // 설정에 해당하는 키가 저장되어있지 않은 경우 설정 초기값으로 초기화.
            if (!PlayerPrefs.HasKey("SFXVolume"))
            {
                FirstTimeGameOpenLanguageInitalize();
                ApplySettingsData();
                ChangeValue();
                UIInitalize();
            }
            else
            {
                LoadSettingData();
            }

            StartLocaleSetting();
        }

        // 언어를 선택함.
        public void LocaleSelect(int LocaleNumber)
        {
            if (localSettingActive) return;

            StartCoroutine(SetLocale(LocaleNumber));

            PlayerPrefs.SetInt("LocaleNumber", LocaleNumber);
            PlayerPrefs.Save();
        }

        // 언어를 변경함.
        public void StartLocaleSetting()
        {
            StartCoroutine(SetLocale(PlayerPrefs.GetInt("LocaleNumber")));
        }

        IEnumerator SetLocale(int localeID)
        {
            localSettingActive = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
            localSettingActive = false;
        }

        // 처음 게임을 열었을 때 게임 언어를 디바이스의 언어로 변경함.
        public void FirstTimeGameOpenLanguageInitalize()
        {
            SystemLanguage lang = Application.systemLanguage;

            switch (lang)
            {
                case SystemLanguage.ChineseSimplified:
                    PlayerPrefs.SetInt("LocaleNumber", 0);
                    break;
                case SystemLanguage.ChineseTraditional:
                    PlayerPrefs.SetInt("LocaleNumber", 1);
                    break;
                case SystemLanguage.English:
                    PlayerPrefs.SetInt("LocaleNumber", 2);
                    break;
                case SystemLanguage.French:
                    PlayerPrefs.SetInt("LocaleNumber", 3);
                    break;
                case SystemLanguage.German:
                    PlayerPrefs.SetInt("LocaleNumber", 4);
                    break;
                case SystemLanguage.Italian:
                    PlayerPrefs.SetInt("LocaleNumber", 5);
                    break;
                case SystemLanguage.Japanese:
                    PlayerPrefs.SetInt("LocaleNumber", 6);
                    break;
                case SystemLanguage.Korean:
                    PlayerPrefs.SetInt("LocaleNumber", 7);
                    break;
                case SystemLanguage.Polish:
                    PlayerPrefs.SetInt("LocaleNumber", 8);
                    break;
                case SystemLanguage.Portuguese:
                    PlayerPrefs.SetInt("LocaleNumber", 9);
                    break;
                case SystemLanguage.Russian:
                    PlayerPrefs.SetInt("LocaleNumber", 10);
                    break;
                case SystemLanguage.Spanish:
                    PlayerPrefs.SetInt("LocaleNumber", 11);
                    break;

                case SystemLanguage.Turkish:
                    PlayerPrefs.SetInt("LocaleNumber", 12);
                    break;
                case SystemLanguage.Vietnamese:
                    PlayerPrefs.SetInt("LocaleNumber", 13);
                    break;
                default:
                    PlayerPrefs.SetInt("LocaleNumber", 2);
                    break;
            }

            PlayerPrefs.Save();
        }

        // 설정을 불러오고 적용함.
        public void LoadSettingData()
        {
            MasterVolume = PlayerPrefs.GetInt("MasterVolume");
            SFXVolume = PlayerPrefs.GetInt("SFXVolume");
            BGMVolume = PlayerPrefs.GetInt("BGMVolume");

            if (PlayerPrefs.GetInt("CameraShake") == 1)
            {
                CameraShakeOn = true;
            }
            else
            {
                CameraShakeOn = false;
            }

            if (PlayerPrefs.GetInt("AttackToggle") == 1)
            {
                AttackToggle = true;
            }
            else
            {
                AttackToggle = false;
            }

            UIInitalize();
            ApplaySettingsToGame();
        }

        // UI 에 환경설정 데이터를 적용함.
        public void UIInitalize()
        {
            isUIInit = true;

            masterVolumeSlider.value = MasterVolume;
            SFXVolumeSlider.value = SFXVolume;
            BGMVolumeSlider.value = BGMVolume;

            masterVolumeText.text = MasterVolume.ToString();
            SFXVolumeText.text = SFXVolume.ToString();
            BGMVolumeText.text = BGMVolume.ToString();


            if (CameraShakeOn)
            {
                cameraShakeToggle.isOn = true;
            }
            else
            {
                cameraShakeToggle.isOn = false;
            }

            if (AttackToggle)
            {
                toggleAttackToggle.isOn = true;
            }
            else
            {
                toggleAttackToggle.isOn = false;
            }


            isUIInit = false;
        }

        // 환경설정 패널의 슬라이더, 토글에서 실행. 변경된 변수를 적용함.
        public void ChangeValue()
        {
            if (isUIInit) return;
            MasterVolume = (int)masterVolumeSlider.value;
            SFXVolume = (int)SFXVolumeSlider.value;
            BGMVolume = (int)BGMVolumeSlider.value;

            if (cameraShakeToggle.isOn)
            {
                CameraShakeOn = true;
            }
            else
            {
                CameraShakeOn = false;
            }


            if (toggleAttackToggle.isOn)
            {
                AttackToggle = true;
            }
            else
            {
                AttackToggle = false;
            }

            UIInitalize();
        }

        // 설정을 데이터에 적용함.
        public void ApplySettingsData()
        {
            PlayerPrefs.SetInt("MasterVolume", MasterVolume);
            PlayerPrefs.SetInt("BGMVolume", BGMVolume);
            PlayerPrefs.SetInt("SFXVolume", SFXVolume);

            if (!CameraShakeOn)
            {
                PlayerPrefs.SetInt("CameraShake", 0);
            }
            else
            {
                PlayerPrefs.SetInt("CameraShake", 1);
            }

            if (!AttackToggle)
            {
                PlayerPrefs.SetInt("AttackToggle", 0);
            }
            else
            {
                PlayerPrefs.SetInt("AttackToggle", 1);
            }

            PlayerPrefs.Save();

            ApplaySettingsToGame();
        }

        // 설정을 게임에 적용함.
        public void ApplaySettingsToGame()
        {

            float realMasterVolume = MasterVolume;
            realMasterVolume /= 100;
            AudioListener.volume = realMasterVolume;

            BGMManager.instance.ChangeVolumeValue(BGMVolume);
            SoundManager.instance.ChangeVolumeValue(SFXVolume);

            if (!CameraShakeOn)
            {
                if (CameraManager.instance != null)
                {
                    CameraManager.instance.CameraShakeOn = false;
                }
            }
            else
            {
                if (CameraManager.instance != null)
                {
                    CameraManager.instance.CameraShakeOn = true;
                }
            }


            if (!AttackToggle)
            {
                if (_weaponController != null)
                {
                    _weaponController.IsToggleAttackMode = false;
                }
            }
            else
            {
                if (_weaponController != null)
                {
                    _weaponController.IsToggleAttackMode = true;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle darkThemeToggle, vibrationToggle, soundToggle;
    [SerializeField] private GameObject darkThemeGO;
    [SerializeField] private AudioManager audioManager;
    private void Start()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        var isDarkTheme = PlayerPrefs.GetInt("Settings_DarkTheme", 0);
        var isVibration = PlayerPrefs.GetInt("Settings_Vibration", 0);
        var isSound = PlayerPrefs.GetInt("Settings_Sound", 1);

        darkThemeToggle.isOn = isDarkTheme == 0 ? false : true;
        vibrationToggle.isOn = isVibration == 0 ? false : true;
        soundToggle.isOn = isSound == 0 ? false : true;
    }

    public void SetDarkTheme(bool isOn)
    {
        PlayerPrefs.SetInt("Settings_DarkTheme", isOn ? 1 : 0);

        darkThemeGO.SetActive(isOn);
    }
    public void SetVibration(bool isOn)
    {
        PlayerPrefs.SetInt("Settings_Vibration", isOn ? 1 : 0);
    }
    public void SetSound(bool isOn)
    {
        PlayerPrefs.SetInt("Settings_Sound", isOn ? 1 : 0);
        audioManager.isAudioActive = isOn;
    }
}

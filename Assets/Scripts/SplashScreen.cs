using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Button playBtn, settingsBtn, exitBtn, backBtn;
    public GameObject mainMenu, settings;
    public Slider gameSpeed, masterVolume, musicVolume, sfxVolume;
    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        playBtn.onClick.AddListener(Play);
        settingsBtn.onClick.AddListener(Settings);
        exitBtn.onClick.AddListener(Exit);
        backBtn.onClick.AddListener(Back);

        gameSpeed.onValueChanged.AddListener(ChangeGameSpeed);
        masterVolume.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolume.onValueChanged.AddListener(ChangeMusicVolume);
        sfxVolume.onValueChanged.AddListener(ChangeSFXVolume);
    }

    private void Play()
    {
        SceneManager.LoadScene("Game");
    }

    private void Settings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void Back()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
    }

    private void ChangeGameSpeed(float value)
    {
        switch (value)
        {
            case 0:
                PlayerPrefs.SetFloat("difficulty", 0.5f);
                break;
            case 1:
                PlayerPrefs.SetFloat("difficulty", 1f);
                break;
            case 2:
                PlayerPrefs.SetFloat("difficulty", 2f);
                break;
        }
    }

    private void ChangeMasterVolume(float value)
    {
        mixer.SetFloat("masterVolume", value);
    }

    private void ChangeMusicVolume(float value)
    {
        mixer.SetFloat("musicVolume", value);
    }

    private void ChangeSFXVolume(float value)
    {
        mixer.SetFloat("sfxVolume", value);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;

    public Slider[] volumeSliders;
    public Toggle[] resolutionCheckBoxes;
    public Toggle fullscreenToggle;
    public int[] screenWidths;
    int activeScreenResIndex;

    void Start()
    {
        activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
        bool isFullScreen = PlayerPrefs.GetInt("fullscreen") == 1 ? true : false;

        for (int i = 0; i < resolutionCheckBoxes.Length; i++)
        {
            resolutionCheckBoxes[i].isOn = i == activeScreenResIndex;
        }

        fullscreenToggle.isOn = isFullScreen;
    }

    public void Play()
    {
        SceneManager.LoadScene("SupaScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void SetScreenReslution(int i)
    {
        if (resolutionCheckBoxes[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
            PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        for (int i = 0; i < resolutionCheckBoxes.Length; i++)
        {
            resolutionCheckBoxes[i].interactable = !isFullScreen;
        }
        if (isFullScreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxRes = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxRes.width, maxRes.height, true);
        }
        else
        {
            SetScreenReslution(activeScreenResIndex);
        }
        PlayerPrefs.SetInt("fullscreen", ((isFullScreen) ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float value)
    {
        //AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value)
    {
        //AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetSFXVolume(float value)
    {
        //AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.SFX);
    }
}

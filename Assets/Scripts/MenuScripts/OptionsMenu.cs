using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField]
    public Dropdown resolutionDropdown;

    [SerializeField]
    public Toggle fullscreenToggle;

    [SerializeField]
    public Slider volumeSlider;

    [SerializeField]
    public Dropdown qualityDropdown;

    Resolution[] resolutions;

    void Start()
    {
        // get the player preferences for the resolution, fullscreen, and volume
        
        int resolutionIndex = PlayerPrefs.GetInt("resolution", 0);
        int fullscreenIndex = PlayerPrefs.GetInt("fullscreen", 1);
        float volume = PlayerPrefs.GetFloat("volume", 0.75f);
        int qualityIndex = PlayerPrefs.GetInt("quality", 2);

        audioMixer.SetFloat("volume", volume);
        volumeSlider.value = volume;

        Screen.fullScreen = fullscreenIndex == 1 ? true : false;
        fullscreenToggle.isOn = fullscreenIndex == 1 ? true : false;

        // Get the resolutions from the screen and store them in the resolutions array to be used in the resolution dropdown
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        //convert the resolutions to strings
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                //set the current resolution index to the current resolution
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue(); //refresh the dropdown to show the current resolution

        resolutionDropdown.value = resolutionIndex;
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);

        QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropdown.value = qualityIndex;
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionIndex);
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    public void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        if (isFullScreen)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }
    }
}

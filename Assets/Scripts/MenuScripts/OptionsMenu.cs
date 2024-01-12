using System;
using System.Collections.Generic;
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

    [SerializeField]
    public GameObject resolutionDropdownGO;

    [SerializeField]
    public GameObject fullscreenToggleGO;

    [SerializeField]
    public GameObject volumeSliderGO;

    [SerializeField]
    public GameObject qualityDropdownGO;

    [SerializeField]
    public GameObject backButton;

    [SerializeField]
    public GameObject volumeText;

    [SerializeField]
    public GameObject resolutionText;

    [SerializeField]
    public GameObject qualityText;

    [SerializeField]
    public GameObject optionsText;

    Resolution[] resolutions;

    public string ParameterName = "MasterVolume";

    void Awake()
    {
        // get the player preferences for the resolution, fullscreen, and volume

        /*
        resolutionDropdown.enabled = false;
        fullscreenToggle.enabled = false;
        volumeSlider.enabled = false;
        qualityDropdown.enabled = false;
        */

        int resolutionIndex = PlayerPrefs.GetInt("resolution", 0);
        int fullscreenIndex = PlayerPrefs.GetInt("fullscreen", 1);
        float volume = PlayerPrefs.GetFloat("volume", 0.75f);
        int qualityIndex = PlayerPrefs.GetInt("quality", 2);

        AudioListener.volume = volume;
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

    public void activateOptionsMenuUI()
    {
        resolutionDropdownGO.SetActive(true);
        fullscreenToggleGO.SetActive(true);
        volumeSliderGO.SetActive(true);
        qualityDropdownGO.SetActive(true);
        backButton.SetActive(true);
        volumeText.SetActive(true);
        resolutionText.SetActive(true);
        qualityText.SetActive(true);
        optionsText.SetActive(true);
    }

    public void deactivateOptionsMenuUI()
    {
        resolutionDropdownGO.SetActive(false);
        fullscreenToggleGO.SetActive(false);
        volumeSliderGO.SetActive(false);
        qualityDropdownGO.SetActive(false);
        backButton.SetActive(false);
        volumeText.SetActive(false);
        resolutionText.SetActive(false);
        qualityText.SetActive(false);
        optionsText.SetActive(false);
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionIndex);
    }

    public void setVolume(float volume)
    {
        AudioListener.volume = volume;
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

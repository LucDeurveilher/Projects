using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public bool mute = false;
    public float volume = 1;
    public int resolution = 0;
    public bool FullScreen = true;

    private AudioManager audioManager;

    public List<TMP_FontAsset> fontList;
    public static event Action FontUpdated;

    List<Vector2> resolutions = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameManager.Instance.AudioManager;

        AddResolution();

        LoadAllSettings();

        SetScreenResolution(resolution);
        VolumeChange(volume);
        Mute(mute);
    }

    private void AddResolution()
    {
        resolutions.Add(new Vector2(2560, 1440));
        resolutions.Add(new Vector2(1920, 1080));
        resolutions.Add(new Vector2(1680, 1050));
        resolutions.Add(new Vector2(1280, 800));
        resolutions.Add(new Vector2(1024, 768));
    }

    public TMP_FontAsset GetFontClass(string classId)
    {
        switch (classId)
        {
            case "MenuText":
                return fontList[0];

            case "CardTitle":
                return fontList[1];

            case "CardBody":
                return fontList[2];

            case "CardBodyBold":
                return fontList[3];

            case "MenuTextBold":
                return fontList[4];

            default:
                return fontList[0];
        }
    }

    public void UpdateFont()
    {
        FontUpdated?.Invoke();
    }

    public void Mute(bool _mute)
    {
        mute = _mute;
        audioManager.Mute(mute);

        //PlayerPrefs.SetString("mute", mute.ToString());
    }

    public void VolumeChange(float newVolume)
    {
        volume = newVolume;
        audioManager.SetVolume(volume);
    }


    public void SaveAllSettings()
    {
        PlayerPrefs.SetString("mute", mute.ToString());
        PlayerPrefs.SetFloat("volume",audioManager.GetVolume());
        PlayerPrefs.SetInt("resolution", resolution);
        PlayerPrefs.SetString("fullScreen", FullScreen.ToString());
    }

    public void LoadAllSettings()
    {
        bool.TryParse(PlayerPrefs.GetString("mute", "false"), out mute);
        volume = PlayerPrefs.GetFloat("volume", 1);
        resolution = PlayerPrefs.GetInt("resolution", 0);
        bool.TryParse(PlayerPrefs.GetString("fullScreen", "true"), out FullScreen);
    }

    public void SetScreenResolution(int index)
    {
        resolution = index;
        Screen.SetResolution((int)resolutions[resolution].x, (int)resolutions[resolution].y, FullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        FullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
}

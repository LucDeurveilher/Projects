using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] Toggle toggleMute;
    [SerializeField] Slider sliderVolume;
    [SerializeField] TMP_Dropdown dropDownResolution;

    private void OnEnable()
    {
        OptionsManager optionManager = GameManager.Instance.OptionsManager;

        toggleMute.isOn = optionManager.mute;
        sliderVolume.value = optionManager.volume;
        dropDownResolution.value = optionManager.resolution;
    }

}

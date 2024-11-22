using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public bool mute = false;

    private AudioManager audioManager;

    public List<TMP_FontAsset> fontList;
    public static event Action FontUpdated;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameManager.Instance.AudioManager;
    }

    // Update is called once per frame
    void Update()
    {

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
}

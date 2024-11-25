using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtility : MonoBehaviour
{
    [SerializeField] CanvasGroup CanvasToFadeIn;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(Utility.FadeIn(CanvasToFadeIn, 1, 1));
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damage;
    CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(Utility.FadeIn(canvasGroup, 1.0f, 0.25f));
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha >= 1.0f)
        {
            StartCoroutine(Utility.FadeOut(canvasGroup, 0.0f, 0.5f));
        }
    }

    public void SetDamageText(Vector3 pos,string text)
    { 
        transform.position = pos;
        damage.text = text;
    }
}

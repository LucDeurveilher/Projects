using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] CanvasGroup gameCanvasGroup;
    [SerializeField] public List<GameObject> texts;
    [SerializeField] List<GameObject> vfx;
    private CanvasGroup canvaGroup;

    private void OnEnable()
    {
        canvaGroup = GetComponent<CanvasGroup>();
        canvaGroup.alpha = 1.0f;
        StartCoroutine(Utility.FadeOut(gameCanvasGroup, 0, 1, () => gameCanvasGroup.gameObject.SetActive(false)));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUp(int numNexus)
    {
        foreach (GameObject text in texts)
        {
            text.SetActive(false);
        }

        texts[numNexus].SetActive(true);
        vfx[numNexus].SetActive(true) ;
    }

    private void OnDisable()
    {
        foreach(GameObject obj in vfx)
        {
            obj.SetActive(false);
        }
    }
}

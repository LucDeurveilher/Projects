using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wheel : MonoBehaviour
{

    [SerializeField] float baseImplusion;
    [SerializeField] float maxTime;

    [SerializeField] TurnManager turnManager;

    [SerializeField] GameObject vfxPoof;
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI winnerText;
    float time;
    float rotationPower;

    bool wheelLauched = false;

    // Update is called once per frame
    void Update()
    {
        if (wheelLauched)
        {
            time += Time.deltaTime;

            rotationPower = Easing.BaseEasing(baseImplusion, 0, time / maxTime, Easing.EaseOutCubic);
            transform.Rotate(0, 0, rotationPower);

            if (rotationPower <=0)
            {
                EndRotationResuilt();

                StartCoroutine(Utility.PlayFonctionAfterTimer(2, () => End()));

                wheelLauched = false ;
            }
        }
    }

    public void LunchTheWheel()
    {
        time = 0;

        baseImplusion += -5 + Random.Range(0, 11);

        wheelLauched = true;
    }

    void EndRotationResuilt()
    {
        winnerText.gameObject.SetActive(true);

        if (transform.localRotation.eulerAngles.z >= 0 && transform.localRotation.eulerAngles.z < 45)
        {
            turnManager.starter = 0;
            winnerText.text = "Player Start !";
            Debug.Log("Player");
        }
        else if (transform.localRotation.eulerAngles.z >= 45 && transform.localRotation.eulerAngles.z < 135)
        {
            turnManager.starter = 1;
            winnerText.text = "AI Start !";
            Debug.Log("AI");
        }
        else if (transform.localRotation.eulerAngles.z >= 135 && transform.localRotation.eulerAngles.z < 225)
        {
            turnManager.starter = 0;
            winnerText.text = "Player Start !";
            Debug.Log("Player");
        }
        else if (transform.localRotation.eulerAngles.z >= 225 && transform.localRotation.eulerAngles.z < 315)
        {
            turnManager.starter = 1;
            winnerText.text = "AI Start !";
            Debug.Log("AI");
        }
        else if (transform.localRotation.eulerAngles.z >= 315 && transform.localRotation.eulerAngles.z < 360)
        {
            turnManager.starter = 0;
            winnerText.text = "Player Start !";
            Debug.Log("Player");
        }

        turnManager.RandomStarter();
    }

    void End()
    {
        CloseWheel();
        winnerText.gameObject.SetActive(false);
        Destroy(Instantiate(vfxPoof), 3);
    }

    public void OpenWheel()
    {
        animator.SetTrigger("Open");
    }

    public void CloseWheel()
    {
        animator.SetTrigger("Close");
    }
}

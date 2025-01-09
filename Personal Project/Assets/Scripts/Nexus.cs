using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [SerializeField] private GameObject UiPrefab;
    private GameObject Ui;
    private HealthUI healthUI;

    public int maxHealth = 100;
    private int health = 100;
    [SerializeField] public Transform attackZone;

    [SerializeField] List<GameObject> deadFx = new List<GameObject>();

    [SerializeField] GameObject endGameCanvas;

    public int isAllieNexus = 0;

    private void Start()
    {
        GameManager.Instance.ResetGame += ResetNexus;
        health = maxHealth;

        Ui = Instantiate(UiPrefab,transform);
        Ui.transform.localScale = new Vector3(2, 2, 1);

        healthUI = Ui.GetComponent<HealthUI>();

        healthUI.UpdateUi(health, maxHealth);
    }

    public void HitNexus(int damages)
    {
        health -= damages;
        health = Mathf.Max(health, 0);

        if (health <= 0 && !GameManager.Instance.GameEnded)
        {
            Die();
        }

        healthUI.UpdateUi(health, maxHealth);
    }

    private void Die()
    {
        GameManager.Instance.GameEnded = true;

        foreach (GameObject fx in deadFx)
        {
            Destroy(Instantiate(fx, gameObject.transform), 2);
        }

        StartCoroutine(Utility.PlayFonctionAfterTimer(2, () => ActiveCanvaEndGame()));
    }

    void ActiveCanvaEndGame()
    {
        if (endGameCanvas != null)
        {
            endGameCanvas.SetActive(true);
            endGameCanvas.GetComponent<EndGame>().SetUp(isAllieNexus);
        }
    }

    public void ResetNexus()
    {
        health = maxHealth;

        if (Ui == null)
        {
            Ui = Instantiate(UiPrefab, transform);
            Ui.transform.localScale = new Vector3(2, 2, 1);
            healthUI = Ui.GetComponent<HealthUI>();
        }

        healthUI.UpdateUi(health, maxHealth);
    }
}

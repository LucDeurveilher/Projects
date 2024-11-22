using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [SerializeField] private GameObject Ui;
    private HealthUI healthUI;

    public int maxHealth = 100;
    private int health = 100;
    [SerializeField] Transform attackZone;

    private void Start()
    {
        health = maxHealth;

        healthUI = Ui.GetComponent<HealthUI>();

        healthUI.UpdateUi(health,maxHealth);
    }

    public void HitNexus(int damages)
    {
        health -= damages;
        health = Mathf.Max(health, 0);

        if (health <= 0)
        {
            Die();
        }

        healthUI.UpdateUi(health, maxHealth);
    }

    private void Die()
    {
        //Destroy(gameObject);
        Debug.Log("I died");
    }
}

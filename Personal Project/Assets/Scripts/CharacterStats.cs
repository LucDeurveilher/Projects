using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;
using static CharacterStats;
using static UnityEngine.GraphicsBuffer;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private GameObject UiPrefab;
    public HealthUI healthUI;

    public Character characterStartData;

    public string characterName;
    public List<CardType> cardElementType = new();
    public int baseMaxHealth;
    public int maxHealth;
    public int health;
    public int damageMin;
    public int damageMax;
    public List<DamageType> damageType = new();//if attack have a effect => != None
    public int range;
    public AttackPattern attackPattern;
    public PriorityTarget priorityTarget;

    private bool statsSet = false;

    Animator animator;

    //Effects
    EffectApplier effectApplier;

    public bool isBleeding = false;
    public bool isPoisoned = false;
    public bool isFreeze = false;

    public bool isAllie = false;

    private bool isDied = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        GameObject temp = Instantiate(UiPrefab, transform.position + (Vector3.down / 6), Quaternion.identity);

        healthUI = temp.GetComponent<HealthUI>();

        effectApplier = GetComponent<EffectApplier>();
    }

    private void Update()
    {
        if (!statsSet && characterStartData != null)
        {
            SetStartStats();
        }

        if (maxHealth < health)
        {
            maxHealth = health;
            healthUI.UpdateUi(health, maxHealth);
        }

        Die();
    }

    private void SetStartStats()
    {
        characterName = characterStartData.name;
        foreach (CardType resourcesCardTypeype in characterStartData.cardType)
        {
            CardType temp = resourcesCardTypeype;
            cardElementType.Add(temp);
        }
        //cardElementType = characterStartData.cardType;
        health = characterStartData.health;
        baseMaxHealth = health;
        maxHealth = health;
        damageMin = characterStartData.damageMin;
        damageMax = characterStartData.damageMax;

        foreach (DamageType resourcesDamageType in characterStartData.damageType)
        {
            DamageType temp = resourcesDamageType;
            damageType.Add(temp);
        }
        //damageType = characterStartData.damageType;
        range = characterStartData.range;
        attackPattern = characterStartData.attackPattern;
        priorityTarget = characterStartData.priorityTarget;
        isAllie = characterStartData.isAllie;

        statsSet = true;

        healthUI.UpdateUi(health, maxHealth);
    }

    void Die()
    {
        if (health <= 0)
        {
            isDied = true;
            healthUI.UpdateUi(health, maxHealth);
            animator.SetTrigger("Die");
        }
    }

    public void Hit()
    {
        healthUI.UpdateUi(health, maxHealth);

        if (health > 0)
        {
            animator.SetTrigger("Hurt");
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void DoDamage(CharacterStats target, DamagePopUp popUp)
    {
        int damage = (int)(Random.Range(damageMin, damageMax + 1) * (isPoisoned ? 0.75 : 1));
        target.health -= damage;
        target.health = Mathf.Max(target.health, 0);

        //Apply effects
        AddEffectsOnAttack(target);

        popUp.SetDamageText(target.transform.position + Vector3.up, damage.ToString());
    }

    private void AddEffectsOnAttack(CharacterStats target)
    {
        foreach (DamageType type in damageType)
        {
            if (type != DamageType.None)
            {
                target.effectApplier.AddEffect(type, 2);
            }
        }
    }

    private void OnDestroy()
    {
        if (healthUI.gameObject != null && !isDied)
            healthUI.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;
using static CharacterStats;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private GameObject UiPrefab;
    public HealthUI healthUI;

    public Character characterStartData;

    public string characterName;
    public List<CardType> cardElementType = new();
    public int maxHealth;
    public int health;
    public int damageMin;
    public int damageMax;
    public List<DamageType> damageType = new();//if attack have a effect => != None
    
    ///new
    public int critChance;
    public int critDamage;
    public int dodgeChance;
    public int effectBoost;
    ///new
    
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
        dodgeChance = characterStartData.dodgeChance;
        maxHealth = health;
        damageMin = characterStartData.damageMin;
        damageMax = characterStartData.damageMax;
        critChance = characterStartData.critChance;
        critDamage = characterStartData.critDamage;

        foreach (DamageType resourcesDamageType in characterStartData.damageType)
        {
            DamageType temp = resourcesDamageType;
            damageType.Add(temp);
        }
        effectBoost = characterStartData.effectBoost;
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

    public void ApplyDamage(int damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0);
    }

    public void DoDamage(CharacterStats target, DamagePopUp popUp)
    {
        int dodge = (int)(Random.Range(0, 100 + 1));

        if (dodge >= target.dodgeChance)//target didnt dodge
        {
            int crit = (int)(Random.Range(0, 100 + 1));
            bool isCrit = false;

            if (crit <= critChance)//is the attack a critOne ?
            {
                isCrit = true;
            }

            int damage = (int)(Random.Range(damageMin, damageMax + 1) * (isPoisoned ? 0.75 : 1));
            damage = damage * (isCrit ? 1 + (critDamage / 100) : 1);

            target.ApplyDamage(damage);

            //Apply effects
            AddEffectsOnAttack(target);

            popUp.SetDamageText(transform.position + Vector3.up, damage.ToString() + (isCrit? " !":""), isCrit ? Color.red : Color.white);
        }
        else
        {
            popUp.SetDamageText(target.transform.position + Vector3.up, "Dodge !",Color.blue);
        }
    }

    private void AddEffectsOnAttack(CharacterStats target)
    {
        foreach (DamageType type in damageType)
        {
            if (type != DamageType.None)
            {
                target.effectApplier.AddEffect(type,effectBoost);
            }
        }
    }

    private void OnDestroy()
    {
        if (healthUI.gameObject != null && !isDied)
            healthUI.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;
using static UnityEngine.GraphicsBuffer;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private GameObject UiPrefab;
    public HealthUI healthUI;

    public Character characterStartData;

    public string characterName;
    public List<CardType> cardElementType;
    public int maxHealth;
    public int health;
    public int damageMin;
    public int damageMax;
    public List<DamageType> damageType;
    public int range;
    public AttackPattern attackPattern;
    public PriorityTarget priorityTarget;

    private bool statsSet = false;

    Animator animator;

    public class Effect
    {
        public DamageType type;
        public int numberOfTurn;
    }

    public List<Effect> effects = new List<Effect>();
    public bool isBleeding = false;
    public bool isPoisoned = false;
    public bool isFreeze = false;

    VFXManager vFXManager;
    GameObject bloodDropVfx = null;
    GameObject freezeVfx = null;
    GameObject poisonVfx = null;

    public bool isAllie = false;

    private void Start()
    {
        TurnManager.EffectTurn += ApplyEffects;

        animator = gameObject.GetComponent<Animator>();
        vFXManager = FindObjectOfType<VFXManager>();

        GameObject temp =Instantiate(UiPrefab,transform.position + (Vector3.down/6),Quaternion.identity);

        healthUI = temp.GetComponent<HealthUI>();
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
            healthUI.UpdateUi(health,maxHealth);
        }

        Die();
    }

    private void SetStartStats()
    {
        characterName = characterStartData.name;
        cardElementType = characterStartData.cardType;
        health = characterStartData.health;
        maxHealth = health;
        damageMin = characterStartData.damageMin;
        damageMax = characterStartData.damageMax;
        damageType = characterStartData.damageType;
        range = characterStartData.range;
        attackPattern = characterStartData.attackPattern;
        priorityTarget = characterStartData.priorityTarget;
        isAllie = characterStartData.isAllie;

        statsSet = true;

        healthUI.UpdateUi(health, maxHealth);
    }

    private void ApplyEffects()
    {
        int bleed = 0;
        int poison = 0;
        int freeze = 0;

        foreach (Effect effect in effects)
        {
            switch (effect.type)
            {
                case DamageType.None:
                    break;
                case DamageType.Bleed:

                    if (!isBleeding)
                    {
                        bloodDropVfx = vFXManager.PlayBloodDrop(transform);
                        isBleeding = true;
                    }

                    vFXManager.PlayBleed(transform.position + (Vector3.up / 2), 1f);
                    health--;

                    bleed++;
                    break;
                case DamageType.Poison:

                    if (!isPoisoned)
                    {
                        poisonVfx = vFXManager.PlayPoison(transform);
                        isPoisoned = true;
                    }

                    poison++;
                    break;
                case DamageType.Freeze:

                    if (!isFreeze)
                    {
                        freezeVfx = vFXManager.PlayFreeze(transform);
                        isFreeze = true;
                    }

                    freeze++;
                    break;
                default:
                    break;
            }
            effect.numberOfTurn--;
        }

        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (effects[i].numberOfTurn <= 0)
            {
                effects.RemoveAt(i);
            }
        }

        if (bleed == 0)
        {
            isBleeding = false;
            Destroy(bloodDropVfx);
        }

        if (poison == 0)
        {
            isPoisoned = false;
            Destroy(poisonVfx);
        }

        if (freeze == 0)
        {
            isFreeze = false;
            Destroy(freezeVfx);
        }

        healthUI.UpdateUi(health, maxHealth);
    }

    void Die()
    {
        if (health <= 0)
        {
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
        AddEffects(target);

        popUp.SetDamageText(target.transform.position + Vector3.up, damage.ToString());
    }

    private void AddEffects(CharacterStats target)
    {
        foreach (DamageType type in damageType)
        {
            if (type != DamageType.None)
            {
                Effect effect = new Effect();
                effect.type = type; effect.numberOfTurn = 2;
                target.effects.Add(effect);
            }
        }
    }

    private void OnDestroy()
    {
        TurnManager.EffectTurn -= ApplyEffects;
        Destroy(bloodDropVfx);
        Destroy(freezeVfx);
        Destroy(poisonVfx);
    }
}

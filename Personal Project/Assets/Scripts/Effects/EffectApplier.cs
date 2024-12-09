using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class EffectApplier : MonoBehaviour
{
    CharacterStats stats;

    public class Effect
    {
        public DamageType type;
        public int numberOfTurn;
    }

    public List<Effect> effects = new List<Effect>();

    VFXManager vFXManager;
    GameObject bloodDropVfx = null;
    GameObject freezeVfx = null;
    GameObject poisonVfx = null;

    // Start is called before the first frame update
    void Start()
    {
        vFXManager = FindObjectOfType<VFXManager>();

        stats = GetComponent<CharacterStats>();
        TurnManager.EffectTurn += ApplyEffects;
    }

    private void ApplyEffects()
    {
        int bleed = 0;
        int poison = 0;
        int freeze = 0;

        //Particular effect after the attack turn
        foreach (Effect effect in effects)
        {
            switch (effect.type)
            {
                case DamageType.None:
                    break;
                case DamageType.Bleed:

                    stats.health--;

                    bleed++;
                    break;
                case DamageType.Poison:

                    poison++;
                    break;
                case DamageType.Freeze:

                    freeze++;
                    break;
                default:
                    break;
            }

            effect.numberOfTurn--;
        }

        //Remove effects when it's done
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (effects[i].numberOfTurn <= 0)
            {
                effects.RemoveAt(i);
            }
        }

        if (bleed == 0)
        {
            stats.isBleeding = false;
            Destroy(bloodDropVfx);
        }

        if (poison == 0)
        {
            stats.isPoisoned = false;
            Destroy(poisonVfx);
        }

        if (freeze == 0)
        {
            stats.isFreeze = false;
            Destroy(freezeVfx);
        }

        stats.healthUI.UpdateUi(stats.health, stats.maxHealth);
    }

    public void AddEffect(DamageType type, int numberOfTurn)
    {
        Effect effect = new Effect();
        effect.type = type; effect.numberOfTurn = 2;

        effects.Add(effect);

        InstantiateEffect(effect);
    }

    public void InstantiateEffect(Effect effect)
    {

        switch (effect.type)
        {
            case DamageType.None:
                break;
            case DamageType.Bleed:

                if (!stats.isBleeding)
                {
                    bloodDropVfx = vFXManager.PlayBloodDrop(transform);
                    stats.isBleeding = true;
                }

                vFXManager.PlayBleed(transform.position + (Vector3.up / 2), 1f);
                break;
            case DamageType.Poison:

                if (!stats.isPoisoned)
                {
                    poisonVfx = vFXManager.PlayPoison(transform);
                    stats.isPoisoned = true;
                }
                break;
            case DamageType.Freeze:

                if (!stats.isFreeze)
                {
                    freezeVfx = vFXManager.PlayFreeze(transform);
                    stats.isFreeze = true;
                }
                break;
            default:
                break;
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

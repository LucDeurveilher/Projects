using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Card;
using static UnityEngine.GraphicsBuffer;

public class AttackManager : MonoBehaviour
{
    GridManager gridManager;
    [SerializeField] GameObject attackPopUp;
    [SerializeField] VFXManager VfxManager;
    public List<GameObject> allies = new List<GameObject>();
    public List<GameObject> ennemies = new List<GameObject>();
    [SerializeField] Nexus[] nexus = new Nexus[2];

    int totalAttacks = 0;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    //Find allCharacters in the grid and put them in differents list
    private void SetAlliesEnnemies()
    {
        allies.Clear();
        ennemies.Clear();

        foreach (GameObject obj in gridManager.gridObjects)
        {
            CharacterStats temp = obj.GetComponent<CharacterStats>();

            if (temp.isAllie)
            {
                allies.Add(obj);
            }
            else
            {
                ennemies.Add(obj);
            }
        }
    }

    public void StartAttackTurn(bool turnPlayer, Action onComplete = null)
    {
        totalAttacks = 0;
        StartCoroutine(AttackTurn(turnPlayer, onComplete));
    }

    private IEnumerator AttackTurn(bool playerTurnToAttack, Action onComplete = null)
    {
        SetAlliesEnnemies();

        List<GameObject> listAttackers = playerTurnToAttack ? allies : ennemies;
        List<GameObject> listVictims = playerTurnToAttack ? ennemies : allies;

        foreach (GameObject obj in listAttackers)
        {
            GameObject attacker = obj;
            CharacterStats attackerStats = attacker.GetComponent<CharacterStats>();
            bool attackerFreeze = attackerStats.isFreeze;

            // is attacker destroy or freezed
            if (attacker == null || attackerFreeze)
            {
                continue;
            }

            List<GameObject> targets = new();

            if (listVictims.Count > 0)
            {
                targets = FindTargets(attacker, listVictims);
            }

            if (targets.Count > 0)
            {
                StartCoroutine(Utility.TranslateGameObject(attacker, targets[0].transform, 0.2f, true, () => Attack.PlayAnimationAttackAndHit(attacker, targets[0]), 0.6f));//move and play animations

                AttackTargets(attackerStats, targets);

                CheckDiedPeople(listVictims);
            }
            else//attack the nexus cause no enemies left
            {
                GameObject popUp = Instantiate(attackPopUp);
                DamagePopUp damagePopUp = popUp.GetComponent<DamagePopUp>();

                Attack.DoDamageNexus(nexus[playerTurnToAttack ? 1 : 0], attacker, damagePopUp);
            }

            yield return new WaitForSeconds(1);
        }

        totalAttacks++;
        if (totalAttacks < 2)
        {
            //opponent attack
            StartCoroutine(AttackTurn(!playerTurnToAttack, onComplete));
        }
        else
        {
            //applies effects
            onComplete?.Invoke();

            CheckDiedPeople(allies);
            CheckDiedPeople(ennemies);

            GameManager.Instance.CardPlayed = false;
        }

        yield return null;
    }

    public List<GameObject> FindTargets(GameObject attackerInGrid, List<GameObject> victims)
    {
        List<GameObject> target = new();
        PriorityTarget priorityTarget = attackerInGrid.GetComponent<CharacterStats>().priorityTarget;
        AttackPattern attackPattern = attackerInGrid.GetComponent<CharacterStats>().attackPattern;

        //Principal target
        switch (priorityTarget)
        {
            case PriorityTarget.Close:
                target.Add(Attack.FindCloseTarget(attackerInGrid, victims));
                break;
            case PriorityTarget.Far:
                target.Add(Attack.FindFarTarget(attackerInGrid, victims));
                break;
            case PriorityTarget.LeastCurrentHealth:
                target.Add(Attack.FindLeastCurrentHealthTarget(attackerInGrid, victims));
                break;
            case PriorityTarget.MostCurrentHealth:
                target.Add(Attack.FindMostCurrentHealthTarget(attackerInGrid, victims));
                break;
            default:
                break;
        }

        GridCell cellFirstVictim = gridManager.GetGridCellByObjectIn(target.First());

        //Second targets
        switch (attackPattern)
        {
            case AttackPattern.Single:
                //nothing to add
                break;
            case AttackPattern.MultiTarget:
                target.AddRange(Attack.MultiPattern(cellFirstVictim, victims));
                break;
            case AttackPattern.Cross:
                target.AddRange(Attack.CrossPattern(cellFirstVictim, gridManager));
                break;
            case AttackPattern.Column:
                target.AddRange(Attack.ColumnPattern(cellFirstVictim, gridManager));
                break;
            case AttackPattern.Row:
                target.AddRange(Attack.RowPattern(cellFirstVictim, gridManager));
                break;
            case AttackPattern.TwoByTwo:
                break;
            case AttackPattern.FourByFour:
                break;
            default:
                break;
        }
        return target;
    }

    void AttackTargets(CharacterStats attacker, List<GameObject> targets)
    {
        foreach (GameObject target in targets)
        {
            StartCoroutine(Utility.PlayFonctionAfterTimer(0.3f, () => VfxManager.PlayVFX((int)attacker.cardElementType[0], target.transform.position, 2f)));
            GameObject popUp = Instantiate(attackPopUp);
            DamagePopUp damagePopUp = popUp.GetComponent<DamagePopUp>();

            Attack.DoDamage(attacker, target, damagePopUp);
        }
    }
    private void CheckDiedPeople(List<GameObject> victims)
    {
        for (int i = victims.Count - 1; i >= 0; i--)
        {
            CharacterStats stats = victims[i].GetComponent<CharacterStats>();
            if (stats.health <= 0)
            {
                GameObject victim = victims[i];
                victims.Remove(victim);
                gridManager.RemoveObjectToGrid(victim);
            }
        }
    }
}

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
                //move and play animations
                StartCoroutine(Utility.ReturnTrip(attacker, targets[0].transform, 0.45f, () => ApplyDamage(attacker,targets, listVictims), 0.6f,Easing.EaseInOutExpo));
            }
            else//attack the nexus cause no enemies left
            {
                Nexus victimNexus = nexus[playerTurnToAttack ? 1 : 0];

                StartCoroutine(Utility.ReturnTrip(attacker, victimNexus.attackZone, 0.45f, () => AttackNexus(victimNexus, attacker), 0.6f, Easing.EaseInOutExpo));
               
            }

            if (GameManager.Instance.GameEnded)
            {
                onComplete?.Invoke();
                CheckDiedPeople(allies);
                CheckDiedPeople(ennemies);

                GameManager.Instance.CardPlayed = false;
                StopAllCoroutines();
            }

            yield return new WaitForSeconds(1.5f);
        }

        totalAttacks++;
        if (totalAttacks < 2)
        {
            //opponent attack
            StartCoroutine(AttackTurn(!playerTurnToAttack, onComplete));
        }
        else
        {
            //applies effects and end Attack turn
            onComplete?.Invoke();

            CheckDiedPeople(allies);
            CheckDiedPeople(ennemies);

            GameManager.Instance.CardPlayed = false;
        }

        yield return null;
    }

    public void ApplyDamage(GameObject attacker, List<GameObject> targets, List<GameObject> victims)
    {
        CharacterStats attackerStats = attacker.GetComponent<CharacterStats>();

        AttackTargets(attackerStats, targets);

        Attack.PlayAnimationAttackAndHit(attacker, targets);

        CheckDiedPeople(victims);
    }

    public List<GameObject> FindTargets(GameObject attackerInGrid, List<GameObject> victims)
    {
        List<GameObject> target = new();
        AttackPattern attackPattern = attackerInGrid.GetComponent<CharacterStats>().attackPattern;

        //Principal target
        target.Add(Attack.FindAttackPriorityTarget(attackerInGrid, victims));

        GridCell cellFirstVictim = gridManager.GetGridCellByObjectIn(target.First());

        //Second targets
        target.AddRange(Attack.FindVictimsAttackPattern(attackerInGrid,cellFirstVictim, gridManager, victims));

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

    void AttackNexus(Nexus nexus,GameObject attacker)
    {
        GameObject popUp = Instantiate(attackPopUp);
        DamagePopUp damagePopUp = popUp.GetComponent<DamagePopUp>();

        Attack.DoDamageNexus(nexus, attacker, damagePopUp, VfxManager);
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

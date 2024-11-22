using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AIBase : MonoBehaviour
{
    //Analize actualState of the game

    int totalHealthPointAICards;
    int totalHealthPointPlayerCards;
    List<GameObject> priorityTargetList = new List<GameObject>();
    List<GameObject> priorityDefendList = new List<GameObject>();

    Dictionary<GameObject, Vector2> cardToPlay = new Dictionary<GameObject, Vector2>();

    private void AnalizeGrid()
    {
        //Initialize
        cardToPlay.Clear();
        priorityTargetList.Clear();
        priorityDefendList.Clear();

        // Récupérer tous les personnages sur le plateau
        List<CharacterStats> allCharacters = new List<CharacterStats>();

        foreach (GameObject obj in gridManager.gridObjects)
        {
            if (obj != null)
            {
                CharacterStats character = obj.GetComponent<CharacterStats>();
                if (character != null)
                {
                    allCharacters.Add(character);
                }
            }
        }

        // Séparer les alliés et les ennemis
        SeparateAlliesAndEnemies(allCharacters);
    }

    private void SeparateAlliesAndEnemies(List<CharacterStats> allCharacters)
    {
        allies.Clear();
        ennemies.Clear();

        foreach (CharacterStats character in allCharacters)
        {
            if (!character.isAllie)
            {
                allies.Add(character.gameObject);
            }
            else
            {
                ennemies.Add(character.gameObject);
            }
        }

        // Appelle une méthode pour évaluer les menaces et les opportunités
        EvaluateThreatsAndOpportunities();
    }

    private void EvaluateThreatsAndOpportunities()
    {
        foreach (GameObject enemy in ennemies)
        {
            CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
            int potentialDamage = CalculatePotentialDamage(enemyStats, enemyStats.health);

            // Vérifie si l'ennemi peut mourir avec les dégâts potentiels
            if (potentialDamage >= enemyStats.health)
            {
                MarkAsPriorityTarget(enemy);
            }
        }

        foreach (GameObject ally in allies)
        {
            CharacterStats allyStats = ally.GetComponent<CharacterStats>();
            int potentialDamage = CalculatePotentialTakeDamage();

            if (potentialDamage >= allyStats.health)
            {
                MarkAsPriorityDefence(ally);
            }
        }

        ChooseNextAction();
    }

    private int CalculatePotentialDamage(CharacterStats enemyStats, int damageToDo)
    {
        int totalPotentialDamage = 0;

        foreach (GameObject card in handManager.cardsInAIHand)
        {
            if (cardToPlay.ContainsKey(card))
                continue;

            if (damageToDo <= totalPotentialDamage)
                return totalPotentialDamage;

            Card cardData = card.GetComponent<CardDisplay>().cardData;

            if (cardData is Character characterCard)
            {
                // Estime les dégâts d'un personnage
                totalPotentialDamage += characterCard.damageMin;
                Vector2 pos;
                do
                {
                    pos = new Vector2(Random.Range(4, 8), Random.Range(0, 4));
                } while (!IsPositionCellValid(pos));

                cardToPlay.Add(card, pos);
            }
        }

        return totalPotentialDamage;
    }

    private int CalculatePotentialTakeDamage()
    {
        int totalPotentialDamage = 0;

        foreach (GameObject card in ennemies)
        {
            int damage = card.GetComponent<CharacterStats>().damageMin;

             totalPotentialDamage += damage;
            
        }

        return totalPotentialDamage;
    }

    private void MarkAsPriorityTarget(GameObject enemyStats)
    {
        //Debug.Log($"AI : I can kill {enemyStats.name}");
        priorityTargetList.Add(enemyStats);
    }

    private void MarkAsPriorityDefence(GameObject allyToDef)
    {
        //Debug.Log($"AI : They can kill {allyToDef.name}");
        priorityDefendList.Add(allyToDef);
    }

    private void SetUpDefense()
    {
        List<GameObject> placed = new List<GameObject>();

        foreach (GameObject allyToDef in priorityDefendList)
        {
            GridCell cell = null;

            foreach (GameObject obj in gridManager.gridCells)
            {
                GridCell objCell = obj.GetComponent<GridCell>();
                if (objCell.objectInCell == allyToDef)
                {
                    cell = objCell;
                    break;
                }
            }

            Vector2 posDef = cell.gridIndex;
            posDef.x -= 1;

            //check if the position is defendable
            if (Utility.CheckValueBetween(posDef.x, 4, 7) && Utility.CheckValueBetween(posDef.y, 0, 3))
            {
                foreach (GameObject key in cardToPlay.Keys)
                {
                    //if he is not already defending
                    if (!placed.Contains(key))
                    {
                        cardToPlay[key] = posDef;
                        placed.Add(key);
                        break;
                    }
                }
            }
        }

        placed.Clear();
    }

    private void ChooseNextAction()
    {
        if (priorityTargetList.Count > 0 || priorityDefendList.Count > 0)
        {
            SetUpDefense();
            ChooseCard();
        }
        else
        {
            DrawCardAI();
        }
    }
}

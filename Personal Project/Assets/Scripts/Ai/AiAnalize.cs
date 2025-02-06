using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

//Before new Script
public partial class AIBase : MonoBehaviour
{
    //Analize actualState of the game

    int totalHealthPointAICards;
    int totalHealthPointPlayerCards;
    List<GameObject> priorityTargetList = new List<GameObject>();
    List<GameObject> priorityDefendList = new List<GameObject>();

    List<GameObject>CardInHand = new List<GameObject>();

    Dictionary<GameObject, Vector2> cardToPlay = new Dictionary<GameObject, Vector2>();

    private void AnalizeGrid()
    {
        //Initialize
        cardToPlay.Clear();
        priorityTargetList.Clear();
        priorityDefendList.Clear();
        CardInHand.Clear();

        // R�cup�rer tous les personnages sur le plateau
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

        // S�parer les alli�s et les ennemis
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

        //my board empty ?
        if (allies.Count <= 0)
        {
            SetUpBoardBase();
        }
        else
        {
            // Appelle une m�thode pour �valuer les menaces et les opportunit�s
            EvaluateThreatsAndOpportunities();
        }

    
    }

    private Dictionary<Trait, List<GameObject>> GetTraitsInHand()
    {
        Dictionary<Trait, List<GameObject>> ActivateTraits = new Dictionary<Trait, List<GameObject>>();
        CardInHand = handManager.cardsInAIHand;

        foreach (GameObject cardGameObj in CardInHand)
        {
            Card cardData = cardGameObj.GetComponent<CardDisplay>().cardData;

            Character character = cardData as Character;

            if (character != null)
            {
                foreach (Trait trait in traitManager.GetAllTraitsOfThisCharacter(character))
                {
                    if (ActivateTraits.ContainsKey(trait))
                    {
                        ActivateTraits[trait].Add(cardGameObj);
                    }
                    else
                    {
                        ActivateTraits.Add(trait, new List<GameObject> { cardGameObj });
                    }
                }
            }
        }

        return ActivateTraits;
    }

    private void SetUpBoardBase()
    {
        Dictionary<Trait, List<GameObject>> tempTraits = GetTraitsInHand();

        // Trier les traits par le nombre de cartes associ�es (ordre croissant)
        var sortedTraits = tempTraits.OrderBy(entry => entry.Value.Count).ToList();

        int maxCardsToPlay = 3; // On veut 3 cartes max
        int cardsAdded = 0;

        foreach (var (trait, cartesAssociees) in sortedTraits)
        {
            foreach (GameObject carte in cartesAssociees)
            {
                if (cardsAdded >= maxCardsToPlay)
                    break; // Stop si on a d�j� ajout� 3 cartes

                Debug.Log($"Ajout de la carte {carte} du trait {trait}");

                // G�n�rer une position valide
                Vector2 pos;
                do
                {
                    pos = new Vector2(Random.Range(4, 8), Random.Range(0, 4));
                } while (!IsPositionCellValid(pos));

                // Ajouter � cardToPlay
                cardToPlay.Add(carte, pos);
                cardsAdded++;
            }

            if (cardsAdded >= maxCardsToPlay)
                break; // Stop apr�s avoir ajout� 3 cartes
        }

        ChooseCard();//PlayCards
    }

    private void EvaluateThreatsAndOpportunities()
    {
        foreach (GameObject enemy in ennemies)
        {
            CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
            int potentialDamage = CalculatePotentialDamage(enemyStats, enemyStats.health);

            // V�rifie si l'ennemi peut mourir avec les d�g�ts potentiels
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
                // Estime les d�g�ts d'un personnage
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

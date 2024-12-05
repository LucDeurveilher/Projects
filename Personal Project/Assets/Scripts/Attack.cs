using System.Collections.Generic;
using System.Linq;
using UnityEngine;


static public class Attack
{
    static public void DoDamage(CharacterStats attacker, GameObject target, DamagePopUp popUp)
    {
        CharacterStats targetStats = target.GetComponent<CharacterStats>();

        attacker.DoDamage(targetStats, popUp);
    }

    static public void DoDamageNexus(Nexus nexus, GameObject attacker, DamagePopUp popUp)
    {
        CharacterStats attackerStats = attacker.GetComponent<CharacterStats>();

        int damage = Random.Range(attackerStats.damageMin, attackerStats.damageMax + 1);
        nexus.HitNexus(damage);

        popUp.SetDamageText(nexus.transform.position + Vector3.up, damage.ToString());
        attackerStats.Attack();
    }

    public static void PlayAnimationAttackAndHit(GameObject attacker, List<GameObject> target)
    {
        CharacterStats attackerStats = attacker.GetComponent<CharacterStats>();

        foreach (GameObject targetItem in target)
        {
            CharacterStats targetStats = targetItem.GetComponent<CharacterStats>();
            targetStats.Hit();
        }
      
        attackerStats.Attack();
    }

    //AttackPriority
    public static GameObject FindCloseTarget(GameObject attacker, List<GameObject> victims)
    {
        return victims
            .Where(victim => victim != attacker)
            .OrderBy(victim => Vector2.Distance(attacker.transform.position, victim.transform.position))
            .FirstOrDefault();
    }

    public static GameObject FindFarTarget(GameObject attacker, List<GameObject> victims)
    {
        return victims
            .Where(victim => victim != attacker)
            .OrderByDescending(victim => Vector2.Distance(attacker.transform.position, victim.transform.position))
            .FirstOrDefault();
    }

    public static GameObject FindLeastCurrentHealthTarget(GameObject attacker, List<GameObject> victims)
    {
        return victims
            .Where(victim => victim != attacker)
            .OrderBy(victim => victim.GetComponent<CharacterStats>().health)
            .FirstOrDefault();
    }

    public static GameObject FindMostCurrentHealthTarget(GameObject attacker, List<GameObject> victims)
    {
        return victims
            .Where(victim => victim != attacker)
            .OrderByDescending(victim => victim.GetComponent<CharacterStats>().health)
            .FirstOrDefault();
    }

    //Attack Pattern
    public static List<GameObject> MultiPattern(GridCell victimCell, List<GameObject> victims)
    {
        List<GameObject> list = new List<GameObject>();
        GameObject temp = FindCloseTarget(victimCell.objectInCell, victims);

        if (temp != null)
        {
            list.Add(temp);
        }

        return list;
    }

    public static List<GameObject> CrossPattern(GridCell victimCell, GridManager gridManager)
    {
        List<GameObject> list = new();
        List<Vector2> posCross = new List<Vector2>
    {
        new Vector2(victimCell.gridIndex.x, victimCell.gridIndex.y + 1), // Haut
        new Vector2(victimCell.gridIndex.x, victimCell.gridIndex.y - 1), // Bas
        new Vector2(victimCell.gridIndex.x - 1, victimCell.gridIndex.y), // Gauche
        new Vector2(victimCell.gridIndex.x + 1, victimCell.gridIndex.y)  // Droite
    };

        CharacterStats victimStats = victimCell.objectInCell.GetComponent<CharacterStats>();
        int xMin = victimStats.isAllie ? 0 : 4;
        int xMax = victimStats.isAllie ? 3 : 7;
        int yMin = 0;
        int yMax = 3;

        foreach (Vector2 cellIndex in posCross)
        {
            if (Utility.CheckValueBetween(cellIndex.x, xMin, xMax) &&
                Utility.CheckValueBetween(cellIndex.y, yMin, yMax))
            {
                var cell = gridManager.gridCells[(int)cellIndex.x, (int)cellIndex.y].GetComponent<GridCell>();
                if (cell.cellFull)
                {
                    list.Add(cell.objectInCell);
                }
            }
        }

        return list;
    }

    public static List<GameObject> ColumnPattern(GridCell victimCell, GridManager gridManager)
    {
        List<GameObject> list = new();
        int column = (int)victimCell.gridIndex.x;
        int row = (int)victimCell.gridIndex.y;

        for (int i = 0; i < 4; i++)
        {
            if (i == row) continue;

            GridCell cell = gridManager.gridCells[column, i].GetComponent<GridCell>();
            if (cell.cellFull)
            {
                list.Add(cell.objectInCell);
            }
        }

        return list;
    }

    public static List<GameObject> RowPattern(GridCell victimCell, GridManager gridManager)
    {
        List<GameObject> list = new();
        int column = (int)victimCell.gridIndex.x;
        int row = (int)victimCell.gridIndex.y;

        bool isAllie = victimCell.objectInCell.GetComponent<CharacterStats>().isAllie;
        int startColumn = isAllie ? 0 : 4;
        int endColumn = isAllie ? 4 : 8;

        for (int i = startColumn; i < endColumn; i++)
        {
            if (i == column) continue; // Ignorer la colonne de la cellule cible

            GridCell cell = gridManager.gridCells[i, row].GetComponent<GridCell>();
            if (cell.cellFull)
            {
                list.Add(cell.objectInCell);
            }
        }

        return list;
    }

}

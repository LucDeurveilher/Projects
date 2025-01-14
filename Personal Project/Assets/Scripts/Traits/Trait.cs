using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Trait", menuName = "Traits")]
public class Trait : ScriptableObject
{
    [SerializeField]
    public List<Character> characters = new();

    [SerializeField]
    string bonus;

    [SerializeField]
    List<int> palier = new List<int>();

    public int number;
}

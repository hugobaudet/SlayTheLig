using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 2)]
public class Character : ScriptableObject
{
    public string characterName;

    [TextArea]
    public string characterDescription;

    public Sprite characterSprite;

    public List<Attack> attackList;
}

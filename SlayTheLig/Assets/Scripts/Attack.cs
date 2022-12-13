using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack", order = 1)]
public class Attack : ScriptableObject
{
    public string cardName;

    [TextArea]
    public string attackDescription;
    
    public Sprite cardSprite;

    [Range(0, 3)]
    public int actionCost;

    public AttackType attackType;

    public int basicDamage;
    public int comboDamage;
    public int basicHeal;
    public int basicDefense;

    public List<Card> comboPieces;
}

public enum AttackType
{
    SimpleAttack,
    ComboAttack,
    Heal,
    Buff,
    Defense,
}
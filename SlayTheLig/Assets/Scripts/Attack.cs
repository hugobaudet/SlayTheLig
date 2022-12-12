using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack", order = 1)]
public class Attack : ScriptableObject
{
    public string cardName;

    [TextArea]
    public string attackDescription;
    
    [Range(0, 3)]
    public int actionCost;

    public Sprite cardSprite;
}

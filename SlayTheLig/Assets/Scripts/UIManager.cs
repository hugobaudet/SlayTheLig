using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PlayerBehaviour player;

    [SerializeField]
    private EnemyBehaviour enemy;

    public TMP_Text actionPoint;

    public void UpdateUIActionPoint()
    {
        actionPoint.text = player.currentActionCost + " / " + player.maxActionCost;
    }
}

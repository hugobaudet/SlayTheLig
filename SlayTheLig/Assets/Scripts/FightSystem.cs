using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSystem : MonoBehaviour
{
    [SerializeField]
    private List<Stage> stages;
}

[System.Serializable]
public class Stage
{
    public string name;

    public List<CharacterBehaviour> enemyList;

}

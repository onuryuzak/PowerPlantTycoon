using System.Collections;
using System.Collections.Generic;
using Artera;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelCountData", menuName = "Artera/Data/LevelCountData")]
public class LevelDataSO : BaseScriptableObject
{
    public int level;
    public int money;

    public override void reset()
    {
       
        level = 1;
        Debug.Log("[LevelDataSO::reset]");
    }
}


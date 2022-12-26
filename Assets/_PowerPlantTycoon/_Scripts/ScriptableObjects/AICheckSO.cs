using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;

[CreateAssetMenu(fileName = "AICheckState", menuName = "Artera/AICheckState")]
public class AICheckSO : BaseScriptableObject
{
    public List<AICheckState> aiCheckStates;

    public bool getStateByType(AIType aiId)
    {
        return aiCheckStates.Find(item => item.type == aiId).state;
    }
    public void SetStateByType(AIType aiId,bool state)
    {
        aiCheckStates.Find(item => item.type == aiId).state = state;
    }

    public override void reset()
    {
        base.reset();
        for (int i = 0; i < aiCheckStates.Count; i++)
        {
            aiCheckStates[i].state = false;
        }
    }
}

[System.Serializable]
public class AICheckState
{
    public AIType type;
    public bool state;
}

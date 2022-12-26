using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;

public class AIManager : BaseSingleton<AIManager>
{
    public List<Transform> miningWaypoints = new List<Transform>();
    [SerializeField] AIBuyAreaPrice aIBuyAreaPrice;
    public CarryAIController carryAIController;
    public MiningAIController miningAIController;
    public AICheckSO AICheckSO;
    public Transform _minerTargetPosition;
    public Transform _coalPosition;

    public int getAreaPrice(AIType aiId)
    {
        return aIBuyAreaPrice.getPriceByType(aiId);
    }

    public void CreateAI(AIType aIType, Vector3 pos)
    {
        if (aIType == AIType.Mining)
        {
            MiningAIController _miningAIController = Instantiate(miningAIController);
            _miningAIController.SetInitPosition(pos);
        }
        if (aIType == AIType.Carry)
        {
            CarryAIController _carryAIController = Instantiate(carryAIController);
            _carryAIController.SetInitPosition(pos);
        }
    }


}

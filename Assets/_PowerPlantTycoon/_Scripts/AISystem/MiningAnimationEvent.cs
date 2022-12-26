using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningAnimationEvent : MonoBehaviour
{
    private Transform pileOfCoalTransform;
    public float Power;
    bool workOneTime = false;
    private void coalMiningEffect()
    {
        for (int i = 0; i < GameManager.instance.player.PlayerPowerUpSO.CoalMiningCount; i++)
        {
            
            // ObjectCreator.instance.MinerTouchVFX(pileOfCoalTransform);
            // CollectableProductItem collectable = ObjectCreator.createCollectableProduct(ProductType.Coal);
            // collectable.GetComponent<CoalItem>().canCollect = true;
            // collectable.transform.position = pileOfCoalTransform + new Vector3(0f, 1.5f, 0f);
            // collectable.transform.rotation = Quaternion.Euler(pileOfCoalTransform);
            // float x = Random.Range(2f, 4f);
            // float y = 2f;
            // float z = Random.Range(2f, 4f);
            // int a = Random.Range(0, 2);
            // int b = Random.Range(0, 2);
            //
            // if (a == 0)
            // {
            //     x *= -1;
            // }
            // if (b == 0)
            // {
            //     z *= -1;
            // }
            // Vector3 pos = new(-x, y, z);
            // collectable.GetComponent<Rigidbody>().AddForce(pos * Power, ForceMode.Impulse);
            pileOfCoalTransform.GetComponent<MiningArea>().SpawnCoal(Power);

        }
    }
    public void GetPileOfCoalPosition(Transform target)
    {
        pileOfCoalTransform = target;
    }
}

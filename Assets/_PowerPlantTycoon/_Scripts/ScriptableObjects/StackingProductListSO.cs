using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;

[CreateAssetMenu(fileName = "RawProduct", menuName = "Artera/Data/RawProduct")]
public class StackingProductListSO : ScriptableObject
{
    public ProductType productType;
    public List<Transform> stackingItems;
    public int itemCount => stackingItems.Count;

}

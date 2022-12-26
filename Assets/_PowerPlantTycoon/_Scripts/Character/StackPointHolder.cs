using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPointHolder : MonoBehaviour
{
    [SerializeField] CollectType _collectType;

    public CollectType collectType => _collectType;
}
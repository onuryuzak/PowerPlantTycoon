using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingArea : MonoBehaviour
{
    [SerializeField] GameObject _buyArea;
    public GameObject _buildingMesh;

    
    public void onSold(bool state)
    {
        _buyArea.SetActive(!state);
        _buildingMesh.SetActive(state);
    }
    
}


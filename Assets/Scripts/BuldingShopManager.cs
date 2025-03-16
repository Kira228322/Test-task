using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldingShopManager : MonoBehaviour
{
    public static BuldingShopManager Instance;
    [HideInInspector] public Building _chosenBuilding;
    
    public void OnBuildingButtonClick(Building building)
    {
        _chosenBuilding = building;
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingShopManager : MonoBehaviour
{
    public static BuildingShopManager Instance; 
    [HideInInspector] public Building ChosenBuilding;
    [SerializeField] private BuildingButton _chosenBuildingButton;
    public void OnBuildingButtonClick(BuildingButton button)
    {
        _chosenBuildingButton.OnAnotherButtonClick();
        _chosenBuildingButton = button;
        _chosenBuildingButton.OnButtonClick();
        ChosenBuilding = button._buildingOfButton;
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        ChosenBuilding = _chosenBuildingButton._buildingOfButton;
    }
}
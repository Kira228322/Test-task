using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializator : MonoBehaviour
{
    [SerializeField] private BuildingGrid _buildingGrid;
    [SerializeField] private BuildingManager _buildingManager;
    [SerializeField] private SaveLoadManager _saveLoadManager;
    [SerializeField] private RemoveBuildingManager _removeBuildingManager;
    private void Awake()
    {
        _buildingGrid.Init();
        _buildingManager.Init();
        _removeBuildingManager.Init();
        
        _saveLoadManager.LoadGame();
    }
}

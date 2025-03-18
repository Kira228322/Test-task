using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private BuildingGrid _buildingGrid;

    public void SaveGame()
    {
        SaveLoadSystem<FloorGridData>.SaveData(_buildingGrid.SaveData(), "FloorGridData");
    }

    public void LoadGame()
    {
        _buildingGrid.LoadData(SaveLoadSystem<FloorGridData>.LoadData("FloorGridData"));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour, ISaveble<FloorGridData>
{
    [HideInInspector] public Building[,] _buildings;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Material _cellsGrid;
    [SerializeField] private Renderer _floorRenderer;
    [SerializeField] private BuildingsDatabase BuildingsDatabase;
    private Texture _baseFloorMaterialTexture;
    private Vector2 _baseFloorMaterialTextureScale;
    private Plane _buildingPlane;
    
    private void Start()
    {
        RemoveBuildingManager.Instance.OnBuildingRemoving += OnBuildingRemoving;
        BuildingManager.Instance.OnBuildingPlaced += SwitchFloorTextureToDefault;
        BuildingManager.Instance.OnCancelingPlaceMode += SwitchFloorTextureToDefault;
    }

    private void OnDisable()
    {
        RemoveBuildingManager.Instance.OnBuildingRemoving -= OnBuildingRemoving;
        BuildingManager.Instance.OnBuildingPlaced -= SwitchFloorTextureToDefault;
        BuildingManager.Instance.OnCancelingPlaceMode -= SwitchFloorTextureToDefault;
    }

    public void Init()
    {
        _baseFloorMaterialTexture = _floorRenderer.material.mainTexture;
        _baseFloorMaterialTextureScale = _floorRenderer.material.mainTextureScale;
        
        _buildings = new Building[_gridSize.x, _gridSize.y];
        _buildingPlane = new Plane(Vector3.up, gameObject.transform.position);
    }
    
    private void SwitchFloorTextureToGrid()
    {
        _floorRenderer.material.mainTexture = _cellsGrid.mainTexture;
        _floorRenderer.material.mainTextureScale = _gridSize / 2;
    }

    private void SwitchFloorTextureToDefault()
    {
        _floorRenderer.material.mainTexture = _baseFloorMaterialTexture;
        _floorRenderer.material.mainTextureScale = _baseFloorMaterialTextureScale;
    }

    public void OnPlaceButtonClick()
    {
        if (BuildingManager.Instance.enabled)
        {
            SwitchFloorTextureToDefault();
        }
        else
        {
            SwitchFloorTextureToGrid();
        }
    }
    
    private void OnBuildingRemoving()
    {
        for (int i = 0; i < _buildings.GetLength(0); i++)
        {
            for (int j = 0; j < _buildings.GetLength(1); j++)
            {
                if (_buildings[i, j] == RemoveBuildingManager.Instance.SelectedBuilding)
                    _buildings[i, j] = null;
            }
        }
    }
    
    public Vector3 GetMousePositionOnGrid()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_buildingPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);

            worldPosition.x = Mathf.Floor(worldPosition.x);
            worldPosition.z = Mathf.Floor(worldPosition.z);
            return worldPosition;
        }

        return Vector3.zero;
    }

    public bool CheckAvailable(Building floatingBuilding)
    {
        Vector3 pos = floatingBuilding.transform.position;
        if (pos.x < transform.position.x ||
            pos.x + floatingBuilding.SizeOnGrid.x >= transform.position.x + _gridSize.x)
            return false;
        if (pos.z < transform.position.z ||
            pos.z + floatingBuilding.SizeOnGrid.y >= transform.position.z + _gridSize.y)
            return false;
        
        int pivotX = WorldToGrid(pos).x;
        int pivotY = WorldToGrid(pos).y;
        for (int x = pivotX; x < pivotX + floatingBuilding.SizeOnGrid.x; x++)
        {
            for (int y = pivotY; y < pivotY + floatingBuilding.SizeOnGrid.y; y++)
            {
                if (_buildings[x, y] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }
    
    public Vector2Int WorldToGrid(Vector3 point)
    {
        return new Vector2Int((int)(point.x - transform.position.x), (int)(point.z - transform.position.z));
    }

    public void LoadData(FloorGridData data)
    {
        foreach (var building in data.Buildings)
        {
            BuildingManager.Instance.PlaceSavedBuilding(building.coords, BuildingsDatabase.GetBuildingByName(building.name));
        }
    }

    public FloorGridData SaveData()
    {
        return new FloorGridData(_buildings);
    }
}

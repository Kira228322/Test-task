using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Camera _camera;
    private Building _floatingBuilding;
    private Plane _buildingPlane;
    private Building[,] _buildings;
    
    private void Start()
    {
        _buildings = new Building[_gridSize.x, _gridSize.y];
        _buildingPlane = new Plane(Vector3.up, gameObject.transform.position);
        enabled = false;
    }

    private void CreateFloatingBuilding(Building building)
    {
        if (_floatingBuilding != null)
            Destroy(_floatingBuilding.gameObject);

        _floatingBuilding = Instantiate(building);
    }

    public void OnBuildingButtonClick(Building building)
    {
        CreateFloatingBuilding(building);
        enabled = true;
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_buildingPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);

            worldPosition.x = Mathf.Floor(worldPosition.x);
            worldPosition.z = Mathf.Floor(worldPosition.z);
            
            _floatingBuilding.transform.position = worldPosition ;
            
            if (PlaceIsAvailable(worldPosition) && Input.GetKeyUp(KeyCode.Mouse0))
            {
                PlaceBuilding(worldPosition);
                enabled = false;
            }
        }
    }

    private void PlaceBuilding(Vector3 worldPosition)
    {
        int pivotX = WorldToGrid(worldPosition).x;
        int pivotY = WorldToGrid(worldPosition).y;
        for (int x = pivotX; x < pivotX + _floatingBuilding.Size.x; x++)
        {
            for (int y = pivotY; y < pivotY + _floatingBuilding.Size.y; y++)
            {
                _buildings[x, y] = _floatingBuilding;
            }
        }
        _floatingBuilding.SetColorToDefault();
        _floatingBuilding = null;
    }
    
    private bool PlaceIsAvailable(Vector3 worldPosition)
    {
        _floatingBuilding.ChangeColorWhileDragging(false);
        if (worldPosition.x < transform.position.x ||
            worldPosition.x + _floatingBuilding.Size.x >= transform.position.x + _gridSize.x)
            return false;
        if (worldPosition.z < transform.position.z ||
                 worldPosition.z + _floatingBuilding.Size.y >= transform.position.z + _gridSize.y)
            return false;
        
        int pivotX = WorldToGrid(worldPosition).x;
        int pivotY = WorldToGrid(worldPosition).y;
        for (int x = pivotX; x < pivotX + _floatingBuilding.Size.x; x++)
        {
            for (int y = pivotY; y < pivotY + _floatingBuilding.Size.y; y++)
            {
                if (_buildings[x, y] != null)
                {
                    return false;
                }
            }
        }
        
        _floatingBuilding.ChangeColorWhileDragging(true);
        return true;
    }
    private Vector2Int WorldToGrid(Vector3 point)
    {
        return new Vector2Int((int)(point.x - transform.position.x), (int)(point.z - transform.position.z));
    }
}



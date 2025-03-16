using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloorGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Camera _camera;
    [SerializeField] private Material _cellsGrid;
    [SerializeField] private Renderer _floorRenderer;
    private Texture _baseFloorMaterialTexture;
    private Vector2 _baseFloorMaterialTextureScale;
    private Building _floatingBuilding;
    private Plane _buildingPlane;
    private Building[,] _buildings;
    private GameInput _gameInput;
    
    private void Awake()
    {
        _gameInput = new GameInput();
        _gameInput.Enable();
        
        Init();
        enabled = false;
    }

    private void OnEnable()
    {
        _gameInput.Building.Build.performed += OnBuildPerform;
    }

    private void OnDisable()
    {
        _gameInput.Building.Build.performed -= OnBuildPerform;
    }

    private void Init()
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
        _floorRenderer.material.mainTextureScale =_baseFloorMaterialTextureScale;
    }
    
    private void CreateFloatingBuilding(Building building)
    {
        if (_floatingBuilding != null)
            Destroy(_floatingBuilding.gameObject);

        _floatingBuilding = Instantiate(building);
    }

    public void OnPlaceButtonClick()
    {
        CreateFloatingBuilding(BuldingShopManager.Instance._chosenBuilding);
        SwitchFloorTextureToGrid();
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

            CheckAvailable();
        }
    }

    private void OnBuildPerform(InputAction.CallbackContext obj)
    {
        if (CheckAvailable())
        {
            PlaceBuilding(_floatingBuilding.transform.position);
            enabled = false;
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
        SwitchFloorTextureToDefault();
        _floatingBuilding = null;
    }
    
    private bool CheckAvailable()
    {
        _floatingBuilding.ChangeColorWhileDragging(false);
        Vector3 pos = _floatingBuilding.transform.position;
        if (pos.x < transform.position.x ||
            pos.x + _floatingBuilding.Size.x >= transform.position.x + _gridSize.x)
            return false;
        if (pos.z < transform.position.z ||
            pos.z + _floatingBuilding.Size.y >= transform.position.z + _gridSize.y)
            return false;
        
        int pivotX = WorldToGrid(pos).x;
        int pivotY = WorldToGrid(pos).y;
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



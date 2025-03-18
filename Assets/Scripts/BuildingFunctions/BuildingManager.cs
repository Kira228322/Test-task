using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    [SerializeField] private BuildingGrid _buildingGrid;
    [SerializeField] private TMP_Text _placeButtonText;
    private Building _floatingBuilding;
    private GameInput _gameInput;
    public event UnityAction OnBuildingPlaced;
    public event UnityAction OnCancelingPlaceMode;
    
    private void Awake()
    {
        _gameInput = new GameInput(); 
        _gameInput.Enable();
    }

    public void Init()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        _gameInput.Building.Build.performed += OnBuildPerform;
    }

    private void OnDisable()
    {
        _gameInput.Building.Build.performed -= OnBuildPerform;
    }

    private void Update()
    {
        _floatingBuilding.transform.position = _buildingGrid.GetMousePositionOnGrid() ;
        ChangeFloatingBuildingColor();
    }
    public void SwitchEnabledPlaceMode()
    {
        if (enabled)
        {
            enabled = false;
            if (_floatingBuilding != null)
                Destroy(_floatingBuilding.gameObject);
            _placeButtonText.text = "Place";
            OnCancelingPlaceMode?.Invoke();
        }
        else
        {
            enabled = true;
            CreateFloatingBuilding(BuildingShopManager.Instance.ChosenBuilding);
            _placeButtonText.text = "Cancel";
            
            if (RemoveBuildingManager.Instance.enabled)
            {
                RemoveBuildingManager.Instance.SwitchEnabledRemoveMode();
            }
        }
    }
    
    public void CreateFloatingBuilding(Building building)
    {
        if (_floatingBuilding != null)
            Destroy(_floatingBuilding.gameObject);

        _floatingBuilding = Instantiate(building);
    }

    public void OnPlaceButtonClick()
    {
        SwitchEnabledPlaceMode();
    }

    private void OnBuildPerform(InputAction.CallbackContext obj)
    {
        if (_buildingGrid.CheckAvailable(_floatingBuilding))
        {
            OnBuildingPlaced?.Invoke();
            PlaceBuilding(_floatingBuilding.transform.position);
            SwitchEnabledPlaceMode();
        }
    }

    private void PlaceBuilding(Vector3 worldPosition)
    {
        int pivotX = _buildingGrid.WorldToGrid(worldPosition).x;
        int pivotY = _buildingGrid.WorldToGrid(worldPosition).y;
        
        for (int x = pivotX; x < pivotX + _floatingBuilding.SizeOnGrid.x; x++)
        {
            for (int y = pivotY; y < pivotY + _floatingBuilding.SizeOnGrid.y; y++)
            {
                _buildingGrid._buildings[x, y] = _floatingBuilding;
            }
        }
        
        _floatingBuilding.SetColorToDefault();
        _floatingBuilding = null;
    }
    
    public void PlaceSavedBuilding(int[] coords, Building savedBuilding)
    {
        int pivotX = coords[0];
        int pivotY = coords[1];
        
        Building building = Instantiate(savedBuilding);
        building.transform.position = new Vector3(pivotX, 0, pivotY) + _buildingGrid.transform.position;
            
        for (int x = pivotX; x < pivotX + building.SizeOnGrid.x; x++)
        {
            for (int y = pivotY; y < pivotY + building.SizeOnGrid.y; y++)
            {
                _buildingGrid._buildings[x, y] = building;
            }
        }
    }
    
    private void ChangeFloatingBuildingColor()
    {
        if (_buildingGrid.CheckAvailable(_floatingBuilding))
            _floatingBuilding.ChangeColorWhileDragging(true);
        else
            _floatingBuilding.ChangeColorWhileDragging(false);
    }
}



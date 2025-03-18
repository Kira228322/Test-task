using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RemoveBuildingManager : MonoBehaviour
{
    public static RemoveBuildingManager Instance;
    [HideInInspector]public Building SelectedBuilding;
    [SerializeField] private Camera _camera;
    [SerializeField] private TMP_Text _removeButtonText;
    private GameInput _gameInput;
    private Building _previousSelectedBuilding;
    public event UnityAction OnBuildingRemoving;
    
    private void Awake()
    {
        _gameInput = new GameInput();
        
        SwitchEnabledRemoveMode();
    }

    public void Init()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        _gameInput.DeleteMode.Delete.performed += RemoveBuilding;
    }

    private void OnDisable()
    {
        _gameInput.DeleteMode.Delete.performed -= RemoveBuilding;
    }

    public void SwitchEnabledRemoveMode()
    {
        if (enabled)
        {
            enabled = false;
            _gameInput.Disable();
            _removeButtonText.text = "Remove";
        }
        else
        {
            enabled = true;
            _gameInput.Enable();
            _removeButtonText.text = "Cancel";
            if (BuildingManager.Instance.enabled)
            {
                BuildingManager.Instance.SwitchEnabledPlaceMode();
            }
        }
    }
    
    
    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            _previousSelectedBuilding = SelectedBuilding;
            SelectedBuilding = hit.collider.GetComponentInParent<Building>();
            
            if (_previousSelectedBuilding != SelectedBuilding)
            {
                if (SelectedBuilding != null)
                    SelectedBuilding.SelectingAnimation();
                if (_previousSelectedBuilding != null)
                    _previousSelectedBuilding.DeselectingAnimation();
            }
            
        }
    }

    private void RemoveBuilding(InputAction.CallbackContext obj)
    {
        if (SelectedBuilding != null)
        {
            OnBuildingRemoving?.Invoke();
            Destroy(SelectedBuilding.gameObject);
            SwitchEnabledRemoveMode();
        }
    }
}

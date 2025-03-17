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
    [SerializeField] private Camera _camera;
    [SerializeField] private TMP_Text _removeButtonText;
    private GameInput _gameInput;
    public event UnityAction OnBuildingRemoving;
    private Building _previousSelectedBuilding;
    public Building SelectedBuilding;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        _gameInput = new GameInput();
        
        SwitchEnabledRemoveMode();
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
        if (!enabled)
        {
            enabled = true;
            _gameInput.Enable();
            _removeButtonText.text = "Cancel";
        }
        else
        {
            enabled = false;
            _gameInput.Disable();
            _removeButtonText.text = "Remove";
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

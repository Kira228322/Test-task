using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBuildingManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Building _selectedBuilding;

    public Building SelectedBuilding
    {
        get
        {
            return _selectedBuilding;
        }
        set
        {
            if (_selectedBuilding != null)
                if (value != _selectedBuilding)
                {
                    _selectedBuilding.SetColorToDefault();
                }
            _selectedBuilding = value;
        }
    }
    
    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            SelectedBuilding = hit.collider.GetComponentInParent<Building>();
            Debug.Log(SelectedBuilding);
            if (SelectedBuilding != null)
            {
                SelectedBuilding.ChangeColorOnSelectToDelete();
            }
        }
    }
}

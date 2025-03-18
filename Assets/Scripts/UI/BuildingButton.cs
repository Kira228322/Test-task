using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Building _buildingOfButton;
    [SerializeField] private Image _outline;

    public void OnButtonClick()
    {
        Color fullAlpha = new Color(_outline.color.r, _outline.color.g, _outline.color.b, 1);
        _outline.color = fullAlpha;

        if (BuildingManager.Instance.enabled)
        {
            BuildingManager.Instance.CreateFloatingBuilding(_buildingOfButton);
        }
    }

    public void OnAnotherButtonClick()
    {
        Color zeroAlpha = new Color(_outline.color.r, _outline.color.g, _outline.color.b, 0);
        _outline.color = zeroAlpha;
    }
}

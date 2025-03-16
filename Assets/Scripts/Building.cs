using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int Size;
    [SerializeField] private Renderer _renderer;
    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = _renderer.material.color;
    }

    public void ChangeColorWhileDragging(bool available)
    {
        if (available)
            _renderer.material.color = Color.green;
        else 
            _renderer.material.color = Color.red;
    }

    public void ChangeColorOnSelectToDelete()
    {
        _renderer.material.color = Color.red;
    }

    public void SetColorToDefault()
    {
        _renderer.material.color = _defaultColor;
    }
}

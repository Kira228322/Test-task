using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Building : MonoBehaviour
{
    public Vector2Int SizeOnGrid;
    [SerializeField] private Renderer _renderer;
    private Color _defaultColor;
    private Coroutine _currenCoroutine;
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

    public void SetColorToDefault()
    {
        _renderer.material.color = _defaultColor;
    }

    public void SelectingAnimation()
    {
        if (_currenCoroutine != null)
            StopCoroutine(_currenCoroutine);
        _currenCoroutine = StartCoroutine(ChangeColorAnimation(0.5f, Color.red));
    }

    public void DeselectingAnimation()
    {
        if (_currenCoroutine != null)
            StopCoroutine(_currenCoroutine);
        _currenCoroutine = StartCoroutine(ChangeColorAnimation(0.25f, _defaultColor));
    }

    private IEnumerator ChangeColorAnimation(float maxAnimationDuration, Color targetColor)
    {
        float frequency = 0.02f; // частота анимации
        int maxCountOfIteration = (int)(maxAnimationDuration/frequency);
        WaitForSeconds waitForSeconds = new WaitForSeconds(frequency);
        
        Color startColor = _renderer.material.color; 
        Color color = _renderer.material.color;
        
        // Для того, чтобы скорость изменения цвета была всегда одинаковой 
        // надо сначала измерить насколько таргетный цвет отличается от исходного.
        Vector4 VectorColor4 = targetColor - color; 
        float maxDeltaColorComponent = new Color(Math.Abs(VectorColor4.x), Math.Abs(VectorColor4.y),
            Math.Abs(VectorColor4.y)).maxColorComponent;
        
        int count = (int)(maxCountOfIteration * maxDeltaColorComponent); // количество итераций - фактически время анимации
        
        for (int i = 1; i <= count; i++)
        {
            color = Color.Lerp(startColor, targetColor, (float)i / count);
            _renderer.material.color = color;
            yield return waitForSeconds;
        }
        _renderer.material.color = targetColor;
        _currenCoroutine = null;
    }
}

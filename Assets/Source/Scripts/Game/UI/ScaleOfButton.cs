using CubeProject.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOfButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    private Color _defaultVertexColor;
    private TextMeshProUGUI _text;
    private Vector3 _scalePointerEnter = new Vector3(1.2f, 1.2f, 1f);
    private float _speedScale = 0.3f;
    

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();        
        _defaultVertexColor = _text.color;       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {       
        transform.DOScale(_scalePointerEnter, _speedScale);
        _text.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, _speedScale);
        _text.color = _defaultVertexColor;        
    }
}

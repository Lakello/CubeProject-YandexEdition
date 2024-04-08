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
    private WordWobble _wordWobble;
    private Color[] _defaultColors;
    private Color _defaultVertexColor;
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _defaultColors = _text.mesh.colors;
        _defaultVertexColor = _text.color;
        Debug.Log(_defaultColors);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {       
        transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.3f);

        if (TryGetComponent(out WordWobble wordWobble))
        {
            _wordWobble = wordWobble;
            _wordWobble.enabled = true;
        }        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);

        if (TryGetComponent(out WordWobble wordWobble))
        {            
            _wordWobble.enabled = false;          
            _text.mesh.colors = _defaultColors;
            _text.color = _defaultVertexColor;
        }        
    }
}

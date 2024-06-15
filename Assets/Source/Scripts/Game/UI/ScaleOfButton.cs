using DG.Tweening;
using LeadTools.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOfButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Vector3 _scalePointerEnter = new Vector3(1.2f, 1.2f, 1f);
	[SerializeField] private float _speedScale = 0.3f;

	private Color _defaultVertexColor;
	private TMP_Text _text;
	private Tweener _tweener;

	private void Start()
	{
		gameObject.GetComponentInChildrenElseThrow(out _text);
		_defaultVertexColor = _text.color;
		SetDefaultScale();
	}

	private void OnDisable() =>
		_tweener?.Kill();

	public void OnPointerEnter(PointerEventData _) =>
		SetHoverScale();

	public void OnPointerExit(PointerEventData _) =>
		SetDefaultScale();

	private void SetDefaultScale()
	{
		Play(Vector3.one, _speedScale);
		_text.color = _defaultVertexColor;
	}

	private void SetHoverScale()
	{
		Play(_scalePointerEnter, _speedScale);
		_text.color = Color.red;
	}

	private void Play(Vector3 endValue, float duration)
	{
		_tweener?.Kill();
		_tweener = transform.DOScale(endValue, duration);
	}
}
using System;
using LeadTools.Extensions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TestGameObjects : MonoBehaviour
{
	[SerializeField] private GameObject _target;
	
	private Toggle _toggle;
	
	private void Awake()
	{
		gameObject.GetComponentElseThrow(out _toggle);
		_toggle.isOn = true;
	}

	private void OnEnable()
	{
		_toggle.onValueChanged.AddListener(OnValueChanged);
	}
	
	private void OnDisable()
	{
		_toggle.onValueChanged.RemoveListener(OnValueChanged);
	}

	private void OnValueChanged(bool arg0)
	{
		_target.SetActive(arg0);
	}
}

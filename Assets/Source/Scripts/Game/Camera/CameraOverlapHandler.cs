using System;
using UnityEngine;

namespace CubeProject.Game
{
	public class CameraOverlapHandler : MonoBehaviour
	{
		private const float MaxAlpha = 255f;
		private const string Alpha = "_Alpha";

		[SerializeField] private bool _isDisableObjects;
		[SerializeField] [Range(0f, MaxAlpha)] private float _hintAlpha;

		private Action<MeshRenderer> _hide;
		private Action<MeshRenderer> _show;

		private void Awake()
		{
			if (_isDisableObjects)
			{
				_hide = (mesh) => mesh.gameObject.SetActive(false);
				_show = (mesh) => mesh.gameObject.SetActive(true);
			}
			else
			{
				var normalAlpha = _hintAlpha / MaxAlpha;
				
				_hide = (mesh) => ChangeAlpha(mesh, normalAlpha);
				_show = (mesh) => ChangeAlpha(mesh, 1);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out MeshRenderer meshRenderer))
			{
				_hide(meshRenderer);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out MeshRenderer meshRenderer))
			{
				_show(meshRenderer);
			}
		}

		private void ChangeAlpha(MeshRenderer meshRenderer, float alpha) =>
			meshRenderer.material.SetFloat(Alpha, alpha);
	}
}
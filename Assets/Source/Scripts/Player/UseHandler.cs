using CubeProject.Game;
using CubeProject.InputSystem;
using LeadTools.Extensions;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Player
{
	public class UseHandler : MonoBehaviour
	{
		private readonly RaycastHit[] _results = new RaycastHit[1];

		[SerializeField] private LayerMask _usableMask;
		[SerializeField] private float _cooldown;

		private IInputHandler _inputHandler;
		private BoxCollider _cubeCollider;
		private Cube _cube;
		private Coroutine _cooldownCoroutine;

		[Inject]
		private void Inject(IInputHandler inputHandler, Cube cube)
		{
			_inputHandler = inputHandler;
			_inputHandler.UsePressed += OnUsePressed;

			_cube = cube;
			_cubeCollider = _cube.ComponentsHolder.SelfCollider;
		}

		private void OnDisable() =>
			_inputHandler.UsePressed -= OnUsePressed;

		private void OnUsePressed()
		{
			if (_cooldownCoroutine != null)
			{
				return;
			}

			_cooldownCoroutine = this.WaitTime(_cooldown, () => _cooldownCoroutine = null);

			if (CheckUse(out var usable))
			{
				usable.TryUse(_cube);
			}
		}

		private bool CheckUse(out IUsable usable)
		{
			usable = null;

			var bounds = _cubeCollider.bounds;

			var hitCount = Physics.BoxCastNonAlloc(
				bounds.center,
				bounds.extents,
				Vector3.down,
				_results,
				Quaternion.identity,
				Mathf.Infinity,
				_usableMask,
				QueryTriggerInteraction.Collide);

			return hitCount > 0 && _results[0].transform.TryGetComponent(out usable);
		}
	}
}
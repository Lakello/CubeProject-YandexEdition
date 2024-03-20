using CubeProject.Game;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Source.Scripts.Game.Level.Camera;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeDiedView))]
	[RequireComponent(typeof(Cube))]
	public class CubeDiedService : MonoBehaviour
	{
		[SerializeField] private Transform _cameraFollow;
		[SerializeField] private Vector3 _offset = new Vector3(0, 0.5f, 0);

		private Cube _cube;
		private CheckPointHolder _checkPointHolder;
		private CubeDiedView _cubeDiedView;
		private TargetCameraHolder _targetCameraHolder;

		private IStateMachine<CubeStateMachine> CubeStateMachine => _cube.ServiceHolder.StateMachine;

		private CubeFallService CubeFallService => _cube.ServiceHolder.FallService;

		[Inject]
		private void Inject(CheckPointHolder checkPointHolder, TargetCameraHolder targetCameraHolder)
		{
			_targetCameraHolder = targetCameraHolder;
			_checkPointHolder = checkPointHolder;
		}

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _cubeDiedView);
			gameObject.GetComponentElseThrow(out _cube);
		}

		private void OnEnable() =>
			_cube.Died += OnDied;

		private void OnDisable() =>
			_cube.Died -= OnDied;

		private void OnDied()
		{
			if (CubeStateMachine.CurrentState == typeof(DieState))
			{
				DissolveInvisible();
			}
		}
		
		private void DissolveInvisible() =>
			_cubeDiedView.Play(false, DissolveVisible);

		private void DissolveVisible()
		{
			_cube.transform.position = ((MonoBehaviour)_checkPointHolder.CurrentCheckPoint).transform.position + _offset;

			_targetCameraHolder.SetTarget();

			_cubeDiedView.Play(true, () =>
			{
				if (CubeFallService.TryFall() is false)
				{
					CubeStateMachine.EnterIn<ControlState>();
				}
			});
		}
	}
}
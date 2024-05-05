using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using DG.Tweening;
using Reflex.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.Scripts.Game.Level.Animations
{
	public class VerticalMoveAnimation : MonoBehaviour
	{
		[SerializeField] private float _yOffset = 0.216f;
		[SerializeField] private float _animationDuration;

		[SerializeField]
		[BoxGroup("Animate")]
		private bool _isThisAnimate;
		[SerializeField]
		[BoxGroup("Animate")]
		[ShowIf(nameof(IsShowAnimateObject))]
		private Transform _animateObject;

		private CubeMoveService _moveService;
		private Tweener _animationTweener;
		private float _enteredPositionY;

		private bool IsShowAnimateObject => _isThisAnimate == false;

		[Inject]
		private void Inject(Cube cube) =>
			_moveService = cube.Component.MoveService;

		private void Awake()
		{
			if (_isThisAnimate)
				_animateObject = transform;

			_enteredPositionY = transform.position.y + _yOffset;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
			{
				_moveService.StepEnded += OnEnterStepEnded;

				if (_animationTweener == null)
				{
					_animationTweener = _animateObject
						.DOMoveY(_enteredPositionY, _animationDuration)
						.SetEase(Ease.InOutFlash)
						.SetAutoKill(false);
				}
				else
				{
					_animationTweener.PlayForward();
				}
			}
		}

		private void OnDisable()
		{
			if (_animationTweener != null)
			{
				_animationTweener.Kill();
				_animationTweener = null;
			}
		}

		private void OnEnterStepEnded()
		{
			_moveService.StepEnded -= OnEnterStepEnded;
			_moveService.StepStarted += OnExitStepStarted;
		}

		private void OnExitStepStarted()
		{
			_moveService.StepStarted -= OnExitStepStarted;

			_animationTweener.PlayBackwards();
		}
	}
}
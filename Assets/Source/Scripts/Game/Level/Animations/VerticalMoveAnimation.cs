using CubeProject.Game.Messages;
using Game.Player;
using Game.Player.Movement;
using DG.Tweening;
using Game.Player.Messages;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace CubeProject.Game.Level.Animations
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

		private CompositeDisposable _disposable;
		private Tweener _animationTweener;
		private float _enteredPositionY;

		private bool IsShowAnimateObject => _isThisAnimate == false;

		private void Awake()
		{
			if (_isThisAnimate)
				_animateObject = transform;

			_enteredPositionY = transform.position.y + _yOffset;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CubeEntity _))
			{
				_disposable = new CompositeDisposable();

				MessageBroker.Default
					.Receive<M_StepEnded>()
					.Subscribe(_ => OnStepEnded())
					.AddTo(_disposable);

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
			_disposable?.Dispose();

			if (_animationTweener != null)
			{
				_animationTweener.Kill();
				_animationTweener = null;
			}
		}

		private void OnStepEnded()
		{
			_disposable?.Dispose();
			_disposable = new CompositeDisposable();

			MessageBroker.Default
				.Receive<M_StepStarted>()
				.Subscribe(_ => OnExitStepStarted())
				.AddTo(_disposable);
		}

		private void OnExitStepStarted()
		{
			_disposable?.Dispose();
			_animationTweener.PlayBackwards();
		}
	}
}
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CubeProject.Game.Level.Portal
{
	public class Teleporter
	{
		private readonly TeleportView _view;
		private readonly AnimationCurve _scaleCurve;
		private readonly AnimationCurve _heightCurve;

		public Teleporter(
			Transform cubeTransform,
			Transform origin,
			Transform targetPoint,
			TeleporterData data)
		{
			_view = new TeleportView(cubeTransform, origin, targetPoint, data.AnimationTime);

			_scaleCurve = data.ScaleCurve;
			_heightCurve = data.HeightCurve;
		}

		public async UniTask Absorb(CancellationToken cancellationToken)
		{
			await _view.AnimationPlay(
				(time) => _scaleCurve.Evaluate(time),
				(time) => _heightCurve.Evaluate(time),
				cancellationToken);
		}

		public async UniTask Return(CancellationToken cancellationToken)
		{
			await _view.AnimationPlay(
				(time) => 1 - _scaleCurve.Evaluate(time),
				(time) => 1 - _heightCurve.Evaluate(time),
				cancellationToken);
		}
	}
}
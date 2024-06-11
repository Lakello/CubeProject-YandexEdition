using System.Collections;
using UnityEngine;

namespace CubeProject.Game.Player
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

		public IEnumerator Absorb()
		{
			return _view.AnimationPlay(
				(time) => _scaleCurve.Evaluate(time),
				(time) => _heightCurve.Evaluate(time));
		}

		public IEnumerator Return()
		{
			return _view.AnimationPlay(
				(time) => 1 - _scaleCurve.Evaluate(time),
				(time) => 1 - _heightCurve.Evaluate(time));
		}
	}
}
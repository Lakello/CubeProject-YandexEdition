using System;
using CubeProject.PlayableCube;
using UnityEngine;

namespace CubeProject.Game
{
	public class Teleporter
	{
		private readonly TeleportView _view;
		private readonly Action _endCallback;
		private readonly AnimationCurve _scaleCurve;
		private readonly AnimationCurve _heightCurve;

		public Teleporter(
			MonoBehaviour mono,
			Cube cube,
			Transform origin,
			Transform targetPoint,
			Action endCallback,
			TeleporterData data)
		{
			_view = new TeleportView(cube, mono, origin, targetPoint, data.AnimationTime);

			_scaleCurve = data.ScaleCurve;
			_heightCurve = data.HeightCurve;
			_endCallback = endCallback;
		}

		public void Absorb(Action endCallback)
		{
			_view.AnimationPlay(
				(time) => _scaleCurve.Evaluate(time),
				(time) => _heightCurve.Evaluate(time),
				endCallback);
		}

		public void Return()
		{
			//_cube.transform.position = TargetPosition;

			_view.AnimationPlay(
				(time) => 1 - _scaleCurve.Evaluate(time),
				(time) => 1 - _heightCurve.Evaluate(time),
				_endCallback);
		}
	}
}
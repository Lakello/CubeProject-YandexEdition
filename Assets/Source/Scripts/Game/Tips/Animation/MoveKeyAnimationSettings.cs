using System;
using UnityEngine;

namespace CubeProject.Tips
{
	[Serializable]
	public struct MoveKeyAnimationSettings
	{
		[SerializeField] private float _targetPositionY;
		[SerializeField] private float _animationTime;

		public float TargetPositionY => _targetPositionY;

		public float AnimationTime => _animationTime;

		public Transform KeyTransform { get; private set; }

		public void Init(Transform keyTransform)
		{
			KeyTransform = keyTransform;
		}
	}
}
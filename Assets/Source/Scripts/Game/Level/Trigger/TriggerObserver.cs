using System;
using UnityEngine;

namespace Source.Scripts.Game.Level.Trigger
{
	public class TriggerObserver : MonoBehaviour
	{
		public bool IsEntered { get; private set; }

		public Transform Target { get; private set; }

		public void Entered(Transform target)
		{
			IsEntered = true;
			Target = target;
		}

		public void Exited(Transform target)
		{
			if (Target == target)
			{
				IsEntered = false;
			}
		}
	}
}
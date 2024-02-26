using System;
using CubeProject.Player;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using UnityEngine;

namespace CubeProject.Game
{
	public class EndLevel : MonoBehaviour, ISubject
	{
		[SerializeField] private float _time;

		public event Action ActionEnded;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
			{
				this.WaitTime(_time, () => ActionEnded?.Invoke());
			}
		}
	}
}
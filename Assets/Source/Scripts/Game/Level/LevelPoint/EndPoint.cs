using System;
using LeadTools.StateMachine;
using Source.Scripts.Game.Level.LevelPoint.Messages;
using UniRx;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(Collider))]
	public class EndPoint : MonoBehaviour, ITransitSubject
	{
		private readonly M_EndLevel _endLevelMessage = new M_EndLevel();

		public event Action StateTransiting;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CubeEntity _))
			{
				MessageBroker.Default
					.Publish(_endLevelMessage);

				StateTransiting?.Invoke();
			}
		}
	}
}
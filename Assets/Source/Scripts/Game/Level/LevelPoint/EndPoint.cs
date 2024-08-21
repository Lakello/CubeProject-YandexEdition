using System;
using CubeProject.Game.Level.LevelPoint.Messages;
using CubeProject.Game.Player;
using LeadTools.FSM.Transit;
using UniRx;
using UnityEngine;

namespace CubeProject.Game.Level.LevelPoint
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
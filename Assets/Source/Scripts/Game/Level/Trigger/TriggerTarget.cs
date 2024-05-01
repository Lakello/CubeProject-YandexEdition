using CubeProject.Game;
using UnityEngine;

namespace Source.Scripts.Game.Level.Trigger
{
	public abstract class TriggerTarget : MonoBehaviour
	{
		public abstract IChargeable Chargeable { get; protected set; }
	}
}
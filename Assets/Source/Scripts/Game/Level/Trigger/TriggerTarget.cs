using CubeProject.Game.Level.Charge;
using UnityEngine;

namespace CubeProject.Game.Level.Trigger
{
	public abstract class TriggerTarget : MonoBehaviour
	{
		public abstract IChargeable Chargeable { get; protected set; }
	}
}
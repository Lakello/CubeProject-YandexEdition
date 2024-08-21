using CubeProject.Game.Level.Charge;
using CubeProject.Game.Level.Trigger;
using LeadTools.Extensions;

namespace CubeProject.Game.Level.Barrier
{
	public class Barrier : TriggerTarget
	{
		public override IChargeable Chargeable { get; protected set; }

		private void Awake() =>
			Chargeable = gameObject.GetComponentElseThrow<IChargeable>();
	}
}
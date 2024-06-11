using LeadTools.Extensions;
using CubeProject.Game.Level.Trigger;

namespace CubeProject.Game.Player
{
	public class Barrier : TriggerTarget
	{
		public override IChargeable Chargeable { get; protected set; }

		private void Awake() =>
			Chargeable = gameObject.GetComponentElseThrow<IChargeable>();
	}
}
using LeadTools.Extensions;
using Source.Scripts.Game.Level.Trigger;

namespace CubeProject.Game
{
	public class Barrier : TriggerTarget
	{
		public override IChargeable Chargeable { get; protected set; }

		private void Awake() =>
			Chargeable = gameObject.GetComponentElseThrow<IChargeable>();
	}
}
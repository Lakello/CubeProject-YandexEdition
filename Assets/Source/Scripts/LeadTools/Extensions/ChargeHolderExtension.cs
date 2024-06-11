using CubeProject.Game.Player;

namespace LeadTools.Extensions
{
	public static class ChargeHolderExtension
	{
		public static void GivePowerTo(this ChargeHolder originHolder, ChargeHolder targetHolder)
		{
			if ((originHolder.IsCharged && targetHolder.IsCharged)
				|| (originHolder.IsCharged is false && targetHolder.IsCharged is false))
			{
				return;
			}

			originHolder.Defuse();
			targetHolder.Charge();
		}
	}
}
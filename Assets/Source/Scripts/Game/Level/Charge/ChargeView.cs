using LeadTools.Other;

namespace CubeProject.Game
{
	public sealed class ChargeView : LightView
	{
		protected override void ColorChangerInit(HDRColorChanger colorChanger) =>
			colorChanger.Init(() => Chargeable.IsCharged);

		protected override void OnChargeChanged()
		{
			ColorChanger.ChangeColor();

			CallChanged();
		}
	}
}
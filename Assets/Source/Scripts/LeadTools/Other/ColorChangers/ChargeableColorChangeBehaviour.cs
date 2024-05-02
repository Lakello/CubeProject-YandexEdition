using CubeProject.Game;
using LeadTools.Extensions;
using Source.Scripts.LeadTools.Other;

namespace CubeProject.PlayableCube
{
	public class ChargeableColorChangeBehaviour : ColorChangeBehaviour
	{
		private IChargeable _chargeable;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeable);

			Do((changer) => changer.Init(() => _chargeable.IsCharged));
		}

		private void OnEnable()
		{
			Do(changer =>
			{
				_chargeable.ChargeChanged += changer.ChangeColor;
				changer.ChangeColor();
			});
		}

		private void OnDisable() =>
			Do(changer => _chargeable.ChargeChanged -= changer.ChangeColor);
	}
}
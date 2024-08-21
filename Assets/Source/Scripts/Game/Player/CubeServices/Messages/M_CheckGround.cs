using System;
using LeadTools.Common;

namespace CubeProject.Game.Player.CubeService.Messages
{
	public class M_CheckGround : Message<M_CheckGround, Action<bool>>
	{
		protected override M_CheckGround Instance => this;
	}
}
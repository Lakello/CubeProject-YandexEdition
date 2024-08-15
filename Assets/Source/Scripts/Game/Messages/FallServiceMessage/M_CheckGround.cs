using System;
using Source.Scripts.LeadTools.Other;

namespace CubeProject.Game.Messages
{
	public class M_CheckGround : Message<M_CheckGround, Action<bool>>
	{
		protected override M_CheckGround Instance => this;
	}
}
using LeadTools.Common;
using UnityEngine;

namespace CubeProject.Game.Player.CubeService.Messages
{
	public class M_MoveDirectionChanged : Message<M_MoveDirectionChanged, Vector3>
	{
		protected override M_MoveDirectionChanged Instance => this;
	}
}
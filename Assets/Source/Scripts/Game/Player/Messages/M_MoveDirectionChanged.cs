using Source.Scripts.LeadTools.Other;
using UnityEngine;

namespace Game.Player.Messages
{
	public class M_MoveDirectionChanged : Message<M_MoveDirectionChanged, Vector3>
	{
		protected override M_MoveDirectionChanged Instance => this;
	}
}
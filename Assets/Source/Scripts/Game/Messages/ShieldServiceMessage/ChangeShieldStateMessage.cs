using Source.Scripts.Game.Level.Shield;

namespace Source.Scripts.Game.Messages.ShieldServiceMessage
{
	public class ChangeShieldStateMessage : Message<CubeShieldService>
	{
		public ChangeShieldStateMessage(MessageId id) : base(id)
		{
		}
	}
}
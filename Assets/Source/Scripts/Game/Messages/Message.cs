using CubeProject.Game.Player;

namespace CubeProject.Game.Messages
{
	public class Message<TSender>
	{
		public readonly MessageId Id;

		public Message(MessageId id) =>
			Id = id;
	}

	public class Message<TData, TSender> : Message<TSender>
	{
		public readonly TData Data;

		public Message(MessageId id, TData data) : base(id) =>
			Data = data;
	}
}
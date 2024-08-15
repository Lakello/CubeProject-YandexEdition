namespace Source.Scripts.LeadTools.Other
{
	public abstract class Message<TMessage, TData>
		where TMessage : Message<TMessage, TData>
	{
		public TData Data;

		public TMessage SetData(TData data)
		{
			Data = data;

			return Instance;
		}

		protected abstract TMessage Instance { get; }
	}
}
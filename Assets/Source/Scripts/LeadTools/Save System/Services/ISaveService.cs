using System;

namespace LeadTools.SaveSystem.Services
{
	public interface ISaveService
	{
		public void GetData(Action<string> callback);

		public void SetData(string save);
	}
}
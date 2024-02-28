using System;

namespace LeadTools.SaveSystem.Services
{
	public class YandexSaveService : ISaveService
	{
		public void GetData(Action<string> callback) =>
			throw new Exception();
			//PlayerAccount.GetCloudSaveData(OnSuccessCallback);;

		public void SetData(string save) =>
			throw new NotImplementedException();
	}
}
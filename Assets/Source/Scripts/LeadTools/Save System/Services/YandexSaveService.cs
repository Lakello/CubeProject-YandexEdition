using System;
using Agava.YandexGames;

namespace LeadTools.SaveSystem.Services
{
	public class YandexSaveService : ISaveService
	{
		public void GetData(Action<string> successCallback) =>
			PlayerAccount.GetCloudSaveData(successCallback);

		public void SetData(string save) =>
			PlayerAccount.SetCloudSaveData(save);
	}
}
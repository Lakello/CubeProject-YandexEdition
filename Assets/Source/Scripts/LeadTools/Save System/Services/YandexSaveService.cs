using System;
using Agava.YandexGames;
using UnityEngine;

namespace LeadTools.SaveSystem.Services
{
	public class YandexSaveService : ISaveService
	{
		public void GetData(Action<string> successCallback) =>
			PlayerAccount.GetCloudSaveData(successCallback, Debug.LogError);

		public void SetData(string save) =>
			PlayerAccount.SetCloudSaveData(save, () => Debug.LogWarning("Save Success"), Debug.LogError);
	}
}
using System;
using System.IO;

namespace LeadTools.SaveSystem.Services
{
	public class JsonSaveService : ISaveService
	{
		private readonly string _saveSimPath = "Assets/Source/Scripts/LeadTools/Emulator/SaveSim.json";

		public void GetData(Action<string> successCallback)
		{
			string data = File.ReadAllText(_saveSimPath);

			successCallback?.Invoke(data);
		}

		public void SetData(string save) =>
			File.WriteAllText(_saveSimPath, save);
	}
}
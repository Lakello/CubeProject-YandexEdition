using System;
using LeadTools.SaveSystem.Services;
using UnityEngine;

namespace LeadTools.SaveSystem
{
	public class GameDataSaver : ISaver
	{
		public static ISaver Instance { get; private set; }

		private ISaveService _saveService;

		private GameSaves _gameSaves = new GameSaves();

		public GameDataSaver() =>
			Instance ??= this;

		public TData Get<TData>()
			where TData : SaveData<TData>, new() =>
			_gameSaves.Get<TData>().Clone();

		public void Set<TData>(TData value)
			where TData : SaveData<TData>, new() =>
			_gameSaves.Get<TData>().TryUpdateValue(value, Save);

		public void SubscribeValueUpdated<TData>(Action<TData> observer)
			where TData : SaveData<TData>, new() =>
			_gameSaves.Get<TData>().ValueUpdated += observer;

		public void UnsubscribeValueUpdated<TData>(Action<TData> observer)
			where TData : SaveData<TData>, new() =>
			_gameSaves.Get<TData>().ValueUpdated -= observer;

		public void Init()
		{
#if !UNITY_EDITOR
			_saveService = new YandexSaveService();
			_saveService.GetData(OnSuccessCallback);
#else
			//_savesEmulator.Init(OnSuccessCallback);
#endif

			return;

			void OnSuccessCallback(string data)
			{
				var saves = JsonUtility.FromJson<GameSaves>(data);
				_gameSaves = saves;
			}
		}

		private void Save()
		{
			string save = JsonUtility.ToJson(_gameSaves);

#if !UNITY_EDITOR
            PlayerAccount.SetCloudSaveData(save);
#else
			//_savesEmulator.Save(save);
#endif
		}
	}
}
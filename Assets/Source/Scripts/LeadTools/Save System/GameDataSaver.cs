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

		public void Init(Action successCallback)
		{
#if !UNITY_EDITOR
			_saveService = new YandexSaveService();
#else
			_saveService = new JsonSaveService();
#endif
			_saveService.GetData(OnSuccessCallback);

			return;

			void OnSuccessCallback(string data)
			{
				var saves = JsonUtility.FromJson<GameSaves>(data);
				_gameSaves = saves;
				successCallback?.Invoke();
			}
		}

		private void Save()
		{
			string gameSaves = JsonUtility.ToJson(_gameSaves);
			_saveService.SetData(gameSaves);
		}
	}
}
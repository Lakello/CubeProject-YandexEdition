using System;
using System.Reflection;
using CubeProject.Saves.Data;
using UnityEngine;

namespace LeadTools.SaveSystem
{
	[Serializable]
	public class GameSaves
	{
		[SerializeField] private CurrentLevel _currentLevel;

		public TData Get<TData>()
			where TData : SaveData<TData>, new()
		{
			var type = GetType();

			var fields = type.GetFields(
				BindingFlags.NonPublic
				| BindingFlags.Instance
				| BindingFlags.DeclaredOnly
				| BindingFlags.GetField);

			foreach (var field in fields)
			{
				if (field.FieldType == typeof(TData))
				{
					var value = field.GetValue(this);

					if (value == null)
					{
						field.SetValue(this, new TData());
						value = field.GetValue(this);
					}

					return (TData)value;
				}
			}

			throw new ArgumentNullException($"{typeof(TData)} Not contains");
		}
	}
}
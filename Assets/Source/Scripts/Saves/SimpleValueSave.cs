using System;
using LeadTools.SaveSystem;
using UnityEngine;

namespace CubeProject.Saves
{
	[Serializable]
	public abstract class SimpleValueSave<TValue, TData> : SaveData<TData>
		where TData : SimpleValueSave<TValue, TData>
	{
		[SerializeField] private TValue _value;

		public TValue Value { get => _value; protected set => _value = value; }

		public override abstract TData Clone();

		protected override void UpdateValue(TData data) =>
			_value = data.Value;

		protected override bool Equals(TData data) =>
			data.Value.Equals(_value);
	}
}
using System;
using UnityEngine;

namespace LeadTools.Extensions
{
	public static class GameObjectExtension
	{
		public static GameObject SetActiveAndReturn(this GameObject origin, bool value)
		{
			origin.SetActive(value);

			return origin;
		}

		public static T GetComponentElseThrow<T>(this GameObject origin) =>
			GetComponentElseThrow(origin, out T _);

		public static T GetComponentElseThrow<T>(this GameObject origin, out T component)
		{
			if (origin.TryGetComponent(out component) is false)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return component;
		}

		public static T GetComponentInChildrenElseThrow<T>(this GameObject origin) =>
			GetComponentInChildrenElseThrow(origin, out T _);

		public static T GetComponentInChildrenElseThrow<T>(this GameObject origin, out T component)
		{
			component = origin.GetComponentInChildren<T>();

			if (component is null)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return component;
		}
	}
}
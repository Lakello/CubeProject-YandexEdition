using System;
using UnityEngine;

namespace LeadTools.Extensions
{
	public static class GameObjectExtension
	{
		public static T FindObjectOfTypeElseThrow<T>(this GameObject origin, out T component)
			where T : UnityEngine.Object
		{
			component = UnityEngine.Object.FindObjectOfType<T>();

			if (component == null)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return component;
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
		
		public static T[] GetComponentsElseThrow<T>(this GameObject origin) =>
			GetComponentsElseThrow(origin, out T[] _);

		public static T[] GetComponentsElseThrow<T>(this GameObject origin, out T[] components)
		{
			components = origin.GetComponents<T>();

			if (components is null)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return components;
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
		
		public static T GetComponentInParentElseThrow<T>(this GameObject origin) =>
			GetComponentInParentElseThrow(origin, out T _);

		public static T GetComponentInParentElseThrow<T>(this GameObject origin, out T component)
		{
			component = origin.GetComponentInParent<T>();

			if (component is null)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return component;
		}

		public static T[] GetComponentsInChildrenElseThrow<T>(this GameObject origin) =>
			GetComponentInChildrenElseThrow(origin, out T[] _);

		public static T[] GetComponentsInChildrenElseThrow<T>(this GameObject origin, out T[] components)
		{
			components = origin.GetComponentsInChildren<T>();

			if (components is null)
			{
				Debug.LogException(new NullReferenceException(), origin);
			}

			return components;
		}
	}
}
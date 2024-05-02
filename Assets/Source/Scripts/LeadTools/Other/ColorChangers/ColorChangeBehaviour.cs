using System;
using LeadTools.Other;
using UnityEngine;

namespace Source.Scripts.LeadTools.Other
{
	public abstract class ColorChangeBehaviour : MonoBehaviour
	{
		[SerializeField] private HDRColorChanger[] _changers;

		protected void Do(Action<HDRColorChanger> action)
		{
			foreach (var changer in _changers)
			{
				action(changer);
			}
		}
	}
}
using System;
using UnityEngine;

namespace LeadTools.StateMachine
{
	public abstract class Window : MonoBehaviour
	{
		public abstract Type WindowType { get; }
	}
}
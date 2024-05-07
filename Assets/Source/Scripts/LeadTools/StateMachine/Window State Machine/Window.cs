using System;
using CubeProject.LeadTools.UI;
using UnityEngine;

namespace LeadTools.StateMachine
{
	public abstract class Window : MonoBehaviour
	{
		[SerializeField] private AnchorAnimationGroup _animations;

		public AnchorAnimationGroup Animations => _animations;
		public abstract Type WindowType { get; }
	}
}
using System.Collections.Generic;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.LeadTools.UI.PageSystem
{
	[RequireComponent(typeof(AnchorAnimator))]
	public class Page : MonoBehaviour
	{
		[SerializeField] private int _maxElements = 9;

		private AnchorGroupAnimator _animator;
		private List<GameObject> _elements = new List<GameObject>();

		public void Init()
		{
			var animator = gameObject.GetComponentElseThrow<AnchorAnimator>();

			var group = new AnchorAnimationGroup(new[]
			{
				animator
			});

			_animator = new AnchorGroupAnimator(group);
		}

		public bool TryTakeElement(GameObject element)
		{
			if (_elements.Count < _maxElements)
			{
				element.transform.SetParent(transform);
				_elements.Add(element);

				return true;
			}

			return false;
		}

		public void Show()
		{
			gameObject.SetActive(true);

			_animator.PlayAnimations(AnchorAnimatorState.Target);
		}

		public void Hide() =>
			_animator.PlayAnimations(
				AnchorAnimatorState.Initial,
				() => gameObject.SetActive(false));
	}
}
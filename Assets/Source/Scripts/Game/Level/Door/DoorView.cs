using DG.Tweening;
using UnityEngine;

namespace CubeProject.Game
{
	public class DoorView : MonoBehaviour
	{
		[SerializeField] private DOTweenAnimation[] _animationDoors;
		[SerializeField] private DOTweenAnimation[] _animationDoorsCenter;
		
		public void Open()
		{
			foreach (var center in _animationDoorsCenter)
			{
				
				center.DOPlay();
			}
		}

		public void Close()
		{
			
		}
	}
}
using UnityEngine;

namespace CubeProject.Game
{
	public class Power : MonoBehaviour
	{
		private ChargeHolder _parentHolder;

		public ChargeHolder ParentHolder => _parentHolder;

		public void Init(ChargeHolder holder)
		{
			_parentHolder = holder;
		}
	}
}
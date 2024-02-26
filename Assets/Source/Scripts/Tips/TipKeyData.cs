using System;
using UnityEngine;

namespace CubeProject.Tips
{
	[Serializable]
	public struct TipKeyData
	{
		[SerializeField] private TipKeyType _tipKeyType;

		public TipKeyType Type => _tipKeyType;

		public bool IsUsableKeyType(TipKeyType tipKeyType) =>
			_tipKeyType == tipKeyType;
	}
}
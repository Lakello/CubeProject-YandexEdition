using System;
using LeadTools.Extensions;
using LeadTools.Other;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Source.Scripts.LeadTools.Other
{
	public abstract class ColorChangeBehaviour : MonoBehaviour
	{
		[SerializeField] private HDRColorChanger[] _changers;

		#if UNITY_EDITOR
		[Button]
		private void SetThisMeshRenderer()
		{
			var meshRenderer = gameObject.GetComponentElseThrow<MeshRenderer>();

			_changers.ForEach(changer => changer.Init(new[]
			{
				meshRenderer
			}));

			EditorUtility.SetDirty(this);
		}
		#endif

		protected void Do(Action<HDRColorChanger> action) =>
			_changers.ForEach(changer => action?.Invoke(changer));
	}
}
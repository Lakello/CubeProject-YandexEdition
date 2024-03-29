using System;
using LeadTools.NaughtyAttributes;
using UnityEngine;

namespace CubeProject.LeadTools.Utils
{
	[Serializable]
	public class PresetType
	{
		[SerializeField] [Dropdown(nameof(GetPresets))] private ObjectSpawnerPreset _value;

		public ObjectSpawnerPreset Value => _value;
		
		private DropdownList<ObjectSpawnerPreset> GetPresets()
		{
			var presets = Resources.FindObjectsOfTypeAll<ObjectSpawnerPreset>();

			var list = new DropdownList<ObjectSpawnerPreset>();
			
			foreach (var preset in presets)
			{
				list.Add(preset.name, preset);
			}

			return list;
		}
	}
}
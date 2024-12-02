#if UNITY_EDITOR
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeadTools.Utils.MeshCombineSystem
{
	[CustomEditor(typeof(MeshCombiner))]
	public class MeshCombinerEditor : Editor
	{
		private const string FolderPath = "Source/Art/Prefabs/CombinedMeshes/";

		public override void OnInspectorGUI()
		{
			MeshCombiner meshCombiner = (MeshCombiner)target;
			Mesh mesh = meshCombiner.GetComponent<MeshFilter>().sharedMesh;

			#region Script:

			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MeshCombiner)target), typeof(MeshCombiner), false);
			GUI.enabled = true;

			#endregion Script.

			#region MeshFiltersToSkip array:

			SerializedProperty meshFiltersToSkip = serializedObject.FindProperty("_meshFiltersToSkip");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(meshFiltersToSkip, true);

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}

			#endregion MeshFiltersToSkip array.

			#region Button which combine Meshes into one Mesh & Toggles with combine options:

			// Button:
			if (GUILayout.Button("Combine Meshes"))
			{
				meshCombiner.CombineMeshes(true);
			}

			// Toggles:
			meshCombiner.CreateMultiMaterialMesh = GUILayout.Toggle(meshCombiner.CreateMultiMaterialMesh, "Create Multi-Material Mesh");
			meshCombiner.CombineInactiveChildren = GUILayout.Toggle(meshCombiner.CombineInactiveChildren, "Combine Inactive Children");

			meshCombiner.DeactivateCombinedChildren = GUILayout.Toggle(meshCombiner.DeactivateCombinedChildren, "Deactivate Combined Children");

			meshCombiner.IsDeactivateCombinedChildrenMeshRenderers = GUILayout.Toggle(meshCombiner.IsDeactivateCombinedChildrenMeshRenderers,
				"Deactivate Combined Children's MeshRenderers");

			meshCombiner.IsGenerateUVMap = GUILayout.Toggle(meshCombiner.IsGenerateUVMap, new GUIContent("Generate UV Map", "It is a slow operation that " +
				"generates a UV map (required for the lightmap).\n\nCan be used only in the Editor."));

			// The last (6) "Destroy Combined Children" Toggle:
			GUIStyle style = new GUIStyle(EditorStyles.toggle);

			if (meshCombiner.IsDestroyCombinedChildren)
			{
				style.onNormal.textColor = new Color(1, 0.15f, 0);
			}

			meshCombiner.IsDestroyCombinedChildren = GUILayout.Toggle(meshCombiner.IsDestroyCombinedChildren,
				new GUIContent("Destroy Combined Children", "In the editor this operation can NOT be undone!\n\n" +
					"If you want to bring back destroyed GameObjects, you have to load again the scene without saving."), style);

			meshCombiner.IsFindByTypeCombineMesh = GUILayout.Toggle(meshCombiner.IsFindByTypeCombineMesh,
				new GUIContent("IsFindByTypeCombineMesh"));

			#endregion Button which combine Meshes into one Mesh & Toggles with combine options.

			#region Path to the folder where combined Meshes will be saved:

			// Create Labels:
			GUILayout.Label("");
			GUILayout.Label(new GUIContent("Folder path:", "Folder path to save combined Mesh."));

			// Create style wherein text color will be red if folder path is not valid:
			style = new GUIStyle(EditorStyles.textField);

			var folderPath = $"{FolderPath}/{SceneManager.GetActiveScene().name}/{meshCombiner.gameObject.name}";

			bool isValidPath = IsValidPath(folderPath);

			if (!isValidPath)
			{
				style.normal.textColor = Color.red;
				style.focused.textColor = Color.red;
			}

			EditorGUILayout.TextField(folderPath, style);

			#endregion Path to the folder where combined Meshes will be saved.

			#region Button which save/show combined Mesh:

			bool meshIsSaved = (mesh != null && AssetDatabase.Contains(mesh));
			GUI.enabled = (mesh != null && (isValidPath || meshIsSaved)); // Valid path is required for not saved Mesh.
			string saveMeshButtonText = (meshIsSaved) ? "Show Saved Combined Mesh" : "Save Combined Mesh";

			if (GUILayout.Button(saveMeshButtonText))
			{
				SaveCombinedMesh(mesh, folderPath);
			}

			GUI.enabled = true;

			#endregion Button which save/show combined Mesh.
		}

		private bool IsValidPath(string folderPath)
		{
			string pattern = "[:*?\"<>|]"; // Prohibited characters.
			Regex regex = new Regex(pattern);

			return (!regex.IsMatch(folderPath));
		}

		private void SaveCombinedMesh(Mesh mesh, string folderPath)
		{
			bool meshIsSaved = AssetDatabase.Contains(mesh); // If is saved then only show it in the project view.

			#region Create directories if Mesh and path doesn't exists:

			folderPath = folderPath.Replace('\\', '/');

			if (!meshIsSaved && !AssetDatabase.IsValidFolder("Assets/" + folderPath))
			{
				string[] folderNames = folderPath.Split('/');
				folderNames = folderNames.Where((folderName) => !folderName.Equals("")).ToArray();
				folderNames = folderNames.Where((folderName) => !folderName.Equals(" ")).ToArray();

				folderPath = "/"; // Reset folder path.

				for (int i = 0; i < folderNames.Length; i++)
				{
					folderNames[i] = folderNames[i].Trim();

					if (!AssetDatabase.IsValidFolder("Assets" + folderPath + folderNames[i]))
					{
						string folderPathWithoutSlash = folderPath.Substring(0, folderPath.Length - 1); // Delete last "/" character.
						AssetDatabase.CreateFolder("Assets" + folderPathWithoutSlash, folderNames[i]);
					}

					folderPath += folderNames[i] + "/";
				}

				folderPath = folderPath.Substring(1, folderPath.Length - 2); // Delete first and last "/" character.
			}

			#endregion Create directories if Mesh and path doesn't exists.

			#region Save Mesh:

			if (!meshIsSaved)
			{
				string meshPath = "Assets/" + folderPath + "/" + mesh.name + ".asset";
				int assetNumber = 1;

				while (AssetDatabase.LoadAssetAtPath(meshPath, typeof(Mesh)) != null) // If Mesh with same name exists, change name.
				{
					meshPath = "Assets/" + folderPath + "/" + mesh.name + " (" + assetNumber + ").asset";
					assetNumber++;
				}

				AssetDatabase.CreateAsset(mesh, meshPath);
				AssetDatabase.SaveAssets();
				Debug.Log("<color=#ff9900><b>Mesh \"" + mesh.name + "\" was saved in the \"" + folderPath + "\" folder.</b></color>"); // Show info about saved mesh.
			}

			#endregion Save Mesh.

			EditorGUIUtility.PingObject(mesh); // Show Mesh in the project view.
		}
	}
}
#endif
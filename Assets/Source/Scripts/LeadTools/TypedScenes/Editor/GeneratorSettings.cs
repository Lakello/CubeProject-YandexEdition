namespace LeadTools.TypedScenes.Editor
{
	public struct GeneratorSettings
	{
		public const string Namespace = "LeadTools.TypedScenes.Scenes";
		public const string BaseTypeName = "TypedScene";
		public const string SceneNameField = "_sceneName";
		public const string UnityEngineImport = "UnityEngine";
		public const string SceneManagementImport = "UnityEngine.SceneManagement";
		public const string FSMImport = "LeadTools.FSM";
		public const string GameFSMImport = "LeadTools.FSM.GameFSM";
		public const string TypedScenesCoreImport = "LeadTools.TypedScenes.Core";
		public const string DomProvider = "CSharp";
		public const string BracingStyle = "C";
		public const string GameStateMachine = nameof(GameStateMachine);
	}
}
namespace LeadTools.TypedScenes.Editor
{
	public struct MethodFactorySettings
	{
		public const string LoadSceneModeParameter = "loadSceneMode = LoadSceneMode.Single";
		public const string ArgumentT = "T";
		public const string Argument = "argument";
		public const string Machine = "machine";
		public const string StateT = "TState";
		public const string StateType = "State";

		public const string StatementName = "LoadScene";

		private const string LoadAsync = nameof(LoadAsync);
		private const string Load = nameof(Load);

		public static string GetStartParameters => $"({GeneratorSettings.SceneNameField}, loadSceneMode";

		public static string GetLoadName(bool isLoadAsync) =>
			isLoadAsync ? LoadAsync : Load;

		public static string TryGetComma(int index)
		{
			if (index > 0)
			{
				return ", ";
			}

			return string.Empty;
		}
	}
}
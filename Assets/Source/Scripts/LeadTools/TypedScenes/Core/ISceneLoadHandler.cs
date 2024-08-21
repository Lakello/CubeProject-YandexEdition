namespace LeadTools.TypedScenes.Core
{
	public interface ISceneLoadHandler
	{
		public void OnSceneLoaded<T>(T argument);
	}
}
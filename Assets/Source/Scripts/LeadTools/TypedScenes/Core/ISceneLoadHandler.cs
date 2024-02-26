namespace LeadTools.TypedScenes
{
	public interface ISceneLoadHandler
	{
		public void OnSceneLoaded<T>(T argument);
	}
}
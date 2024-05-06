using System.Linq;

namespace CubeProject.LeadTools.UI.PageSystem
{
	public class PageService
	{
		private readonly Page[] _pages;

		private Page _currentPage;
		
		public PageService(Page[] pages)
		{
			_pages = pages;
			_currentPage = _pages.First();
		}

		public void NextPage()
		{
			
		}
	}
}
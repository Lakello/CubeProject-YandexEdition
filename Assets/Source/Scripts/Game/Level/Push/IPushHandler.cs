using System;

namespace CubeProject.Tips
{
	public interface IPushHandler
	{
		public event Action Pushing;
	}
}
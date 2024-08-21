using System;

namespace CubeProject.Game.AudioSystem
{
	public interface IAudioSubject
	{
		public event Action AudioPlaying;
	}
}
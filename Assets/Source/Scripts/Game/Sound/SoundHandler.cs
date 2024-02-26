using UnityEngine;

namespace CubeProject.Game.Sound
{
	public class SoundHandler : MonoBehaviour
	{
		[SerializeField] private AudioSource _audioSourceOne;
		[SerializeField] private AudioSource _audioSourceTwo;

		private AudioSource _currentSource;
		
		public void Play()
		{
			
		}
	}
}
using CubeProject.Player;

namespace CubeProject.Effects
{
	public struct ElectricInit
	{
		public Cube Cube { get; }

		public float StartRadius { get; }

		public float EndRadius { get; }

		public float LifeTime { get; }

		public ElectricInit(Cube cube, float startRadius, float endRadius, float lifeTime)
		{
			Cube = cube;
			StartRadius = startRadius;
			EndRadius = endRadius;
			LifeTime = lifeTime;
		}
	}
}
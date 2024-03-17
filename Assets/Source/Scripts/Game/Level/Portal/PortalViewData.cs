using UnityEngine;

namespace CubeProject.Game
{
	public class PortalViewData
	{
		public PortalViewData(
			Color color,
			float rotateSpeed = 0,
			float scaleSpeed = 0,
			float lightPulseSpeed = 0,
			float scaleProgress = 0,
			float lightPulseProgress = 0)
		{
			RotateSpeed = rotateSpeed;
			ScaleSpeed = scaleSpeed;
			LightPulseSpeed = lightPulseSpeed;
			Color = color;
			ScaleProgress = scaleProgress;
			LightPulseProgress = lightPulseProgress;
		}

		public float RotateSpeed { get; private set; }

		public float ScaleProgress { get; private set; }

		public float LightPulseProgress { get; private set; }

		public Color Color { get; private set; }

		private float ScaleSpeed { get; set; }

		private float LightPulseSpeed { get; set; }

		public void UpdateSpeeds(float rotateSpeed, float scaleSpeed, float lightPulseSpeed)
		{
			RotateSpeed = rotateSpeed;
			ScaleSpeed = scaleSpeed;
			LightPulseSpeed = lightPulseSpeed;
		}

		public void UpdateProgress()
		{
			ScaleProgress = GetUpdatedProgress(ScaleProgress, ScaleSpeed);
			LightPulseProgress = GetUpdatedProgress(LightPulseProgress, LightPulseSpeed);
		}

		private float GetUpdatedProgress(float progress, float speed)
		{
			progress += speed * Time.deltaTime;
			
			if (progress > 1)
			{
				progress = 0;
			}

			return progress;
		}
	}
}
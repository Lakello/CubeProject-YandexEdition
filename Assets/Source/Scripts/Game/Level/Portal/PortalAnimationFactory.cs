using DG.Tweening;
using LeadTools.Extensions;
using Unity.Mathematics;
using UnityEngine;

namespace CubeProject.Game
{
	public static class PortalAnimationFactory
	{
		public static Tweener CreateAppear(Transform origin, float appearScaleDuration, Ease appearScaleEase)
		{
			Vector3 newScale = new Vector3();
			
			return DOTween
				.To(
					progress =>
					{
						newScale = newScale.SetAll(progress);
						origin.localScale = newScale;
					},
					0,
					1,
					appearScaleDuration)
				.SetEase(appearScaleEase)
				.SetAutoKill(false)
				.Pause();
		}
		
		public static Tweener CreateRotation(Transform origin, float durationTurnover)
		{
			Vector3 newRotation = new Vector3();
			
			return DOTween
				.To(progress =>
					{
						newRotation.y = progress;
						origin.localRotation = quaternion.Euler(newRotation);
					},
					0,
					360,
					durationTurnover)
				.SetLoops(-1, LoopType.Incremental)
				.SetAutoKill(false)
				.Pause();
		}

		public static Tweener CreateScale(Transform origin, Vector2 playingScaleRange, float playingScaleSpeed)
		{
			Vector3 newPlayingScale = new Vector3();

			return DOTween
				.To(progress =>
					{
						newPlayingScale = newPlayingScale.SetAll(progress);
						origin.localScale = newPlayingScale;
					},
					playingScaleRange.x,
					playingScaleRange.y,
					playingScaleSpeed)
				.SetLoops(-1, LoopType.Yoyo)
				.SetAutoKill(false)
				.Pause();
		}
	}
}
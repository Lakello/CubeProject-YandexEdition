using System;
using CubeProject.Game.Level;
using CubeProject.Game.Messages;
using CubeProject.Game.Player;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Source.Scripts.Yandex
{
	public class AdService : IDisposable
	{
		private const float ShowCooldownInSeconds = 60f;
		private const float AdDelayInEditor = 3f;

		private CompositeDisposable _disposable;
		private CompositeDisposable _preLevelLoadingDisposable;

		public AdService()
		{
			_disposable = new CompositeDisposable();
			
			StartAdCooldown();
		}

		public void Dispose()
		{
			_disposable?.Dispose();
			_preLevelLoadingDisposable?.Dispose();
		}

		private void StartAdCooldown()
		{
			MessageBroker.Default
				.Publish(new Message<AdService>(MessageId.ResumeLevelLoading));
			
			Observable.Timer(TimeSpan.FromSeconds(ShowCooldownInSeconds))
				.Subscribe(_ => WaitPossibleShowAd())
				.AddTo(_disposable);
		}

		private void WaitPossibleShowAd()
		{
			_preLevelLoadingDisposable = new CompositeDisposable();
			
			MessageBroker.Default
				.Receive<Message<LevelLoader>>()
				.Where(message => message.Id == MessageId.PreLevelLoading)
				.Subscribe(_ => ShowAd())
				.AddTo(_preLevelLoadingDisposable);

			MessageBroker.Default
				.Publish(new Message<AdService>(MessageId.SuspendLevelLoading));
		}
		
		private void ShowAd()
		{
			_preLevelLoadingDisposable?.Dispose();
			_preLevelLoadingDisposable = null;
			
#if !UNITY_EDITOR
			Agava.YandexGames.InterstitialAd.Show(onCloseCallback: _ => StartAdCooldown());
#else
			Observable.Timer(TimeSpan.FromSeconds(AdDelayInEditor))
				.Subscribe(_ => StartAdCooldown())
				.AddTo(_disposable);
#endif
		}
	}
}
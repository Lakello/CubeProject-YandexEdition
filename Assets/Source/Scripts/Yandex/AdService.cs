using System;
using CubeProject.Game.Level.Loader.Messages;
using CubeProject.Yandex.Messages;
using UniRx;
using UnityEngine;

namespace CubeProject.Yandex
{
	public class AdService : IDisposable
	{
		private const float ShowCooldownInSeconds = 60f;
		private const float AdDelayInEditor = 3f;

		private readonly M_ADCooldown _adCooldownMessage = new M_ADCooldown();
		private readonly M_ADReady _adReadyMessage = new M_ADReady();
		private readonly M_ADShow _adShowMessage = new M_ADShow();
		private readonly CompositeDisposable _disposable = new CompositeDisposable();

		private CompositeDisposable _preLevelLoadingDisposable;

		public AdService()
		{
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
				.Publish(_adCooldownMessage);

			Observable.Timer(TimeSpan.FromSeconds(ShowCooldownInSeconds))
				.Subscribe(_ => WaitPossibleShowAd())
				.AddTo(_disposable);
		}

		private void WaitPossibleShowAd()
		{
			_preLevelLoadingDisposable = new CompositeDisposable();

			MessageBroker.Default
				.Receive<M_PreLevelLoading>()
				.Subscribe(_ => ShowAd())
				.AddTo(_preLevelLoadingDisposable);

			MessageBroker.Default
				.Publish(_adReadyMessage);
		}

		private void ShowAd()
		{
			_preLevelLoadingDisposable?.Dispose();
			_preLevelLoadingDisposable = null;

			MessageBroker.Default
				.Publish(_adShowMessage);

#if !UNITY_EDITOR
			Agava.YandexGames.InterstitialAd.Show(onCloseCallback: _ => StartAdCooldown());
#else
			Debug.Log("SHOW AD");

			Observable.Timer(TimeSpan.FromSeconds(AdDelayInEditor))
				.Subscribe(_ => StartAdCooldown())
				.AddTo(_disposable);
#endif
		}
	}
}
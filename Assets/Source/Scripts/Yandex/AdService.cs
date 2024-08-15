using System;
using Game.Level.Message;
using Yandex.Messages;
using UniRx;

namespace Yandex
{
	public class AdService : IDisposable
	{
		private const float ShowCooldownInSeconds = 60f;
		private const float AdDelayInEditor = 3f;

		private readonly M_ADCooldown _adCooldownMessage = new M_ADCooldown();
		private readonly M_ADShow _adShowMessage = new M_ADShow();

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
				.Publish(_adShowMessage);
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
using System;
using UniRx;
using UnityEngine;
using Yandex.Messages;

namespace Yandex
{
	public class AdObserver : IDisposable
	{
		private readonly CompositeDisposable _disposable = new CompositeDisposable();
		
		public AdObserver()
		{
			MessageBroker.Default
				.Receive<M_ADShow>()
				.Subscribe(_ =>ChangeVolume(true))
				.AddTo(_disposable);
			
			MessageBroker.Default
				.Receive<M_ADCooldown>()
				.Subscribe(_ => ChangeVolume(false))
				.AddTo(_disposable);
		}

		public bool IsMute { get; private set; }

		public void Dispose()
		{
			_disposable?.Dispose();
		}

		private void ChangeVolume(bool isMute)
		{
			IsMute = isMute;

			AudioListener.volume = isMute ? 0 : 1;
		}
	}
}
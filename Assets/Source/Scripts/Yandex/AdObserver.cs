using System;
using UniRx;
using UnityEngine;
using Yandex.Messages;

namespace Yandex
{
	public class AdObserver : IDisposable
	{
		private CompositeDisposable _disposable = new CompositeDisposable();
		
		public AdObserver()
		{
			MessageBroker.Default
				.Receive<M_ADShow>()
				.Subscribe(_ => AudioListener.volume = 0)
				.AddTo(_disposable);
			
			MessageBroker.Default
				.Receive<M_ADCooldown>()
				.Subscribe(_ => AudioListener.volume = 1)
				.AddTo(_disposable);
		}
		
		public void Dispose()
		{
			_disposable?.Dispose();
		}
	}
}
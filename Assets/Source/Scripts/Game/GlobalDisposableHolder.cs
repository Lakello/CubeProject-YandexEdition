using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class GlobalDisposableHolder : MonoBehaviour
	{
		private readonly List<IDisposable> _disposables = new List<IDisposable>();
		
		public void Add(IDisposable disposable) =>
			_disposables.Add(disposable);

		private void OnDestroy() =>
			_disposables.ForEach(disposable => disposable?.Dispose());
	}
}
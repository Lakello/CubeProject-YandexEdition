using CubeProject.InputSystem;
using Reflex.Attributes;

namespace CubeProject.Tips
{
	public class UseTipKeyHandler : TipKeyHandler
	{
		private bool _isRelease;
		private IInputHandler _handler;

		public bool CanUse { get; private set; }

		[Inject]
		protected void Inject(IInputHandler inputHandler)
		{
			_handler = inputHandler;

			_handler.UsePressed += OnUsePressed;
			_handler.UseReleased += OnUseReleased;

			TryChangeKeyState();
		}

		public void OnCanUseChanged(bool canUse)
		{
			CanUse = canUse;
			TryChangeKeyState();
		}

		private void OnDisable()
		{
			_handler.UsePressed -= OnUsePressed;
			_handler.UseReleased -= OnUseReleased;
		}

		private void OnUsePressed()
		{
			_isRelease = false;
			TryChangeKeyState();
		}

		private void OnUseReleased()
		{
			_isRelease = true;
			TryChangeKeyState();
		}

		private void TryChangeKeyState()
		{
			if (TipKey.Data.IsUsableKeyType(TipKeyType.Space) is false)
			{
				return;
			}

			if (CanUse)
			{
				if (_isRelease)
				{
					TryRelease();
				}
				else
				{
					TryPress();
				}
			}
			else
			{
				TryPress();
			}
		}
	}
}
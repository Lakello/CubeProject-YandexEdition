using System;
using System.Collections.Generic;
using CubeProject.Game.Level.Loader;
using LeadTools.FSM;
using LeadTools.FSM.GameFSM;
using LeadTools.FSM.GameFSM.States;
using LeadTools.FSM.WindowFSM.States;
using Reflex.Attributes;
using UnityEngine;

namespace LeadTools.UI.PageSystem
{
	public class PageBehaviour : MonoBehaviour
	{
		[SerializeField] private Page _pagePrefab;
		[SerializeField] private Transform _container;

		private List<Page> _pages = new List<Page>();
		private IStateChangeable<GameStateMachine> _stateChangeable;
		private int _currentPageIndex;

		[Inject]
		private void Inject(IStateChangeable<GameStateMachine> stateChangeable)
		{
			_stateChangeable = stateChangeable;
		}

		private void OnEnable()
		{
			_pages[_currentPageIndex].Show();

			_stateChangeable?.SubscribeTo<MenuState<SelectLevelWindowState>>(OnStateChanged);
		}

		private void OnDisable() =>
			_stateChangeable.UnSubscribeTo<MenuState<SelectLevelWindowState>>(OnStateChanged);

		public void Init(Queue<LevelButton> buttons)
		{
			if (_pages.Count != 0)
				return;

			Page currentPage = InitPage();

			while (buttons.Count > 0)
			{
				var button = buttons.Dequeue();

				AddButton(button);
			}

			return;

			void AddButton(LevelButton button)
			{
				if (currentPage.TryTakeElement(button.gameObject) == false)
				{
					currentPage = InitPage();
					AddButton(button);
				}
			}

			Page InitPage()
			{
				var page = Instantiate(_pagePrefab, _container);
				page.Init();
				page.Hide();
				_pages.Add(page);

				return page;
			}
		}

		public void NextPage() =>
			ChangePage(() =>
			{
				_currentPageIndex++;

				if (_currentPageIndex >= _pages.Count)
					_currentPageIndex = 0;
			});

		public void PreviousPage() =>
			ChangePage(() =>
			{
				_currentPageIndex--;

				if (_currentPageIndex <= 0)
					_currentPageIndex = _pages.Count - 1;
			});

		private void ChangePage(Action changeIndex)
		{
			_pages[_currentPageIndex].Hide();

			changeIndex();

			_pages[_currentPageIndex].Show();
		}

		private void OnStateChanged(bool isEntered)
		{
			if (isEntered)
				return;

			_pages[_currentPageIndex].Hide();
		}
	}
}
using System;
using System.Collections.Generic;
using Source.Scripts.Game.Level;
using UnityEngine;

namespace CubeProject.LeadTools.UI.PageSystem
{
	public class PageBehaviour : MonoBehaviour
	{
		[SerializeField] private Page _pagePrefab;
		[SerializeField] private Transform _container;

		private List<Page> _pages;

		private int _currentPageIndex;

		public void Init(Queue<LevelButton> buttons)
		{
			if (_pages != null)
				return;

			_pages = new List<Page>();

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
				_pages.Add(page);

				return page;
			}
		}

		private void Start()
		{
			_pages.ForEach(page => page.Hide());

			_pages[_currentPageIndex].Show();
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
	}
}
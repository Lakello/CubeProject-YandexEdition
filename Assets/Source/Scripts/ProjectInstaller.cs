using System;
using System.Collections.Generic;
using LeadTools.Other;
using LeadTools.SaveSystem;
using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using Reflex.Core;
using UnityEngine;

namespace CubeProject
{
	public class ProjectInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private AudioClip _audioClipBackground;

		private Action _unsubscribe;

		public void InstallBindings(ContainerDescriptor descriptor)
		{
			var audioSource = new GameObject(nameof(AudioSource)).AddComponent<AudioSource>();
			audioSource.clip = _audioClipBackground;
			audioSource.loop = true;
			audioSource.playOnAwake = false;

			DontDestroyOnLoad(audioSource.gameObject);

			audioSource.Play();

			var context = new GameObject(nameof(Context)).AddComponent<Context>();
			DontDestroyOnLoad(context.gameObject);

			GameStateMachine gameStateMachine;

			InitStateMachine();

			ProjectInit();

			return;

			#region InitMethods

			void InitStateMachine()
			{
				var windowStateMachine = new WindowStateMachine(
					() => new Dictionary<Type, State<WindowStateMachine>>
					{
						[typeof(MenuWindowState)] = new MenuWindowState(),
						[typeof(PlayLevelWindowState)] = new PlayLevelWindowState(),
						[typeof(EndLevelWindowState)] = new EndLevelWindowState(),
					});

				gameStateMachine = new GameStateMachine(
					windowStateMachine,
					() => new Dictionary<Type, State<GameStateMachine>>
					{
						[typeof(MenuState)] = new MenuState(() => windowStateMachine.EnterIn<MenuWindowState>()),
						[typeof(PlayLevelState)] = new PlayLevelState(() => windowStateMachine.EnterIn<PlayLevelWindowState>()),
						[typeof(EndLevelState)] = new EndLevelState(() => windowStateMachine.EnterIn<EndLevelWindowState>()),
					});

				descriptor.AddSingleton(gameStateMachine, typeof(IStateChangeable));
			}

			void ProjectInit()
			{
				var projectInitializer = new GameObject(nameof(ProjectInitializer)).AddComponent<ProjectInitializer>();

				projectInitializer.Init(
					gameStateMachine,
					() =>
					{
						var saver = new GameDataSaver();
						saver.Init();
					});
			}

			#endregion
		}
	}
}
using Cinemachine;
using CubeProject.PlayableCube;
using CubeProject.Player;
using Reflex.Attributes;
using Source.Scripts.Game;
using UnityEngine;

namespace CubeProject.Game
{
	public class EndPoint : MonoBehaviour
	{
		private CinemachineVirtualCamera _virtualCamera;
		private LayerMask _groundMask;

		[Inject]
		private void Inject(CinemachineVirtualCamera virtualCamera, MaskHolder maskHolder)
		{
			_groundMask = maskHolder.GroundMask;
			_virtualCamera = virtualCamera;
		}

		private void Awake()
		{
			if (Physics.Raycast(transform.position, Vector3.down, out var hit, _groundMask))
			{
				hit.transform.gameObject.SetActive(false);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube cube))
			{
				cube.ComponentsHolder.StateHandler.EnterIn(CubeState.EndLevel);

				_virtualCamera.LookAt = null;
				_virtualCamera.Follow = null;
			}
		}
	}
}
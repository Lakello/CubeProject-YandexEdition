using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeProject.UI 
{
    public class LeaderboardViewer : MonoBehaviour
    {
        [SerializeField] private Transform _scrollViewContent;
        [SerializeField] private PlayerInfo _uIPlayerInfo;

        private List<PlayerInfo> _spawnedResultPlayersOnLeaderboard = new List<PlayerInfo>();

        public void ShowLeaderboard(List<ResultPlayer> resultPlayers)
        {
            ClearResultPlayer();

            for (int i = 0; i < resultPlayers.Count; i++)
            {
                PlayerInfo resultPlayer = Instantiate(_uIPlayerInfo, _scrollViewContent);
                resultPlayer.ShowInfo(resultPlayers[i]);
                _spawnedResultPlayersOnLeaderboard.Add(resultPlayer);
            }
        }

        private void ClearResultPlayer()
        {
            foreach (var resultPlayer in _spawnedResultPlayersOnLeaderboard)
            {
                Destroy(resultPlayer.gameObject);
            }

            _spawnedResultPlayersOnLeaderboard = new List<PlayerInfo>();
        }
    }
}
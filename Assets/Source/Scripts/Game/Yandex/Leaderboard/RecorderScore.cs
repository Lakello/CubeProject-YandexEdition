using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leaderboard = Agava.YandexGames.Leaderboard;

namespace CubeProject.UI
{
    public class RecorderScore : MonoBehaviour
    {
        private string _leaderboardName = "PlayersScore";

        public void AddPlayer(int score)
        {
            Leaderboard.GetPlayerEntry(_leaderboardName, result =>
            {
                Leaderboard.SetScore(_leaderboardName, score);                
            });

            /* if (_virtualEmulator.GetPlayerEntry() != null)
             {
                 _virtualLeaderboard.RecordResultPlayer(score);
                 GetLeaderboard();
             }*/
        }
    }
}
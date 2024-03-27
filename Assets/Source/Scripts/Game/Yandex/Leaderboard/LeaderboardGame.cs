using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Agava.YandexGames;
using Leaderboard = Agava.YandexGames.Leaderboard;

namespace CubeProject.UI
{
    public class LeaderboardGame : MonoBehaviour
    {
        [SerializeField] private LeaderboardViewer _leaderboardViewer;
        //[SerializeField] private VirtualEmulator _virtualEmulator;
        //[SerializeField] private VirtualLeaderboard _virtualLeaderboard;
        //[SerializeField] private LeanLocalization _leanLocalization;

        private List<ResultPlayer> _resultPlayers = new List<ResultPlayer>();
        private string _leaderboardName = "PlayersScore";

        public void AddPlayer(int score)
        {
            Leaderboard.GetPlayerEntry(_leaderboardName, result =>
            {
                Leaderboard.SetScore(_leaderboardName, score);
                GetLeaderboard();
            });

           /* if (_virtualEmulator.GetPlayerEntry() != null)
            {
                _virtualLeaderboard.RecordResultPlayer(score);
                GetLeaderboard();
            }*/
        }

        public void GetLeaderboard()
        {
            ClearEntries();


             Leaderboard.GetEntries(_leaderboardName, result =>
             {
                 int AmountEntries = result.entries.Length;
                 AmountEntries = Mathf.Clamp(AmountEntries, 1, 5);

                 for (int i = 0; i < AmountEntries; i++)
                 {
                     string name = result.entries[i].player.publicName;

                     /*if (string.IsNullOrEmpty(name))
                     {
                         if (_leanLocalization.CurrentLanguage == "Russian")
                         {
                             name = "Аноним";
                         }
                         else if (_leanLocalization.CurrentLanguage == "English")
                         {
                             name = "anonymous";
                         }
                         else if (_leanLocalization.CurrentLanguage == "Turkish")
                         {
                             name = "Anonim";
                         }
                     }*/

                     int score = result.entries[i].score;
                     int rank = result.entries[i].rank;

                     _resultPlayers.Add(new ResultPlayer(name, score, rank));
                 }

                 _leaderboardViewer.ShowLeaderboard(_resultPlayers);
             });
            /*
            if (_virtualLeaderboard.ResultPlayers.Count != 0)
            {
                int amountEntries = _virtualLeaderboard.ResultPlayers.Count;

                for (int i = 0; i < amountEntries; i++)
                {
                    string name = _virtualLeaderboard.ResultPlayers[i].Name;
                    int score = _virtualLeaderboard.ResultPlayers[i].Score;
                    int rank = _virtualLeaderboard.ResultPlayers[i].Rank;

                    _resultPlayers.Add(new ResultPlayer(name, score, rank));
                }

                _leaderboardViewer.ShowLeaderboard(_resultPlayers);
            
            */
        }    

        private void ClearEntries()
        {
            _resultPlayers.Clear();
        }
    }
}



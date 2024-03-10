using CubeProject.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualLeaderboard : MonoBehaviour
{
    [SerializeField] private AuthorizePlayer _authorizePlayer;

    public readonly List<ResultPlayer> ResultPlayers = new List<ResultPlayer>();   

    public void RecordResultPlayer(int score)
    {
        ResultPlayer resultPlayer = _authorizePlayer.ResultPlayer;

        for (int i = 0; i < ResultPlayers.Count; i++)
        {
            if (ResultPlayers[i].Name == resultPlayer.Name)
            {
                ResultPlayers.RemoveAt(i);
                ResultPlayers.Add(new ResultPlayer(resultPlayer.Name, resultPlayer.Rank, score));               
            }           
        } 

        if (ResultPlayers.Count == 0)
        {
            Debug.Log("Debug");
            ResultPlayers.Add(new ResultPlayer(resultPlayer.Name, resultPlayer.Rank, score));
        }
    }  
}

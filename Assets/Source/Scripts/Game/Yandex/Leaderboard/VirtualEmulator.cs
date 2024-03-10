using CubeProject.UI;
using Reflex.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualEmulator : MonoBehaviour
{
    [SerializeField] private VirtualLeaderboard _virtualLeaderboard;    

    public List<ResultPlayer> GetPlayerEntry()
    {
        if (_virtualLeaderboard == null)
            throw new ArgumentNullException(nameof(_virtualLeaderboard));

        Debug.Log("Emulator");
        return _virtualLeaderboard.ResultPlayers;
    }
}

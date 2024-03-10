using CubeProject.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;

public class AuthorizePlayer : MonoBehaviour
{
    public ResultPlayer ResultPlayer { get; private set; }

    public bool DoRequestOnPersonalData { get; private set; } = false;
    private string _requestPersonalData = "RequestPersonalData";

    private void Start()
    {
        if (PlayerPrefs.HasKey(_requestPersonalData))
        {
            DoRequestOnPersonalData = PlayerPrefs.GetInt(_requestPersonalData) != 0;
        }
        else
        {
            RequestData();
        }
    }

    public void RequestData()
    {
        if (DoRequestOnPersonalData == false)
        {
            PlayerAccount.Authorize();
            DoRequestOnPersonalData = true;
            PlayerPrefs.SetInt(_requestPersonalData, DoRequestOnPersonalData ? 1 : 0);

            if (PlayerAccount.IsAuthorized == true)
            {
                PlayerAccount.RequestPersonalProfileDataPermission();
            }
        }

        if (DoRequestOnPersonalData == false)
        {           
            ResultPlayer = new ResultPlayer("Anonim", 0, 0);
            DoRequestOnPersonalData = true;
            //PlayerPrefs.SetInt(_requestPersonalData, DoRequestOnPersonalData ? 1 : 0);
            Debug.Log("Register");
        }
    }
}

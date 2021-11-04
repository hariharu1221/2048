using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackEndFederaionAuth : MonoBehaviour
{
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail()                 //�̸��� ��û
            .RequestIdToken()               //��ū ��û
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = false;      //���۷α��� ������ �α׸� ���� ���� true��

        //GPGS
        PlayGamesPlatform.Activate();
        GoogleAuth();
    }

    private void GoogleAuth()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate(success =>
            {
                if(success == false)
                {
                    Debug.Log("�α��� ����");
                    return;
                }

                //success
                Debug.Log("GetIdToken: " + PlayGamesPlatform.Instance.GetIdToken());
                Debug.Log("Email: " + ((PlayGamesLocalUser)Social.localUser).Email);
                Debug.Log("GoogleId: " + Social.localUser.id);
                Debug.Log("UserName: " + Social.localUser.userName);
                Debug.Log("UserName: " + PlayGamesPlatform.Instance.GetUserDisplayName());
            });
        }

    }

    private string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("������ �Ǿ����� ����");
            GoogleAuth();
            return null;
        }
    }
}

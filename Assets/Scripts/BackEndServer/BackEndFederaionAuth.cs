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
            .RequestEmail()                 //이메일 요청
            .RequestIdToken()               //토큰 요청
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = false;      //구글로그인 문제시 로그를 보기 위해 true로

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
                    Debug.Log("로그인 실패");
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
            Debug.Log("접속이 되어있지 않음");
            GoogleAuth();
            return null;
        }
    }
}

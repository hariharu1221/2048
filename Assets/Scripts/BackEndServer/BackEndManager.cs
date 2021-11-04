using BackEnd;
using UnityEngine;

public class BackEndManager : MonoBehaviour
{
    public static BackEndManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InitBackEnd();
    }

    private void InitBackEnd()
    {
        Backend.Initialize(BRO =>
        {
            Debug.Log("뒤끝 초기화 진행 " + BRO);

            //Success
            if (BRO.IsSuccess())
            {
                Debug.Log(Backend.Utils.GetServerTime());
            }
            //Fail
            else
            {
                Debug.LogError("Failed to initialize the backend");
            }
        });
    }

    public string ShowErrorUI(BackendReturnObject backendReturn)
    {

        int statusCode = int.Parse(backendReturn.GetStatusCode());

        switch(statusCode)
        {
            case 401:
                Debug.Log("ID 또는 비밀번호가 틀렸습니다.");
                break;
            case 403:
                Debug.Log(backendReturn.GetErrorCode());
                break;
            case 404:
                Debug.Log("game not found, game을 찾을 수 없습니다");
                break;
            case 408:
                //타임 아웃 (응답이 늦음)
                Debug.Log(backendReturn.GetMessage());
                Debug.Log("Time Out!");
                break;
            case 409:
                Debug.Log("중복된 ID");
                break;
            case 410:
                Debug.Log("bad refreshToken, 잘못된 토큰 입니다");
                break;
            case 429:
                //데이터베이스 할당량 초과
                //데이터베이스 할당량 업데이트 중
                Debug.Log(backendReturn.GetMessage());
                break;
            case 503:
                //서버가 정상적으로 작동하지 않을 경우
                Debug.Log(backendReturn.GetMessage());
                break;
            case 504:
                //타임아웃
                Debug.Log(backendReturn.GetMessage());
                break;
        }

        return backendReturn.GetMessage();
    }
}

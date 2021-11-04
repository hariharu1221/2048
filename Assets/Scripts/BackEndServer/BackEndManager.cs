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
            Debug.Log("�ڳ� �ʱ�ȭ ���� " + BRO);

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
                Debug.Log("ID �Ǵ� ��й�ȣ�� Ʋ�Ƚ��ϴ�.");
                break;
            case 403:
                Debug.Log(backendReturn.GetErrorCode());
                break;
            case 404:
                Debug.Log("game not found, game�� ã�� �� �����ϴ�");
                break;
            case 408:
                //Ÿ�� �ƿ� (������ ����)
                Debug.Log(backendReturn.GetMessage());
                Debug.Log("Time Out!");
                break;
            case 409:
                Debug.Log("�ߺ��� ID");
                break;
            case 410:
                Debug.Log("bad refreshToken, �߸��� ��ū �Դϴ�");
                break;
            case 429:
                //�����ͺ��̽� �Ҵ緮 �ʰ�
                //�����ͺ��̽� �Ҵ緮 ������Ʈ ��
                Debug.Log(backendReturn.GetMessage());
                break;
            case 503:
                //������ ���������� �۵����� ���� ���
                Debug.Log(backendReturn.GetMessage());
                break;
            case 504:
                //Ÿ�Ӿƿ�
                Debug.Log(backendReturn.GetMessage());
                break;
        }

        return backendReturn.GetMessage();
    }
}

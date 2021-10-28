using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class BackEndAuthentication : MonoBehaviour
{
    public InputField idInput;
    public InputField paInput;

    BackendReturnObject bro = new BackendReturnObject();
    bool isSuccess = false;

    void Update()
    {
        if (isSuccess)
        {
            //SaveToken(BackendReturnObject bro) -> void
            //�񵿱� �޼ҵ�� ������Ʈ���� SaveToken�� �ݵ�� �����ؾ� �Ѵ�.
            //Backend.BMember.SaveToken(bro);
            isSuccess = false;
            bro.Clear();
        }
    }

    public void OnClickSignUp()
    {
        Backend.BMember.CustomSignUp(idInput.text, paInput.text, "�� �α���", (BRO) =>
        {
            bro = BRO;
            isSuccess = BRO.IsSuccess();

            if (isSuccess)
            {
                Debug.Log("Ŀ���� ȸ������ �Ϸ�");
            }
            else
            {
                BackEndManager.instance.ShowErrorUI(BRO);
            }
        });
    }

    public void OnClickLogin()
    {
        Backend.BMember.CustomLogin(idInput.text, paInput.text, (BRO) =>
        {
            bro = BRO;
            isSuccess = BRO.IsSuccess();

            if (isSuccess)
            {
                Debug.Log("Ŀ���� �α��� �Ϸ�");
            }
            else
            {
                BackEndManager.instance.ShowErrorUI(bro);
            }
        });
    }

    public void OnClickCustom()
    {

    }

    /* ���� ���
    public void OnClickSignUp1()
    {
        BackendReturnObject BRO = Backend.BMember.CustomSignUp(idInput.text, paInput.text, "�� �α���");

        if(BRO.IsSuccess())
        {
            Debug.Log("�� ȸ�� ���� �Ϸ�");
        }
        else
        {
            BackEndManager.instance.ShowErrorUI(BRO);
        }
    }

    public void OnClickLogin1()
    {
        BackendReturnObject BRO = Backend.BMember.CustomLogin(idInput.text, paInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("�� �α��� �Ϸ�");
        }
        else
        {
            BackEndManager.instance.ShowErrorUI(BRO);
        }
    }
    */
}

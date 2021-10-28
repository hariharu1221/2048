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
            //비동기 메소드는 업데이트에서 SaveToken을 반드시 적용해야 한다.
            //Backend.BMember.SaveToken(bro);
            isSuccess = false;
            bro.Clear();
        }
    }

    public void OnClickSignUp()
    {
        Backend.BMember.CustomSignUp(idInput.text, paInput.text, "앱 로그인", (BRO) =>
        {
            bro = BRO;
            isSuccess = BRO.IsSuccess();

            if (isSuccess)
            {
                Debug.Log("커스텀 회원가입 완료");
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
                Debug.Log("커스텀 로그인 완료");
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

    /* 동기 방식
    public void OnClickSignUp1()
    {
        BackendReturnObject BRO = Backend.BMember.CustomSignUp(idInput.text, paInput.text, "앱 로그인");

        if(BRO.IsSuccess())
        {
            Debug.Log("앱 회원 가입 완료");
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
            Debug.Log("앱 로그인 완료");
        }
        else
        {
            BackEndManager.instance.ShowErrorUI(BRO);
        }
    }
    */
}

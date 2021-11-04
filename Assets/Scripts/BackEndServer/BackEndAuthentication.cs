using BackEnd;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class BackEndAuthentication : MonoBehaviour
{
    public InputField id;
    public InputField password;

    public GameObject customLoginInput;
    public GameObject loginButton;
    Text Help;
    Material blur;

    BackendReturnObject bro = new BackendReturnObject();
    bool isSuccess = false;
    bool GotoMain = false;

    void Awake()
    {
        customLoginInput = GameObject.Find("CustomLoginInput");
        SetButton(customLoginInput.transform.GetChild(1).gameObject, OnClickLogin);
        SetButton(customLoginInput.transform.GetChild(2).gameObject, OnClickSignUp);
        blur = GameObject.Find("Blur").GetComponent<Image>().material;
        blur.SetInt("_Radius", 1);

        loginButton = GameObject.Find("LoginButton");
        SetButton(loginButton.transform.GetChild(0).gameObject, OnClickCustom);
        SetButton(loginButton.transform.GetChild(1).gameObject, OnClickGoogleLogin);
        SetButton(loginButton.transform.GetChild(2).gameObject, OnClickFacebookLogin);
        SetButton(loginButton.transform.GetChild(3).gameObject, OnClickGuestLogin);
        SetButton(GameObject.Find("Back"), OnClickBack);
        Help = GameObject.Find("Help").GetComponent<Text>();
        Help.gameObject.SetActive(false);

        id.characterLimit = 15;
        password.characterLimit = 16;
        id.onValueChanged.AddListener((s) => checkUnicode(s, id));
        password.onValueChanged.AddListener((s) => checkUnicode(s, password));

        customLoginInput.SetActive(false);

    }

    private void Start()
    {
        //AutoLogin();
    }


    void SetButton(GameObject ob, UnityEngine.Events.UnityAction act)
    {
        Button BT = ob.GetComponent<Button>();
        BT.onClick.AddListener(act);
    }

    void ShowErrorUI(string status)
    {
        Help.text = status;
        Help.gameObject.SetActive(true);
    }

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

        if (GotoMain)
        {
            GotoMain = false;
            LoadingSceneController.LoadScene("MainMenu");
        }
    }

    void OnClickGoogleLogin()
    {

    }

    void OnClickFacebookLogin()
    {

    }

    void OnClickGuestLogin()
    {

    }

    void OnClickSignUp()
    {
        if (id.text.Length < 5 || id.text.Length >= 12) { ShowErrorUI("아이디는 5~12글자이여야합니다."); return; }
        if (password.text.Length < 8 || password.text.Length >= 16) { ShowErrorUI("비밀번호는 8~16글자이여야합니다."); return; }

        Backend.BMember.CustomSignUp(id.text, password.text, (BRO) =>
        {
            bro = BRO;
            isSuccess = BRO.IsSuccess();

            if (isSuccess)
            {
                Debug.Log("커스텀 회원가입 완료");
                AutoLogin();
            }
            else
            {
                BackEndManager.instance.ShowErrorUI(BRO);
            }
        });
    }

    void OnClickSignOut()
    {
        BackendReturnObject BRO = Backend.BMember.SignOut();

        if (BRO.IsSuccess())
        {
            Debug.Log("7일의 유예기간이 존재합니다. 이 아이디로 7일 이내에 다시 로그인을 시도할 경우 탈퇴가 철회됩니다.");
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
        }
    }

    void OnClickLogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();


        if (BRO.IsSuccess())
        {
            Debug.Log("로그아웃되어 토큰이 삭제됩니다.");
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
        }
    }

    void OnClickLogin()
    {
        Backend.BMember.CustomLogin(id.text, password.text, (BRO) =>
        {
            bro = BRO;
            isSuccess = BRO.IsSuccess();

            if (isSuccess)
            {
                Debug.Log("커스텀 로그인 완료");
                GotoMain = true;
            }
            else
            {
                ShowErrorUI(BackEndManager.instance.ShowErrorUI(bro));
            }
        });
    }

    Coroutine Setblur;
    void OnClickCustom()
    {
        Setblur = StartCoroutine(SetBlur(7, 0.3f));
    }

    void OnClickBack()
    {
        if (customLoginInput.activeSelf) StartCoroutine(SetBlur(-7, 0.3f, false));
    }

    IEnumerator SetBlur(float v, float maxcool, bool alpha = true)
    {
        if (alpha) customLoginInput.SetActive(true);
        int a = blur.GetInt("_Radius");
        float cool = 0;

        float value = v * Screen.height / 1080;

        while (cool < maxcool)
        {
            if (alpha) customLoginInput.GetComponent<CanvasGroup>().alpha = cool / maxcool;
            else customLoginInput.GetComponent<CanvasGroup>().alpha = 1 - cool / maxcool;

            blur.SetFloat("_Radius", (float)(a + (value * cool / maxcool)));
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        blur.SetFloat("_Radius", (a + value));

        if (alpha) customLoginInput.GetComponent<CanvasGroup>().alpha = 1;
        else
        {
            customLoginInput.GetComponent<CanvasGroup>().alpha = 0;
            customLoginInput.SetActive(false);
        }

        yield return new WaitForFixedUpdate();
        StopCoroutine(Setblur);
    }

    void checkUnicode(string argStr, InputField input)
    {
        string retStr = "";
        retStr = Regex.Replace(argStr, @"[^0-9a-zA-Z.-_]", "");
        input.text = retStr;
    }

    void AutoLogin()
    {
        Backend.BMember.LoginWithTheBackendToken((callback) =>
        {
            bro = callback;
            isSuccess = callback.IsSuccess();

            if (isSuccess)
            {
                Debug.Log("자동 로그인 완료");
                GotoMain = true;
            }
            else
            {
                BackEndManager.instance.ShowErrorUI(bro);
            }
        });
    }

    public void OnClickRefreshToken()
    {
        Backend.BMember.RefreshTheBackendToken();
    }

    public bool OnClickIsTokenAlive()
    {
        return Backend.BMember.IsAccessTokenAlive().GetMessage() == "Success" ? true : false;
    }
}

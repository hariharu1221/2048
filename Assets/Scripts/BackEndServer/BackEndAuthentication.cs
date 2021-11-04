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
            //�񵿱� �޼ҵ�� ������Ʈ���� SaveToken�� �ݵ�� �����ؾ� �Ѵ�.
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
        if (id.text.Length < 5 || id.text.Length >= 12) { ShowErrorUI("���̵�� 5~12�����̿����մϴ�."); return; }
        if (password.text.Length < 8 || password.text.Length >= 16) { ShowErrorUI("��й�ȣ�� 8~16�����̿����մϴ�."); return; }

        Backend.BMember.CustomSignUp(id.text, password.text, (BRO) =>
        {
            bro = BRO;
            isSuccess = BRO.IsSuccess();

            if (isSuccess)
            {
                Debug.Log("Ŀ���� ȸ������ �Ϸ�");
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
            Debug.Log("7���� �����Ⱓ�� �����մϴ�. �� ���̵�� 7�� �̳��� �ٽ� �α����� �õ��� ��� Ż�� öȸ�˴ϴ�.");
        }
        else
        {
            Debug.Log("���� ���� ���� �߻�: " + BRO.GetMessage());
        }
    }

    void OnClickLogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();


        if (BRO.IsSuccess())
        {
            Debug.Log("�α׾ƿ��Ǿ� ��ū�� �����˴ϴ�.");
        }
        else
        {
            Debug.Log("���� ���� ���� �߻�: " + BRO.GetMessage());
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
                Debug.Log("Ŀ���� �α��� �Ϸ�");
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
                Debug.Log("�ڵ� �α��� �Ϸ�");
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

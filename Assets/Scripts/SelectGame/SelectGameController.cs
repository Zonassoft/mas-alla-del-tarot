using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class Module
{
    public int id; 
    public string name; 
}

[Serializable]
public class UsersModules
{
    public Module[] moduleList;
}

public class SelectGameController : MonoBehaviour
{
    public GameObject panelTurnBlack;
    private bool panelTurnBlackActive;
    
    public GameObject panelTurnGray;
    private bool panelTurnGrayActive;
    
    public GameObject panelLoadingMobile;
    public GameObject panelLoadingDesktop;
    
    public GameObject[] buttonsList = new GameObject[5];
    private UsersModules objectCardInfo = new UsersModules();
    public GameObject componentButtons;
    
    private void Start()
    {
        if (SoundUi.Instance.TokenAPI == "")
        {
            if (SoundUi.Instance.isMobile())
                panelLoadingMobile.SetActive(true);
            else
                panelLoadingDesktop.SetActive(true);

            StartCoroutine(GetToken());    
        }
    }
    
    public IEnumerator GetToken()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", SoundUi.Instance.username);
        form.AddField("password", SoundUi.Instance.password);
        
        UnityWebRequest req = UnityWebRequest.Post(SoundUi.Instance.urlToken, form);
        yield return req.SendWebRequest();
        
        if (req.result == UnityWebRequest.Result.ProtocolError || req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            string jsonString = req.downloadHandler.text;
            Token dataKey = JsonUtility.FromJson<Token>(jsonString);
            SoundUi.Instance.TokenAPI = dataKey.key;
            
            //SoundUi.Instance.TokenAPI = dataKey.auth_token;
            //StartCoroutine(GetModules());
            
            panelLoadingDesktop.SetActive(false);
            panelLoadingMobile.SetActive(false);
        }
    }
    
    public IEnumerator GetModules()
    {
        UnityWebRequest req = UnityWebRequest.Get(SoundUi.Instance.urlModules);
        req.SetRequestHeader("Authorization", "Token " + SoundUi.Instance.TokenAPI);
        
        yield return req.SendWebRequest();
        
        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);    
            objectCardInfo = JsonUtility.FromJson<UsersModules>("{\"moduleList\":" + req.downloadHandler.text + "}");

            int countButtons = objectCardInfo.moduleList.Length;
            for (int i = 0; i < objectCardInfo.moduleList.Length; i++)
            {
                buttonsList[objectCardInfo.moduleList[i].id - 1].SetActive(true);

                if (objectCardInfo.moduleList[i].id == 2)
                {
                    buttonsList[4].SetActive(true);
                    countButtons++;
                } 
            }

            if (countButtons == 3)
                componentButtons.GetComponent<VerticalLayoutGroup>().spacing = -150;

            if (countButtons <= 2)
            {
                componentButtons.GetComponent<VerticalLayoutGroup>().spacing = -200;
                componentButtons.GetComponent<VerticalLayoutGroup>().padding.top = 40;
            }
            
            // Para horoscopo como un modulo aparte
//            for (int i = 0; i < objectCardInfo.moduleList.Length; i++)
//                buttonsList[objectCardInfo.moduleList[i].id - 1].SetActive(true);
//
//            if (objectCardInfo.moduleList.Length == 3)
//                componentButtons.GetComponent<VerticalLayoutGroup>().spacing = -150;
//
//            if (objectCardInfo.moduleList.Length == 2)
//            {
//                componentButtons.GetComponent<VerticalLayoutGroup>().spacing = -200;
//                componentButtons.GetComponent<VerticalLayoutGroup>().padding.top = 40;
//            }
            
            panelLoadingDesktop.SetActive(false);
            panelLoadingMobile.SetActive(false);
        }
    }
    
    public void StartScene(string nameScene)
    {
        SceneManager.LoadScene	(nameScene); 
    }
    
    private void Update()
    {
        if (SoundUi.Instance.varIsMobile)
        {
            if (Screen.fullScreen && !SoundUi.Instance.isPortraitScreen())
            {
                panelTurnBlack.SetActive(true);
            }
            else if (!Screen.fullScreen && !SoundUi.Instance.isPortraitScreen())
            {
                panelTurnGray.SetActive(true);
            }
            else if (SoundUi.Instance.isPortraitScreen())
            {
                panelTurnBlack.SetActive(false);
                panelTurnGray.SetActive(false);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[Serializable]
public class TokenAPIHoroscopo
{
    public string key;
}

[Serializable]
public class HoroscopoInfo
{
    public int number;
    public string zodiac_sign;
    public string description_number;
    public string description_zodiac;
}

public class HoroscopoControllerDesktop : MonoBehaviour
{
    public GameObject Menu;
    public GameObject panelOptions;
    public GameObject panelLoading;
    public GameObject panelButtons;
    public GameObject panelReading;
    public GameObject gameObjectImageSign;
    
    private bool panelOptionActive;

    public Text nameSignSelected;
    public Text dateSignSelected;
    public Text descriptionSignSelected;
    
    private string[] nameSign = new[] {"Acuario", "Piscis", "Aries", "Tauro", "Géminis", "Cáncer", "Leo", "Virgo", "Libra", "Escorpio", "Sagitario", "Capricornio"};
    private string[] dateSign = new[] {"(20 enero-18 febrero)", "(19 febrero-20 marzo)", "(21 marzo-19 abril)", "(20 abril-20 mayo)", "(21 mayo-20 junio)", 
                                       "(21 junio-22 julio)", "(23 julio-22 agosto)", "(23 agosto-22 septiembre)", "(23 septiembre-22 octubre)", "(23 octubre-21 noviembre)", 
                                       "(22 noviembre-21 diciembre)", "(22 diciembre-19 enero)"};
   
    AudioSource[] audioSources;
    private AudioSource revelacion, menu;
    
    [DllImport("__Internal")]
    private static extern void FullScreenFunction();
    
    public Button buttonFullScreen;
    public Button buttonMinimize;
    
    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        revelacion = audioSources[0];
        menu = audioSources[1];
            
        buttonFullScreen.onClick.AddListener(TaskOnClickMax);
        buttonMinimize.onClick.AddListener(TaskOnClickMin);
    }
    
    private void Update()
    {
        if (Screen.fullScreen)
        {
            buttonMinimize.gameObject.SetActive(true);
            buttonFullScreen.gameObject.SetActive(false);
        }
        else
        {
            buttonMinimize.gameObject.SetActive(false);
            buttonFullScreen.gameObject.SetActive(true);
        }
    }
    
    void TaskOnClickMax()
    {
        StartCoroutine(WaitMax());
    }

    public IEnumerator WaitMax()
    {
        yield return new WaitForSeconds(0.5f);
        
         #if !UNITY_EDITOR && UNITY_WEBGL
            FullScreenFunction();
         #endif
    }

    void TaskOnClickMin()
    {
        StartCoroutine(WaitMin());
    }
    
    public IEnumerator WaitMin()
    {
        yield return new WaitForSeconds(0.5f);
        Screen.fullScreen = !Screen.fullScreen;
    }
    
    public void ViewZodiac(int posSign)
    {
        panelLoading.SetActive(true);
        
        StartCoroutine(GetToken("Http://82.223.139.65/api/v1/auth/login/", "admin", "destino", posSign));
    }
    
    public IEnumerator GetToken(string url, string username, string password, int posSign)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        
        UnityWebRequest req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();
        
        if (req.isNetworkError || req.isHttpError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            string jsonString = req.downloadHandler.text;
            TokenAPIHoroscopo dataKey = JsonUtility.FromJson<TokenAPIHoroscopo>(jsonString);

            string date = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            
            if (posSign == 0)
                date = "25/1/" + DateTime.Now.Year;
            if (posSign == 1)
                date = "25/2/" + DateTime.Now.Year;
            if (posSign == 2)
                date = "25/3/" + DateTime.Now.Year;
            if (posSign == 3)
                date = "25/4/" + DateTime.Now.Year;
            if (posSign == 4)
                date = "25/5/" + DateTime.Now.Year;
            if (posSign == 5)
                date = "25/6/" + DateTime.Now.Year;
            if (posSign == 6)
                date = "25/7/" + DateTime.Now.Year;
            if (posSign == 7)
                date = "25/8/" + DateTime.Now.Year;
            if (posSign == 8)
                date = "25/9/" + DateTime.Now.Year;
            if (posSign == 9)
                date = "25/10/" + DateTime.Now.Year;
            if (posSign == 10)
                date = "25/11/" + DateTime.Now.Year;
            if (posSign == 11)
                date = "25/12/" + DateTime.Now.Year;
            
            StartCoroutine(GetHoroscopoDescription("Http://82.223.139.65/api/v1/client/date/", date, dataKey.key, posSign));
        }
    }
    
    public IEnumerator GetHoroscopoDescription(string url, string date, string token, int posSign)
    {
        WWWForm form = new WWWForm();
        form.AddField("date_born", date);
        
        UnityWebRequest req = UnityWebRequest.Post(url, form);
        req.SetRequestHeader("Authorization", "Token " + token);
        
        yield return req.SendWebRequest();
        
        if (req.isNetworkError || req.isHttpError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            string jsonString = req.downloadHandler.text;

            if (jsonString.Contains("{"))
            {
                jsonString = jsonString.Remove(0, 10);
                jsonString = Regex.Replace(jsonString, @"(.*)}$", "$1");

                HoroscopoInfo data = JsonUtility.FromJson<HoroscopoInfo>(jsonString);

                string descriptionZodiacOk = data.description_zodiac;

                if (descriptionZodiacOk.Contains("\u2028"))
                    descriptionZodiacOk = descriptionZodiacOk.Replace("\u2028", " ");
                
                descriptionSignSelected.text = descriptionZodiacOk;
            }
            else
            {
                descriptionSignSelected.text = "Lo sentimos, en estos momentos no presentamos una descripción para su signo";
            }
            
            nameSignSelected.text = nameSign[posSign];
            dateSignSelected.text = dateSign[posSign];
            Debug.Log(dateSign[posSign]);
            
            for (int i = 0; i < 12; i++)
            {
                GameObject childImg = gameObjectImageSign.transform.GetChild(i).gameObject;

                if (i == posSign)
                    childImg.SetActive(true);
                else
                    childImg.SetActive(false);
            }

            panelButtons.SetActive(false);
            panelReading.SetActive(true);
            panelLoading.SetActive(false);
            revelacion.Play();
        }
    }
    
    public void ButtonOptions()
    {
        panelOptionActive = true;
        panelOptions.SetActive(true);
        Menu.GetComponent<Animation>().Play("MenuInDesktop");
        menu.Play();
    }
    
    public void ButtonQuitOptions()
    {
        panelOptionActive = false;
        menu.Play();
        Menu.GetComponent<Animation>().Play("MenuOutDesktop");
        panelOptions.SetActive(false);
    }

    public IEnumerator AnimationMenuStartScene(string nameScene)
    {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene	(nameScene); 
    }
    
    public void StartScene(string nameScene)
    {
        if (panelOptionActive && nameScene != "SelectGame")
        {
            panelOptionActive = false;
            menu.Play();
            Menu.GetComponent<Animation>().Play("MenuOutDesktop");
            StartCoroutine(AnimationMenuStartScene(nameScene));
        }
        else if (panelOptionActive && nameScene == "SelectGame")
        {
            SceneManager.LoadScene(nameScene);
        }
        else if (!panelOptionActive)
        {
            SceneManager.LoadScene(nameScene);
        }
    }
}

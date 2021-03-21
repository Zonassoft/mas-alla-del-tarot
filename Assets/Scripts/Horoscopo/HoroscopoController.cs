using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class HoroscopoInfoM
{
    public int number;
    public string zodiac_sign;
    public string description_number;
    public string description_zodiac;
}

public class HoroscopoController : MonoBehaviour
{
    public GameObject Menu;
    public GameObject panelOptions;
    public GameObject panelLoading;
    public GameObject panelButtons;
    public GameObject panelReading;
    public GameObject gameObjectImageSign;
    
    public TextMeshProUGUI nameSignSelected;
    public TextMeshProUGUI dateSignSelected;
    public TextMeshProUGUI descriptionSignSelected;
    
    private string[] nameSign = {"Acuario", "Piscis", "Aries", "Tauro", "Géminis", "Cáncer", "Leo", "Virgo", "Libra", "Escorpio", "Sagitario", "Capricornio"};
    private string[] dateSign = {"(20 enero-18 febrero)", "(19 febrero-20 marzo)", "(21 marzo-19 abril)", "(20 abril-20 mayo)", "(21 mayo-20 junio)", 
        "(21 junio-22 julio)", "(23 julio-22 agosto)", "(23 agosto-22 septiembre)", "(23 septiembre-22 octubre)", "(23 octubre-21 noviembre)", 
        "(22 noviembre-21 diciembre)", "(22 diciembre-19 enero)"};
    
    public Button buttonFullScreen;
    public Button buttonMinimize;
    
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

    public void ViewZodiac(int posSign)
    {
        panelLoading.SetActive(true);
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
            
        StartCoroutine(GetHoroscopoDescription("http://82.223.139.65/api/v1/client/date/", date, posSign));
    }
    
    public IEnumerator GetHoroscopoDescription(string url, string date, int posSign)
    {
        WWWForm form = new WWWForm();
        form.AddField("date_born", date);
        
        UnityWebRequest req = UnityWebRequest.Post(url, form);
        req.SetRequestHeader("Authorization", "Token " + SoundUi.Instance.TokenAPI);
        
        yield return req.SendWebRequest();
        
        if (req.result == UnityWebRequest.Result.ProtocolError || req.result == UnityWebRequest.Result.ConnectionError)
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

                HoroscopoInfoM data = JsonUtility.FromJson<HoroscopoInfoM>(jsonString);
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
            
            GameObject childImg = gameObjectImageSign.transform.GetChild(posSign).gameObject;
            childImg.SetActive(true);
            
            panelButtons.SetActive(false);
            panelReading.SetActive(true);
            panelLoading.SetActive(false);
            SoundUi.Instance.PlaySound(3);
        }
    }
    
    public void ButtonOptions()
    {
        SoundUi.Instance.Options(panelOptions, Menu, "MenuIn");
    }
    
    public void ButtonQuitOptions()
    {
        SoundUi.Instance.QuitOptions(panelOptions, Menu, "MenuOut");
    }

    public void StartScene(string nameScene)
    {
        SoundUi.Instance.StartScene(nameScene, Menu, "MenuOut");
    }

    public void Restart(string nameScene)
    {
        SceneManager.LoadScene	(nameScene);
    }
}

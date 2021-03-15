using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using System.Runtime.InteropServices;

[Serializable]
public class DateInfo
{
    public int number;
    public string zodiac_sign;
    public string description_number;
    public string description_zodiac;
}

public class GameDatesController : MonoBehaviour
{
    public GameObject panelOptions;
    public GameObject panelShowing;
    public GameObject panelLoading;
    public GameObject panelDescription;
    public GameObject panelHoroscopo;
    public GameObject Menu;
    public GameObject buttonRestart;
    public GameObject buttonHoroscopo;
    public GameObject componentes;
    public GameObject DropDownMonths;
    public GameObject particleNumbers;
    
    public Text description;
    public Text numberDescription;
    public Text zodiacSingDescription;
    public Text signHoroscopo;
    public Text descriptionHoroscopo;
    public Text numberCenter;
    public Text day;
    public Text year;
    public Text titleRevelacion;
    
    private bool panelOptionActive;
    private bool rightDay;
    private bool rightYear;
    private bool rightMonth;
    private bool validation = true;
    private bool stopNumberLegth;
    
//    [DllImport("__Internal")]
//    private static extern void FullScreenFunction();
    
    public Button buttonFullScreen;
    public Button buttonMinimize;
    public Button buttonShowing;
    
    private string dayAPI;
    private string yearAPI;
    
    public string[] listNumbers = 
    {
        "01", "02", "03", "04", "05", "06", "07", "08", "09", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", 
        "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"
    };

    private List<string> numberWrited = new List<string>();
    public ParticleSystem[] particles = new ParticleSystem[10];
    public Material[] materialParticles = new Material[10];
    public Material[] materialLightParticles = new Material[10];
    
    public Sprite buttonEnnable;
    public Sprite buttonDisable;
    
    private void Start()
    {
        particleNumbers.gameObject.SetActive(true);
//        buttonFullScreen.onClick.AddListener(TaskOnClickMax);
//        buttonMinimize.onClick.AddListener(TaskOnClickMin);
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
        
        if (validation)
        {
            if (day.text.Length == 1 || day.text.Length == 2)
            {
                if (((IList) listNumbers).Contains(day.text))
                    rightDay = true;
                else
                    rightDay = false;
            }
            else
            {
                rightDay = false;
            }

            if (year.text.Length == 4)
            {
                int firstYear = DateTime.Now.Year - 100;
                
                if (Convert.ToInt32(year.text) <= DateTime.Now.Year && Convert.ToInt32(year.text) >= firstYear)
                    rightYear = true;
                else
                    rightDay = false;
            }
            else
            {
                rightDay = false;
            }

            if (DropDownMonths.transform.GetComponent<Dropdown>().value != 0)
                rightMonth = true;
            else
                rightMonth = false;

            if (rightDay && rightYear && rightMonth)
            {
                buttonShowing.gameObject.GetComponent<Image>().sprite = buttonEnnable;
                buttonShowing.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            else if (!rightDay || !rightYear || !rightMonth)
            {
                buttonShowing.gameObject.GetComponent<Image>().sprite = buttonDisable;
                buttonShowing.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
        
        if (!stopNumberLegth)
        {
            numberWrited.Clear();
            
            for (int i = 0; i < day.text.Length; i++)
                numberWrited.Add(day.text[i].ToString());
            
            for (int i = 0; i < year.text.Length; i++)
                numberWrited.Add(year.text[i].ToString());
                    
            ActivateNumber();
        }
    }
    
//    void TaskOnClickMax()
//    {
//        StartCoroutine(WaitMax());
//    }
//
//    public IEnumerator WaitMax()
//    {
//        yield return new WaitForSeconds(0.5f);
//        SoundUi.Instance.FullScreenMethod();
//        
////        #if !UNITY_EDITOR && UNITY_WEBGL
////           FullScreenFunction();
////        #endif
//    }
//
//    void TaskOnClickMin()
//    {
//        StartCoroutine(WaitMin());
//    }
//    
//    public IEnumerator WaitMin()
//    {
//        yield return new WaitForSeconds(0.5f);
//        Screen.fullScreen = !Screen.fullScreen;
//    }
    
    public void ActivateNumber()
    {
        if (numberWrited.Contains("0"))
            AuxLightLetter(0);
        else
            AuxNonLightLetter(0);
        
        if (numberWrited.Contains("1"))
            AuxLightLetter(1);
        else
            AuxNonLightLetter(1);
        
        if (numberWrited.Contains("2"))
            AuxLightLetter(2);
        else
            AuxNonLightLetter(2);
        
        if (numberWrited.Contains("3"))
            AuxLightLetter(3);
        else
            AuxNonLightLetter(3);
        
        if (numberWrited.Contains("4"))
            AuxLightLetter(4);
        else
            AuxNonLightLetter(4);
        
        if (numberWrited.Contains("5"))
            AuxLightLetter(5);
        else
            AuxNonLightLetter(5);
        
        if (numberWrited.Contains("6"))
            AuxLightLetter(6);
        else
            AuxNonLightLetter(6);
        
        if (numberWrited.Contains("7"))
            AuxLightLetter(7);
        else
            AuxNonLightLetter(7);
        
        if (numberWrited.Contains("8"))
            AuxLightLetter(8);
        else
            AuxNonLightLetter(8);
        
        if (numberWrited.Contains("9"))
            AuxLightLetter(9);
        else
            AuxNonLightLetter(9);
    }

    public void AuxLightLetter(int pos)
    {
        particles[pos].gameObject.GetComponent<ParticleSystemRenderer>().material = materialLightParticles[pos];
    }
    
    public void AuxNonLightLetter(int pos)
    {
        particles[pos].gameObject.GetComponent<ParticleSystemRenderer>().material = materialParticles[pos];
    }

    public void ButtonShowing()
    {
        if (rightDay && rightYear && rightMonth)
        {
            if (day.text.Length != 0)
                dayAPI = day.text;
            else
                dayAPI = DateTime.Now.Day.ToString();
            
            if (year.text.Length != 0)
                yearAPI = year.text;
            else
                yearAPI = DateTime.Now.Year.ToString();

            validation = false;
            stopNumberLegth = true;
            panelLoading.SetActive(true);
            
            int monthUser = DropDownMonths.transform.GetComponent<Dropdown>().value;
            string date = dayAPI + "/" + monthUser + "/" + yearAPI;
            StartCoroutine(GetDateDescription(date));
        }
        else
        {
            StartCoroutine(Message());
        }
    }
    
    public IEnumerator Message()
    {
        GameObject msgButton = buttonShowing.gameObject.transform.GetChild(1).gameObject;
        msgButton.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        msgButton.SetActive(false);
    }
    
    public IEnumerator GetDateDescription(string date)
    {
        WWWForm form = new WWWForm();
        form.AddField("date_born", date);
        
        UnityWebRequest req = UnityWebRequest.Post(SoundUi.Instance.urlDate, form);
        req.SetRequestHeader("Authorization", "Token " + SoundUi.Instance.TokenAPI);
        
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
                jsonString = Regex.Replace(jsonString, @"(.*)}$","$1");
                
                DateInfo data = JsonUtility.FromJson<DateInfo>(jsonString);
                numberCenter.text = data.number.ToString();
                string descriptionNumberOk = data.description_number;
            
                if (descriptionNumberOk.Contains("\u2028"))
                    descriptionNumberOk = descriptionNumberOk.Replace("\u2028", " ");

                description.text = descriptionNumberOk;
                numberDescription.text = data.number.ToString();
                signHoroscopo.text = data.zodiac_sign;
                zodiacSingDescription.text = data.zodiac_sign;
                string descriptionZodiacOk = data.description_zodiac;
            
                if (descriptionZodiacOk.Contains("\u2028"))
                    descriptionZodiacOk = descriptionZodiacOk.Replace("\u2028", " ");
                
                descriptionHoroscopo.text = descriptionZodiacOk;
                componentes.SetActive(false);
                buttonRestart.SetActive(true);
                panelLoading.SetActive(false);
                panelShowing.SetActive(true);
                StartCoroutine(Description());
            }
            else
            {
                numberCenter.text = 0.ToString();
                description.text = "Lo sentimos predicción no encontrada para la fecha " + date + ". Si lo desea puede ingresar otra fecha";
                numberDescription.text = 0.ToString();
                signHoroscopo.text = "";
            
                componentes.SetActive(false);
                buttonRestart.SetActive(true);
                panelLoading.SetActive(false);
                panelShowing.SetActive(true);
                buttonHoroscopo.SetActive(false);
                StartCoroutine(Description());
            }
        }
    }

    public IEnumerator Description()
    {
        yield return new WaitForSeconds(1f);
        panelDescription.SetActive(true);
        SoundUi.Instance.PlaySound(3);
    }
    
    public void ButtonHoroscopo()
    {
        panelHoroscopo.SetActive(true);
        titleRevelacion.text = "Según la fecha de nacimiento tu signo es";
        SoundUi.Instance.PlaySound(3);
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

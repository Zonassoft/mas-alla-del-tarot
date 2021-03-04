using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;


[Serializable]
public class TokenAPIDatePC
{
    public string key;
}

[Serializable]
public class DateInfoPC
{
    public int number;
    public string zodiac_sign;
    public string description_number;
    public string description_zodiac;
}

public class GameDateDesktop : MonoBehaviour
{
    public GameObject panelShowing;
    public GameObject panelLoading;
    public GameObject panelDescription;
    public GameObject panelHoroscopo;
    public GameObject buttonRestart;
    public GameObject SignAndZodiacName;
    public GameObject componentes;
    public GameObject buttonHoroscopo;
    public GameObject DropDownMonths;
    public GameObject Menu;
    public GameObject panelOptions;
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
    
    [DllImport("__Internal")]
    private static extern void FullScreenFunction();
    
    public Button buttonFullScreen;
    public Button buttonMinimize;
    public Button buttonShowing;
    
    private string dayAPI;
    private string yearAPI;

    private List<string> listYears = new List<string>();
    private List<string> listNumbers = new List<string>();
    private List<string> numberWrited = new List<string>();
    public ParticleSystem[] particles = new ParticleSystem[10];
    public Material[] materialParticles = new Material[10];
    public Material[] materialLightParticles = new Material[10];
    
    public Sprite buttonEnnable;
    public Sprite buttonDisable;

    private void Start()
    {
        particleNumbers.gameObject.SetActive(true);
        buttonFullScreen.onClick.AddListener(TaskOnClickMax);
        buttonMinimize.onClick.AddListener(TaskOnClickMin);
        int firstYear = DateTime.Now.Year - 100;
        
        listNumbers.Add("01");
        listNumbers.Add("02");
        listNumbers.Add("03");
        listNumbers.Add("04");
        listNumbers.Add("05");
        listNumbers.Add("06");
        listNumbers.Add("07");
        listNumbers.Add("08");
        listNumbers.Add("09");

        for (int i = 0; i < 101; i++)
        {
            int x = firstYear + i;
            listYears.Add(x.ToString());

            if (i < 31)
            {
                int y = i + 1;
                listNumbers.Add(y.ToString());
            }
        }
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
                if (listNumbers.Contains(day.text))
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
                if (listYears.Contains(year.text))
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
            StartCoroutine(GetToken("Http://82.223.139.65/api/v1/auth/login/", "admin", "destino"));
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
    
    public IEnumerator GetToken(string url, string username, string password)
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
            TokenAPIDatePC dataKey = JsonUtility.FromJson<TokenAPIDatePC>(jsonString);
            
            int monthUser = DropDownMonths.transform.GetComponent<Dropdown>().value;
            string date = dayAPI + "/" + monthUser + "/" + yearAPI;
            StartCoroutine(GetDateDescription("http://82.223.139.65/api/v1/client/date/", date, dataKey.key));
        }
    }

    public IEnumerator GetDateDescription(string url, string date, string token)
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
                jsonString = Regex.Replace(jsonString, @"(.*)}$","$1");
                
                DateInfoPC data = JsonUtility.FromJson<DateInfoPC>(jsonString);
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
                zodiacSingDescription.text = " ";
            
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
        StartCoroutine(WaitPanelHoroscopo());
    }
    
    public IEnumerator WaitPanelHoroscopo()
    {
        yield return new WaitForSeconds(0.5f);
        SignAndZodiacName.SetActive(false);
        titleRevelacion.text = "Según la fecha de nacimiento tu signo es";
        panelHoroscopo.SetActive(true);
        SoundUi.Instance.PlaySound(3);
    }
    
    public void ButtonOptions()
    {
        panelOptionActive = true;
        panelOptions.SetActive(true);
        Menu.GetComponent<Animation>().Play("MenuInDesktop");
        SoundUi.Instance.PlaySound(2);
    }
    
    public void ButtonQuitOptions()
    {
        panelOptionActive = false;
        SoundUi.Instance.PlaySound(2);
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
            SoundUi.Instance.PlaySound(2);
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

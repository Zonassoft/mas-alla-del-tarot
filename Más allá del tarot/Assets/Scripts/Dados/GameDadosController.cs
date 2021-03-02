using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Text.RegularExpressions;

[Serializable]
public class TokenAPIDicesPC
{
    public string key;
}

[Serializable]
public class DicesInfoPC
{
    public string description_blue;
    public string description_red;
}

public class GameDadosController : MonoBehaviour
{
    public GameObject Menu;
    public GameObject panelOptions;
    public GameObject panelLoading;
    public GameObject panelShowing;
    public GameObject components;
    
    public GameObject dados;
    public GameObject dadosMovil;

    private bool panelOptionActive;
   
    AudioSource[] audioSources;
    private AudioSource revelacion, menu, dadosturn, tilin;
    
    [DllImport("__Internal")]
    private static extern void FullScreenFunction();
    
    public Button buttonFullScreen;
    public Button buttonMinimize;
    public Button buttonReading;
    public Button buttonTurnDices;

    public bool clicked;
    private bool d;
    public GameObject dado;
    public int num = 7;
    
    public int x;
    public int y;
    public int z;

    public Vector3 n1;
    public Vector3 n2;
    public Vector3 n3;
    public Vector3 n4;
    public Vector3 n5;
    public Vector3 n6;
    
    public GameObject dado2;
    public int numD2 = 7;

    public Text descriptionBlue;
    public Text descriptionRed;
    public Text numberBlue;
    public Text numberRed;

    public float speed;
    public Vector3 direction = Vector3.zero;
    public Vector3 direction2 = Vector3.zero;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        revelacion = audioSources[0];
        menu = audioSources[1];
        dadosturn = audioSources[2];
        tilin = audioSources[3];
            
        buttonFullScreen.onClick.AddListener(TaskOnClickMax);
        buttonMinimize.onClick.AddListener(TaskOnClickMin);
        
        dados.SetActive(true);
        dadosMovil.SetActive(false);
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

        if (num == 0)
        {
            direction = new Vector3(0, 1, 0);
            dado.transform.Rotate(direction * Time.deltaTime * speed, Space.World);
            
            direction = new Vector3(1, 0, 0);
            dado.transform.Rotate(direction * Time.deltaTime * speed, Space.World);
            
            direction = new Vector3(0, 0, 1);
            dado.transform.Rotate(direction * Time.deltaTime * speed, Space.World);
        }
        
        if (num == 1)
            dado.transform.rotation = Quaternion.Euler(n1.x, n1.y, n1.z);
        if (num == 2)
            dado.transform.rotation = Quaternion.Euler(n2.x, n2.y, n2.z);
        if (num == 3)
            dado.transform.rotation = Quaternion.Euler(n3.x, n3.y, n3.z);
        if (num == 4)
            dado.transform.rotation = Quaternion.Euler(n4.x, n4.y, n4.z);
        if (num == 5)
            dado.transform.rotation = Quaternion.Euler(n5.x, n5.y, n5.z);
        if (num == 6)
            dado.transform.rotation = Quaternion.Euler(n6.x, n6.y, n6.z);

        if (numD2 == 0)
        {
            direction2 = new Vector3(0, 0, 1);
            dado2.transform.Rotate(direction2 * Time.deltaTime * speed, Space.World);
            
            direction2 = new Vector3(1, 0, 0);
            dado2.transform.Rotate(direction2 * Time.deltaTime * speed, Space.World);
            
            direction2 = new Vector3(0, 1, 0);
            dado2.transform.Rotate(direction2 * Time.deltaTime * speed, Space.World);
        }
        
        if (numD2 == 1)
            dado2.transform.rotation = Quaternion.Euler(n1.x, n1.y, n1.z);
        if (numD2 == 2)
            dado2.transform.rotation = Quaternion.Euler(n2.x, n2.y, n2.z);
        if (numD2 == 3)
            dado2.transform.rotation = Quaternion.Euler(n3.x, n3.y, n3.z);
        if (numD2 == 4)
            dado2.transform.rotation = Quaternion.Euler(n4.x, n4.y, n4.z);
        if (numD2 == 5)
            dado2.transform.rotation = Quaternion.Euler(n5.x, n5.y, n5.z);
        if (numD2 == 6)
            dado2.transform.rotation = Quaternion.Euler(n6.x, n6.y, n6.z);
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
    
    public void ClicDado()
    {
        if (!clicked)
        {
            clicked = true;
            buttonTurnDices.interactable = false;
            dadosturn.Play();
            if (!d)
            {
                d = true;
                Invoke(nameof(IO), 2.5f);
                Dado1();
            }
        }
    }
    
    public void IO()
    {
        d = false;
    }
    
    public void Dado1()
    {
        num = 0;
        numD2 = 0;
        Invoke(nameof(Dado2), 2.5f);
        Invoke(nameof(MusicTilin), 2.5f);
    }
    
    public void MusicTilin()
    {
        tilin.Play();
    }
        
    public void Dado2()
    {
        num = Random.Range(1, 7);
        Dado2Random();
            
        buttonReading.gameObject.SetActive(true);
        buttonTurnDices.gameObject.SetActive(false);
    }
    
    public void Dado2Random()
    {
        numD2 = Random.Range(1, 7);
    }
    
    public void ButtonReading()
    {
        panelLoading.SetActive(true);
        
        StartCoroutine(GetToken("Http://82.223.139.65/api/v1/auth/login/", "admin", "destino"));
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
            TokenAPIDicesPC dataKey = JsonUtility.FromJson<TokenAPIDicesPC>(jsonString);
            
            StartCoroutine(GetDicesDescription("Http://82.223.139.65/api/v1/client/dado/", dataKey.key));
        }
    }
    
    public IEnumerator GetDicesDescription(string url, string token)
    {
        WWWForm form = new WWWForm();
        form.AddField("dado_azul", num);
        form.AddField("dado_rojo", numD2);
        
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
            jsonString = jsonString.Remove(0, 10);
            jsonString = Regex.Replace(jsonString, @"(.*)}$","$1");
            
            DicesInfoPC dataDescription = JsonUtility.FromJson<DicesInfoPC>(jsonString);
            
            string descriptionBlueOk = dataDescription.description_blue;
            string descriptionRedOk = dataDescription.description_red;
            
            if (descriptionBlueOk.Contains("\u2028"))
                descriptionBlueOk = descriptionBlueOk.Replace("\u2028", " ");
            
            if (descriptionRedOk.Contains("\u2028"))
                descriptionRedOk = descriptionRedOk.Replace("\u2028", " ");
            
            components.SetActive(false);
            dados.SetActive(false);
            panelShowing.SetActive(true);
            
            descriptionBlue.text = descriptionBlueOk;
            descriptionRed.text = descriptionRedOk;
            numberBlue.text = num.ToString();
            numberRed.text = numD2.ToString();
            
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

using System;
using System.Collections;
using System.Collections.Generic;
//using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Text.RegularExpressions;

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
    public GameObject dado;
    public GameObject dado2;
    
//    [DllImport("__Internal")]
//    private static extern void FullScreenFunction();
    
    public Button buttonFullScreen;
    public Button buttonMinimize;
    public Button buttonReading;
    public Button buttonTurnDices;

    public bool clicked;
    private bool d;
    private bool panelOptionActive;
    
    public Vector3 n1;
    public Vector3 n2;
    public Vector3 n3;
    public Vector3 n4;
    public Vector3 n5;
    public Vector3 n6;
    public Vector3 direction = Vector3.zero;
    public Vector3 direction2 = Vector3.zero;
    
    public int num = 7;
    public int numD2 = 7;
    public float speed;

    public Text descriptionBlue;
    public Text descriptionRed;
    public Text numberBlue;
    public Text numberRed;

    private void Start()
    {
//        buttonFullScreen.onClick.AddListener(TaskOnClickMax);
//        buttonMinimize.onClick.AddListener(TaskOnClickMin);
        
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
    
    public void ClicDado()
    {
        if (!clicked)
        {
            clicked = true;
            buttonTurnDices.interactable = false;
            SoundUi.Instance.PlaySound(9);
            
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
        SoundUi.Instance.PlaySound(4);
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
        StartCoroutine(GetDicesDescription());
    }
    
    public IEnumerator GetDicesDescription()
    {
        WWWForm form = new WWWForm();
        form.AddField("dado_azul", num);
        form.AddField("dado_rojo", numD2);
        
        UnityWebRequest req = UnityWebRequest.Post(SoundUi.Instance.urlDados, form);
        req.SetRequestHeader("Authorization", "Token " + SoundUi.Instance.TokenAPI);
        
        yield return req.SendWebRequest();
        
        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
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
            SoundUi.Instance.PlaySound(3);
        }
    }
    
    public void ButtonOptions()
    {
        SoundUi.Instance.Options(panelOptions, Menu, "MenuInDesktop");
    }
    
    public void ButtonQuitOptions()
    {
        SoundUi.Instance.QuitOptions(panelOptions, Menu, "MenuOutDesktop");
    }

    public void StartScene(string nameScene)
    {
        SoundUi.Instance.StartScene(nameScene, Menu, "MenuOutDesktop");
    }
    
    public void Restart(string nameScene)
    {
        SceneManager.LoadScene	(nameScene);
    }
}

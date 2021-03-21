using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

[Serializable]
public class Token
{
    public string key;
    //public string auth_token;
}

public class SoundUi : Singleton<SoundUi>
{
    public AudioClip[] SoundClips;
    public AudioSource AudioSource;
    public static SoundUi Instance;
    
    [DllImport("__Internal")]
    private static extern void FullScreenFunction();
      
    [DllImport("__Internal")]
    private static extern bool CheckOrientation();
        
    [DllImport("__Internal")]
    private static extern bool CheckOrientationIOS();
        
    [DllImport("__Internal")]
    private static extern bool IsMobile();

    public string urlDados;
    public string urlCards;
    public string urlLesturaCards;
    public string urlDate;
    public string urlName;
    public string urlToken;
    public string urlModules;
    public string username;
    public string password;
    public string TokenAPI;
    
    public bool varIsMobile;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void OnEnable()
    {
        PlaySoundButton.OnClicked += PlaySound;
    }

    void OnDisable()
    {
        PlaySoundButton.OnClicked -= PlaySound;
    }

    public void PlaySound(int index)
    {
        AudioSource.PlayOneShot(SoundClips[index]);
    }

    private void Start()
    {
        if (isMobile() || IsPad() || SystemInfo.deviceName.Contains("iPad"))
            varIsMobile = true;

        urlName = "Http://82.223.139.65/api/v1/client/name/";
        urlDate = "http://82.223.139.65/api/v1/client/date/";
        urlLesturaCards = "Http://82.223.139.65/api/v1/client/tarot/";
        urlCards = "Http://82.223.139.65/api/v1/admin/card/";
        urlDados = "Http://82.223.139.65/api/v1/client/dado/";
        urlToken = "Http://82.223.139.65/api/v1/auth/login/";
        //urlToken = "Http://82.223.139.65/api/v1/auth/token/login/";
        //urlToken = "http://127.0.0.1:8000/api/v1/auth/token/login/";
        urlModules = "http://127.0.0.1:8000/api/v1/admin/conf/availables_modules/";
        //urlModules = "http://82.223.139.65/api/v1/admin/conf/availables_modules/";
        username = "admin";
        password = "destino";
        //username = "pizarrosa";
        //password = "pao123456";
    }
    
    public bool isMobile()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobile();
        #endif
        return false;
    }
    
    public bool isPortraitScreen()
    {
        if (SystemInfo.operatingSystem.Contains("Android"))
        {
             #if !UNITY_EDITOR && UNITY_WEBGL
                 return CheckOrientation();
             #endif
        }
        else if (SystemInfo.operatingSystem.Contains("iOS"))
        {
             #if !UNITY_EDITOR && UNITY_WEBGL
                 return CheckOrientationIOS();
             #endif
        }
        return false;
    }
    
    public bool IsPad()
    {
        string type = SystemInfo.deviceModel.ToLower().Trim();

        if (type.Substring(0, 3) == "iph")
            return false;
        if (type.Substring(0, 3) == "ipa")
            return true;
        else
            return false;
    }

    public void FullScreenMethod()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            FullScreenFunction();
        #endif
    }
    
    public void Options(GameObject panelOptions, GameObject Menu, string nameAnim)
    {
        panelOptions.SetActive(true);
        Menu.GetComponent<Animation>().Play(nameAnim);
        PlaySound(2);
    }
    
    public void QuitOptions(GameObject panelOptions, GameObject Menu, string nameAnim)
    {
        PlaySound(2);
        Menu.GetComponent<Animation>().Play(nameAnim);
        StartCoroutine(AnimationMenu(panelOptions));
    }

    public IEnumerator AnimationMenu(GameObject panelOptions)
    {
        yield return new WaitForSeconds(0.7f);
        panelOptions.SetActive(false);
    }
    
    public void StartScene(string nameScene, GameObject Menu, string nameAnim)
    {
        if (nameScene != "SelectGame")
        {
            PlaySound(2);
            Menu.GetComponent<Animation>().Play(nameAnim);
            StartCoroutine(AnimationMenuStartScene(nameScene));
        }
        else
        {
            SceneManager.LoadScene(nameScene);
        }
    }
    
    public IEnumerator AnimationMenuStartScene(string nameScene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene	(nameScene); 
    }
}
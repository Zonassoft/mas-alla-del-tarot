using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
//using System.Runtime.InteropServices;

public class SelectGameController : MonoBehaviour
{
    public GameObject panelTurnBlack;
    private bool panelTurnBlackActive;
    
    public GameObject panelTurnGray;
    private bool panelTurnGrayActive;
    
    public GameObject panelLoadingMobile;
    public GameObject panelLoadingDesktop;
    
//    [DllImport("__Internal")]
//    private static extern bool IsMobile();
//    
//    [DllImport("__Internal")]
//    private static extern bool CheckOrientation();
//    
//    [DllImport("__Internal")]
//    private static extern bool CheckOrientationIOS();
// 
//    public bool isMobile()
//    {
//         #if !UNITY_EDITOR && UNITY_WEBGL
//             return IsMobile();
//         #endif
//        return false;
//    }
//    
//    public bool isPortraitScreen()
//    {
//        if (SystemInfo.operatingSystem.Contains("Android"))
//        {
//             #if !UNITY_EDITOR && UNITY_WEBGL
//                 return CheckOrientation();
//             #endif
//        }
//        else if (SystemInfo.operatingSystem.Contains("iOS"))
//        {
//             #if !UNITY_EDITOR && UNITY_WEBGL
//                 return CheckOrientationIOS();
//             #endif
//        }
//        return false;
//    }
    
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
        
        if (req.isNetworkError || req.isHttpError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            string jsonString = req.downloadHandler.text;
            Token dataKey = JsonUtility.FromJson<Token>(jsonString);
            SoundUi.Instance.TokenAPI = dataKey.key;
            
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
        //if (SoundUi.Instance.isMobile() || SoundUi.Instance.IsPad() || SystemInfo.deviceName.Contains("iPad"))
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
    
//    public static bool IsPad()
//    {
//        string type = SystemInfo.deviceModel.ToLower().Trim();
//
//        if (type.Substring(0, 3) == "iph")
//            return false;
//        if (type.Substring(0, 3) == "ipa")
//            return true;
//        else
//            return false;
//    }
}

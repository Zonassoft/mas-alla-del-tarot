using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
//using UnityEditor.PackageManager.UI;
using UnityEngine.SceneManagement;

public class ScreenStart : MonoBehaviour
{
    public Button startButton;
    public Canvas canvasGame;
    public GameObject textDestiny;
    
    public GameObject panelTurnScreen;
    private bool panelTurnScreenActive;
    
    [DllImport("__Internal")]
    private static extern void FullScreenFunction();
    
    [DllImport("__Internal")]
    private static extern bool CheckOrientation();
    
    [DllImport("__Internal")]
    private static extern bool CheckOrientationIOS();
    
    [DllImport("__Internal")]
    private static extern bool IsMobile();

    void Start()
    {
        if (canvasGame.transform.GetComponent<RectTransform>().rect.width < textDestiny.transform.GetComponent<RectTransform>().rect.width)
            canvasGame.transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.8f;
        
        startButton.onClick.AddListener(TaskOnClick);
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
    
    void TaskOnClick()
    {
        StartCoroutine(WaitStart());
    }
    
    public IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (isMobile() || SystemInfo.deviceModel.Contains("iPad") || SystemInfo.deviceName.Contains("iPad"))
        {
            if (isPortraitScreen())
            {
                #if !UNITY_EDITOR && UNITY_WEBGL
                    FullScreenFunction();
                #endif
                    
                SceneManager.LoadScene	("SelectGame");
            }
        }
        else
        {
            #if !UNITY_EDITOR && UNITY_WEBGL
                FullScreenFunction();
            #endif
            
            SceneManager.LoadScene	("SelectGame");
        }
    }

    void Update()
    {
        if (isMobile() || IsPad() || SystemInfo.deviceName.Contains("iPad"))
        {
            if (!isPortraitScreen())
                panelTurnScreen.SetActive(true);
            else
                panelTurnScreen.SetActive(false);
        }
    }

    public static bool IsPad()
    {
        string type = SystemInfo.deviceModel.ToLower().Trim();

        if (type.Substring(0, 3) == "iph")
            return false;
        if (type.Substring(0, 3) == "ipa")
            return true;
        else
            return false;
    }
}

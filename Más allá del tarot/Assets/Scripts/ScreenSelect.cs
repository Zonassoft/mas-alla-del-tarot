using UnityEngine;
using System.Runtime.InteropServices;
//using UnityEngine.iOS;

public class ScreenSelect : MonoBehaviour
{
    public GameObject ScreenMobile;
    public GameObject ScreenDesktop;
    
    public GameObject ScreenMobileScript;
    public GameObject ScreenDesktopScript;

    public bool isPortrait;
    
    public GameObject panelTurnBlack;
    private bool panelTurnBlackActive;
    
    public GameObject panelTurnGray;
    private bool panelTurnGrayActive;
    
    [DllImport("__Internal")]
    private static extern bool IsMobile();
    
    [DllImport("__Internal")]
    private static extern bool CheckOrientation();
    
    [DllImport("__Internal")]
    private static extern bool CheckOrientationIOS();
 
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
    
    void Start()
    {
        // Device.generation.ToString().IndexOf("iPad") > -1
        if (isMobile() || IsPad() || SystemInfo.deviceName.Contains("iPad"))
        {
            ScreenMobile.SetActive(true);
            ScreenDesktop.SetActive(false);
    
            ScreenDesktopScript.SetActive(false);
            ScreenMobileScript.SetActive(true);
        }
    }

    private void Update()
    {
        if (isMobile() || IsPad() || SystemInfo.deviceName.Contains("iPad"))
        {
            if (Screen.fullScreen && !isPortraitScreen())
            {
                panelTurnBlack.SetActive(true);
            }
            else if (!Screen.fullScreen && !isPortraitScreen())
            {
                panelTurnGray.SetActive(true);
            }
            else if (isPortraitScreen())
            {
                panelTurnBlack.SetActive(false);
                panelTurnGray.SetActive(false);
            }
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

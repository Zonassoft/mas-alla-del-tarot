using UnityEngine;
using UnityEngine.SceneManagement;
//using System.Runtime.InteropServices;

public class SelectGameController : MonoBehaviour
{
    public GameObject panelTurnBlack;
    private bool panelTurnBlackActive;
    
    public GameObject panelTurnGray;
    private bool panelTurnGrayActive;
    
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

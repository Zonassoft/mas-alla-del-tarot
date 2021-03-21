using UnityEngine;

public class ScreenSelect : MonoBehaviour
{
    public GameObject ScreenMobile;
    public GameObject ScreenDesktop;
    
    public GameObject ScreenMobileScript;
    public GameObject ScreenDesktopScript;

    public GameObject panelTurnBlack;
    private bool panelTurnBlackActive;
    
    public GameObject panelTurnGray;
    private bool panelTurnGrayActive;
    
    void Start()
    {
        if (SoundUi.Instance.varIsMobile)
        {
            ScreenMobile.SetActive(true);
            ScreenDesktop.SetActive(false);
    
            ScreenDesktopScript.SetActive(false);
            ScreenMobileScript.SetActive(true);
        }
    }

    private void Update()
    {
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
    
    public void ButtonMax()
    {
        SoundUi.Instance.FullScreenMethod();
    }
    
    public void ButtonMin()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}

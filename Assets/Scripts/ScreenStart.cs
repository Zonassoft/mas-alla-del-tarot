using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenStart : MonoBehaviour
{
    public Button startButton;
    public Canvas canvasGame;
    public GameObject logo;
    
    public GameObject panelTurnScreen;
    private bool panelTurnScreenActive;

    void Start()
    {
//        if (canvasGame.transform.GetComponent<RectTransform>().rect.width < logo.transform.GetComponent<RectTransform>().rect.width)
//            canvasGame.transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.8f;
        
        startButton.onClick.AddListener(TaskOnClick);
    }
    
    void TaskOnClick()
    {
        if (SoundUi.Instance.varIsMobile)
        {
            if (SoundUi.Instance.isPortraitScreen())
            {
                SoundUi.Instance.FullScreenMethod();
                SceneManager.LoadScene	("SelectGame");
            }
        }
        else
        {
            SoundUi.Instance.FullScreenMethod();
            SceneManager.LoadScene	("SelectGame");
        }
    }
    
    void Update()
    {
        if (SoundUi.Instance.varIsMobile)
        {
            if (!SoundUi.Instance.isPortraitScreen())
                panelTurnScreen.SetActive(true);
            else
                panelTurnScreen.SetActive(false);
        }
    }
}

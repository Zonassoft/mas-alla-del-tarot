using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Random = System.Random;
//using System.Runtime.InteropServices;

[Serializable]
public class DescriptionAPIPC
{
    public int id_card;
    public int id_position;
    public int isInverse;
    public string combinations;
    public string predictions;
}

[Serializable]
public class CardAPIPC
{
    public int id; 
    public string name; 
    public string image;
}

[Serializable]
public class CardsInfoPC
{
    public CardAPIPC[] cardAPIList;
}

public class TarotControllerDesktop : MonoBehaviour
{
    public GameObject panelSelectCards;
    public GameObject panelReading;
    public GameObject panelHelp;
    public GameObject cardInBox;
    public GameObject panelBoxs;
    public GameObject LoadingPanel;
    public GameObject ScrollCards;
    public GameObject Menu;
    public GameObject panelOptions;
    public GameObject imgMouse;
    
    public Canvas canvasGame;
    
    public Button buttonReading;
    public Button buttonShuffle;
    public Button buttonShowing;
    public Button buttonAccept;
    public Button buttonFullScreen;
    public Button buttonMinimize;
    public Button buttonNext;
    public Button buttonPrevious;
    
    // Vista Lectura
    public GameObject[] boxDestiny;                // Las cuatro casillas donde van las cartas
    public GameObject[] boxReading;                // El componente que tiene carta, nombre, invertida
    
    private CardsInfoPC myObject = new CardsInfoPC();
	
    // Datos de las cartas
    public int[] ids = new int[22]; 
    public string[] names = new String[22];
    private GameObject[] cards = new GameObject[22];
    private int[] pos = {10, 11, 9, 12, 8, 13, 7, 14, 6, 15, 5, 16, 4, 17, 3, 18, 2, 19, 1, 20, 0, 21, 22};
    public GameObject prefabCard;
    
    public GameObject[] objectsImage = new GameObject[4];
    public Image Image;
    
    public Sprite buttonEnnable;
    public Sprite buttonDisable;
	
    public int countselectedCard;
    public GameObject[] cardsInBoxList = new GameObject[4];   // Cartas que están volteadas en la mesa
    
    private int[] idCards = new int[4];
    private string[] isInverses = new string[4];
    private int[] positions = new int[4] {1, 2, 3, 4};
    
    public Sprite[] buttonRevSprite = new Sprite[2];
    
    private string API_KEY;

    public int revelacionScreen = 1;
    
    private bool panelOptionActive;
    private bool dragCard;
    
//    [DllImport("__Internal")]
//    private static extern void FullScreenFunction();
    
    private void Start()
    {
        if (canvasGame.transform.GetComponent<RectTransform>().rect.width < 1500)
            canvasGame.transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.7f;
        
//        buttonFullScreen.onClick.AddListener(TaskOnClickMax);
//        buttonMinimize.onClick.AddListener(TaskOnClickMin);
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
////            FullScreenFunction();
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
    
    private void Update()
    {
        if (dragCard)
        {
            int count = 0;

            for (int i = 0; i < 4; i++)
            {
                if (boxDestiny[i].gameObject.transform.childCount == 2)
                    count++;
                else
                    break;
            }

            if (count == 4)
            {
                dragCard = false;
                buttonShowing.gameObject.SetActive(true);
            }
        }
        
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
    }
    
    public void ButtonShuffle()
    {
        LoadingPanel.SetActive(true);
        panelSelectCards.SetActive(true);
        GameObject panelCards = panelSelectCards.gameObject.transform.GetChild(2).gameObject;
        
        if (canvasGame.transform.GetComponent<RectTransform>().rect.width > panelCards.transform.GetComponent<RectTransform>().rect.width)
            imgMouse.SetActive(false);
        
        buttonAccept.gameObject.GetComponent<Image>().sprite = buttonDisable;
        StartCoroutine(GetDatesCards());
    }
    
    public IEnumerator GetDatesCards()
    {
        UnityWebRequest req = UnityWebRequest.Get(SoundUi.Instance.urlCards);
        req.SetRequestHeader("Authorization", "Token " + SoundUi.Instance.TokenAPI);
        
        yield return req.SendWebRequest();
        
        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);                                 
            myObject = JsonUtility.FromJson<CardsInfoPC>("{\"cardAPIList\":" + req.downloadHandler.text + "}");
            
            for (int i = 0; i < myObject.cardAPIList.Length; i++)
            {
                ids[i] = myObject.cardAPIList[i].id;
                names[i] = myObject.cardAPIList[i].name;
            }
			
            for (int j = 0; j < myObject.cardAPIList.Length; j++)
            {
                GameObject fatherCard = ScrollCards.gameObject.transform.GetChild(pos[j]).gameObject;  
                fatherCard.SetActive(true);
                GameObject newCard = Instantiate(prefabCard, fatherCard.transform);                   
                newCard.gameObject.transform.SetParent(fatherCard.transform);                        
			
                cards[j] = newCard;
                cards[j].GetComponent<CardPC>().idCard = j;
                cards[j].GetComponent<CardPC>().AssignCard(j);
            }
			
            shuffle();
        }
    }
    public void shuffle() 
    {
        List<int> arr = new List<int>();

        for (int i = 0; i < ids.Length; i++)
        {
            if (ids[i] != 0)
                arr.Add(ids[i]);
        }

        Random random = new Random();
		
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                int id = random.Next(arr.Count);
                int value = arr[id];
                int idInvert = random.Next(10);
			
                if (idInvert < 5)
                    cards[i].GetComponent<CardPC>().invert = true;
                else if (idInvert >= 5)
                    cards[i].GetComponent<CardPC>().invert = false;
            
                cards[i].GetComponent<CardPC>().AssignCard(value);
                arr.RemoveAt(id);
            }
        }
        
        SoundUi.Instance.PlaySound(6);
        StartCoroutine(WaitLoadingPanel());
    }
    
    public IEnumerator WaitLoadingPanel()
    {
        yield return new WaitForSeconds(0.5f);
        LoadingPanel.SetActive(false);
        buttonShuffle.gameObject.SetActive(false);
    }
    
    public void ButtonAcceptCards()
    {
        if (countselectedCard == 4)
        {
            panelSelectCards.SetActive(false);
            panelHelp.SetActive(true);
            buttonShuffle.gameObject.SetActive(false);
       
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < myObject.cardAPIList.Length; j++)
                {
                    if (myObject.cardAPIList[j].id == cardsInBoxList[i].GetComponent<CardPC>().idCardAPI)
                    {
                        StartCoroutine(DownloadAndSetImage(myObject.cardAPIList[j].image, i));
                        break;
                    }
                }
            }
        }
        else
        {
            StartCoroutine(Message());
        }
    }
    
    public IEnumerator Message()
    {
        GameObject msgButton = buttonAccept.gameObject.transform.GetChild(1).gameObject;
        msgButton.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        msgButton.SetActive(false);
    }
    
    IEnumerator DownloadAndSetImage(string url, int pos)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        DownloadHandler handle = www.downloadHandler;
        www.SetRequestHeader("Authorization", "Token " + SoundUi.Instance.TokenAPI);
        
        yield return www.SendWebRequest();
        
        if (!handle.isDone || www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            Debug.Log("Success");
            var texture = DownloadHandlerTexture.GetContent(www);
            Image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            objectsImage[pos].GetComponent<Image>().sprite = Image.sprite;

            if (pos == 3)
            {
                yield return new WaitForSeconds(2f);
                panelHelp.SetActive(false);
                dragCard = true;
            }
        }
    }
    
    public void ButtonTurnCards()
    {
        StartCoroutine(TurnCards());
    }
    
    public IEnumerator TurnCards()
    {
        buttonShowing.interactable = false;
        buttonShowing.gameObject.GetComponent<Image>().sprite = buttonDisable;
        
        DatesViewReading();
        SoundUi.Instance.PlaySound(5);
        
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(ChangeTextureCard(cardsInBoxList[i], i));
            
            if (cardsInBoxList[i].GetComponent<CardPC>().invert)
                cardsInBoxList[i].GetComponent<Animation>().Play("VoltearInvertir");
            else
                cardsInBoxList[i].GetComponent<Animation>().Play("Voltear");
        }
        
        yield return new WaitForSeconds(1f);
        buttonReading.gameObject.SetActive(true);
    }
    
    public void DatesViewReading()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject cardInBox = boxDestiny[i].transform.GetChild(1).gameObject;
            GameObject cardReading = boxReading[i].transform.GetChild(1).gameObject;    
        
            if (cardInBox.GetComponent<CardPC>().invert)                                 
            {
                Quaternion rot = Quaternion.Euler(cardReading.transform.rotation.x, cardReading.transform.rotation.y, 180f);    
                cardReading.transform.rotation = rot;
                GameObject textI = boxReading[i].transform.GetChild(3).gameObject;
                textI.GetComponent<Text>().text = "(invertida)";
            }
            else
            {
                GameObject textI = boxReading[i].transform.GetChild(3).gameObject;
                textI.GetComponent<Text>().text = "(al derecho)";
            }
            
            GameObject nameCardReading = boxReading[i].transform.GetChild(2).gameObject;            
            nameCardReading.GetComponent<Text>().text = cardInBox.GetComponent<CardPC>().Name;
        }
    }
    
    IEnumerator ChangeTextureCard(GameObject card, int pos)
    {
        yield return new WaitForSeconds(0.45f);
        
        for (int i = 0; i < myObject.cardAPIList.Length; i++)
        {
            if (myObject.cardAPIList[i].id == card.GetComponent<CardPC>().idCardAPI)
            {
                card.GetComponent<Image>().sprite = objectsImage[pos].GetComponent<Image>().sprite;
                break;
            }
        }
    }
    
    public void ButtonReading()
    {
        LoadingPanel.SetActive(true);
        buttonReading.gameObject.SetActive(false);
        buttonShowing.gameObject.SetActive(false);
        cardInBox.SetActive(false);
        panelBoxs.SetActive(false);
        
        for (int i = 0; i < 4; i++)
        {
            GameObject cardInBox = boxDestiny[i].transform.GetChild(1).gameObject;
            GameObject cardReading = boxReading[i].transform.GetChild(1).gameObject;
            cardReading.GetComponent<Image>().sprite = cardInBox.GetComponent<Image>().sprite;
            idCards[i] = cardInBox.GetComponent<CardPC>().idCardAPI;
            
            if (cardInBox.GetComponent<CardPC>().invert)
                isInverses[i] = "True";
            else
                isInverses[i] = "False";
        }
        
        StartCoroutine(GetDatesDescription());
    }
   
    public IEnumerator GetDatesDescription()
    {
        for (int i = 0; i < 4; i++)
        {
            WWWForm form = new WWWForm();
            form.AddField("cards", idCards[i]);
            form.AddField("positions", positions[i]);
            form.AddField("isInverses", isInverses[i]);
        
            UnityWebRequest req = UnityWebRequest.Post(SoundUi.Instance.urlLesturaCards, form);
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
                
                if (jsonString.Contains("["))
                {
                    jsonString = jsonString.Remove(0, 11);
                    jsonString = Regex.Replace(jsonString, @"(.*)]}$","$1");
                    DescriptionAPIPC dataDescription = JsonUtility.FromJson<DescriptionAPIPC>(jsonString);
                    
                    string predictionOk = dataDescription.predictions;
            
                    if (predictionOk.Contains("\u2028"))
                        predictionOk = predictionOk.Replace("\u2028", " ");
                    
                    GameObject cardDescription = boxReading[i].transform.GetChild(4).gameObject;
                    cardDescription.GetComponent<Text>().text = predictionOk;
                }
                else
                {
                    GameObject cardDescription = boxReading[i].transform.GetChild(4).gameObject;
                    cardDescription.GetComponent<Text>().text = "Lo sentimos, no encontramos una predicción para esa combinación.";
                }
                
                if (i == 3)
                {
                    LoadingPanel.SetActive(false);
                    panelReading.SetActive(true);
                    SoundUi.Instance.PlaySound(3);
                }
            }
        }
    }

    public void ButtonNextRevelation()
    {
        if (revelacionScreen == 0) // Presente
        {
            boxReading[0].GetComponent<Animation>().Play("NextT");
            boxReading[3].GetComponent<Animation>().Play("NextTarot2");
            revelacionScreen = 3;
            return;
        }
        if (revelacionScreen == 1) // Amor
        {
            boxReading[1].GetComponent<Animation>().Play("NextT"); // se va para la izquierda
            boxReading[0].GetComponent<Animation>().Play("NextTarot2"); // entra por la derecha
            revelacionScreen = 0;
            buttonPrevious.gameObject.GetComponent<Image>().sprite = buttonRevSprite[0];
            return;
        }
        if (revelacionScreen == 3) // Trabajo
        {
            boxReading[3].GetComponent<Animation>().Play("NextT");
            boxReading[2].GetComponent<Animation>().Play("NextTarot2");
            revelacionScreen = 2;
            buttonNext.gameObject.GetComponent<Image>().sprite = buttonRevSprite[1];
        }
    }
    
    public void ButtonPreviousRevelation()
    {
        if (revelacionScreen == 0) // Presente
        {
            boxReading[0].GetComponent<Animation>().Play("PreviousTarot2"); // se va por la derecha
            boxReading[1].GetComponent<Animation>().Play("PreviousT"); // entra por la izquierda
            revelacionScreen = 1;
            buttonPrevious.gameObject.GetComponent<Image>().sprite = buttonRevSprite[1];
            return;
        }
        if (revelacionScreen == 2) // Mensaje
        {
            boxReading[2].GetComponent<Animation>().Play("PreviousTarot2"); 
            boxReading[3].GetComponent<Animation>().Play("PreviousT"); 
            revelacionScreen = 3;
            buttonNext.gameObject.GetComponent<Image>().sprite = buttonRevSprite[0];
            return;
        }
        if (revelacionScreen == 3) // Trabajo
        {
            boxReading[3].GetComponent<Animation>().Play("PreviousTarot2");
            boxReading[0].GetComponent<Animation>().Play("PreviousT");
            revelacionScreen = 0;
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

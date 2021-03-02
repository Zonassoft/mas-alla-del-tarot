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
using System.Runtime.InteropServices;

[Serializable]
public class TokenAPITarot
{
    public string key;
}

[Serializable]
public class DescriptionAPI
{
    public int id_card;
    public int id_position;
    public int isInverse;
    public string combinations;
    public string predictions;
}

[Serializable]
public class CardAPI
{
    public int id; 
    public string name; 
    public string image;
}

[Serializable]
public class CardsInfo
{
    public CardAPI[] cardAPIList;
}

// Orden de las casillas: Presente id 1, amor id 2, mensaje id 3, circunstancias id 4
public class GameTarotController : MonoBehaviour
{
    public GameObject panelSelectCards;
    public GameObject panelOptions;
    public GameObject Menu;
    public GameObject panelReading;
    public GameObject panelHelp;
    public GameObject cardInBox;
    public GameObject panelBoxs;
    public GameObject restartButton;
    public GameObject LoadingPanel;
    public GameObject ScrollCards;
    
    public Button buttonReading;
    public Button buttonShuffle;
    public Button buttonShowing;
    public Button buttonAccept;
    public Button buttonFullScreen;
    public Button buttonMinimize;
    
    public Canvas canvasGame;
    
    [DllImport("__Internal")]
    private static extern void FullScreenFunction();
    
    // Vista Lectura
    public GameObject[] boxDestiny;                // Las cuatro casillas donde van las cartas
    public GameObject[] boxReading;                // El componente que tiene carta, nombre, invertida
    public Text[] descriptionsTextList;            // texto de la descripción
    public Text[] invertTextList;                  // texto para invertida
    
    public CardsInfo myObject = new CardsInfo();
	
    // Datos de las cartas
    public int[] ids = new int[22]; 
    public string[] names = new String[22];
    private GameObject[] cards = new GameObject[22];
    private int[] pos = {10, 11, 9, 12, 8, 13, 7, 14, 6, 15, 5, 16, 4, 17, 3, 18, 2, 19, 1, 20, 0, 21, 22};
    public GameObject prefabCard;
    
    public GameObject[] objectsImage = new GameObject[4];
    public Image Image;
	
    public int countselectedCard;
    public GameObject[] cardsInBoxList = new GameObject[4];   // Cartas que están volteadas en la mesa
    
    private int[] idCards = new int[4];
    private string[] isInverses = new string[4];
    private int[] positions = new int[4] {1, 2, 3, 4};
    
    private string API_KEY;
    
    private bool panelOptionActive;
    private bool dragCard;
    
    AudioSource[] audioSources;
    private AudioSource barajar, voltear, revelacion, menu;
    
    public Sprite buttonEnnable;
    public Sprite buttonDisable;
    public GameObject childpanelBoxs;

    private void Start()
    {
        if (canvasGame.transform.GetComponent<RectTransform>().rect.width < childpanelBoxs.transform.GetComponent<RectTransform>().rect.width)
            canvasGame.transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.9f;
        
        audioSources = GetComponents<AudioSource>();
        barajar = audioSources[0];
        voltear = audioSources[1];
        revelacion = audioSources[2];
        menu = audioSources[3];
        
        buttonFullScreen.onClick.AddListener(TaskOnClickMax);
        buttonMinimize.onClick.AddListener(TaskOnClickMin);
    }

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
    
    public void ButtonShuffle()
    {
        LoadingPanel.SetActive(true);
        panelSelectCards.SetActive(true);
        
        buttonAccept.gameObject.GetComponent<Image>().sprite = buttonDisable;
        buttonAccept.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        GameObject textbutton = buttonAccept.gameObject.transform.GetChild(0).gameObject;
        textbutton.gameObject.GetComponent<Text>().color = new Color(0.78f, 0.72f, 0.78f, 1f);
        
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
            TokenAPITarot dataKey = JsonUtility.FromJson<TokenAPITarot>(jsonString);
            API_KEY = dataKey.key;
            
            StartCoroutine(GetDatesCards("Http://82.223.139.65/api/v1/admin/card/", dataKey.key));
        }
    }
    
    public IEnumerator GetDatesCards(string url, string token)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
	    req.SetRequestHeader("Authorization", "Token " + token);
        
        yield return req.SendWebRequest();
        
        if (req.isNetworkError || req.isHttpError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            myObject = JsonUtility.FromJson<CardsInfo>("{\"cardAPIList\":" + req.downloadHandler.text + "}");
            
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
                cards[j].GetComponent<CardNew>().idCard = j;
                cards[j].GetComponent<CardNew>().AssignCard(j);
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
                    cards[i].GetComponent<CardNew>().invert = true;
                else if (idInvert >= 5)
                    cards[i].GetComponent<CardNew>().invert = false;
            
                cards[i].GetComponent<CardNew>().AssignCard(value);
                arr.RemoveAt(id);
            }
        }
        
        barajar.Play();
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
            StartCoroutine(WaitButtonAccept());
        else
            StartCoroutine(Message());
    }
    
    public IEnumerator Message()
    {
        GameObject msgButton = buttonAccept.gameObject.transform.GetChild(1).gameObject;
        msgButton.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        
        msgButton.SetActive(false);
    }
    
    public IEnumerator WaitButtonAccept()
    {
        yield return new WaitForSeconds(0.5f);
        panelSelectCards.SetActive(false);
        panelHelp.SetActive(true);
        buttonShuffle.gameObject.SetActive(false);
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < myObject.cardAPIList.Length; j++)
            {
                if (myObject.cardAPIList[j].id == cardsInBoxList[i].GetComponent<CardNew>().idCardAPI)
                {
                    StartCoroutine(DownloadAndSetImage(myObject.cardAPIList[j].image, i));
                    break;
                }
            }
        }
    }
    
    IEnumerator DownloadAndSetImage(string url, int pos)
    {
        Debug.Log("Url de la imagen" + url);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        www.SetRequestHeader("Authorization", "Token " + API_KEY);
        DownloadHandler handle = www.downloadHandler;
        
        yield return www.SendWebRequest();
        
        if (!handle.isDone || www.isHttpError || www.isNetworkError)
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
        GameObject textbutton = buttonShowing.gameObject.transform.GetChild(0).gameObject;
        textbutton.gameObject.GetComponent<Text>().color = new Color(0.78f, 0.72f, 0.78f, 1f);
        
        DatesViewReading();
        voltear.Play();
        
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(ChangeTextureCard(cardsInBoxList[i], i));
            
            if (cardsInBoxList[i].GetComponent<CardNew>().invert)
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
        
            if (cardInBox.GetComponent<CardNew>().invert)                                 
            {
                Quaternion rot = Quaternion.Euler(cardReading.transform.rotation.x, cardReading.transform.rotation.y, 180f);    
                cardReading.transform.rotation = rot;
                invertTextList[i].text = "(invertida)";
            }
            else
            {
                invertTextList[i].text = "(al derecho)";
            }
        
            GameObject nameCardReading = boxReading[i].transform.GetChild(2).gameObject;            
            nameCardReading.GetComponent<Text>().text = cardInBox.GetComponent<CardNew>().Name;
        }
    }
    
    IEnumerator ChangeTextureCard(GameObject card, int pos)
    {
        yield return new WaitForSeconds(0.45f);

        for (int i = 0; i < myObject.cardAPIList.Length; i++)
        {
            if (myObject.cardAPIList[i].id == card.GetComponent<CardNew>().idCardAPI)
            {
                Debug.Log(card.GetComponent<Image>().name);
                card.GetComponent<Image>().sprite = objectsImage[pos].GetComponent<Image>().sprite;
                break;
            }
        }
    }
    
    public void ButtonReading()
    {
        StartCoroutine(WaitReadingPanel());
    }

    public IEnumerator WaitReadingPanel()
    {
        yield return new WaitForSeconds(0.5f);
        LoadingPanel.SetActive(true);
        buttonReading.gameObject.SetActive(false);
        restartButton.SetActive(true);
        buttonShowing.gameObject.SetActive(false);
        cardInBox.SetActive(false);
        panelBoxs.SetActive(false);
        
        for (int i = 0; i < 4; i++)
        {
            GameObject cardInBox = boxDestiny[i].transform.GetChild(1).gameObject;
            GameObject cardReading = boxReading[i].transform.GetChild(1).gameObject;
            cardReading.GetComponent<Image>().sprite = cardInBox.GetComponent<Image>().sprite;
            idCards[i] = cardInBox.GetComponent<CardNew>().idCardAPI;
            
            if (cardInBox.GetComponent<CardNew>().invert)
                isInverses[i] = "True";
            else
                isInverses[i] = "False";
        }
        
        StartCoroutine(GetDatesDescription("Http://82.223.139.65/api/v1/client/tarot/"));
    }

    public IEnumerator GetDatesDescription(string url)
    {
        for (int i = 0; i < 4; i++)
        {
            WWWForm form = new WWWForm();
            form.AddField("cards", idCards[i]);
            form.AddField("positions", positions[i]);
            form.AddField("isInverses", isInverses[i]);
        
            UnityWebRequest req = UnityWebRequest.Post(url, form);
            req.SetRequestHeader("Authorization", "Token " + API_KEY);
            
            yield return req.SendWebRequest();
        
            if (req.isNetworkError || req.isHttpError)
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
                    DescriptionAPI dataDescription = JsonUtility.FromJson<DescriptionAPI>(jsonString);
                    
                    string predictionOk = dataDescription.predictions;
            
                    if (predictionOk.Contains("\u2028"))
                        predictionOk = predictionOk.Replace("\u2028", " ");
                    
                    descriptionsTextList[i].text = predictionOk;
                }
                else
                {
                    descriptionsTextList[i].text = "Lo sentimos, no encontramos una predicción para esa combinación.";
                }
                
                if (i == 3)
                {
                    LoadingPanel.SetActive(false);
                    panelReading.SetActive(true);
                    revelacion.Play();
                }
            }
        }
    }
    
    public void ButtonOptions()
    {
        panelOptionActive = true;
        panelOptions.SetActive(true);
        Menu.GetComponent<Animation>().Play("MenuIn");
        menu.Play();
    }
    
    public void ButtonQuitOptions()
    {
        panelOptionActive = false;
        menu.Play();
        Menu.GetComponent<Animation>().Play("MenuOut");
        StartCoroutine(AnimationMenu());
    }

    public IEnumerator AnimationMenu()
    {
        yield return new WaitForSeconds(1f);
        panelOptions.SetActive(false);
    }
    
    public IEnumerator AnimationMenuStartScene(string nameScene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene	(nameScene); 
    }
    
    public void StartScene(string nameScene)
    {
        if (panelOptionActive && nameScene != "SelectGame")
        {
            panelOptionActive = false;
            menu.Play();
            Menu.GetComponent<Animation>().Play("MenuOut");
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


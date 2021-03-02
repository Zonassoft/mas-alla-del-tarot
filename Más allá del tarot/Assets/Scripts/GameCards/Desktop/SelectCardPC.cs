using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectCardPC : MonoBehaviour, IPointerClickHandler
{
    private TarotControllerDesktop classController;
    public Text number;
    private bool selected;
    
    AudioSource[] audioSources;
    private AudioSource clic;

    void Awake()
    {
        classController = FindObjectOfType<TarotControllerDesktop>();
    }

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        clic = audioSources[0];
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
       clic.Play();
        
       if (classController.countselectedCard < 4 && !selected)
       {
            selected = true;
            classController.countselectedCard++;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 20f, gameObject.transform.position.y + 40f, gameObject.transform.position.z);
                
            GameObject cardChild = gameObject.transform.GetChild(1).gameObject;
            
            for (int i = 0; i < 4; i++)
            {
                if (classController.cardsInBoxList[i].GetComponent<CardPC>().idCardAPI == 0)
                {
                    int num = i + 1;
                    number.text = num.ToString();
                    classController.cardsInBoxList[i].SetActive(true);
                    classController.cardsInBoxList[i].GetComponent<CardPC>().AssignCard(cardChild.GetComponent<CardPC>().idCardAPI);
                    classController.cardsInBoxList[i].GetComponent<CardPC>().invert = cardChild.GetComponent<CardPC>().invert;
                    break;
                }
            }

           if (classController.countselectedCard == 4)
           {
               classController.buttonAccept.gameObject.GetComponent<Image>().sprite = classController.buttonEnnable;
               classController.buttonAccept.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
               GameObject textbutton = classController.buttonAccept.gameObject.transform.GetChild(0).gameObject;
               textbutton.gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
           }
        }
    else if (selected)
        {
            classController.buttonAccept.gameObject.GetComponent<Image>().sprite = classController.buttonDisable;
            classController.buttonAccept.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            GameObject textbutton = classController.buttonAccept.gameObject.transform.GetChild(0).gameObject;
            textbutton.gameObject.GetComponent<Text>().color = new Color(0.78f, 0.72f, 0.78f, 1f);
            selected = false;
            classController.countselectedCard--;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 20f, gameObject.transform.position.y - 40f, gameObject.transform.position.z);
            number.text = "";
                
            GameObject cardChild = gameObject.transform.GetChild(1).gameObject;
                
            for (int i = 0; i < 4; i++)
            {
                if (classController.cardsInBoxList[i].GetComponent<CardPC>().idCardAPI == cardChild.GetComponent<CardPC>().idCardAPI)
                {
                    classController.cardsInBoxList[i].GetComponent<CardPC>().idCardAPI = 0;
                    classController.cardsInBoxList[i].SetActive(false);
                    break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SelectCard : MonoBehaviour, IPointerClickHandler
{
    private GameTarotController classController;
    public TextMeshProUGUI number;
    private bool selected;
    
    void Awake()
    {
        classController = FindObjectOfType<GameTarotController>();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
       SoundUi.Instance.PlaySound(1);
       number = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        
       if (classController.countselectedCard < 4 && !selected)
       {
            selected = true;
            classController.countselectedCard++;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 20f, gameObject.transform.position.y + 40f, gameObject.transform.position.z);
            GameObject cardChild = gameObject.transform.GetChild(1).gameObject;
            
            for (int i = 0; i < 4; i++)
            {
                if (classController.cardsInBoxList[i].GetComponent<CardNew>().idCardAPI == 0)
                {
                    int num = i + 1;
                    number.text = num.ToString();
                    classController.cardsInBoxList[i].SetActive(true);
                    classController.cardsInBoxList[i].GetComponent<CardNew>().AssignCard(cardChild.GetComponent<CardNew>().idCardAPI);
                    classController.cardsInBoxList[i].GetComponent<CardNew>().invert = cardChild.GetComponent<CardNew>().invert;
                    break;
                }
            }

           if (classController.countselectedCard == 4)
               classController.buttonAccept.gameObject.GetComponent<Image>().sprite = classController.buttonEnnable;
        }
        else if (selected)
        {
            classController.buttonAccept.gameObject.GetComponent<Image>().sprite = classController.buttonDisable;
            selected = false;
            classController.countselectedCard--;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 20f, gameObject.transform.position.y - 40f, gameObject.transform.position.z);
            number.text = "";
            GameObject cardChild = gameObject.transform.GetChild(1).gameObject;
                
            for (int i = 0; i < 4; i++)
            {
                if (classController.cardsInBoxList[i].GetComponent<CardNew>().idCardAPI == cardChild.GetComponent<CardNew>().idCardAPI)
                {
                    classController.cardsInBoxList[i].GetComponent<CardNew>().idCardAPI = 0;
                    classController.cardsInBoxList[i].SetActive(false);
                    break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardNew : MonoBehaviour
{
    public int posFront;
    public int idCard;
    public int idCardAPI;
    public string Name;
    public bool invert;

    private GameTarotController classController;

    void Awake()
    {
        classController = FindObjectOfType<GameTarotController>();
    }
    
    public void AssignCard(int id)
    {
        int pos = 0;

        for (int i = 0; i < classController.ids.Length; i++)
        {
            if (classController.ids[i] == id)
            {
                pos = i;
                break;
            }
        }
        
        posFront = pos;
        idCardAPI = classController.ids[pos];
        Name = classController.names[pos];
    }
}

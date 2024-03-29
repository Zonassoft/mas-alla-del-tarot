﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDraggerPC : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static Transform draggedCardPC;                          
    public Transform hand;                                        
    public GameObject papelera;
	
    public void OnBeginDrag (PointerEventData eventData)
    {
        SoundUi.Instance.PlaySound(8);
        
        if (transform.childCount == 0)                           
        { 
            eventData.pointerDrag = null;           
            return;
        }

        papelera.gameObject.SetActive (true);
        draggedCardPC = transform.GetChild (0);                     
        draggedCardPC.SetParent (hand, false); 
    }
    
    public void OnDrag (PointerEventData eventData)
    {
        hand.position = Input.mousePosition;                    
    }
	
    public void OnEndDrag (PointerEventData eventData)
    {
        // Si solté el clic en alguna casilla vacía
        if (draggedCardPC == null)                               
        {
            gameObject.SetActive(false);
            return;                          
        }
		
        draggedCardPC.SetParent(transform);
        draggedCardPC.gameObject.transform.position = gameObject.transform.position;
        draggedCardPC.gameObject.transform.rotation = gameObject.transform.rotation;
        draggedCardPC = null;
    }
}

using UnityEngine.EventSystems;
using UnityEngine;

public class CardGetterPC : MonoBehaviour, IDropHandler
{
    private Transform droppedCard;
    
    public void OnDrop (PointerEventData eventData) 
    {
        droppedCard = CardDraggerPC.draggedCardPC;           
        
        if (gameObject.CompareTag (droppedCard.tag))    
        {
            // Si en esa casilla no hay ninguna carta ya puesta
            if (transform.childCount == 1) 
            {
                SoundUi.Instance.PlaySound(7);
                CardDraggerPC.draggedCardPC = null;
                droppedCard.SetParent(transform,false);
            }
        } 
    }
}

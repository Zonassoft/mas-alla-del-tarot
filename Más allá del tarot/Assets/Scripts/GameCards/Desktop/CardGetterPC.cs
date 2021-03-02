using UnityEngine.EventSystems;
using UnityEngine;

public class CardGetterPC : MonoBehaviour, IDropHandler
{
    private Transform droppedCard;
    
    AudioSource[] audioSources;
    private AudioSource soltar;
	
    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        soltar = audioSources[0];
    }
	
    public void OnDrop (PointerEventData eventData) 
    {
        droppedCard = CardDraggerPC.draggedCardPC;           
        
        if (gameObject.CompareTag (droppedCard.tag))    
        {
            // Si en esa casilla no hay ninguna carta ya puesta
            if (transform.childCount == 1) 
            {
                soltar.Play();
                CardDraggerPC.draggedCardPC = null;
                droppedCard.SetParent(transform,false);
            }
        } 
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

// Casillas donde se deben colocar las cartas

public class CardGetter : MonoBehaviour, IDropHandler
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
		droppedCard = CardDragger.draggedCard;           
        
		if (gameObject.CompareTag (droppedCard.tag))    
		{
			// Si en esa casilla no hay ninguna carta ya puesta
			if (transform.childCount == 1) 
			{
				soltar.Play();
				CardDragger.draggedCard = null;
				droppedCard.SetParent(transform,false);
			}
		} 
	}
}

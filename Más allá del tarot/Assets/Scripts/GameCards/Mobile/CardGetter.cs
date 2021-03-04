using UnityEngine;
using UnityEngine.EventSystems;

// Casillas donde se deben colocar las cartas

public class CardGetter : MonoBehaviour, IDropHandler
{
	public PlaySoundButton playSoundClass;
	private Transform droppedCard;
	
	public void OnDrop (PointerEventData eventData) 
	{
		droppedCard = CardDragger.draggedCard;           
        
		if (gameObject.CompareTag (droppedCard.tag))    
		{
			// Si en esa casilla no hay ninguna carta ya puesta
			if (transform.childCount == 1) 
			{
				SoundUi.Instance.PlaySound(7);
				CardDragger.draggedCard = null;
				droppedCard.SetParent(transform,false);
			}
		} 
	}
}

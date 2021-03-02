using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// Objeto que tiene la carta

public class CardDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static Transform draggedCard;                          
	public Transform hand;                                        
	public GameObject papelera;
	
	AudioSource[] audioSources;
	private AudioSource coger;
	
	private void Start()
	{
		audioSources = GetComponents<AudioSource>();
		coger = audioSources[0];
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		coger.Play();
		
		if (transform.childCount == 0)                           
		{ 
			eventData.pointerDrag = null;           
			return;
		}

		papelera.gameObject.SetActive (true);
		draggedCard = transform.GetChild (0);                     
		draggedCard.SetParent (hand, false); 
	}
    
	public void OnDrag (PointerEventData eventData)
	{
		hand.position = Input.mousePosition;                    
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		// Si solté el clic en alguna casilla vacía
		if (draggedCard == null)                               
		{
			gameObject.SetActive(false);
			return;                          
		}
		
		draggedCard.SetParent(transform);
		draggedCard.gameObject.transform.position = gameObject.transform.position;
		draggedCard.gameObject.transform.rotation = gameObject.transform.rotation;
    	draggedCard = null;
	}
}

using System.Collections;
using UnityEngine;

public class DadoMobile : MonoBehaviour
{
    private GameDadosControllerMovil classController;
    private bool finishedAddForce = true;
    public GameObject reference;
    
    private void Awake()
    {
        classController = FindObjectOfType<GameDadosControllerMovil>();
    }
    
    private void Update()
    {
        if (finishedAddForce)
            StartCoroutine(AddForce());
    }
    
    public void OnMouseUpAsButton()
    {
        classController.ClicDado();
    }
    
    public IEnumerator AddForce()
    {
        finishedAddForce = false;
        float force = Random.Range(0.3f, 1f);  
        float normalizedTime = 0.0f;
        while (normalizedTime < 1)  
        {
            transform.GetComponent<Rigidbody>().AddForce(reference.transform.up * force);
            normalizedTime += Time.deltaTime / 3f;
            yield return null;
        }
        
        normalizedTime = 0.0f;
        while (normalizedTime < 1)  
        {
            transform.GetComponent<Rigidbody>().AddForce(-reference.transform.up * force);
            normalizedTime += Time.deltaTime / 3f;
            yield return null;
        }
        finishedAddForce = true;
    }
}

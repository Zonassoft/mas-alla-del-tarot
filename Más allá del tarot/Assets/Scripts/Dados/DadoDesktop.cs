using System.Collections;
using UnityEngine;

public class DadoDesktop : MonoBehaviour
{
    private GameDadosController classController;
    private bool finishedAddForce = true;
    public GameObject reference;
    
    [SerializeField]
    public float maxForce;
    
    [SerializeField]
    public float minForce;

    private void Awake()
    {
        classController = FindObjectOfType<GameDadosController>();
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

using UnityEngine;
using System.Collections;

public class SwordControl : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        StartCoroutine(Despawn());

    }

    // Update is called once per frame
    private void Update()
    {

        
    }
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5f);

        Destroy(gameObject);

    }
   
}

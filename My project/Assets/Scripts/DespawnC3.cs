using UnityEngine;
using System.Collections;

public class dialogueControl3 : MonoBehaviour
{
    public float time = 1f;
    
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
        GameObject enemies = GameObject.FindWithTag("ENEMIES");
        
        enemies.SetActive(false);


        yield return new WaitForSeconds(time);

        Destroy(gameObject);
       
        enemies.SetActive(true);
    }
   
}

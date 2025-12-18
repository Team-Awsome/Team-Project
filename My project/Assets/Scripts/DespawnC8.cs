using UnityEngine;
using System.Collections;

public class DSDialogue : MonoBehaviour
{
    public float time;
   public AudioSource E;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
       
      StartCoroutine(Despawn());
        

    }

    // Update is called once per frame
    private void Update()
    {


    }
   public IEnumerator Despawn()
    {
        
        yield return new WaitForSeconds(time);
       
        E.Play();

    }

}

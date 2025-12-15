using System.Collections;
using TMPro;
using UnityEngine;

public class dialogueControl5 : MonoBehaviour
{
    TextMeshProUGUI dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        dialogue = GameObject.FindGameObjectWithTag("text").GetComponent<TextMeshProUGUI>();
        StartCoroutine(Talk());

    }

    // Update is called once per frame
    private void Update()
    {

        
    }
    IEnumerator Talk()
    {
        dialogue.text = " " + "HELLO. I AM. ELPPA.";

        yield return new WaitForSeconds(5f);
        
        dialogue.text = " " + "I AM. YOUR FRIEND";
        
        yield return new WaitForSeconds(5f);
        
        dialogue.text = " " + "CONTROLS? HOW TO PLAY?";
        
        yield return new WaitForSeconds(5f);

        dialogue.text = " " + "use wasd to move";

        yield return new WaitForSeconds(1f);
        
        dialogue.text = " " + "use LEFT SHIFT to run";
        
        yield return new WaitForSeconds(1f);
        
        dialogue.text = " " + "left click to attack";
        
        yield return new WaitForSeconds(1f);
        
        dialogue.text = " " + "oKAY THAT IS ALL THE TIME I HAVE. GOODBYE";
        
        yield return new WaitForSeconds(4f);
        
        Destroy(gameObject);
       
    }
   
}

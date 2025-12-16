using System.Collections;
using TMPro;
using UnityEngine;

public class dialogueControl7 : MonoBehaviour
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
        dialogue.text = " " + "so";

        yield return new WaitForSeconds(2.5f);
        
        dialogue.text = " " + "you passed THE TUTORIAL";
        
        yield return new WaitForSeconds(4.5f);
        
        dialogue.text = " " + "LEARNED CONTROLS? AND GOT SOME EXP YE?";
        
        yield return new WaitForSeconds(4f);

        dialogue.text = " " + "but";

        yield return new WaitForSeconds(3f);
        
        dialogue.text = " " + "I WILL HAVE TO";
        
        yield return new WaitForSeconds(3f);
        
        dialogue.text = " " + "LET YOU LEARN THE REST FROM HERE";
        
        yield return new WaitForSeconds(4f);
        
        dialogue.text = " " + "NEW ENEMIES WILL ARISE FROM NOW ON";
        
        yield return new WaitForSeconds(4f);
        
        dialogue.text = " " + "GOODBYE";
        
        yield return new WaitForSeconds(2f);
        
        Destroy(gameObject);
       
    }
   
}

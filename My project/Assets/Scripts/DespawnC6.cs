using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class dialogueControl6 : MonoBehaviour
{
    GameObject Adialogue;
    GameObject mb1;
    GameObject mb2;
    TextMeshProUGUI dialogue;
    public static bool Edialogue = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Adialogue = GameObject.FindGameObjectWithTag("dialogue_ui");
        if (!Edialogue)
        {
            dialogue = GameObject.FindGameObjectWithTag("text").GetComponent<TextMeshProUGUI>();
            dialogue.text = " " + "Bozo";
            SceneManager.LoadScene(0);
        }
        mb1 = GameObject.FindGameObjectWithTag("mb");
        mb1.SetActive(false); 
        
        mb2 = GameObject.FindGameObjectWithTag("mb2");
        mb2.SetActive(false);
        
        dialogue = GameObject.FindGameObjectWithTag("text").GetComponent<TextMeshProUGUI>();
        StartCoroutine(Talk());

    }

    // Update is called once per frame
    private void Update()
    {


    }
    IEnumerator Talk()
    {
        mb1.SetActive(true);

        dialogue.text = " " + "so";

        yield return new WaitForSeconds(5f);

        dialogue.text = " " + "YOU COMPLETED THE TUTORIAL";

        yield return new WaitForSeconds(5f);

        dialogue.text = " " + "but";

        yield return new WaitForSeconds(5f);

        dialogue.text = " " + "THIS IS NOT THE FULL GAME";

        yield return new WaitForSeconds(5f);

        dialogue.text = " " + "MISSING SOUNDS, PLACEHOLDERS, NO STORY... UNBALANCED";

        yield return new WaitForSeconds(5f);

        dialogue.text = " " + "THAT TUTORIAL WAS A TEST";

        yield return new WaitForSeconds(5f);

        dialogue.text = " " + "NOW YOUR TRUE CHALLENGE... BEGINS :)";

        yield return new WaitForSeconds(5f);
       
        mb1.SetActive(false);
      
        mb2.SetActive(true);

        Edialogue = false;
        
        Destroy(gameObject);

    }

}

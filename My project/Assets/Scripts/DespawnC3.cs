using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class dialogueControl3 : MonoBehaviour
{
    public bool mouse = true;   
    public float time = 1f;
    GameObject enemies;
    GameObject mb1;
    GameObject mb2;
    public AudioSource E;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Skip()
    {
        
        mb1.SetActive(false);
        E.Play();
        Destroy(gameObject);
        enemies.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    void Start()
    {
        mb1 = GameObject.FindGameObjectWithTag("mb");
        mb2 = GameObject.FindGameObjectWithTag("mb2");
        
        mouse = true;
        enemies = GameObject.FindWithTag("ENEMIES");
        StartCoroutine(Despawn());

    }

    // Update is called once per frame
    private void Update()
    {
        if (mouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
    IEnumerator Despawn()
    {
       

       
        
        enemies.SetActive(false);


        yield return new WaitForSeconds(time);

        Destroy(gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        enemies.SetActive(true);
    }
    
}

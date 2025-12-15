using UnityEngine;
using System.Collections;

public class dialogueControl3 : MonoBehaviour
{
    public float time = 1f;
    GameObject enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Skip()
    {

        Destroy(gameObject);
        enemies.SetActive(true);

    }
    void Start()
    {
        enemies = GameObject.FindWithTag("ENEMIES");
        StartCoroutine(Despawn());

    }

    // Update is called once per frame
    private void Update()
    {

        
    }
    IEnumerator Despawn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

       
        
        enemies.SetActive(false);


        yield return new WaitForSeconds(time);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Destroy(gameObject);
       
        enemies.SetActive(true);
    }
    
}

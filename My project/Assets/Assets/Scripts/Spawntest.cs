using UnityEngine;
using System.Collections;

public class spawn : MonoBehaviour
{
    public Transform SPAWNPOINT;
    public GameObject orb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Spawn()
    {

        yield return new WaitForSeconds(5);

       Instantiate(orb, SPAWNPOINT.position, SPAWNPOINT.rotation);

        yield return new WaitForSeconds(15);

        StartCoroutine(Spawn());
    }
}

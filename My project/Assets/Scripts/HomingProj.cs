using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;
public class HomingProjectile : MonoBehaviour
{
    public float Speed;
    public float DespawnTime;
    
    public GameObject target;
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("player");
        StartCoroutine(Move());
        StartCoroutine(Despawn());
    }

    IEnumerator Move()
    {
        while (target != null && Vector3.Distance(transform.position, target.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                Speed * Time.deltaTime
            );

            yield return null;
        }
    }
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(DespawnTime);

        Destroy(gameObject);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

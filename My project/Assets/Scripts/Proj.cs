using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class Projectile : MonoBehaviour
{
    public float Speed;
    public float DespawnTime;
    
    public GameObject target;
    private Vector3 targetPosition;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("player");
        targetPosition = target.transform.position;
        StartCoroutine(Move());
        StartCoroutine(Despawn());
    }

    IEnumerator Move()
    {
        while (target != null && Vector3.Distance(transform.position, target.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
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

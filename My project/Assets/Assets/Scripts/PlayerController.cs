using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerController : MonoBehaviour
{
    Camera playerCam;
    Rigidbody rb;
    Ray jumpRay;
    Ray interactRay;
    RaycastHit interactHit;
    GameObject gameoverscreen;
    GameObject Selection;
    public PlayerInput input;
    float verticalMove;
    float horizontalMove;
    public AudioSource deathspeaker;
    public Camera firingDirection;

    public float speed = 11f;
    public float jumpHeight = 10f;
    public float interactDistance = 1f;
    public float jumpRayDistance = 1.1f;
    public float level = 1f; 
    public float xp = 1f; 

    public int health = 20;
    public int maxhealth = 20;

    public bool attacking = false;
    public bool isSprinting = false;
    public bool isSlashing = false;
    public bool isShooting = false;
    public static bool SwordF = false;
    public static bool BowF = false;

    
    public Transform Spawnpoint1;
    public Transform Spawnpoint2;
    public GameObject Sword;
    public GameObject bullet;

    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 2)
        {
            gameoverscreen = GameObject.FindGameObjectWithTag("ui_gameOver");

            gameoverscreen.SetActive(false);


            Time.timeScale = 1;


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            deathspeaker = GetComponent<AudioSource>();

            
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            
            Selection = GameObject.FindGameObjectWithTag("ui_selection");


            Selection.SetActive(true);
            Time.timeScale = 0;


            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            deathspeaker = GetComponent<AudioSource>();

            
        }
       

       

        input = GetComponent<PlayerInput>();
        jumpRay = new Ray(transform.position, -transform.up);
        interactRay = new Ray(transform.position, transform.forward);
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
      


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (health <= 0)
        {
            gameoverscreen.SetActive(true);
        }
        if (health <= 0)
            if (!deathspeaker.isPlaying)
            { 
                //Destroy(GameObject.FindWithTag("MUSICBOX"));
                //Destroy(GameObject.FindWithTag("musicbox2"));
                deathspeaker.Play();
            }
            

        if (health <= 0)
        {
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
     //LEVELS
        if (level >= 2f)
        {
            maxhealth = 25;
        }
       
        if (level >= 3f)
        {
            maxhealth = 30;
        }
       
        if (level >= 4f)
        {
            maxhealth = 40;
        }
       
        if (level >= 5f)
        {
            maxhealth = 50;
        }
       
    //XP  
        if (xp >= 30f)
        {
            level = 2f;
        }
       
        if (xp >= 100f)
        {
            level = 3f;
        }
       
        if (xp >= 300f)
        {
            level = 4f;
        }
       
        if (xp >= 1000f)
        {
            level = 5f;
        }

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        // Player Rotation (Horiztonally)
        Quaternion playerRotation = playerCam.transform.rotation;
        playerRotation.x = 0;
        playerRotation.z = 0;
        transform.rotation = playerRotation;


        // Movement System (Take vert/horiz input and convert to 3D movement)
        Vector3 temp = rb.linearVelocity;

        temp.x = verticalMove * speed;
        temp.z = horizontalMove * speed;

        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

        
       

        rb.linearVelocity = (temp.x * transform.forward) +
                            (temp.y * transform.up) +
                            (temp.z * transform.right);
    }
    // Read player input and convert to values to be used by rb for movement
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();

        verticalMove = inputAxis.y;
        horizontalMove = inputAxis.x;
    }
    // If raycast downward sees collider, player can jump.
    public void Jump()
    {
        if (Physics.Raycast(jumpRay, jumpRayDistance))
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }


    public void BowS()
    {
        BowF = true;       // Use bow
        SwordF = false;    // Disable sword
        
       
        SceneManager.LoadScene(2);
    }

    public void SwordS()
    {
        SwordF = true;     // Use sword
        BowF = false;      // Disable bow
        
        SceneManager.LoadScene(2);

    }
    public void Sprint()
    {

        //if (!isSprinting)
        {
          //  speed = 23;

        }

    }

    
    public void Slash()
    {
        if (!isSlashing && SwordF)
        {
            StartCoroutine(Slashh());
             
        }
    }
    IEnumerator Slashh()
    {
        yield return new WaitForSeconds(0.25f);
        
        isSlashing = true;
        
        GameObject swordObj = Instantiate(Sword, Spawnpoint1.position, Spawnpoint1.rotation);
        swordObj.transform.SetParent(Spawnpoint1);

        isSlashing = false;
    }
    public void Shoot()
    {
        if (!isShooting && BowF)
        {
            StartCoroutine(Shoott());
            
        }
    }
    IEnumerator Shoott()
    {
        isShooting = true;

        yield return new WaitForSeconds(0.25f);

        Rigidbody rb = Instantiate(bullet, Spawnpoint2.position, Spawnpoint2.rotation).GetComponent<Rigidbody>();
        rb.AddForce(Spawnpoint2.forward * 44f, ForceMode.Impulse);
        rb.AddForce(Spawnpoint2.up * 2.5f, ForceMode.Impulse);

        isShooting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KillZone")
        {
            health = 0;

        }


        if ((other.tag == "Health") && (health < maxhealth))
        {
            health += 3;    // or health ++; for one
            Destroy(other.gameObject); //or other.gameObject.SetActive(false);

        }
        if (other.tag == "exit")
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
       
        //Level
        if (other.gameObject.tag == "drop1")
        {
            xp += 10f;
            Destroy(other.gameObject);
        }
       
        if (other.gameObject.tag == "drop2")
        {
            xp += 15f;
            Destroy(other.gameObject);
        }
       
        if (other.gameObject.tag == "drop3")
        {
            xp += 20f;
            Destroy(other.gameObject);
        }
       
        if (other.gameObject.tag == "drop4")
        {
            xp += 30f;
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.tag == "drop5")
        {
            xp += 50f;
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.tag == "drop6")
        {
            xp += 100f;
            Destroy(other.gameObject);
        }
        
    }
    private void OnCollisionEnter(Collision collision) //enter is once every collison, stay is constant while collision is true
    {
     //Damage
        if (collision.gameObject.tag == "Hazard")
        {
            health -= 1;
        }
        
        if (collision.gameObject.tag == "Enemy")
        {
            health--;
        }
        
        if (collision.gameObject.tag == "Enemy2")
        {
            health -= 2;
        }
        if (collision.gameObject.tag == "Ink")
        {
            health -= 5;
        }

        if (collision.gameObject.tag == "HomingProj")
        {
            health--;
        }

        if (collision.gameObject.tag == "Enemy3")
        {
            health -= 1;
            
            StartCoroutine(Poison());
            
        }

    

    }
    private void OnCollisionStay(Collision collision) //enter is once every collison, stay is constant while collision is true
    {
        if (collision.gameObject.tag == "Hazard2")
        {
            health--;
        }
        
        

        
    }

    IEnumerator Poison()
    {
        yield return new WaitForSeconds(0.5f);
        
        health -= 3;
        
        yield return new WaitForSeconds(0.5f);
        
        health -= 3;
        
        yield return new WaitForSeconds(0.5f);
        
        health -= 3;
       

    }
}
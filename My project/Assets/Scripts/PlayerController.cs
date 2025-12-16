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
    GameObject winscreen;
    GameObject Selection;
    GameObject SwordPng;
    GameObject BowPng;
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
    public static float xp = 1f; 
    public float damage = 0.5f; 
    public int Kills = 0;
    public int Key = 0;
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
            
            winscreen = GameObject.FindGameObjectWithTag("WIN");

            winscreen.SetActive(false);


            Time.timeScale = 1;


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            deathspeaker = GetComponent<AudioSource>();

            BowPng = GameObject.FindGameObjectWithTag("BowPNG");
            BowPng.SetActive(false);
            if (BowF)
            {
                BowPng.SetActive(true);

            }

            SwordPng = GameObject.FindGameObjectWithTag("SwordPNG");
            SwordPng.SetActive(false);

            if (SwordF)
            {
                SwordPng.SetActive(true);

            }
            
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
            
            maxhealth = 45;
            damage = 1f;
        }
       
        if (level >= 3f)
        {
            
            maxhealth = 70;
            damage = 1.5f;
        }
       
        if (level >= 4f)
        {
            
            maxhealth = 95;
            damage = 2f;
        }
       
        if (level >= 5f)
        {
            
            maxhealth = 120;
            damage = 3f;
        }
       
    //XP  
        if (xp >= 50f)
        {
            level = 2f;
        }
       
        if (xp >= 200f)
        {
            level = 3f;
        }
       
        if (xp >= 600f)
        {
            level = 4f;
        }
       
        if (xp >= 1250f)
        {
            level = 5f;
        }

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        // MOVEMENT (Top-down WASD)
        float moveX = horizontalMove; // A/D
        float moveZ = verticalMove;   // W/S

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        // Apply movement
        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);

        // ROTATION (face movement direction)
        if (move.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // JUMP + INTERACT RAYS
        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

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

        if (!isSprinting)
        {
            isSprinting = true;
            speed = 16;

        }

        else
         {
            isSprinting = false;
            speed = 11;
            
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
        //Damage
        if (other.gameObject.tag == "Hazard")
        {
            health -= 1;
        }

        if (other.gameObject.tag == "Enemy")
        {
            health -= 1;
        }

        if (other.gameObject.tag == "Enemy2")
        {
            health -= 3;
        }
        if (other.gameObject.tag == "Ink")
        {
            health -= 5;
        }

        if (other.gameObject.tag == "HomingProj")
        {
            health--;
        }

        if (other.gameObject.tag == "Enemy3")
        {
            health -= 1;

            StartCoroutine(Poison());

        }


        if (other.tag == "KillZone")
        {
            health = 0;

        }


        if ((other.tag == "health") && (health < maxhealth))
        {
            health += 3;    // or health ++; for one
            Destroy(other.gameObject); //or other.gameObject.SetActive(false);

        }
        
        if (other.tag == "Key")
        {
            Key += 1;    
            Destroy(other.gameObject);

        }
        
        if (other.tag == "Lock")
        {
            if (Key >= 1)
            {
                Key += - 1;
                Destroy(other.gameObject); 
            
            }
           
           

        }
        
        if (other.tag == "Proceed")
        {
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Destroy(gameObject);
            winscreen.SetActive(true);

        }
       
        //Level
        if (other.gameObject.tag == "drop1")
        {
            xp += 10f;
            Destroy(other.gameObject);
        }
       
        if (other.gameObject.tag == "drop2")
        {
            xp += 25f;
            Destroy(other.gameObject);
        }
       
        if (other.gameObject.tag == "drop3")
        {
            xp += 45f;
            Destroy(other.gameObject);
        }
       
        if (other.gameObject.tag == "drop4")
        {
            xp += 80f;
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.tag == "drop5")
        {
            xp += 100f;
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.tag == "drop6")
        {
            xp += 250f;
            Destroy(other.gameObject);
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
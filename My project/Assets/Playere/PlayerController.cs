using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Camera playerCam;
    Rigidbody rb;
    Ray jumpRay;
    Ray interactRay;
    RaycastHit interactHit;
    GameObject gameoverscreen;
    public PlayerInput input;
    float verticalMove;
    float horizontalMove;
    public AudioSource deathspeaker;

    public float speed = 11f;
    public float jumpHeight = 10f;
    public float interactDistance = 1f;
    public float jumpRayDistance = 1.1f;

    public int health = 20;
    public int maxhealth = 20;

    public bool isSprinting = false;
    public bool isSlashing = false;
    public Transform Spawnpoint1;
    public GameObject Sword;

    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 1)
        {
            gameoverscreen = GameObject.FindGameObjectWithTag("ui_gameOver");

            gameoverscreen.SetActive(false);

            Time.timeScale = 1;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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

   
    public void Sprint()
    {

        if (!isSprinting)
        {
            speed = 23;

        }

    }
    public void Slash()
    {
        if (!isSlashing)
        {
            GameObject swordObj = Instantiate(Sword, Spawnpoint1.position, Spawnpoint1.rotation);
            swordObj.transform.SetParent(Spawnpoint1); // <<<<<< IMPORTANT
        }
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

    }
    private void OnCollisionEnter(Collision collision) //enter is once every collison, stay is constant while collision is true
    {
        if (collision.gameObject.tag == "Hazard")
        {
            health -= 1;
        }
        

        if (collision.gameObject.tag == "Enemy")
        {
            health--;
        }
    }
    private void OnCollisionStay(Collision collision) //enter is once every collison, stay is constant while collision is true
    {
        if (collision.gameObject.tag == "Hazard2")
        {
            health--;
        }

        if (collision.gameObject.tag == "Enemy2")
        {
            health--;
        }
    }


}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public int count;
    private float movementX;
    private float movementY;

    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Vector3 targetPos;
    [SerializeField] private bool isMoving = false; 

    // Audio
    public AudioSource pickup;

    // VFX
    public GameObject explosionFX;
    public GameObject pickupFX;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winTextObject.SetActive(false);
        pickup = GetComponent<AudioSource>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 16)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    private void Update() {
        if (Input.GetMouseButton(0)) // Check if left mouse button is held down
        {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
        
        RaycastHit hit; // Define variable to hold raycast hit information

        // Check if raycast hits an object
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {                
        targetPos = hit.point; // Set target position
        isMoving = true; // Start player movement
            }
        }
        else
        {
         isMoving = false; // Stop player movement
        }
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        OutOfBounds();

         if (isMoving)
        {
        // Move the player towards the target position
        Vector3 direction = targetPos - rb.position;
        direction.Normalize();
        rb.AddForce(direction * speed);
        }

        // Stop moving the player if it is close to the target position
        if (Vector3.Distance(rb.position, targetPos) < 0.5f)
        {
        isMoving = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            var currentPickupFX = Instantiate(pickupFX, other.transform.position, Quaternion.identity); // VFX
            Destroy(currentPickupFX, 3);
            pickup.Play(); // play Pickup sound
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var currentExplosionFX = Instantiate(explosionFX, transform.position, Quaternion.identity);
            Destroy(currentExplosionFX, 4);
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            collision.gameObject.GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponentInChildren<Animator>().SetFloat("speed_f", 0);
        }
    }

    public void OutOfBounds()
    {
        if (gameObject.transform.position.y < 0)
        {
            var currentExplosionFX = Instantiate(explosionFX, transform.position, Quaternion.identity);
            Destroy(currentExplosionFX, 4);
            gameObject.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}

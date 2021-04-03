using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private Text pointsGUIText;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D;
    private int coinScore = 0;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpHeight;
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private List<String> colourNameList = new List<string>(new []{"Red", "Green", "Blue", "Yellow"});
    private List<Color> colourList = new List<Color>(new []{Color.red, Color.green, Color.blue, Color.yellow});
    private String colour;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private Transform raycastFiringPoint;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        // Pick Random colour
        int colourIndex = PickRandomColour();
        // Set player colour
        UpdatePlayerColour(colourIndex);
    }

    private int PickRandomColour()
    {
        int colourIndex = Random.Range(0, colourList.Count);
        colour = colourNameList[colourIndex];
        return colourIndex;
    }

    private void UpdatePlayerColour(int colourIndex)
    {
        gameObject.GetComponent<SpriteRenderer>().color = colourList[colourIndex];
    }

    void Update()
    {
        // Send a raycast and if we collide with a collider that has hase tag as us then ignore that collision
        RaycastHit2D raycastHit2D = Physics2D.Raycast(raycastFiringPoint.position, Vector2.up, raycastDistance);
        Collider2D raycastCollider = raycastHit2D.collider;
        // If hit something
        if (raycastCollider != null)
        {
            // If we hit    
            GameObject colliderGameObject = raycastCollider.gameObject;
            if (colliderGameObject.CompareTag(colour))
            {
                Physics2D.IgnoreCollision(colliderGameObject.GetComponent<Collider2D>(), circleCollider2D);
            }
        }
    }
    
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        // Jump
        /*
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0f, jumpForce));    
        */
        rb.velocity = Vector2.up * jumpHeight; 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject collisionGameObject = other.gameObject;
        if (collisionGameObject.CompareTag("ColourSwitch"))
        {
            Physics2D.IgnoreCollision(collisionGameObject.GetComponent<CircleCollider2D>(), circleCollider2D);
            // Get another colour
            int index = PickRandomColour();
            // Update colour
            UpdatePlayerColour(index);
            Destroy(collisionGameObject);
            return;
        }
        else if (collisionGameObject.CompareTag("Coin"))
        {
            Physics2D.IgnoreCollision(collisionGameObject.GetComponent<BoxCollider2D>(), circleCollider2D);
            coinScore++;
            pointsGUIText.text = coinScore.ToString();
            Destroy(collisionGameObject);
            return;
        }
        collisionGameObject = other.contacts[0].collider.gameObject;
        if (!collisionGameObject.CompareTag(colour))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

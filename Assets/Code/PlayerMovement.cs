using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // VariableType VariableName = valueForThatVariable;
    // float speed = 3.0f;

    // Now to expose it in Inspector, you add "public"
    // You can add [Range(2.0f,8.0f)] to make things into a slider! :)
    [Range(2.0f,8.0f)] public float speed = 3.0f;
    public BoxCollider2D feetCollider;
    public bool isGrounded;
    public string deathTag;
    
    // this will not show up in the inspector, because it is private :D
    private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Uses a float to both:
    /// 1. Change the transform.position 2. Change the transform.rotation
    /// This moves the player and adjusts their sprite
    /// </summary>
    /// <param name="horizontalDirection"></param>
    void MoveAndChangeDirection(float horizontalDirection)
    {
        if(Mathf.Approximately(horizontalDirection, 0.0f) == false)
        {
            animator.SetTrigger("walking");
        }

        PhysicsMovement(horizontalDirection);
        PlayerDirection(horizontalDirection);
    }

    void Movement(float horizontalDirection)
    {
        Vector3 movementVector = Vector3.right * Time.deltaTime * speed * horizontalDirection;

        // this.transform.position = this.transform.position + Vector3.left * Time.deltaTime;
        // this.transform.position = this.transform.position + movementVector;
        this.transform.position += movementVector;
    }

    void PhysicsMovement(float horizontalDirection, float extraSpeed = 1.0f)
    {
        float horizontalSpeed = extraSpeed * speed * horizontalDirection;
        
        Vector2 newVelocity = new Vector2(horizontalSpeed,rb.velocity.y);
        
        rb.velocity = newVelocity;
    }

    void PlayerDirection(float horizontalDirection)
    {
        if(horizontalDirection > 0)
        {
            // Euler angles are what you read inside the transform component within a game object
            transform.eulerAngles = Vector3.zero;
        }
        else if(horizontalDirection < 0)
        {
            // transform.eulerAngles = new Vector3(0,180,0);
            transform.eulerAngles = Vector3.up * 180;
        }
    }

    // Update is called once per frame
    // One frame is usually 60x per second
    void Update()
    {
        // Input is a class
        // to get something from inside Input, we do "Input.thing"
        // float horizontalDirection = Input.GetAxis("Horizontal");
        float horizontalDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown("space") && isGrounded)
        {
            isGrounded = false;
            rb.velocity += Vector2.up * 5;
        }
             
        // These two lines are the same operation, "kind of",
        // but the second one is faster, because the method there takes only one parameter
        // transform.RotateAroundLocal(Vector3.forward, Time.deltaTime * 5);         
        // transform.Rotate(Vector3.forward * Time.deltaTime * 5);

        // Commenting because we no longer need to check this
        // Debug.Log("Current horizontal Input = " + horizontalDirection);

        MoveAndChangeDirection(horizontalDirection);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(deathTag))
        {
            Destroy(this.gameObject);
        }
    }
}

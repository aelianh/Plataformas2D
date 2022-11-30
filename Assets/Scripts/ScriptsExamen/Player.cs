using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField]private float horizontal;
    [SerializeField]private float speed = 3;
    [SerializeField]private float jumpForce = 10;
    [SerializeField]private Transform groundSensor;
    [SerializeField]private bool isGrounded;
    [SerializeField]private LayerMask sensorLayer;
    [SerializeField]private float sensorRadius;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if(horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            anim.SetBool("isRunning", true);
        }

        else if(horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            anim.SetBool("isRunning", true);
        }
        else if(horizontal == 0)
        {
            anim.SetBool("isRunning", false);
        }

        isGrounded = Physics2D.OverlapCircle(groundSensor.position, sensorRadius, sensorLayer);
        
        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetBool("isJumpin", true);
        }
    }

    void FixedUpdate()
    {
        rb.velocity  = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.layer == 3)
        {
            anim.SetBool("isJumpin", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Star")
        {
            GameManager.Instance.LoadLevel(1);
        }
        else if(other.gameObject.tag == "Moneda")
        {
            GameManager.Instance.AddCoin(other.gameObject);
        }
    }
}

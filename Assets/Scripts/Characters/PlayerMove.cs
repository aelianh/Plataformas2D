using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float runSpeed = 2f;
    public float jumpForce = 10f;

    private Rigidbody2D body;
    private Animator anim;
    private float dirX;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        if(dirX < 0)
        {
            anim.SetBool("walk", true);
            transform.rotation = Quaternion.Euler(0,180,0);
            
        }
        else if(dirX > 0)
        {
            anim.SetBool("walk", true);
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            anim.SetBool("walk", false); 
        }

        if(Input.GetButtonDown("Jump") && CheckGround.isGrounded)
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetBool("jump", true);
        }

    }

    void FixedUpdate()
    {
        body.velocity = new Vector2(dirX * runSpeed, body.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Grounds")
        {
            anim.SetBool("jump", false);
        }
    }
}

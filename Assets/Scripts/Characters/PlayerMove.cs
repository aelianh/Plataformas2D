using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float runSpeed = 2f;
    public float jumpForce = 10f;
    public bool isJoystickRight;
    public bool isJoystickLeft;
    public bool isJoystickJump;

    private Rigidbody2D body;
    private Animator anim;
    private float dirX;

    private bool isTouch;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isJoystickRight = false;
        isJoystickLeft = false;
        isJoystickJump = false;
        isTouch = false;
    }

    public void JoystickJumpPress()
    {
        isJoystickJump = true;
    }

    public void JoysticJumpRelease()
    {
        isJoystickJump = false;
    }

    public void JoystickRightPress()
    {
        isJoystickRight = true;
    }

    public void JoystickLeftPress()
    {
        isJoystickLeft = true;
    }

    public void JoystickRightRelease()
    {
        isJoystickRight = false;
    }

    public void JoystickLeftRelease()
    {
        isJoystickLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        if (dirX < 0 || isJoystickRight == true)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            anim.SetBool("walk", true);
        } 
        else if (dirX > 0 || isJoystickLeft == true)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);            
            anim.SetBool("walk", true);            
        }
        else
        {
            anim.SetBool("walk", false);            
        }

        if (Input.GetButtonDown("Jump") && CheckGround.isGrounded || isJoystickJump && CheckGround.isGrounded)
        {
            body.velocity = new Vector2(body.velocity.x,jumpForce);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bomb" && isTouch == false)
        {
            isTouch = true;
            SoundManager.instance.PlayBombFX();
            GameManager.instance.LoseLive();
            StartCoroutine("WaitState");
        }
        if (collision.tag == "Stars" && isTouch == false)
        {
            isTouch = true;
            GameManager.instance.AddPoint();
            GameManager.instance.ShowScore();
            StartCoroutine("WaitState");
        }
    }

    private IEnumerator WaitState()
    {
        yield return new WaitForSeconds(0.4f);
        isTouch = false;
    }
}

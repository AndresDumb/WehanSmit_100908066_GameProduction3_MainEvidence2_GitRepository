using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    
    public Camera Cam;
    public Slider JumpSlider;
    public float MovementSpeed;
    public float Maxjump;
    public float JumpIncrement;
    private float currentjump = 0f;
    private Vector2 Movement;
    private float xInput;
    private bool grounded;
    private bool charging;
    

    
    

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
        Cam.transform.position = new Vector3(0f, transform.position.y, Cam.transform.position.z);
        

        if (Input.GetKeyUp(KeyCode.Space) && currentjump > 0f && grounded)
        {
            Jump();
        }
        
    }

    private void FixedUpdate()
    {
        
        SetMovement();
        if (grounded && !charging)
        {
            transform.Translate(Movement);
        }
        
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            
            
            if (currentjump < Maxjump)
            {
                currentjump += JumpIncrement;
            }
            else
            {
                currentjump = Maxjump;
            }

            charging = true;

        }
        
    }

    public void SetMovement()
    {
        xInput = Input.GetAxis("Horizontal");
        Movement = new Vector2((xInput * MovementSpeed) / 100, 0f);
        
    }

    public void Dash()
    {
        
    }

    public void Jump()
    {
        Vector2 JumpVec = new Vector2(Movement.x * 50, currentjump);
        rb.AddForce(JumpVec, ForceMode2D.Impulse);
        currentjump = 0f;
        charging = false;
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            grounded = false;
        }
    }
}

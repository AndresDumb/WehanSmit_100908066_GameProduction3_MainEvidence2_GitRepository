using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string DisplayName = null;
    public Camera  Cam;
    public Slider JumpSlider;
    public float MovementSpeed;
    public float Maxjump;
    public float JumpIncrement;
    private float currentjump = 0f;
    private Vector2 Movement;
    private float xInput;
    private bool grounded;
    private bool charging;
    private bool canDash = true;
    private bool Dashing;
    private float DashPower = 20f;
    private float DashingTime = 0.2f;
    private float DashingCooldown = 1f;
    public TrailRenderer tr;
    

    
    

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        DisplayName = PlayerPrefs.GetString("PlayerName");
        Cam.transform.position = new Vector3(GameObject.FindGameObjectWithTag("CameraCenter").transform.position.x, transform.position.y, Cam.transform.position.z);
        

        if (Input.GetKeyUp(KeyCode.Space) && currentjump > 0f && grounded)
        {
            Jump();
        }

        if (Dashing)
        {
            tr.emitting = true;
        }
        else
        {
            tr.emitting = false;
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Dash();
        }
        
    }

    public void SetMovement()
    {
        xInput = Input.GetAxis("Horizontal");
        Movement = new Vector2((xInput * MovementSpeed) / 100, 0f);
        
    }

    private IEnumerator Dash()
    {
        canDash = false;
        Dashing = true;
        float ORGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(xInput * DashPower, 0f);
        yield return new WaitForSeconds(DashingTime);
        Dashing = false;
        rb.gravityScale = ORGravity;
        yield return new WaitForSeconds(DashingCooldown);
        canDash = true;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cinemachine;
using Mirror;
using Steamworks;
using TMPro;
using Button = UnityEngine.UI.Button;

public class PlayerMovement : NetworkBehaviour
{
    public bool isWalking;
    public UnityEvent JumpSound;
    public UnityEvent StartWalking;
    public UnityEvent StopWalking;
    public UnityEvent StunSound;
    public UnityEvent DashSound;
    [SerializeField] private string DisplayName = null;
    [SerializeField] private Animator animPlayer = null;
    public bool isHost = false;
    public GameObject Player;
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
    private bool Stunned = false;
    private float DashPower = 20f;
    private float DashingTime = 0.2f;
    private float DashingCooldown = 1f;
    public TrailRenderer tr;
    private bool isLocal;
    public string Nametext;
    public TMP_Text Name;
    public UnityEngine.UI.Slider JumpBar;
    
    [SerializeField] public GameObject BackgroundPicture;
    private float stunTimer ;
    public float MaxStun;
    private SpriteRenderer sR;
    public bool Paused = false;
    

    
    

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        sR = GetComponent<SpriteRenderer>();
       GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Players.Add(Player);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isLocalPlayer)
        {
                 isLocal = true;
                 BackgroundPicture.SetActive(true);
        }
        else
        {
                 isLocal = false;
                 return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        if (Paused)
        {
            return;
        }
        

        if (currentjump > 0f)
        {
            JumpBar.gameObject.SetActive(true);
        }
        else
        {
            JumpBar.gameObject.SetActive(false);
        }
        JumpBar.value = currentjump / Maxjump;
        Nametext = SteamUser.GetSteamID().ToString();
        Name.text = Nametext;
        Cam.transform.position = new Vector3(GameObject.FindGameObjectWithTag("CameraCenter").transform.position.x, transform.position.y, Cam.transform.position.z);
        
        
        if (Stunned)
        {
            stunTimer += Time.deltaTime;
            if (stunTimer >= MaxStun)
            {
                Stunned = false;
            }
            return;
        }
        else
        {
            stunTimer = 0f;
        }
        if (Input.GetKeyUp(KeyCode.Space) && currentjump > 0f && grounded)
        {
            Jump();
        }

        if (xInput < 0f)
        {
            sR.flipX = true;
        }
        else
        {
           sR.flipX = false;
        }

        if (Paused && Input.GetKeyDown(KeyCode.Escape))
        {
            NetworkClient.Disconnect();
            
        }

        
        
    }

    private void Stun()
    {
        Stunned = true;
        StunSound.Invoke();
    }

    private void FixedUpdate()
    {
        if (!isLocal)
        {
                 return;
        }
        if (Paused)
        {
            return;
        }
        

        if (Stunned)
        {
            currentjump = 0f;
            return;
        }
        SetMovement();
        if (grounded && !charging)
        {
            animPlayer.SetBool("isJumping", false);
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
        if (xInput != 0f)
        {
            animPlayer.SetBool("isWalking",true);
            animPlayer.SetBool("isJumping", false);
            if (!isWalking)
            {
                StartWalking.Invoke();
            }
            isWalking = true;
        }
        else
        {
            if (isWalking)
            {
                StopWalking.Invoke();
            }
            animPlayer.SetBool("isWalking", false);
            isWalking = false;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        Dashing = true;
        animPlayer.SetBool("isWalking", false);
        animPlayer.SetBool("isJumping", false);
        animPlayer.SetBool("isDashing", true);
        tr.emitting = true;
        float ORGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        DashSound.Invoke();
        rb.velocity = new Vector2(xInput * DashPower, 0f);
        yield return new WaitForSeconds(DashingTime);
        Dashing = false;
        animPlayer.SetBool("isDashing", false);
        tr.emitting = false;
        rb.gravityScale = ORGravity;
        yield return new WaitForSeconds(DashingCooldown);
        canDash = true;
    }

    public void Jump()
    {
        Vector2 JumpVec = new Vector2(Movement.x * 50, currentjump);
        JumpSound.Invoke();
        rb.AddForce(JumpVec, ForceMode2D.Impulse);
        currentjump = 0f;
        charging = false;
        animPlayer.SetBool("isJumping", true);
        
    }

    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            grounded = true;
        }

        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerMovement>().Dashing)
        {
            Stun();
        }
        

        if (other.gameObject.CompareTag("Damage"))
        {
            rb.AddForce(new Vector2(-Movement.x, 1f), ForceMode2D.Impulse);
            Stun();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    protected InputMaster controls;
    protected Animator anim;
    protected SpriteRenderer spr;
    protected Rigidbody2D rb2D;
    

    [SerializeField]
    protected ContactFilter2D groundFilter;

    Vector2 move;

    [SerializeField, Range(0.1f, 10f)]
    float moveSpeed;

    [SerializeField, Range(0.1f, 10f)]
    float sprintSpeed;

    [SerializeField, Range(0.1f, 15f)]
    float jumpForce;

   public InputAction movimiento;
   public CharacterController controller;

   public bool isSprinting;

    void Awake() {
        controls = new InputMaster();

        controls.PlayerControls.Move.performed += _ => move = _.ReadValue<Vector2>();
        controls.PlayerControls.Move.canceled += _ => move = Vector2.zero;

        controls.PlayerControls.SprintStart.performed += _ => SprintPressed();
        controls.PlayerControls.SprintFinish.performed += _ => SprintReleased();

        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void SprintPressed()
    {
        isSprinting = true;
    }

    private void SprintReleased()
    {
        isSprinting = false;
    }

    void OnEnable() {
        controls.PlayerControls.Enable();
    }

     void OnDisable() {
        controls.PlayerControls.Disable();
    }

    void Start()
    {
        controls.PlayerControls.Jump.performed += _ => Jump();
       controller = GetComponent<CharacterController>();
    }

    void Jump(){
        if(IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }

    

    void Update()
    { 
        if(isSprinting)
        {
            transform.Translate(Vector2.right * axis.x * sprintSpeed * Time.deltaTime);
            anim.SetFloat("walk", Mathf.Abs(axis.x+axis.x));
        }
        else
        {
            transform.Translate(Vector2.right * axis.x * moveSpeed * Time.deltaTime);
            anim.SetFloat("walk", Mathf.Abs(axis.x));
        }
        
    }  

    
    void LateUpdate() {
        spr.flipX = flipSprite;
    }

    void FixedUpdate() {
        anim.SetFloat("jumpAnim", rb2D.velocity.y);
    }

    Vector2 axis => controls.PlayerControls.Move.ReadValue<Vector2>();

    bool flipSprite => axis.x > 0 ? false : axis.x < 0 ? true : spr.flipX;

    bool IsGrounding => rb2D.IsTouching(groundFilter);

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Can")){
            Can can = other.GetComponent<Can>();
            GameManager.instance.GetScore.AddPoints(can.Points);
            Destroy(other.gameObject);
            //Debug.Log(can.Points);
        }
        if(other.gameObject.tag == "Win")
        {
            GameManager.instance.GetScore.ResetPoints();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            GameManager.instance.GetScore.ResetPoints();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        


    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField, Range(0.1f, 15f)]
    float jumpForce;

   public InputAction movimiento;
   public CharacterController controller;

    void Awake() {
        controls = new InputMaster();

        controls.PlayerControls.Move.performed += _ => move = _.ReadValue<Vector2>();
        controls.PlayerControls.Move.canceled += _ => move = Vector2.zero;

        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
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
        transform.Translate(Vector2.right * axis.x * moveSpeed * Time.deltaTime);
        anim.SetFloat("walk", Mathf.Abs(axis.x));
        
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
}

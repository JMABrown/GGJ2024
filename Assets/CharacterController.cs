using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public CharacterBody CharacterBody;
    public Animator anim;
    private Rigidbody rb;
    private Camera camera;

    private GameObject lookVector;

    private Coroutine StandUpRoutine;

    private void Awake()
    {
        DamageManager.OnDamageReported.AddListener(HandleDamageUpdate);
        camera = Camera.main;

        lookVector = new GameObject("Look Vector");
    }
    
    private void HandleDamageUpdate(DamageInfo damage)
    {
        //return;
        rb.AddForce(damage.Impulse, ForceMode.Impulse);
        CharacterBody.GoRagdoll();
        if (StandUpRoutine != null)
        {
            StopCoroutine(StandUpRoutine);
        }
        StandUpRoutine = StartCoroutine(GetBackUpRoutine());
    }

    private IEnumerator GetBackUpRoutine()
    {
        yield return new WaitForSeconds(3f);
        var newPosition = CharacterBody.Hips.transform.position;
        newPosition = new Vector3(newPosition.x, newPosition.y + 0.2f, newPosition.z);
        transform.rotation = Quaternion.identity;
        CharacterBody.GoAnimated();
        gameObject.transform.position = newPosition;
        CharacterBody.ResetPosition();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move();
        
        var lookDirection = camera.gameObject.transform.forward;

        lookDirection = new Vector3(lookDirection.x, 0f, lookDirection.z).normalized;
        
        float verticalAxis = Input.GetAxis("Vertical");
        //float horizontalAxis = Input.GetAxis("Horizontal");

        lookVector.transform.forward = lookDirection;
        
        if (animator != null && animator.enabled)
        {
            float curSpeed = animator.GetFloat("Speed");
            curSpeed = Mathf.Lerp(curSpeed, Mathf.Clamp01(verticalAxis), acceleration * Time.deltaTime);
            rigidBody.MoveRotation(Quaternion.Slerp(rigidBody.rotation, Quaternion.LookRotation(lookDirection, Vector3.up), rotationSpeed * Time.time * Mathf.Abs(verticalAxis)));
            animator.SetFloat("Speed", curSpeed);
        }
        
        Jump();
    }

    private void Move()
    {
        var lookDirection = camera.gameObject.transform.forward;

        lookDirection = new Vector3(lookDirection.x, 0f, lookDirection.z).normalized;
        
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        lookVector.transform.forward = lookDirection;

        Vector3 movement = lookVector.transform.forward * verticalAxis + lookVector.transform.right * horizontalAxis;

        //Vector3 movement = new Vector3(lookDirection.x * horizontalAxis, 0f, lookDirection.z * verticalAxis);
        //movement.Normalize();

        rb.AddForce(movement * (1000f * Time.deltaTime), ForceMode.Force);
        
        //this.transform.position += movement * 0.15f;

        //this.anim.SetFloat("vertical", verticalAxis);
        //this.anim.SetFloat("horizontal", horizontalAxis);
    }
    
    [SerializeField] private Animator animator = null;
    [SerializeField] private Rigidbody rigidBody = null;
    [SerializeField] private float acceleration = 5;
    [SerializeField] private float rotationScale = 90;
    [SerializeField] private float rotationSpeed = 5;
    private void OnAnimatorMove()
    {
        if (animator != null)
        {
            var velocity = animator.velocity;
            velocity.y = rigidBody.velocity.y;
            rigidBody.velocity = velocity;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
            //this.anim.SetBool("jump", true);
        }
        else
        {
            //this.anim.SetBool("jump", false);
        }
    }
}

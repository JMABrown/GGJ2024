using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public CharacterBody CharacterBody;
    public Animator anim;
    private Rigidbody rb;

    private void Awake()
    {
        DamageManager.OnDamageReported.AddListener(HandleDamageUpdate);
    }
    
    private void HandleDamageUpdate(float damage)
    {
        CharacterBody.GoRagdoll();
        StartCoroutine(GetBackUpRoutine());
    }

    private IEnumerator GetBackUpRoutine()
    {
        yield return new WaitForSeconds(3f);
        transform.rotation = Quaternion.identity;
        CharacterBody.GoAnimated();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        Vector3 movement = this.transform.forward * verticalAxis + this.transform.right * horizontalAxis;
        //movement.Normalize();

        this.transform.position += movement * 0.15f;

        //this.anim.SetFloat("vertical", verticalAxis);
        //this.anim.SetFloat("horizontal", horizontalAxis);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rb.AddForce(Vector3.up * 3 * Time.deltaTime, ForceMode.Impulse);
            //this.anim.SetBool("jump", true);
        }
        else
        {
            //this.anim.SetBool("jump", false);
        }
    }
}

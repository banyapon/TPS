using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;
    float rotationSpeed = 30;
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    Vector3 inputVec;
    Vector3 targetDirection;
    private Vector3 moveDirection = Vector3.zero;

    void Start(){
            Time.timeScale = 1;
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxisRaw("Horizontal");
        float x = -(Input.GetAxisRaw("Vertical"));
        inputVec = new Vector3(x, 0, z);

        animator.SetFloat("Input X", z);
        animator.SetFloat("Input Z", -(x));

        if (x != 0 || z != 0 )
        {
            animator.SetBool("Moving", true);
            animator.SetBool("Running", true);
        }else{
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
        //Gun
        StartCoroutine (COStunPause(.6f));
        }
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f,         
                                        Input.GetAxis("Vertical"));
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
            {
            moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
        UpdateMovement();
    }
    void UpdateMovement()
    {
        Vector3 motion = inputVec;
        motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) 
               == 1)?.7f:1;

        RotateTowardMovementDirection();
        GetCameraRelativeMovement();
    }

    public IEnumerator COStunPause(float pauseTime)
    {
        yield return new WaitForSeconds(pauseTime);
    }
    void RotateTowardMovementDirection()
    {
        if (inputVec != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                 Quaternion.LookRotation(targetDirection), 
                                 Time.deltaTime * rotationSpeed);
        }
    }
    void GetCameraRelativeMovement()
    {
        Transform cameraTransform = Camera.main.transform;

        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right= new Vector3(forward.z, 0, -forward.x);
        float v= Input.GetAxisRaw("Vertical");
        float h= Input.GetAxisRaw("Horizontal");

        targetDirection = h * right + v * forward;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public VariableJoystick joystick;
    private float horizontal = 5f;
    public float moveSpeed = 5f;
    public float moveSpeedLR = 2f;
    public float jumpForce = 3f;


    public Rigidbody rb;
    private int coinCount;
    public Animator animator;


    public int plankCount;
    public GameObject plank;
    public GameObject holdingPlank;
    public GameObject planksParent;
    public Transform plankPoint;
    public List<GameObject> planksHolding;
    public Transform allPlankParent;



    RaycastHit hit;



    bool run;
    bool jump = false;
    bool playerController;
    bool playerMoveToPosition;
    bool rankDecided;
    bool pathCreation;
    bool hitOtherNumber;
    private float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = .5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        StartRuning();
        if (Input.GetMouseButtonDown(0) && !playerController)
        {
            run = true;
            playerController = true;
            pathCreation = true;
            Run();
        }
    }

    private void StartRuning()
    {
        if (run)
        {
            MoveForward();
            if (jump)
            {
                // Debug.Log("StarrRunning");
                if (plankCount == 0)
                {
                    Run();
                }

            }
        }
    }

    private void Run()
    {
        animator.SetBool("Run", true);
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {


        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > .01f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.down), out hit, 0.4f) && pathCreation)
        {
            if (hit.transform.CompareTag("Water") && plankCount <= 0)
            {
                return;
            }

            if (!jump)
            {
                jump = true;
                rb.velocity = Vector3.zero;
            }
           // Debug.DrawRay(this.transform.position, transform.TransformDirection(Vector3.down), Color.red, hit.distance);
        }
        else
        {
            if (jump)
            {

                if (plankCount > 0)
                {

                    Vector3 playerPos = new Vector3(this.transform.position.x, this.transform.position.y - 0.05f, this.transform.position.z);
                    GameObject bridgePlank = Instantiate(plank, playerPos, this.transform.rotation);
                    bridgePlank.transform.parent = allPlankParent;
                    plankCount--;
                    planksHolding.Remove(planksParent.gameObject.transform.GetChild(plankCount).gameObject);
                    Destroy(planksParent.gameObject.transform.GetChild(plankCount).gameObject);
                    plankPoint.transform.position = new Vector3(plankPoint.transform.position.x, plankPoint.transform.position.y - 0.081f, plankPoint.transform.position.z);
                }
                else
                {
                    Jump();
                    jump = false;
                }
            }
        }

    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            plankCount++;
            other.gameObject.SetActive(false);
            other.transform.parent.GetComponent<PlankGroup>().childDeActiveCount++;

            GameObject addPlankToPlayerStack = Instantiate(holdingPlank, plankPoint.transform.position, plankPoint.transform.rotation);
            addPlankToPlayerStack.transform.parent = planksParent.transform;

            plankPoint.transform.position = new Vector3(plankPoint.transform.position.x, plankPoint.transform.position.y + 0.081f, plankPoint.transform.position.z);
            planksHolding.Add(addPlankToPlayerStack);
        }
        if (other.gameObject.CompareTag("Destoy"))
        {
            UiManager.Instance.LevelFailed();
        }

        if (other.gameObject.CompareTag("Win"))
        {
            run = false;
            other.gameObject.SetActive(false);
            planksParent.SetActive(false);
            animator.SetTrigger("Win");
            StartCoroutine(LevelWin());
            
        }

    }
    IEnumerator LevelWin()
    {
        yield return new WaitForSeconds(4f);
        UiManager.Instance.LevelComplete();
    }



}



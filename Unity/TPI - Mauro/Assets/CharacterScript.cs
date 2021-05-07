using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    Animator animator;
    Transform transform;
    float runningTime = 0.25f;
    float runningTimer = 0;
    bool isRunning = false;
    float verticalInput = 0;
    float horizontalInput = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        transform = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            transform.position = new Vector3(transform.position.x + verticalInput * Time.deltaTime / runningTime, transform.position.y , transform.position.z + horizontalInput*Time.deltaTime/runningTime);
        }
        else
        {
            float verticalAxis = Input.GetAxis("Vertical"), horizontalaxis = Input.GetAxis("Horizontal");
            if(Mathf.Abs(verticalAxis) > Mathf.Abs(horizontalaxis) & Mathf.Abs(verticalAxis) >0.2)
            {
                if (verticalAxis > 0)
                {
                    verticalInput = 1;
                    transform.eulerAngles=new Vector3(transform.eulerAngles.x,90,transform.eulerAngles.z);
                }
                else
                {
                    verticalInput = -1;
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270, transform.eulerAngles.z);
                }
                horizontalInput = 0;
                animator.SetTrigger("IsRunning");
                isRunning = true;
                runningTimer = 0;
            }
            if (Mathf.Abs(verticalAxis) < Mathf.Abs(horizontalaxis) & Mathf.Abs(horizontalaxis) > 0.2)
            {
                if (horizontalaxis > 0)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x,  180, transform.eulerAngles.z);
                    horizontalInput = -1;
                }
                else
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                    horizontalInput = 1;
                }
                verticalInput = 0;  
                animator.SetTrigger("IsRunning");
                isRunning = true;
                runningTimer = 0;
            }
            
        }
        if(isRunning & runningTime<runningTimer)
        {
            
            isRunning = false;
        }
        runningTimer += Time.deltaTime;
    }
}

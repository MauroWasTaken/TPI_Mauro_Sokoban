using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    char lastmove;
    Animator animator;
    float runningTime = 0.35f;
    [SerializeField]
    float runningTimer = 0;
    bool isRunning = false;
    float verticalInput = 0;
    float horizontalInput = 0;
    [SerializeField]
    List<GameObject> colliders = new List<GameObject>();
    Collider northCollision, southCollision, westCollision, eastCollision;
    public char Lastmove { get => lastmove; }
    GameManagerScript gameManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindObjectOfType<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Mouvement();
        runningTimer += Time.deltaTime;
    }
    void Mouvement()
    {
        if (isRunning)
        {
            transform.position = new Vector3(transform.position.x + verticalInput * Time.deltaTime / runningTime, transform.position.y, transform.position.z + horizontalInput * Time.deltaTime / runningTime);
            if (runningTimer > runningTime * 0.85)
            {
                SetColliders(true);
            }

            if (runningTime < runningTimer)
            {
                transform.position = new Vector3(Mathf.Round(transform.position.x - 0.5f) + 0.5f, transform.position.y, Mathf.Round(transform.position.z / 0.5f) * 0.5f);
                isRunning = false;
                SetColliders(true);
            }
        }
        else if (runningTime < runningTimer)
        {
            float verticalAxis = Input.GetAxis("Vertical"), horizontalaxis = Input.GetAxis("Horizontal");
            if (Mathf.Abs(verticalAxis) > Mathf.Abs(horizontalaxis) & Mathf.Abs(verticalAxis) > 0.2)
            {
                if (verticalAxis > 0)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
                    if (northCollision == null)
                    {
                        verticalInput = 1;
                        horizontalInput = 0;
                        validMove();
                        lastmove = 'N';
                    }
                    else if (northCollision.gameObject.layer == 7)
                    {
                        if (northCollision.gameObject.GetComponent<BoxScript>().Push("North"))
                        {
                            verticalInput = 1;
                            horizontalInput = 0;
                            validMove();
                            lastmove = 'N';
                        }
                    }

                }
                else
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270, transform.eulerAngles.z);
                    if (southCollision == null)
                    {
                        verticalInput = -1;
                        horizontalInput = 0;
                        validMove();
                        lastmove = 'S';
                    }
                    else if (southCollision.gameObject.layer == 7)
                    {
                        if (southCollision.gameObject.GetComponent<BoxScript>().Push("South"))
                        {
                            verticalInput = -1;
                            horizontalInput = 0;
                            validMove();
                            lastmove = 'S';
                        }
                    }
                }
            }
            if (Mathf.Abs(verticalAxis) < Mathf.Abs(horizontalaxis) & Mathf.Abs(horizontalaxis) > 0.2)
            {
                if (horizontalaxis > 0)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
                    if (eastCollision == null)
                    {
                        horizontalInput = -1;
                        verticalInput = 0;
                        validMove();
                        lastmove = 'E';
                    }
                    else if (eastCollision.gameObject.layer == 7)
                    {
                        if (eastCollision.gameObject.GetComponent<BoxScript>().Push("East"))
                        {
                            horizontalInput = -1;
                            verticalInput = 0;
                            validMove();
                            lastmove = 'E';
                        }
                    }
                }
                else
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                    if (westCollision == null)
                    {
                        horizontalInput = 1;
                        verticalInput = 0;
                        validMove();
                        lastmove = 'W';
                    }
                    else if (westCollision.gameObject.layer == 7)
                    {
                        if (westCollision.gameObject.GetComponent<BoxScript>().Push("West"))
                        {
                            horizontalInput = 1;
                            verticalInput = 0;
                            validMove();
                            lastmove = 'W';
                        }
                    }
                }
                verticalInput = 0;
            }
        }
    }
    void validMove()
    {
        SetColliders(false);
        animator.SetTrigger("IsRunning");
        isRunning = true;
        runningTimer = 0;
        gameManager.PlaySound(0);
        gameManager.AddMoveCounter();
    }
    public void Move(string direction)
    {
        switch (direction)
        {
            case "North":
                verticalInput = 1;
                horizontalInput = 0;
                validMove();
                lastmove = 'N';
                break;
            case "South":
                verticalInput = -1;
                horizontalInput = 0;
                validMove();
                lastmove = 'S';
                break;
            case "West":
                horizontalInput = 1;
                verticalInput = 0;
                validMove();
                lastmove = 'W';
                break;
            case "East":
                horizontalInput = -1;
                verticalInput = 0;
                validMove();
                lastmove = 'E';
                break;
        }

    }
    void SetColliders(bool status)
    {
        foreach (GameObject collider in colliders)
        {
            collider.SetActive(status);
        }
        if (!status)
        {
            northCollision = null;
            southCollision = null;
            westCollision = null;
            eastCollision = null;
        }
    }
    public void AddCollision(string side, Collider item)
    {

        float rotation = 0;
        switch (side)
        {
            case "Left":
                rotation = 270;
                break;
            case "Right":
                rotation = 90;
                break;
            case "Front":
                rotation = 0;
                break;
            case "Back":
                rotation = 180;
                break;

        }
        rotation = Mathf.RoundToInt((rotation + transform.localRotation.eulerAngles.y) / 90) * 90;
        switch (rotation % 360)
        {
            case 270:
                southCollision = item;
                break;
            case 90:
                northCollision = item;
                break;
            case 0:
                westCollision = item;
                break;
            case 180:
                eastCollision = item;
                break;
        }

    }
}

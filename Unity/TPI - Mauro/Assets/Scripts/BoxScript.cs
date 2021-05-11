using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    float movingTime = 0.20f;
    float movingTimer = 0;
    bool isMoving = false;
    float verticalInput = 0;
    float horizontalInput = 0;
    [SerializeField]
    List<GameObject> colliders = new List<GameObject>();
    int northCollision=0, southCollision=0, westCollision=0, eastCollision=0;
    void Update()
    {
        Mouvement();
    }
    void Mouvement()
    {
        if (isMoving)
        {
            transform.position = new Vector3(transform.position.x + verticalInput * Time.deltaTime / movingTime, transform.position.y, transform.position.z + horizontalInput * Time.deltaTime / movingTime);

        }
        if (isMoving & movingTime < movingTimer)
        {

            transform.position = new Vector3(Mathf.Round(transform.position.x - 0.5f) + 0.5f, transform.position.y, Mathf.Round(transform.position.z / 0.5f) * 0.5f);
            isMoving = false;
            SetColliders(true);
        }
        movingTimer += Time.deltaTime;
    }
    void SetColliders(bool status)
    {
        foreach (GameObject collider in colliders)
        {
            collider.SetActive(status);
        }
        if (!status)
        {
            northCollision = 0;
            southCollision = 0;
            westCollision = 0;
            eastCollision = 0;
        }
    }
    public bool Push(string direction)
    {
        switch (direction)
        {
            case "North":
                if (northCollision == 0)
                {
                    verticalInput = 1;
                    horizontalInput = 0;
                    isMoving = true;
                    movingTimer = 0;
                    SetColliders(false);
                }
                break;
            case "South":
                if (southCollision == 0)
                {
                    verticalInput = -1;
                    horizontalInput = 0;
                    isMoving = true;
                    movingTimer = 0;
                    SetColliders(false);
                }
                break;
            case "West":
                if (westCollision == 0)
                {
                    verticalInput = 0;
                    horizontalInput = 1;
                    isMoving = true;
                    movingTimer = 0;
                    SetColliders(false);
                }
                break;
            case "East":
                if (eastCollision == 0)
                {
                    verticalInput = 0;
                    horizontalInput = -1;
                    isMoving = true;
                    movingTimer = 0;
                    SetColliders(false);
                }
                break;
        }
        return isMoving;
    }
    public void AddCollision(string side, int item)
    {
        switch (side)
        {
            case "North":
                northCollision = item;
                break;
            case "South":
                southCollision = item;
                break;
            case "West":
                westCollision = item;
                break;
            case "East":
                eastCollision = item;
                break;
        }
        Debug.Log(this.gameObject.name+" - " +side+ " - "+item);
    }
}

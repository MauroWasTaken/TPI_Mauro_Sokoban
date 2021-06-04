using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    GameManagerScript gameManager;
    char lastmove;
    float movingTime = 0.2f;
    float movingTimer = 0;
    bool isMoving = false;
    float verticalInput = 0;
    float horizontalInput = 0;
    bool onspot = false;
    [SerializeField]
    List<GameObject> colliders = new List<GameObject>();
    [SerializeField]
    List<Material> materials = new List<Material>();
    int northCollision = 0, southCollision = 0, westCollision = 0, eastCollision = 0, upCollision=0;
    int UpCollision
    {
        get { return upCollision; }
        set
        {
            if (value == 9)
            {
                this.gameObject.GetComponent<MeshRenderer>().material = materials[1];
                gameManager.BoxOnSpot(1);
                onspot = true;
            }
            else
            {
                if (onspot)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material = materials[0];
                    onspot = false;
                    gameManager.BoxOnSpot(-1);
                }
            }
            upCollision = value;
        }
    }

    public char Lastmove { get => lastmove; set => lastmove = value; }
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManagerScript>();
    }
    void Update()
    {
        Mouvement();
    }
    /// <summary>
    /// script qui gere le deplacement de la boite
    /// </summary>
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
    /// <summary>
    /// reset l'etat des colliders apres un déplacement
    /// </summary>
    /// <param name="status">true-active les colliders / false les desactive</param>
    void SetColliders(bool status)
    {
        foreach (GameObject collider in colliders)
        {
            collider.SetActive(status);
        }
        northCollision = 0;
        southCollision = 0;
        westCollision = 0;
        eastCollision = 0;
        UpCollision = 0;

    }
    /// <summary>
    /// déplace la boite en regardant les differentes collisions
    /// </summary>
    /// <param name="direction">direction du déplacement de la boite</param>
    /// <returns>retourne true si c'est une déplacement valable</returns>
    public bool Push(string direction)
    {
        switch (direction)
        {
            case "North":
                if (northCollision == 0 | northCollision == 10)
                {
                    verticalInput = 1;
                    horizontalInput = 0;
                    isMoving = true;
                    movingTimer = 0;
                    SetColliders(false);
                    Lastmove = 'N';
                }
                break;
            case "South":
                if (southCollision == 0 | southCollision == 10)
                {
                    verticalInput = -1;
                    horizontalInput = 0;
                    isMoving = true;
                    movingTimer = 0;
                    SetColliders(false);
                    Lastmove = 'S';
                }
                break;
            case "West":
                if (westCollision == 0 | westCollision == 10)
                {
                    verticalInput = 0;
                    horizontalInput = 1;
                    isMoving = true;
                    movingTimer = 0;
                    SetColliders(false);
                    Lastmove = 'W';
                }
                break;
            case "East":
                if (eastCollision == 0 | eastCollision == 10)
                {
                    verticalInput = 0;
                    horizontalInput = -1;
                    isMoving = true;
                    movingTimer = 0;
                    SetColliders(false);
                    Lastmove = 'E';
                }
                break;
        }
        if (isMoving) gameManager.PlaySound(1);
        return isMoving;
    }
    /// <summary>
    /// permets de pousser une boite sans regarder les collision (implementer à cause d'un bug avec les téléporteurs)
    /// </summary>
    /// <param name="direction">direction du déplacement de la boite</param>
    /// <returns>retourne true si c'est une direction juste</returns>
    public bool ForcePush(string direction)
    {
        switch (direction)
        {
            case "North":
                verticalInput = 1;
                horizontalInput = 0;
                isMoving = true;
                movingTimer = 0;
                SetColliders(false);
                Lastmove = 'N';
                break;
            case "South":

                verticalInput = -1;
                horizontalInput = 0;
                isMoving = true;
                movingTimer = 0;
                SetColliders(false);
                Lastmove = 'S';
                break;
            case "West":

                verticalInput = 0;
                horizontalInput = 1;
                isMoving = true;
                movingTimer = 0;
                SetColliders(false);
                Lastmove = 'W';
                break;
            case "East":

                verticalInput = 0;
                horizontalInput = -1;
                isMoving = true;
                movingTimer = 0;
                SetColliders(false);
                Lastmove = 'E';

                break;
        }
        if (isMoving) gameManager.PlaySound(1);
        return isMoving;
    }
    /// <summary>
     /// fonction appelée quand un item entre dans un collider
     /// </summary>
     /// <param name="side">cote de la collision</param>
     /// <param name="item">item qui a fait collision</param>
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
            case "Up":
                if(item == 9)
                {
                    UpCollision = item;
                }
                break;
        }
    }
    /// <summary>
    /// fonction appelée quand un item sors dans un collider
    /// </summary>
    /// <param name="side">cote de la collision</param>
    public void RemoveCollision(string side)
    {
        switch (side)
        {
            case "North":
                northCollision = 0;
                break;
            case "South":
                southCollision = 0;
                break;
            case "West":
                westCollision = 0;
                break;
            case "East":
                eastCollision = 0;
                break;
        }
    }
}

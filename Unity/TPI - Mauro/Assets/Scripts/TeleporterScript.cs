using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    [SerializeField]
    TeleporterScript destination;
    float cooldownTime = 0.3f;
    //TODO make accessor
    public float cooldownTimer= 0.3f;
    Collider northCollision= null, southCollision = null, westCollision = null, eastCollision = null;
    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Up")| other.gameObject.name.Contains("character"))
        {
            if (cooldownTime <= destination.cooldownTimer)
            {
                Teleport(other.gameObject);
                cooldownTimer = 0;
            }
        }

    }
    private bool Teleport(GameObject item)
    {
        char direction;
        if (item.layer == 6)
        {
            direction = item.GetComponent<CharacterScript>().Lastmove;
        }
        else if (item.layer == 7)
        {
            item = item.transform.parent.gameObject;
            direction = item.GetComponent<BoxScript>().Lastmove;
        }
        else
        {
            return false;
        }
        string teleportAvailable = "";
        switch (direction)
        {
            case 'N':
                if (destination.northCollision == null)
                {
                    teleportAvailable = "North";
                }
                else
                {
                    if (destination.northCollision.gameObject.layer == 7)
                    {
                        if (item.layer == 6)
                        {
                            if (destination.northCollision.gameObject.GetComponent<BoxScript>().Push("North"))
                            {
                                teleportAvailable = "North";
                            }
                        }
                    }
                }
                break;
            case 'S':
                if (destination.southCollision==null)
                {
                    teleportAvailable = "South";
                }
                else
                {
                    if (destination.southCollision.gameObject.layer == 7)
                    {
                        if (item.layer == 6)
                        {
                            if (destination.southCollision.gameObject.GetComponent<BoxScript>().Push("South"))
                            {
                                teleportAvailable = "South";
                            }
                        }
                    }
                }
                break;
            case 'W':
                if (destination.westCollision==null)
                {
                    teleportAvailable = "West";
                }
                else
                {
                    if (destination.westCollision.gameObject.layer == 7)
                    {
                        if (item.layer == 6)
                        {
                            if (destination.westCollision.gameObject.GetComponent<BoxScript>().Push("West"))
                            {
                                teleportAvailable = "West";
                            }
                        }
                    }
                }
                break;
            case 'E':
                if (destination.eastCollision==null)
                {
                    teleportAvailable = "East";
                }
                else
                {
                    if (destination.eastCollision.gameObject.layer == 7)
                    {
                        if (item.layer == 6)
                        {
                            if (destination.eastCollision.gameObject.GetComponent<BoxScript>().Push("East"))
                            {
                                teleportAvailable = "East";
                            }
                        }
                    }
                }
                break;
        }
        if (teleportAvailable!="")
        {
            item.transform.position = destination.gameObject.transform.position + new Vector3(0, -0.5f, -0.3f);
            if (item.layer == 6)
            {
                item.GetComponent<CharacterScript>().Move(teleportAvailable);
            }
            else
            {
                item.GetComponent<BoxScript>().ForcePush(teleportAvailable);
            }
        }
        else
        {
            string invertedDirection = InvertMouvement(GameObject.Find("character(Clone)").GetComponent<CharacterScript>().Lastmove);
            if (item.layer == 7)
            {
                item.GetComponent<BoxScript>().ForcePush(invertedDirection);
                
            }
            GameObject.Find("character(Clone)").GetComponent<CharacterScript>().Move(invertedDirection);
            return false;
        }
        return true;
    }
    string InvertMouvement(char direction)
    {
        switch (direction)
        {
            case 'N':
                return "South";
            case 'S':
                return "North";
            case 'W':
                return "East";
            case 'E':
                return "West";
        }
        return "";
    }
    public void AddCollision(string side, Collider item)
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
    }
    public void RemoveCollision(string side)
    {
        switch (side)
        {
            case "North":
                northCollision = null;
                break;
            case "South":
                southCollision = null;
                break;
            case "West":
                westCollision = null;
                break;
            case "East":
                eastCollision = null;
                break;
        }
    }
    public void LinkToTeleporter(GameObject teleporter)
    {
        if (teleporter.layer == 10)
        {
            destination = teleporter.GetComponent<TeleporterScript>();
        }
    }
}

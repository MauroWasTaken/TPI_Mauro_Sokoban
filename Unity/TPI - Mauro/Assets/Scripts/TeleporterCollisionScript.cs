using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterCollisionScript : MonoBehaviour
{
    [SerializeField]
    TeleporterScript teleporter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer >= 6)
        {
            if (other.gameObject.name.Contains("Box") | other.gameObject.name.Contains("Wall") | other.gameObject.name.Contains("character"))
            {
                teleporter.AddCollision(this.gameObject.name, other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer >= 6)
        {
            if (other.gameObject.name.Contains("Box") | other.gameObject.name.Contains("Wall") | other.gameObject.name.Contains("character"))
            {
                teleporter.RemoveCollision(this.gameObject.name);
            }

        }
    }
}
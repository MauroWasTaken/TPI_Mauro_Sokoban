using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterCollisionScript : MonoBehaviour
{
    [SerializeField]
    TeleporterScript teleporter;
    /// <summary>
    /// fonction appelée quand un item entre dans un collider
    /// </summary>
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
    /// <summary>
    /// fonction appelée quand un item sors dans un collider
    /// </summary>
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
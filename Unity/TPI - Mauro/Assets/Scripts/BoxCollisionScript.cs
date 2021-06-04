using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollisionScript : MonoBehaviour
{
    [SerializeField]
    BoxScript box;
    /// <summary>
    /// fonction appelée quand un item entre dans un collider
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer >= 7)
        {
            box.AddCollision(this.gameObject.name, other.gameObject.layer);
        }
    }
    /// <summary>
    /// fonction appelée quand un item sors dans un collider
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer >= 7)
        {
            box.RemoveCollision(this.gameObject.name);

        }
    }

}
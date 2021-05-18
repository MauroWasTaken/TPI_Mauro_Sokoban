using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollisionScript : MonoBehaviour
{
    [SerializeField]
    BoxScript box;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer >= 7)
        {
            box.AddCollision(this.gameObject.name, other.gameObject.layer);
        }
    }

}
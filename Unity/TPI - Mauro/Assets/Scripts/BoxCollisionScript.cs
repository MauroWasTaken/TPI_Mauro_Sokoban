using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollisionScript : MonoBehaviour
{
    [SerializeField]
    BoxScript box;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 | other.gameObject.layer == 8)
        {
            box.AddCollision(this.gameObject.name, other.gameObject.layer);
            //Debug.Log(box.gameObject.name+" - "+this.gameObject.name + " - " + other.gameObject.layer);
        }
    }

}
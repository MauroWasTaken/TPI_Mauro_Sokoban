using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionScript : MonoBehaviour
{
    [SerializeField]
    CharacterScript character;
    /// <summary>
    /// fonction appelée quand un item entre dans un collider
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==7| other.gameObject.layer == 8)
        {
             character.AddCollision(this.gameObject.name, other);
        }
    }

}

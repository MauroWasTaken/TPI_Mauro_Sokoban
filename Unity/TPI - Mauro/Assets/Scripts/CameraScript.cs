using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script utilis� pour la camera 
/// </summary>
public class CameraScript : MonoBehaviour
{
    GameObject player=null;
    /// <summary>
    /// joueur � suivre
    /// </summary>
    public GameObject Player { set => player = value; }

    // Update is called once per frame
    void Update()
    {

        if (player != null)
        {
            this.transform.position = player.transform.position + new Vector3(-3, 6, 0);
        }
    }
}

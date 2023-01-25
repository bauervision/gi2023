using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public GameObject redBall;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(redBall, transform.position, transform.rotation);
    }

}

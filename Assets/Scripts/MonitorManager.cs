using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public GameObject InteractionManager;
    private bool managerPresent = false;
    private void Awake()
    {
        //check to see if the Interaction Manager is present in scene
        managerPresent = GameObject.FindGameObjectWithTag("InteractionManager");
        print("Interaction Manager present = " + managerPresent);
        if (!managerPresent)
        {
            Instantiate(InteractionManager, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

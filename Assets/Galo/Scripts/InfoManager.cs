using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;
    public UnityEvent onEnter = new UnityEvent();
    public UnityEvent onExit = new UnityEvent();
    public GameObject InfoCanvas;
    bool isPlayerWaiting;

    private void Awake() { instance = this; }

    void Start()
    {
        infoText.text = infoTextArray[0];
        counterText.text = $"1/{infoTextArray.Length}";
    }
    private void Update()
    {
        if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.E)) { UpdateInfoDisplay(); }
    }

    public Text infoText;
    public Text counterText;

    public string[] infoTextArray;
    int infoIndex = 0;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerWaiting = true;
            infoText.text = infoTextArray[0];//always start from the beginning
            onEnter.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerWaiting = false;
            onExit.Invoke();


        }
    }

    public void UpdateInfoDisplay()
    {
        if (isPlayerWaiting)
        {
            infoIndex++;

            if (infoIndex > infoTextArray.Length - 1)
                infoIndex = 0;//reset

            infoText.text = infoTextArray[infoIndex];
            counterText.text = $"{infoIndex + 1}/{infoTextArray.Length}";
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class EnteredWarningZone : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // notifyPanel.GetComponent<Image>().color = OnColor;
            // notifyText.color = new Color(1, 1, 1, 1);
            // notifyText.text = "You've found the Island Heart!\nEnter to join it with the blood to revive the island!";


        }
    }

    private void OnTriggerExit(Collider other)
    {

        // notifyPanel.GetComponent<Image>().color = OffColor;
        // notifyText.text = "";
    }




}

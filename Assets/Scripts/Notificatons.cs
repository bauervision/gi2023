using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Notificatons : MonoBehaviour
{
    public static Notificatons instance;

    private GameObject messagePanel;
    private static Text messageText;


    private static Color OffColor = new Color(0, 0, 0, 0);
    private static Color OnColor = new Color(0, 0, 0, 0.7f);

    private static Color TextStartColor = new Color(1, 1, 1, 1);
    private static Color TextEndColor = new Color(0, 0, 0, 0);

    private static Image messagePanelImage;


    private void Awake()
    {
        if (GameObject.Find("NotifyPanel") != null)
        {
            messagePanelImage = GameObject.Find("NotifyPanel").GetComponent<Image>();
            messageText = GameObject.Find("NotifyText").GetComponent<Text>();
        }
    }

    private void Start()
    {
        instance = this;
    }

    public static void ShowNotification(string message, float fadeDuration)
    {
        // turn everything on
        messagePanelImage.color = OnColor;
        messageText.color = TextStartColor;
        // set the message
        messageText.text = message;

        // and begin to fade it out
        instance.LaunchCoroutine(fadeDuration);

    }
    void LaunchCoroutine(float fadeDuration)
    {
        StartCoroutine(Fade(fadeDuration));
    }

    IEnumerator Fade(float fadeDuration)
    {
        yield return new WaitForSeconds(4f);
        float counter = 0f;
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            messageText.color = Color.Lerp(TextStartColor, TextEndColor, counter);
            messagePanelImage.color = Color.Lerp(OnColor, OffColor, counter);


            yield return null;
        }
    }

}
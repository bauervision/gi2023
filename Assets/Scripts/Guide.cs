using UnityEngine;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{

    public static Guide instance;
    private GameObject guideBook;
    private GameObject profilePage;

    public Text guideUpdateText;
    private bool showGuide;
    private bool showProfile;

    private void Awake()
    {
        instance = this;

        guideBook = GameObject.Find("Guide");
        profilePage = GameObject.Find("ProfilePage");


    }

    private void Start()
    {
        guideBook.SetActive(false);
        profilePage.SetActive(false);
    }

    public void UpdateGuide(string message)
    {
        guideUpdateText.text = message;
    }


    private void Update()
    {
        guideBook.SetActive(showGuide);
        profilePage.SetActive(showProfile);
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.G))
            showGuide = !showGuide;

        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.H))
            showProfile = !showProfile;

    }


}
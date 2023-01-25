using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class RegistrationFlow : MonoBehaviour
{
    public static RegistrationFlow instance;

    [SerializeField] private InputField _nameField = null;
    [SerializeField] private InputField _pinField = null;
    [SerializeField] private InputField _pinFieldConfirm = null;
    [SerializeField] private Button _registerUser = null;
    [SerializeField] private Button _loginUser = null;
    [SerializeField] private Text _newUserTitle = null;
    [SerializeField] public Text _warningText = null;

    [SerializeField] private InputField _returningPinField = null;

    public string Name => _nameField.text;
    public string Pin => _pinField.text;
    public string ConfirmPin => _pinFieldConfirm.text;

    private string UserPin => _returningPinField.text;

    public Color defaultColor = new Color(255, 255, 255);
    public Color goodColor = new Color(255, 255, 255);
    public Color highlightedColor = new Color(0, 200, 50);
    public Color redColor = new Color(255, 0, 0);

    private Color defaultTextColor;

    private bool nameGood, pinGood, confirmedGood;



    public enum FormState { Name, Pin, PinsDontMatch, Ok };

    public static FormState myForm;

    public enum ReturningFormState { Pin, BadPin, Ok };

    public static ReturningFormState myReturningForm;

    public static string staticName;


    private void Start()
    {
        instance = this;

        _nameField.onEndEdit.AddListener(HandleNameChanged);

        _pinField.onEndEdit.AddListener(HandlePinChanged);
        _pinFieldConfirm.onEndEdit.AddListener(HandlePinConfirmChanged);

        _returningPinField.onValueChanged.AddListener(HandleValueChangedReturning);

        _registerUser.onClick.AddListener(HandleRegisterUser);
        _registerUser.gameObject.SetActive(false);

        _loginUser.onClick.AddListener(HandleLoginUser);
        _loginUser.gameObject.SetActive(false);

        _warningText.text = "";
    }

    public void HandleRegisterUser()
    {
        // DataManager.instance.SaveNewUserData(Pin, Name);
        SuccessfulRegistration();
    }

    public void SuccessfulRegistration()
    {
        print("Reg was succesful with user: " + Name);
        //LoginManager.instance.ShowCharacterScreenNew();
    }

    public void LogOutUser()
    {
        // DataManager.instance.playerData = null;
        //LoginManager.instance.ShowInitialScreen();
        _returningPinField.text = string.Empty;
    }

    public void HandleLoginUser()
    {
        // can we find the user in our data?
        PlayerData returningPlayer = new PlayerData("67", "nam"); //DataManager.instance.FoundReturningPlayer(UserPin);

        // if (returningPlayer != null)
        //     LoginManager.instance.ShowCharacterScreen(returningPlayer);
        // else
        //     myReturningForm = ReturningFormState.BadPin;

    }

    public static void SuccessfulLogin(Task<PlayerData> loadedData)
    {
        print("Login was succesful with user: " + loadedData.Result.name);
        myReturningForm = ReturningFormState.Ok;

    }

    public static void FailedLogin(string email)
    {
        print("Login failed for user: " + email);
        instance._warningText.text = "Email or Password error, please try again";
        instance._loginUser.gameObject.SetActive(false);
        myReturningForm = ReturningFormState.BadPin;

        //instance.emailBad = true;

    }

    private void HandleNameChanged(string _)
    {
        if (!string.IsNullOrEmpty(Name))
        {
            myForm = FormState.Name;
            nameGood = true;
            _newUserTitle.text = $"Welcome {Name}!";
        }
    }

    private void HandlePinChanged(string _)
    {
        if (!string.IsNullOrEmpty(Pin))
        {
            myForm = FormState.Pin;
            pinGood = true;
        }
    }

    private void HandlePinConfirmChanged(string _)
    {
        if (Pin != ConfirmPin)
        {
            myForm = FormState.PinsDontMatch;
        }
        else
        {
            myForm = FormState.Ok;
            confirmedGood = true;
            // unlock the create button
            _registerUser.gameObject.SetActive(true);
        }
    }

    private void HandleValueChangedReturning(string _)
    {
        // if we have a valid string
        if (!string.IsNullOrEmpty(UserPin))
        {
            myReturningForm = ReturningFormState.Ok;
            _warningText.text = "";

            // unlock the login button
            _loginUser.gameObject.SetActive(true);

        }

    }



    private void TurnAllFieldsWhite()
    {
        _nameField.gameObject.GetComponent<Image>().color = nameGood ? goodColor : defaultColor;
        _pinField.gameObject.GetComponent<Image>().color = pinGood ? goodColor : defaultColor;
        _pinFieldConfirm.gameObject.GetComponent<Image>().color = confirmedGood ? goodColor : defaultColor;
    }




    void Update()
    {

        switch (myForm)
        {
            case FormState.Name:
                {
                    TurnAllFieldsWhite();
                    _nameField.gameObject.GetComponent<Image>().color = highlightedColor;
                    break;
                }
            case FormState.Pin: { TurnAllFieldsWhite(); _pinField.gameObject.GetComponent<Image>().color = highlightedColor; break; }
            case FormState.PinsDontMatch: { TurnAllFieldsWhite(); _pinFieldConfirm.gameObject.GetComponent<Image>().color = redColor; break; }
            case FormState.Ok: { TurnAllFieldsWhite(); break; }//unlock the submit button

        }


        switch (myReturningForm)
        {
            case ReturningFormState.BadPin:
                {
                    _returningPinField.gameObject.GetComponent<Image>().color = redColor;
                    _warningText.text = "Pin Not Found!";
                    break;
                }
            case ReturningFormState.Ok:
                {
                    _returningPinField.gameObject.GetComponent<Image>().color = defaultColor;
                    break;
                }
        }
    }
}

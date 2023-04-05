using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GangFighter{
public class CanvasManager : MonoBehaviour {

	public static  CanvasManager instance;

	public Canvas pLobby;

	public Canvas mainMenu;

	public Canvas uiChooseCharacter;

	public Canvas uiNewGame;

	public Canvas uijoin;

	public Canvas gameCanvas;

	public InputField inputLogin;

	public InputField inputHostName;

	public Canvas alertgameDialog;

	public Text alertDialogText;

	public Canvas alertGameOverDialog;

	public Text alertGameOverText;

	public Text txtSearchServerStatus;

	public GameObject lobbyCamera;

	public string currentMenu;

	public string lastMenu;

	public Canvas loadingImg;

	public Slider healthSlider;

	public Text txtHealth;

	public float delay = 0f;

	
	[Header("Mobile Buttons")]
	public Canvas mobileButtons;

	[Header("Mobile Joystick")]
	public Canvas mobileControlRig;

	public  AudioClip buttonAudioClip;

	public AudioClip lobbyAudioClip;

	public AudioClip  gameAudioClip;

	public AudioSource musicAudioSource;



	// Use this for initialization
	void Start () {

		if (instance == null) {

			DontDestroyOnLoad (this.gameObject);

			instance = this;

			OpenScreen("lobby");

			CloseAlertDialog ();

			#if UNITY_EDITOR  && UNITY_ANDROID || UNITY_ANDROID || UNITY_IOS

			mobileButtons.enabled = true;
			mobileControlRig.enabled = true;

			
			#else

			mobileButtons.enabled = false;
			mobileControlRig.enabled = false;
				
		     #endif
		}
		else
		{
			Destroy(this.gameObject);
		}



	}

	void Update()
	{
		delay += Time.deltaTime;

		if (Input.GetKey ("escape") && delay > 1f) {

		  switch (currentMenu) {

			case "lobby":
			 Application.Quit ();
			break;

			case "choose_character":
			 OpenScreen ("lobby");
			 delay = 0f;
			break;
			
			case "new_game":
			 OpenScreen ("lobby");
			 delay = 0f;
			break;
			
			case "game":
			 Application.Quit ();
			break;

		 }//END_SWITCH

	 }//END_IF
}
	/// <summary>
	/// Opens the screen.
	/// </summary>
	/// <param name="_current">Current.</param>
	public void  OpenScreen(string _current)
	{
		switch (_current)
		{
		    //lobby menu
		    case "lobby":
		
			lastMenu = currentMenu;
			currentMenu = _current;
			pLobby.enabled = true;
			uiChooseCharacter.enabled = false;
			mainMenu.enabled = true;
			uiNewGame.enabled = false;
			uijoin.enabled = false;	
			gameCanvas.enabled = false;
			lobbyCamera.GetComponent<Camera> ().enabled = true;
			if(lastMenu.Equals(string.Empty)|| lastMenu.Equals("game"))
			{
				PlayMusic(lobbyAudioClip);
			}
			break;


		    case "choose_character":
			lastMenu = currentMenu;
			currentMenu = _current;
			uiChooseCharacter.enabled = true;
			uiNewGame.enabled = false;
			mainMenu.enabled = false;
			uijoin.enabled = false;
			gameCanvas.enabled = false;
			lobbyCamera.GetComponent<Camera> ().enabled = true;
			break;

		    case "new_game":
			lastMenu = currentMenu;
			currentMenu = _current;

			uijoin.enabled = false;
			uiChooseCharacter.enabled = false;
			uiNewGame.enabled = true;
			mainMenu.enabled = false;
			gameCanvas.enabled = false;
			lobbyCamera.GetComponent<Camera> ().enabled = true;
			break;

			case "join_game":
			currentMenu = _current;
			uiChooseCharacter.enabled = false;
			uijoin.enabled = true;
			uiNewGame.enabled = false;
			mainMenu.enabled = false;
			gameCanvas.enabled = false;
			lobbyCamera.GetComponent<Camera> ().enabled = true;
			break;
	
			//no lobby menu
		case "game":
			currentMenu = _current;
			pLobby.enabled = false;
			gameCanvas.enabled = true;
			StopMusic();
			PlayMusic(gameAudioClip);
			
			#if UNITY_ANDROID 

				mobileButtons.enabled = true;	

			
					
			#else
			    mobileButtons.enabled = false;	

			#endif

			break;

			
		}

	}



	/// <summary>
	/// Shows the alert dialog.
	/// </summary>
	/// <param name="_message">Message.</param>
	public void ShowAlertDialog(string _message)
	{
		alertDialogText.text = _message;
		alertgameDialog.enabled = true;
	}

	public void ShowLoadingImg()
	{
		loadingImg.enabled = true;


	}
	public void CloseLoadingImg()
	{
		loadingImg.enabled = false;

	}

	
	/// <summary>
	/// Closes the alert dialog.
	/// </summary>
	public void CloseAlertDialog()
	{
		alertgameDialog.enabled = false;
	}

	public void ShowGameOverDialog()
	{
		
		alertGameOverText.text = "YOU LOSE!";
		alertGameOverDialog.enabled = true;
	}

	public void CloseGameOverDialog()
	{
		alertGameOverDialog.enabled = false;

		OpenScreen ("lobby");
	}

	public void PlaybuttonAudioClip()
	{
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().PlayOneShot(buttonAudioClip);
	}

	public void PlayMusic(AudioClip _music)
	{
		musicAudioSource.clip = _music;
		musicAudioSource.Play();
		
	}

	public void StopMusic()
	{
		musicAudioSource.Stop();
	}
}

}
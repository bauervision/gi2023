using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

namespace GangFighter{
public class ButtonChooseManager : MonoBehaviour {


	public static  ButtonChooseManager  instance;
	
	public int maxCharacters = 3;

	public Image[] character_selected_images;

	public GameObject[] sprites;

	public Image characterImg;
	
	public int current_character = 0;


	// Use this for initialization
	void Start () {
	
		// if don't exist an instance of this class
		if (instance == null) {


			// define the class as a static variable
			instance = this;
			
			current_character = 0;


		}
	}
	



	//method called by the BtnNext button that selects the next avatar
	public void ChooseCharacter(int _current_character)
	{
	
		int last_character_index = current_character;
		current_character = _current_character;
	    characterImg.sprite = sprites[_current_character].GetComponent<SpriteRenderer>().sprite;
		character_selected_images[_current_character].enabled = true;
		character_selected_images[last_character_index].enabled = false;

		
		
	}
	
	
	
}
}

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using UDPCore;
using GangServer;

namespace GangFighter{
public class NetworkManager : MonoBehaviour {

	//from UDP Socket API
	private UDPComponent udpClient;

	//Variable that defines comma character as separator
	static private readonly char[] Delimiter = new char[] {':'};

	//useful for any gameObject to access this class without the need of instances her or you declare her
	public static NetworkManager instance;

	//flag which is determined the player is logged in the arena
	public bool onLogged = false;

	//store localPlayer
	public GameObject localPlayer;

	//local player id
	public string myId = string.Empty;

	//local player id
	public string local_player_id;

	//store all players in game
	public Dictionary<string, PlayerController> networkPlayers = new Dictionary<string, PlayerController>();

	//store the  players' models
	public GameObject playerPref;


	//stores the spawn points 
	public Transform[] spawnPoints;

	//camera prefab
	public GameObject camFollowPref;

	[HideInInspector]
	public GameObject camFollow;

	public int serverPort = 3310;
	
	public int clientPort = 3000;

	public bool serverAlreadyStarted;

	public bool tryJoinServer;

	public bool waitingAnswer;

	public bool serverFound;

	public bool waitingSearch;

	public bool gameIsRunning;

	public int maxReconnectTimes = 10;

	public int contTimes;

	public float maxTimeOut;

	public float timeOut;

	public List<string> _localAddresses { get; private set; }

	public Transform minPX;
	public Transform  maxPX;
	public Transform  minPY;
	public Transform  maxPY;
	public Transform  minPZ;
	public Transform  maxPZ;
	public Transform  inicialCamPos;

	public bool isGameOver;


	// Use this for initialization
	void Start () {

		// if don't exist an instance of this class
		if (instance == null) {

			//it doesn't destroy the object, if other scene be loaded
			DontDestroyOnLoad (this.gameObject);

			instance = this;// define the class as a static variable
			
			udpClient = gameObject.GetComponent<UDPComponent>();
			
			int randomPort = UnityEngine.Random.Range(3001, 3310);

			
			//find any  server in others hosts
			ConnectToUDPServer(serverPort, randomPort);

		
		}
		else
		{
			//it destroys the class if already other class exists
			Destroy(this.gameObject);
		}
		
	}





	/// <summary>
	/// Connect client to any UDP server.
	/// </summary>
	public void ConnectToUDPServer(int _serverPort, int _clientPort)
	{


		if (udpClient.GetServerIP () != string.Empty) {

			//connect to udp server
			udpClient.connect (udpClient.GetServerIP (), _serverPort, _clientPort);

			//The On method in simplistic terms is used to map a method name to an annonymous function.
			udpClient.On ("PONG", OnPrintPongMsg);

			udpClient.On ("JOIN_SUCCESS", OnJoinGame);

			udpClient.On ("SPAWN_PLAYER", OnSpawnPlayer);

			//receives a "JOIN_SUCCESS" notification from the server
			udpClient.On ("RESPAWN_PLAYER", OnRespawnPlayer);

			udpClient.On ("UPDATE_POS_AND_ROT", OnUpdatePosAndRot);

			udpClient.On ("UPDATE_PLAYER_DAMAGE",OnUpdatePlayerDamage);

			udpClient.On ("GAME_OVER",OnGameOver);

			udpClient.On ("UPDATE_PLAYER_ANIMATOR", OnUpdateAnim);

			udpClient.On ("USER_DISCONNECTED", OnUserDisconnected);

		
		}


	}

	void Update()
	{


		// if there is no wifi network
		if (udpClient.noNetwork) {

		
			CanvasManager.instance.txtSearchServerStatus.text = "Please Connect to Wifi Hotspot";

			serverFound = false;

			CanvasManager.instance.ShowLoadingImg ();
		}


		//if it was not found a server
		if (!serverFound) {

			CanvasManager.instance.txtSearchServerStatus.text = string.Empty;

			// if there is a wifi connection but the server has not been started
			if (!udpClient.noNetwork) {
				
				//PoolCanvasManager.instance.txtSearchServerStatus.text = "Please start the server ";

			}
			else
			{
				
				CanvasManager.instance.txtSearchServerStatus.text = "Please Connect to Wifi Hotspot ";
			}
		
			// start routine to detect a server on wifi network
			StartCoroutine ("PingPong");
		}
		
	}


	/// <summary>
	/// corroutine called  of times in times to send a ping to the server
	/// </summary>
	/// <returns>The pong.</returns>
	private IEnumerator PingPong()
	{

		
		if (waitingSearch)
		{
			yield break;
		}

		waitingSearch = true;

		//sends a ping to server
		EmitPing ();
		
		// wait 1 seconds and continue
		yield return new WaitForSeconds(1);
		
		waitingSearch = false;

	}



	//it generates a random id for the local player
	public string generateID()
	{
		string id = Guid.NewGuid().ToString("N");

		//reduces the size of the id
		id = id.Remove (id.Length - 15);

		return id;
	}

	/// <summary>
	///  receives an answer of the server.
	/// from  void OnReceivePing(string [] pack,IPEndPoint anyIP ) in server
	/// </summary>
	public void OnPrintPongMsg(UDPEvent data)
	{

	
		/*
		 * data.pack[0]= CALLBACK_NAME: "PONG"
		 * data.pack[1]= "pong!!!!"
		*/

		Debug.Log("receive pong");

		CanvasManager.instance.txtSearchServerStatus.text = "------- server is running -------";
		
		serverFound = true;
		
		if(serverAlreadyStarted)
		{
		   EmitJoin();
		   serverAlreadyStarted = false;
		}
	
	}




	// <summary>
	/// sends ping message to server.
	/// to  case "PING":
	///     OnReceivePing(pack,anyIP);
	///     break;
	/// take a look in UDPServer.cs script
	/// </summary>
	public void EmitPing() {

		//hash table <key, value>	
		Dictionary<string, string> data = new Dictionary<string, string>();

		//JSON package
		data["callback_name"] = "PING";

		//store "ping!!!" message in msg field
		data["msg"] = "ping!!!!";

		//The Emit method sends the mapped callback name to  the server
		udpClient.EmitToServer (data["callback_name"] ,data["msg"]);

	}
		

	/// <summary>
	///  tries to put the player in game
	/// </summary>
	public void EmitJoin()
	{
	  try{
	  
		
		//checks if the player was already logged in and suffered a game over, or if you just opened the application
		if (!isGameOver) {
		
		 // check if there is a wifi connection
		if (!udpClient.noNetwork) {
		
			// check if there is a server running
		   if (serverFound) {
		
		      Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON

		      data ["callback_name"] = "JOIN";//set up callback name

		      data["player_name"] = CanvasManager.instance.inputLogin.text;

		      data["character_index"] = ButtonChooseManager.instance.current_character.ToString();

		     //it is already verified an id was generated
		     if (myId.Contains (string.Empty)) {

			    myId = generateID ();

			    data ["player_id"] = myId;
		     }
		     else
		     {
			   data ["player_id"] = myId;
		     }

		
		      int index = 0;

	
		      data["position"] = UtilsClass.Vector3ToString(spawnPoints[index].position);

		      //send the position point to server
		      string msg =  data["player_id"]+":"+data["player_name"]+ ":"+ data["position"]+":"+data["character_index"];

		      //sends to the server through socket UDP the jo package 
		     udpClient.EmitToServer (data ["callback_name"], msg);
		   }

			else
			{
			   serverAlreadyStarted = true;
				   
			   UDPServer.instance.CreateServer();
				
			}
			
		}//END_IF
		else
		{
		  CanvasManager.instance.ShowAlertDialog("Please Connect to Wifi Hotspot");
		}
			
		}//END_IF
		else
		{ //if the player was already in play and suffered GameOver
		  
		   Dictionary<string, string> data = new Dictionary<string, string>();

		   data["callback_name"] = "RESPAWN";
		
		   data["player_name"] = CanvasManager.instance.inputLogin.text;
			  
		   data["character_index"] = ButtonChooseManager.instance.current_character.ToString();

		   int index = 0;

		   data["position"] = UtilsClass.Vector3ToString(spawnPoints[index].position);

		   string msg = myId+":"+data["player_name"]+":"+ data["character_index"]+":"+data["position"];
		     
		   udpClient.EmitToServer (data["callback_name"] ,msg);
		   
				
		}//END_ELSE
		
     }//END_IF
     catch ( Exception e ){

        Debug.LogError( e.ToString());	
     }

	}

	
	/// <summary>
	/// Joins the local player in game.
	/// </summary>
	/// <param name="_data">Data.</param>
	public void OnJoinGame(UDPEvent data)
	{

		/*
		 * data.data.pack[0] = CALLBACK_NAME: "JOIN_SUCCESS" from server
		 * data.data.pack[1] = id (local player id)
		 * data.data.pack[2]= name (local player name)
		 * data.data.pack[3] = position
		 * data.data.pack[4] = character_index
		*/

		Debug.Log("Login successful, joining game");


		if (!localPlayer) {

			// take a look in PlayerController.cs script
			PlayerController newPlayer;
			
			// newPlayer = GameObject.Instantiate( local player avatar or model, spawn position, spawn rotation)
			newPlayer = GameObject.Instantiate (playerPref ,
			UtilsClass.StringToVector3(data.pack[3]),Quaternion.identity).GetComponent<PlayerController> ();

		

			Debug.Log("player instantiated");

			newPlayer.id = data.pack [1];

			//this is local player
			newPlayer.isLocalPlayer = true;

			//now local player online in the arena
			newPlayer.isOnline = true;

			//set local player's 3D text with his name
			newPlayer.Set3DName(data.pack[2]);

			newPlayer.SetUpCharacter(int.Parse(data.pack[4]));

			newPlayer.SetAnimator();

			//puts the local player on the list
			networkPlayers [data.pack [1]] = newPlayer;

			localPlayer= networkPlayers [data.pack[1]].gameObject;

			local_player_id =  data.pack [1];

			//spawn cam
			camFollow = GameObject.Instantiate (camFollowPref, camFollowPref.transform.position, camFollowPref.transform.rotation);

			camFollow.GetComponent<CameraFollow> (). SetPointsPositions(minPX, maxPX,  minPY,
	                          maxPY,minPZ, maxPZ, inicialCamPos);

			//set local player how  being MultipurposeCameraRig target to follow him
			camFollow.GetComponent<CameraFollow> ().SetTarget (localPlayer.transform);

			CanvasManager.instance.healthSlider.value = newPlayer.gameObject.GetComponent<PlayerHealth>().health;

			CanvasManager.instance.txtHealth.text = "HP " + newPlayer.gameObject.GetComponent<PlayerHealth>().health + " / " +
				newPlayer.gameObject.GetComponent<PlayerHealth>().maxHealth;
			
			//hide the lobby menu (the input field and join buton)
			CanvasManager.instance.OpenScreen("game");

			CanvasManager.instance.CloseLoadingImg ();

			CanvasManager.instance.lobbyCamera.GetComponent<Camera> ().enabled = false;

			gameIsRunning = true;

			CanvasManager.instance.CloseLoadingImg();

			//take a look in public IEnumerator WaitAnswer()
			tryJoinServer = false;

			// the local player now is logged
			onLogged = true;

			Debug.Log("player in game");
		}
	}

	/// <summary>
	/// Raises the spawn player event.
	/// </summary>
	/// <param name="_msg">Message.</param>
	void OnSpawnPlayer(UDPEvent data)
	{

		/*
		 * data.pack[0] = SPAWN_PLAYER
		 * data.pack[1] = id (network player id)
		 * data.pack[2]= name
		 * data.pack[3] = position
		 * data.pack[4] = character_index
		
		*/

		 Debug.Log("try spawn player");
		if (onLogged ) {

		
			bool alreadyExist = false;

			//verify all players to  prevents copies
			foreach(KeyValuePair<string, PlayerController> entry in networkPlayers)
			{
				// same id found ,already exist!!! 
				if (entry.Value.id== data.pack [1])
				{
					alreadyExist = true;
				}
			}
			if (!alreadyExist) {

				Debug.Log("creating a new player");

				PlayerController newPlayer;

				// newPlayer = GameObject.Instantiate( network player avatar or model, spawn position, spawn rotation)
				newPlayer = GameObject.Instantiate (playerPref,
					UtilsClass.StringToVector3(data.pack[3]),Quaternion.identity).GetComponent<PlayerController> ();

				//it is not the local player
				newPlayer.isLocalPlayer = false;

				//network player online in the arena
				newPlayer.isOnline = true;

				//set the network player 3D text with his name
				newPlayer.Set3DName(data.pack[2]);

				newPlayer.SetUpCharacter(int.Parse(data.pack[4]));

				newPlayer.SetAnimator();

				newPlayer.gameObject.name = data.pack [1];

				//puts the local player on the list
				networkPlayers [data.pack [1]] = newPlayer;
			}

		}

	}


		/// <summary>
	/// method to handle notification that arrived from the server.
	/// </summary>
	/// <remarks>
	/// respawn local player .
	/// </remarks>
	/// <param name="data">received package from server.</param>
	void OnRespawnPlayer(UDPEvent data)
	{   
		/*
		 * data.pack[0] = RESPAWN_PLAYER
		 * data.pack[1] = id 
		 * data.pack[2]= name
		 * data.pack[3] = "position.x;position.y;position.z"
		 
		*/
		Debug.Log("respawn received");
        try{
		Debug.Log("Respawn successful, joining game");
		
		
		isGameOver = false;

		if (localPlayer== null) {

			// take a look in PlayerController.cs script
			PlayerController newPlayer;
			
			// newPlayer = GameObject.Instantiate( local player avatar or model, spawn position, spawn rotation)
			newPlayer = GameObject.Instantiate (playerPref ,
			UtilsClass.StringToVector3(data.pack[3]),Quaternion.identity).GetComponent<PlayerController> ();

		

			Debug.Log("player instantiated");

			newPlayer.id = data.pack [1];

			//this is local player
			newPlayer.isLocalPlayer = true;

			//now local player online in the arena
			newPlayer.isOnline = true;

			//set local player's 3D text with his name
			newPlayer.Set3DName(data.pack[2]);

			newPlayer.SetUpCharacter(int.Parse(data.pack[4]));

			//puts the local player on the list
			networkPlayers [data.pack [1]] = newPlayer;

			localPlayer= networkPlayers [data.pack[1]].gameObject;

			local_player_id =  data.pack [1];

			//spawn cam
			camFollow = GameObject.Instantiate (camFollowPref, camFollowPref.transform.position, camFollowPref.transform.rotation);

			camFollow.GetComponent<CameraFollow> (). SetPointsPositions(minPX, maxPX,  minPY,
	                          maxPY,minPZ, maxPZ, inicialCamPos);

			//set local player how  being MultipurposeCameraRig target to follow him
			camFollow.GetComponent<CameraFollow> ().SetTarget (localPlayer.transform);

			CanvasManager.instance.healthSlider.value = newPlayer.gameObject.GetComponent<PlayerHealth>().health;

			CanvasManager.instance.txtHealth.text = "HP " + newPlayer.gameObject.GetComponent<PlayerHealth>().health + " / " +
				newPlayer.gameObject.GetComponent<PlayerHealth>().maxHealth;
			
			//hide the lobby menu (the input field and join buton)
			CanvasManager.instance.OpenScreen("game");

			CanvasManager.instance.CloseLoadingImg ();

			CanvasManager.instance.lobbyCamera.GetComponent<Camera> ().enabled = false;

			gameIsRunning = true;

			CanvasManager.instance.CloseLoadingImg();

			//take a look in public IEnumerator WaitAnswer()
			tryJoinServer = false;

			// the local player now is logged
			onLogged = true;

			Debug.Log("player in game");

		}//END_IF
		}//END_TRY
		catch(Exception e)
		{
		  Debug.LogError(e.ToString());
		}

	}


	/// <summary>
	///  Update the network player position to local player.
	/// </summary>
	/// <param name="_msg">Message.</param>
	void OnUpdatePosAndRot(UDPEvent data)
	{

		/* data.pack[0] = UPDATE_POS_AND_ROT
		 * data.pack[1] = id (network player id)
		 * data.pack[2] = "position.x;position.y;posiiton.z"
		 * data.pack[3] = "rotation.x; rotation.y; rotation.z; rotation.w"
		*/

        try{
		
		//it reduces to zero the accountant meaning that answer of the server exists to this moment
		contTimes = 0;

		if (networkPlayers [data.pack [1]] != null)
		{
			//find network player
			PlayerController netPlayer = networkPlayers [data.pack [1]];
			netPlayer.timeOut = 0f;

			//update with the new position
			netPlayer.UpdatePosition(UtilsClass.StringToVector3(data.pack[2]));

			//update new player rotation
			netPlayer.UpdateRotation(UtilsClass.StringToQuaternion(data.pack[3]));

		}
		}//END_TRY
		catch(Exception e)
		{
		  Debug.Log("error: "+data.pack[2]);
		  Debug.Log("error 2: "+data.pack[3]);
		  Debug.LogError(e.ToString());
		}


	}


	/// <summary>
	/// method to send local player position and rotation update to the server.
	/// </summary>
	/// <param name="id">local player id.</param>
	/// <param name="_pos">local player position.</param>
	/// <param name="_rot">local player rotation.</param>
	public void EmitPosAndRot(Vector3 _pos, Quaternion _rot)
	{
	  //Identifies with the name "POS_AND_ROT", the notification to be transmitted to the server,
	  //and send to the server the player's position and rotation

	  //hash table <key, value>
		Dictionary<string, string> data = new Dictionary<string, string>();

		data["local_player_id"] =  localPlayer.GetComponent<PlayerController>().id;

		data["position"] = UtilsClass.Vector3ToString(_pos);
		
		data["rotation"] = UtilsClass.QuaternionToString(_rot);
		
		string msg = data["local_player_id"]+":"+data["position"]+":"+data["rotation"];


	     udpClient.EmitToServer ("POS_AND_ROT" ,msg);
	}


	

	/// <summary>
	/// Emits the local player phisicst damage to server.
	/// </summary>
	/// <param name="_fighterId">Shooter identifier.</param>
	/// <param name="_targetId">Target identifier.</param>
	public void EmitDamage(string _fighterId, string _targetId)
	{

		//hash table <key, value>
		Dictionary<string, string> data = new Dictionary<string, string>();

		data ["fighterId"] = _fighterId;
		data ["targetId"] = _targetId;

		//join info
		string msg = data ["fighterId"]+":"+data ["targetId"];

		//sends to the server through socket UDP the jo package 
		udpClient.EmitToServer("PHISICS_DAMAGE" ,msg );

	}


	//it updates the suffered damage to local player
	void OnUpdatePlayerDamage (UDPEvent data)
	{

		/*
		 * data.pack[0] = UPDATE_PHISICS_DAMAGE
		 * data.pack[1] = attacker.id or shooter.id (network player id)
		 * data.pack[2] = target.id (network player id)
		 * data.pack[3] = target.health
		 */



		if (networkPlayers [data.pack [2]] != null) 
		{
			CameraShake.Shake(0.4f,0.1f);

			PlayerController PlayerTarget = networkPlayers[data.pack [2]];
			PlayerTarget. GetComponent<PlayerHealth> ().TakeDamage ();

		}

	}

	  /// <summary>
	///  Update the network player animation to local player.
	/// </summary>
	/// <param name="_msg">Message.</param>
	void OnGameOver(UDPEvent data)
	{
		/*
		 * data.pack[1] = id (network player id)
		 
		*/
		

		if (networkPlayers.ContainsKey(data.pack[1])) 
		{
			PlayerController PlayerTarget = networkPlayers[data.pack [1]];
			
			// if local player is the target
			if (PlayerTarget.isLocalPlayer) 
			{
				 PlayerTarget.GetComponent<PlayerHealth>().Death();
			 
			}
			else
			{
			  PlayerTarget.GetComponent<PlayerHealth>().DoRagDoll();
		      Destroy( networkPlayers[ PlayerTarget.id].gameObject);
		      networkPlayers.Remove(PlayerTarget.id);
			}

		}
		
	}


	public void GameOver()
	{
		
		isGameOver = true;
	
		CanvasManager.instance.ShowGameOverDialog ();
		CanvasManager.instance.lobbyCamera.GetComponent<Camera> ().enabled = true;
		networkPlayers.Remove(localPlayer.GetComponent<PlayerController>().id);
		Destroy( localPlayer);
		Destroy (camFollow.gameObject);
	
		
	}





	/// <summary>
	/// Emits the local player animation to Server.js.
	/// </summary>
	/// <param name="_animation">Animation.</param>
	public void EmitAnimation(string _animation)
	{
		//hash table <key, value>
		Dictionary<string, string> data = new Dictionary<string, string>();

		//JSON package
		data["callback_name"] = "ANIMATION";//preenche com o id da callback receptora que está no servidor

		data["local_player_id"] = localPlayer.GetComponent<PlayerController>().id;

		data ["animation"] = _animation;

		//send the position point to server
		string msg = data["local_player_id"]+":"+data ["animation"];

		//sends to the server through socket UDP the jo package 
		udpClient.EmitToServer (data["callback_name"] ,msg);


	}

	/// <summary>
	///  Update the network player animation to local player.
	/// </summary>
	/// <param name="_msg">Message.</param>
	void OnUpdateAnim(UDPEvent data)
	{
		/*
		 * data.pack[0] = UPDATE_PLAYER_ANIMATOR
		 * data.pack[1] = id (network player id)
		 * data.pack[2] = animation (network player animation)
		*/

		contTimes = 0;

		//find network player by your id
		PlayerController netPlayer = networkPlayers[data.pack[1]];
		netPlayer.timeOut = 0f;
		//updates current animation
		netPlayer.UpdateAnimator(data.pack[2]);

	}


	void Disconnect()
	{
		if(localPlayer)
		{
			//hash table <key, value>
			Dictionary<string, string> data = new Dictionary<string, string>();

			//JSON package
			data["callback_name"] = "disconnect";

			data ["local_player_id"] = local_player_id;

			if (udpClient.serverRunning) {

				data ["isMasterServer"] = "true";
			}
			else 
			{
				data ["isMasterServer"] = "false";
			}
				
			//send the position point to server
			string msg = data["local_player_id"]+":"+data ["isMasterServer"];

			//Debug.Log ("emit disconnect");

			//we make four attempts of similar sending of preventing the loss of packages
			udpClient.EmitToServer (data["callback_name"] ,msg);

			udpClient.EmitToServer (data["callback_name"] ,msg);

			udpClient.EmitToServer (data["callback_name"] ,msg);

			udpClient.EmitToServer (data["callback_name"] ,msg);
		}

		if (udpClient != null) {

			udpClient.disconnect ();


		}
	}


	/// <summary>
	/// inform the local player to destroy offline network player
	/// </summary>
	/// <param name="_msg">Message.</param>
	//disconnect network player
	void OnUserDisconnected(UDPEvent data )
	{

		/*
		 * data.pack[0]  = USER_DISCONNECTED
		 * data.pack[1] = id (network player id)
		 * data.pack[2] = isMasterServer
		*/
		Debug.Log ("disconnect!");

		if (bool.Parse (data.pack [2])) {
			
			RestartGame ();
		}
		else
		{
			
				if (networkPlayers [data.pack [1]] != null) {
				
					//destroy network player by your id
					Destroy (networkPlayers [data.pack [1]].gameObject);

					//remove from the dictionary
					networkPlayers.Remove (data.pack [1]);
				}
		}


	}

	public void RestartGame()
	{
		CanvasManager.instance.txtSearchServerStatus.text = "PLEASE START SERVER";

		Destroy (camFollow.gameObject);
		foreach(KeyValuePair<string, PlayerController> entry in networkPlayers)
		{
			if (networkPlayers [entry.Key] != null) {
				Destroy (networkPlayers [entry.Key].gameObject);
			}
		}

		networkPlayers.Clear ();

		gameIsRunning = false;

		serverFound = false;

		myId = string.Empty;

		CanvasManager.instance.OpenScreen ("lobby");

	}



	void OnApplicationQuit() {

		Debug.Log("Application ending after " + Time.time + " seconds");

		Disconnect ();
			
	}
		
}
}

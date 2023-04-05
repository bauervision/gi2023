﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using UDPCore;
using TicTacToeServerModule;

namespace TicTac{
public class TicTacNetworkManager : MonoBehaviour {


//from UDP Socket API
	private UDPComponent udpClient;

	//Variable that defines comma character as separator
	static private readonly char[] Delimiter = new char[] {':'};

	//useful for any gameObject to access this class without the need of instances her or you declare her
	public static TicTacNetworkManager instance;

	//flag which is determined the player is logged in the arena
	public bool onLogged = false;

	//store localPlayer
	public GameObject myPlayer;

	//local player id
	public string myId = string.Empty;

	//local player id
	public string local_player_id;

	public int serverPort = 3310;

	public int clientPort = 3000;

	public bool waitingAnswer;

	public bool serverFound;

	public bool waitingSearch;


	public List<string> _localAddresses { get; private set; }

	public enum PlayerType { SQUARE,X}; 

	public PlayerType playerType;
	
	public bool myTurn;

	// Use this for initialization
	void Start () {
	
	 // if don't exist an instance of this class
	 if (instance == null) {

		//it doesn't destroy the object, if other scene be loaded
		DontDestroyOnLoad (this.gameObject);

		instance = this;// define the class as a static variable


		udpClient = gameObject.GetComponent<UDPComponent>();
		
		//find any  server in others hosts
		ConnectToUDPServer();

		
	 }
	 else
	 {
		//it destroys the class if already other class exists
		Destroy(this.gameObject);
	 }
		
	}
	
	
	/// <summary>
	/// Connect client to TicTactoeServer.cs
	/// </summary>
	public void ConnectToUDPServer()
	{


		if (udpClient.GetServerIP () != string.Empty) {

		    int randomPort = UnityEngine.Random.Range(3001, 3310);
			//connect to TicTacttoeServer
			udpClient.connect (udpClient.GetServerIP (), serverPort, randomPort);

			udpClient.On ("PONG", OnPrintPongMsg);

			udpClient.On ("JOIN_SUCCESS", OnJoinGame);
			
			udpClient.On("START_GAME", OnStartGame);
			
			udpClient.On("UPDATE_BOARD", OnUpdateBoard);
			
			udpClient.On("GAME_OVER", OnGameOver);
			
			udpClient.On ("USER_DISCONNECTED", OnUserDisconnected);


		}


	}

	void Update()
	{

		// if there is no wifi network
		if (udpClient.noNetwork) {

		
			TicTacCanvasManager.instance.txtSearchServerStatus.text = "Please Connect to Wifi Hotspot";

			serverFound = false;

			TicTacCanvasManager.instance.ShowLoadingImg ();
		}


		//if it was not found a server
		if (!serverFound) {

			TicTacCanvasManager.instance.txtSearchServerStatus.text = string.Empty;

			// if there is a wifi connection but the server has not been started
			if (!udpClient.noNetwork) {
				
				TicTacCanvasManager.instance.txtSearchServerStatus.text = "Please start the server ";

			}
			else
			{
				
				TicTacCanvasManager.instance.txtSearchServerStatus.text = "Please Connect to Wifi Hotspot ";
			}
		
			// start routine to detect a server on wifi network
			StartCoroutine ("PingPong");
		}
		//found server
		else
		{

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
		
		serverFound = true;
		//arrow the located text in the inferior part of the game screen
		TicTacCanvasManager.instance.txtSearchServerStatus.text = "------- server is running -------";

	}




	// <summary>
	/// sends ping message to UDPServer.
	///     case "PING":
	///     OnReceivePing(pack,anyIP);
	///     break;
	/// take a look in TicTacttoeServer.cs script
	/// </summary>
	public void EmitPing() {

		//hash table <key, value>	
		Dictionary<string, string> data = new Dictionary<string, string>();

		//JSON package
		data["callback_name"] = "PING";

		//store "ping!!!" message in msg field
		data["msg"] = "ping!!!!";

		//CanvasManager.instance.ShowAlertDialog ("try emit ping");
		//The Emit method sends the mapped callback name to  the server
		udpClient.EmitToServer (data["callback_name"] ,data["msg"]);
		
		//CanvasManager.instance.ShowAlertDialog ("ping sended: "+serverFound);

	}
	

	/// <summary>
	/// Emits the join game to ticTacttoeServer.
	/// case "JOIN_GAME":
	///   OnReceiveJoinGame(pack,anyIP);
	///  break;
	/// take a look in TicTactToeServer.cs script
	/// </summary>
	public void EmitJoinGame()
	{
		// check if there is a wifi connection
		if (!udpClient.noNetwork) {
		
			// check if there is a server running
		   if (serverFound) {
		
		      Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON

		      data["callback_name"] = "JOIN_GAME";
		
		      //it is already verified an id was generated
		      if (myId.Equals (string.Empty)) {

			    myId = generateID ();
               
			    data ["player_id"] = myId;
		      }
		      else
		      {
			    data ["player_id"] = myId;
		      }
		    
			  //send the message join to TicTactToeServer
		      string msg = data["player_id"];
		
		      udpClient.EmitToServer (data["callback_name"] ,msg);
		    }
			
			else
			{
			  TicTacCanvasManager.instance.ShowAlertDialog("please start the server");
			}
		}
		
		else
		{
		  TicTacCanvasManager.instance.ShowAlertDialog("Please Connect to Wifi Hotspot");
		}
	}


	/// <summary>
	/// Raises the join game event from TictactToeServer.
	/// only the first player to connect gets this feedback from the server
	/// </summary>
	/// <param name="data">Data.</param>
	void OnJoinGame(UDPEvent data)
	{
		Debug.Log ("\n joining ...\n");

		// open game screen only for the first player, as the second has not logged in yet
		TicTacCanvasManager.instance.OpenScreen(1);
	
	    Debug.Log("try to loading board");

		// load the board only for the first player, because the second one hasn't logged in yet
		BoardManager.instance.LoadBoard ();

		// set square to the first player to connect
		SetPlayerType("square");
		
		TicTacCanvasManager.instance.txtHeader.text = "You are player O";
		 
		TicTacCanvasManager.instance.txtFooter .text = "connected! \n Waiting for another player";

		Debug.Log ("\n first player SQUARE joined...\n");

	}


	/// <summary>
	/// Raises the start game event.
	/// both players receive this response from the server
	/// </summary>
	/// <param name="data">Data.</param>
	void OnStartGame(UDPEvent data)
	{
		Debug.Log ("\n game is runing...\n");


		// check if it's the first player to connect
		if(GetPlayerType().Equals("square"))
		{
		  // define as first to play	
		  myTurn = true;

		  TicTacCanvasManager.instance.txtHeader.text = "You are player O";
		}
		else// if you are the second player
		{

		   myTurn = false;

		   TicTacCanvasManager.instance.txtHeader.text = "You are player X";

			// load the game screen for this player only,
			//as the screen has already been loaded for the first player logged into the OnJoinGame method
		   TicTacCanvasManager.instance.OpenScreen(1);

			//load the board for this player only
		   BoardManager.instance.LoadBoard ();
		}
		


		// check if you are the first player to connect
		if (myTurn) {

		   
			TicTacCanvasManager.instance.txtFooter .text = "Your move";
		}
		else
		{
			TicTacCanvasManager.instance.txtFooter.text = "Opponent move";
		}

		Debug.Log ("\n game loaded...\n");

	}


	/// <summary>
	/// Emits the update board to TictactToeServer
	/// </summary>
	public void EmitUpdateBoard(int i, int j)
	{
		Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON

		// now the turn belongs to the opposing player
		myTurn = false;

		TicTacCanvasManager.instance.txtFooter.text = "Opponent move";
			
		
		data["callback_name"] = "SERVER_UPDATE_BOARD";
		
		data["player_id"] = myId;
		
		data["player_type"] = GetPlayerType();
		
		
		data["i"]= i.ToString();
		
		data["j"]= j.ToString();

		//send the message  to ticTacttoeServer
		string msg = data["player_id"]+":"+data["player_type"]+":"+data["i"]+":" + data["j"];
	
		//sends to the server through socket UDP the jo package 
		udpClient.EmitToServer (data ["callback_name"], msg);

		Debug.Log("update board sended to server");

	}



	/// <summary>
	/// updates the board with information from TicTactToeServer
	/// </summary>

	void OnUpdateBoard(UDPEvent data)
	{
	
	    /*
		 * data.data.pack[0] = CALLBACK_NAME: "UPDATE_BOARD" from server
		 * data.data.pack[1] = id of the opponent who made the last move
		 * data.data.pack[2] = player_type
		 * data.data.pack[3]= j
		 * data.data.pack[4] = i

		*/

		try{

			Debug.Log("receive update board from server");

		// how the server message is transmitted to both players,
		// we should check if we are the next player to play, message target
		//data.pack[1] stores the id of the player who finished his move
		if(!data.pack[1].Equals(myId))
		{
				Debug.Log("ok");
		
			// set row i and column j which should be updated in BoardManager
			BoardManager.instance.current_i = int.Parse (data.pack [3]);

			// set row i and column j which should be updated in BoardManager
			BoardManager.instance.current_j = int.Parse (data.pack [4]);


			// check the type of cell to update O or X
		    if(data.pack[2].Equals("square"))
		    {

				BoardManager.instance.SpawnSquare(); 
		     
		    }
		    else
	        {
				BoardManager.instance.SpawnX();
		    }
		  
		   TicTacCanvasManager.instance.txtFooter.text = "Your move";

		   myTurn = true;
		}
			Debug.Log("done");
		}
		catch(Exception e)
		{
			Debug.LogError(e.ToString());
		}

		
		
	}


	/// <summary>
	/// Send a message to the server to notify you that the next player has lost the game.
	/// </summary>
	public void EmitGameOver()
	{
		Dictionary<string, string> data = new Dictionary<string, string>();//pacote JSON

		myTurn = false;

		TicTacCanvasManager.instance.txtFooter.text = " ";
			
		
		data["callback_name"] = "SERVER_GAME_OVER";
		
		data["player_id"] = myId;

		string msg = data["player_id"];

	
		//sends to the server through socket UDP the msg package 
		udpClient.EmitToServer (data ["callback_name"], msg);

	}


	/// <summary>
	/// get the server update that the player of this instance lost the game
	/// </summary>

	void OnGameOver(UDPEvent data)
	{
	
	    /*
		 * data.data.pack[0] = CALLBACK_NAME: "GAME_OVER" from server
		 * data.data.pack[1] = player_id
		
		*/

		// how the server message is transmitted to both players,
		// we should check if we are the next player to play, the loser
		//data.pack[1] stores the id of the player who won the match

		if(!data.pack[1].Equals(myId))
		{
		  BoardManager.instance.ResetGameForLoserPlayer();
		}
		
		myTurn = false;

		
		
	}


	/// <summary>
	/// Emits the disconnect to server
	/// </summary>
	void EmitDisconnect()
	{
		//hash table <key, value>
		Dictionary<string, string> data = new Dictionary<string, string> ();

		//JSON package
		data ["callback_name"] = "disconnect";

		data ["local_player_id"] = myId;

		if (TicTacToeServer.instance.serverRunning) {

			data ["isMasterServer"] = "true";
		} else {
			data ["isMasterServer"] = "false";
		}


		string msg = data ["local_player_id"]+":"+data ["isMasterServer"];

		Debug.Log ("emit disconnect");

		udpClient.EmitToServer (data ["callback_name"], msg);

	
	}

	/// <summary>
	/// inform the local player to destroy offline network player
	/// </summary>
	void OnUserDisconnected(UDPEvent data )
	{

		/*
		 * data.pack[0]  = USER_DISCONNECTED
		 * data.pack[1] = id (network player id)
		 * data.pack[2] = isMasterServer
		*/
		Debug.Log ("disconnect!");

		// check if the disconnected player was the master server
		if (bool.Parse (data.pack [2])) {


			RestartGame ();
		}
		else
		{

			BoardManager.instance.ResetGameForWOPlayer();
		
		     myTurn = false;

		}


	}

	public void RestartGame()
	{
		
	    serverFound = false;

		TicTacCanvasManager.instance.OpenScreen (0);

	}
	
	private void OnDestroy() {
	
		if (udpClient != null) {
			udpClient.disconnect ();
		}
	}



	void CloseApplication()
	{

		if (udpClient != null) {

			EmitDisconnect ();

			udpClient.disconnect ();

		}
	}


	void OnApplicationQuit() {

		Debug.Log("Application ending after " + Time.time + " seconds");

		CloseApplication ();

	}


	public string GetPlayerType()
	{
		switch (playerType) {

		case PlayerType.SQUARE:
			return "square";
		break;
		case PlayerType.X:
			return "x";
		break;
		
		}
		return string.Empty;
	}


	/// <summary>
	/// Sets the type of the user.
	/// </summary>
	/// <param name="_userType">User type.</param>
	public void SetPlayerType(string _playerType)
	{
		switch (_playerType) {

		case "square":
			playerType = PlayerType.SQUARE;	
		break;
		case "x":
			playerType = PlayerType.X;	
		break;
		}
	}
}
}

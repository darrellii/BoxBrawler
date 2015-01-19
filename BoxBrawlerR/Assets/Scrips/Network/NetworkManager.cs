using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const int mySendRate = 100;
	private const string typeName = "BoxBrawlerR";
	private const string gameName = "BoxBrawlerR";
	private string comment = "BoxBrawlerR";
	private HostData[] hostData;
	private bool gameConnected = false;
	private bool gameStarted = false;
	private ArrayList players;
	public GUIText txt;
	public Object playerPrefab;
	private int spawnx=  -6;
	public Vector3 spawn = new Vector3(0,0,0);
	private int connectAttemps = 0;
	private int hostIndex = 0;
	private bool recieved;
	private float start;
	private bool isConnecting = false;



	void Start()
	{
		players = new ArrayList ();
		comment = "alfaone"; 
		gameConnected = false;
		start = Time.time;
		connectAttemps = 0;
		//starts looking for games
		Network.InitializeServer(4, 25001, !Network.HavePublicAddress());
		MasterServer.RequestHostList (gameName);
//		Network.sendRate = mySendRate;
	}
	void Awake() {
		Network.sendRate = mySendRate;
	}
	void Update()
	{
		if(!gameConnected)
		{

			//checks if no games are available after atempting 5 times to find a game. 
			if (MasterServer.PollHostList ().Length == 0 && connectAttemps >= 5)
			{
				Debug.Log("You will be host");
				txt.text="You will be host";
				//you will be host
				MasterServer.RegisterHost(typeName, gameName, comment);
				gameConnected = true;
			
			}else if(hostData != null){
				attemptToConnect();
			}
			//checks if games are availableto join
			else if (MasterServer.PollHostList ().Length != 0 )
			{
				//Try to connect!
				hostIndex = 0;
				hostData = MasterServer.PollHostList();
				attemptToConnect();
			}
			else if(recieved){
				MasterServer.RequestHostList (gameName);
				recieved = false;

			}else{
				//keep waiting
			}
		          
		}
		else if(!gameStarted)
		{

			//once connected we need to see if game is ready to start
			float deltaT = Time.time - start;
			//checks if 4 people connected and waits 10 sec for more to join
			if(deltaT > 10 && Network.connections.Length>=1
			   ||Network.connections.Length == 3){
					
					txt.text = "starting game";
				Debug.Log ("Sending start...");
				Debug.Log (players.Count);
				networkView.RPC("StartGame",RPCMode.All,spawnx);


			}
			else
			{
				txt.text = "waiting for more players to join";
//				Debug.Log("waiting for more players "+ Network.connections.Length);
				//wait for more players
			}
		}
		else
		{
			//game is in play
		}
			
	}


	[RPC]
	public void getGameState(){
		Debug.Log("getGameState");
		if(Network.isServer){
			if(gameStarted){
				networkView.RPC ("isStarted", RPCMode.All);
			}else{
				networkView.RPC ("welcomeToGame", RPCMode.All);
			}
		}

	}
	[RPC]
	public void isStarted(){
		Debug.Log("isStarted");
		if(Network.isClient && !gameConnected){
			Network.Disconnect ();
			isConnecting = false;
		}
	}
	[RPC]
	public void welcomeToGame(){
		Debug.Log("welcomeToGame");
		isConnecting = false;
		gameConnected = true;
	}

	void attemptToConnect ()
	{
		if(!isConnecting){
			Network.Connect(hostData[hostIndex++]);
			isConnecting = true;
		}
		if(hostIndex >= hostData.Length){
			hostData = null;

		}
	}

	[RPC]
	void StartGame (int x)
	{

		Debug.Log("StartGame");
		spawn = new Vector2 (x, 2);
		//handle game initialization;
		if(!gameStarted){
			Debug.Log("Starting game");
			Network.Instantiate (playerPrefab, spawn,Quaternion.identity,0);
			gameStarted = true;
		}
	}
	
	private void StartServer()
	{
		if(!gameStarted && !gameConnected)
		{
			MasterServer.RegisterHost(typeName, gameName, comment);
		}
	}

	private void OnMasterServerEvent(MasterServerEvent mse)
	{
		
		if (mse == MasterServerEvent.RegistrationFailedGameName)
		{
			gameConnected = false;
			Debug.Log("Registration failed because an empty game name was given.");
		}
		else if (mse == MasterServerEvent.RegistrationFailedGameType)
		{
			gameConnected = false;
			Debug.Log("Registration failed because an empty game type was given.");
		}
		else if (mse == MasterServerEvent.RegistrationFailedNoServer)
		{
			gameConnected = false;
			Debug.Log("Registration failed because no server is running.");

		}
		else if (mse == MasterServerEvent.RegistrationSucceeded)
		{
			gameConnected = true;
			Debug.Log("Registration to master server succeeded, received confirmation.");
			if(txt != null)
			{
//				txt.text = "You are host";
			}
		}
		else if (mse == MasterServerEvent.HostListReceived)
		{

			Debug.Log("Received a host list from the master server.");
			if(MasterServer.PollHostList ().Length == 0){
				recieved = true;
				connectAttemps++;
			}
		}
			

	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		players.Remove (player);
		Network.DestroyPlayerObjects(player);
		if(Network.connections.Length == 1)
		{
			//endGame
			Network.Disconnect();
			Application.Quit();
		}
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer){
			Debug.Log("Local server connection disconnected");
			Application.Quit();
		}
		else if (info == NetworkDisconnection.LostConnection){
			Debug.Log("Lost connection to the server");
			Application.Quit();
		}
		else
			Debug.Log("Successfully diconnected from the server");
	}

	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
		if(txt != null)
		{
			txt.text = "Server Initializied";
		}

//		gameConnected = true;
	}

	void OnConnectedToServer() {
		if(txt != null)
		{
			txt.text = "Connected to server";
		}

		networkView.RPC ("getGameState", RPCMode.Server);
		isConnecting = true;
		Debug.Log("Connected to server");
	}

	void OnPlayerConnected(NetworkPlayer player) {
		if (players == null)
						players = new ArrayList ();
		txt.text = "player count " + players.Count; 
		players.Add (player);
	}
}

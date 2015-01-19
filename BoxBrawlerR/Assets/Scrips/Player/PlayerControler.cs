using UnityEngine;
using System.Collections.Generic;

public class PlayerControler : MonoBehaviour {


	//gravity stuff
	float g = 9.81f;
	float startT = 0f;
	float endT = 0f;
	float deltT = 0f;

	private PlayerAnimationHandler animationHandler;

	private InputHandler inputHandler;

	private PlayerValues values;


	protected Transform _transform;
	protected Rigidbody2D _rigidbody;


	// raycast stuff
	private RaycastHit2D hit;
	private Vector2 physVel = new Vector2();
	private Transform groundCheck;


	public virtual void Awake()
	{
		_transform = transform;
		_rigidbody = rigidbody2D;
		values = new PlayerValues ();
		animationHandler = new PlayerAnimationHandler (GetComponent<Animator>(), values);
//		inputHandler = new InplutHandlerKeyboard (values);

		inputHandler = new ScreanInput (values);
	}


	// Use this for initialization
	void Start () {
		groundCheck = transform.Find("GroundCheck");
		Physics2D.IgnoreLayerCollision (9, 9);
		startT = Time.time;

	}

	// Update is called once per frame
	void FixedUpdate () {



		if (networkView.isMine) 
		{
			inputHandler.updateInput ();
			move ();
			animationHandler.updateAnimation ();
		}
		else
		{
			//let update on own
		}

	}




	//when this is done it should all work better on network

	private void move(){

		physVel.x = getMoveVel (values.getDirectionalX());
		float increasedFall = getIncreasedFallVell(values.getDirectionalY());
		values.grounded = checkGrounded();
		Vector2 gravityVel = Vector2.zero;
		float vy = _rigidbody.velocity.y;


		if (values.grounded) {
			values.jumps = 0;
		}else{
//			_rigidbody.AddForce(-Vector3.up * (values.fallVel));
		}
		// jumpS
		if(values.getJumps() == PlayerValues.inputState.Jump)
		{
			if(values.jumps < values.maxJumps)
			{
				values.jumps += 1;
				vy = values.jumpVel;
//				_rigidbody.velocity = new Vector2(physVel.x, values.jumpVel);
				
			}
		}
		//calculate gravity yo

		endT = Time.time;
		deltT = endT - startT;
		startT = Time.time;

		//change to x and y seperate calcs dawg
//		vy = _rigidbody.velocity.y;
		if(!values.grounded){//still need to check something above
			vy = vy*increasedFall -g*deltT;
//			_rigidbody.velocity = new Vector2(physVel.x, _rigidbody.velocity.y*increasedFall + gravityVel.y);
		}else if ( vy < 0){
			vy = 0;
//			gravityVel = Vector2.zero;
//			_rigidbody.velocity = new Vector2(physVel.x, gravityVel.y);
		}
		// actually move the player
		_rigidbody.velocity = new Vector2(physVel.x, vy);


	}


	/***LOGIC METHODS ***/

	private float getMoveVel(PlayerValues.inputState currentInputState){

		switch (currentInputState) {
			
		case PlayerValues.inputState.WalkLeft:{
			return -values.walkVel;
		}
			
		case PlayerValues.inputState.WalkRight:{
			return values.walkVel;
		}
			
		case PlayerValues.inputState.RunLeft:{
			return -values.runVel;
		}
			
		case PlayerValues.inputState.RunRight:{
			return values.runVel;
		}
		case PlayerValues.inputState.None:
		default:
			return 0f;
	
		}


	}

	private float getIncreasedFallVell(PlayerValues.inputState currentInputState){
		
		if (_rigidbody.velocity.y < 0) 
		{
			if (currentInputState == PlayerValues.inputState.Fall) {
					return 1.02f;
			} else if (currentInputState == PlayerValues.inputState.Float) {
					return  .99f;
			} else {
					return 1f;
			}	
		}
		else
		{
			if (currentInputState == PlayerValues.inputState.Fall) {
				return .98f;
			} else if (currentInputState == PlayerValues.inputState.Float) {
				return  1.02f;
			} else {
				return 1f;
			}
		}
	}

	private bool checkGrounded()
	{
		return Physics2D.Linecast (_transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground"))
			||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x-.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))
				||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x+.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))	;
	}
	//TODO
	private bool checkAbove()
	{
		return Physics2D.Linecast (_transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground"))
			||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x-.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))
				||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x+.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))	;
	}
	//TODO
	private bool checkLeft()
	{
		return Physics2D.Linecast (_transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground"))
			||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x-.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))
				||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x+.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))	;
	}
	//TODO
	private bool checkRight()
	{
		return Physics2D.Linecast (_transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground"))
			||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x-.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))
				||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x+.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))	;
	}


	public void endAttack()
	{
		animationHandler.returnToIdle ();
	}
	public void catchRight(){
		animationHandler.catchRight ();

	}
	public void catchLeft(){
		animationHandler.catchLeft ();
		
	}
	void Respond ()
	{
		this.transform.position = new Vector2 (0f, 3f);
		this._rigidbody.velocity =   new Vector2 (0f, 0f);
	}

	public void kill()
	{	
		values.setLives (values.getLives() - 1);
//		if (values.getLives () > 0) {
				Respond ();
//		} 
//		else 
//		{
//			//checkIfEndGame
//		}
	}



	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		Vector3 syncPos = rigidbody2D.position;
		Vector3 velocity = rigidbody2D.velocity;
		
		stream.Serialize(ref syncPos);
		stream.Serialize(ref velocity);
		
		if (stream.isReading)
		{
			rigidbody2D.position = syncPos;
			rigidbody2D.velocity = velocity;
		}


	}

	
}

using UnityEngine;
using System.Collections;

public class InplutHandlerKeyboard :InputHandler {

	public PlayerValues playerVal;
	private float timerR = -1;
	private float timerL = -1;
	private float timeTellRun = 2f;

	public InplutHandlerKeyboard(PlayerValues val)
	{
		playerVal = val;
	}
	
	public override void updateInput()
	{
		
		// move left
		if (Input.GetKey (KeyCode.LeftArrow)|| Input.GetKey (KeyCode.A)) 
		{ 
			playerVal.setDirectionalX(PlayerValues.inputState.WalkLeft);
			playerVal.setFacing(PlayerValues.facing.Left);
			timerR = -1;
			if (timerL == -1) {
				timerL = Time.time;
			} else if (Time.time > timerL + timeTellRun) {
				//run
				playerVal.setDirectionalX(PlayerValues.inputState.RunLeft);
			}
		} else if (Input.GetKey (KeyCode.RightArrow)|| Input.GetKey (KeyCode.D)) {
			playerVal.setDirectionalX(PlayerValues.inputState.WalkRight);
			playerVal.setFacing(PlayerValues.facing.Right);
			timerL = -1;
			if (timerR == -1) {
				timerR = Time.time;
			} else if (Time.time > timerR + timeTellRun) {
				//run
				playerVal.setDirectionalX(PlayerValues.inputState.RunRight);
			}
			
		} else {
			timerL = -1;
			timerR = -1;
			playerVal.setDirectionalX(PlayerValues.inputState.None);
		}
		
		//float or fall now is the time 
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
			playerVal.setDirectionalY(PlayerValues.inputState.Fall);
		} else if (Input.GetKey (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
			playerVal.setDirectionalY(PlayerValues.inputState.Float);
		} else {
			playerVal.setDirectionalY(PlayerValues.inputState.None);
		}
		
		// jump
		if (Input.GetKeyDown (KeyCode.Space)) { 
			playerVal.setJumps(PlayerValues.inputState.Jump);
		} else {
			playerVal.setJumps(PlayerValues.inputState.None);
		}
		
		if (Input.GetKeyDown (KeyCode.F)) {
			playerVal.setAttack(PlayerValues.inputState.HitMain);
		} else if (Input.GetKey (KeyCode.D)) {

		} else {
			playerVal.setAttack(PlayerValues.inputState.None);
		}
		

	}



}

using UnityEngine;
using System.Collections;

public class PlayerValues  {

	private facing dir = facing.Left;
	private inputState directionalX = inputState.None;
	private inputState directionalY = inputState.None;
	private inputState jump = inputState.None;
	private inputState attack = inputState.None;
	private int lives = 5;

	public float scale;
	public float runVel;  	
	public float walkVel; 	
	public float jumpVel; 	
	public float fallVel;
	public int maxJumps;

	public int jumps = 0;		
	public bool grounded = false;

	public PlayerValues()
	{
		scale = 3f;
		runVel  = scale * 3f;
		walkVel = scale *2f;
		jumpVel = scale *2.5f;
		fallVel = scale *2f;
		maxJumps = 2;
		
	}

	public enum facing{
		Left,
		Right
	}

	public enum inputState 
	{ 
		None,
		WalkLeft, 
		WalkRight,
		RunRight,
		RunLeft,
		Jump,
		Fall,
		Float,
		HitMain
	}
	public void setLives(int lives)
	{
		this.lives = lives;
	}
	public int getLives()
	{
		return lives;
	}
	public facing getFacing()
	{
		return dir;
	}
	public void setFacing(facing toSet)
	{
		this.dir = toSet;
	}
	public facing switchFacing(){
		dir = dir == facing.Left ? facing.Right : facing.Left;
		return dir;
	}

	public inputState getDirectionalX()
	{
		return directionalX;
	}
	public inputState getDirectionalY()
	{
		return directionalY;
	}
	public inputState getJumps()
	{
		return jump;
	}
	public inputState getAttack(){
		return attack;
	}


	public void setDirectionalX(inputState toSet)
	{
		directionalX = toSet;
	}
	public void setDirectionalY(inputState toSet)
	{
		 directionalY = toSet;
	}
	public void setJumps(inputState toSet)
	{
		 jump = toSet;
	}
	public void setAttack(inputState toSet){
		 attack = toSet;
	}



	/*Player Spicific Values*/

	public class ThornValues : PlayerValues
	{
		public ThornValues()
		{
			scale = 3f;
			runVel  = scale * 3f;
			walkVel = scale *2f;
			jumpVel = scale *2.5f;
			fallVel = scale *2f;
			maxJumps = 2;
			
		}
	}

}

﻿using UnityEngine;
using System.Collections;

public class KillZonehandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



	}

	void OnTriggerExit2D(Collider2D other) {


		if (other.gameObject.CompareTag("Player"))
		{
			 other.gameObject.SendMessage("kill");

		}
	}
}
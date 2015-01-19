using UnityEngine;
using System.Collections;

public class LeftCorner : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
//			PlayerControler control = (PlayerControler) other.GetComponent<PlayerControler>();
//			control.catchLeft();
//			//move to correct location
//			other.gameObject.transform.position = this.transform.position + new Vector3(-64,-32,0);
//			//freeze at positon for a while
//			other.gameObject.rigidbody2D.isKinematic = true;
		}
	}
}

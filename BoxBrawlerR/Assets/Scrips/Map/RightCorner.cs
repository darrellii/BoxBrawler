using UnityEngine;
using System.Collections;

public class RightCorner : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
//			//animate yoyo
//			PlayerControler control = other.GetComponent<PlayerControler>();
//			control.catchRight();
//			//move to correct location
//			other.gameObject.transform.position = this.transform.position + new Vector3(64,-32,0);
//			//freeze at positon for a while
//			other.gameObject.rigidbody2D.isKinematic = true;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaScript : MonoBehaviour {

	public Transform Player;

	void LateUpdate()
	{
		Vector3 newPosition = Player.position;
		newPosition.y = transform.position.y;
		transform.position = newPosition;
	}
}

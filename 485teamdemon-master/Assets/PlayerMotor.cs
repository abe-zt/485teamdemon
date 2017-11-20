using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();

    }
	

}

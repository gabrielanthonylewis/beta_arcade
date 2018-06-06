using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gabriel Lewis (Q5094111)

[RequireComponent(typeof(AIDynamicStats))]
public class AIBomb : MonoBehaviour 
{

	private AIDynamicStats _AIDyanmicStats = null;
	private Transform _player1 = null;
	private Transform _player2 = null;

	void Start()
	{
		_AIDyanmicStats = this.GetComponent<AIDynamicStats> ();
		_player1 = GameObject.FindGameObjectWithTag ("P1").transform;
		_player2 = GameObject.FindGameObjectWithTag("P2").transform;

	}


	void Update()
	{
		// Left
		if (_AIDyanmicStats.side == false) 
		{
			if (this.transform.position.z < (_player1.transform.position + _player1.transform.forward * 0.0f).z)
				this.GetComponent<AIHealth> ().DieAndMinusScoreFrom (_player1.gameObject);
		}
		// Right
		else if (_AIDyanmicStats.side == true) 
		{
			if (this.transform.position.z < (_player2.transform.position + _player2.transform.forward * 0.0f).z)
				this.GetComponent<AIHealth> ().DieAndMinusScoreFrom (_player2.gameObject);
		}
	}
}

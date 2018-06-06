using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gabriel Lewis (Q5094111)

public class RoomMove : MonoBehaviour {

	[SerializeField]
	private float _speedMulti = 8.0f;



	private MovingBackground _MovingBackground = null;

	void Start()
	{
		_MovingBackground = GameObject.FindObjectOfType<MovingBackground> ();
	}

	// Update is called once per frame
	void Update ()
	{
		
		this.transform.position += -Camera.main.transform.forward * Time.deltaTime * _speedMulti;

		if (this.transform.position.z < Camera.main.transform.position.z - _MovingBackground._roomLength / 1.5f)
		{
			_MovingBackground.RemoveRoom (this.gameObject);
		}

	}

}

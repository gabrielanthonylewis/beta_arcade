using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gabriel Lewis (Q5094111)

public class AISimpleMove : MonoBehaviour {

	float _speed = 10.0f;
	public float speed { set { _speed = value; } }

    [SerializeField]
    bool Powerup = false;
	bool killWave = false;

	private Transform _deathWall = null;
	private float _extents = 0.0f;

	void Start()
	{

		_deathWall = GameObject.FindGameObjectWithTag ("DeathWall").transform;
		_extents = 0.5f;
	}
		
    // Update is called once per frame
    void Update () {


		Vector3 newPosition = this.transform.position;

		newPosition.z -= _speed * Time.deltaTime;

		this.transform.position = newPosition;


        Vector3 cameraForward = Camera.main.transform.forward;
        //Vector3 thisForward = this.transform.forward;



        // Behind if Rotation(0,0,0)
        if (Powerup == false && killWave == false)
        {
			if (this.transform.position.z < _deathWall.position.z - _extents
				|| this.transform.position.z < Camera.main.transform.position.z)
            {
                if (this.GetComponent<AIWave>() != null)
                {
                   // Debug.Log("DESTROY WAVE :) " + this.gameObject.name);

                    this.GetComponent<AIWave>().KillWave(this.gameObject);
					killWave = true;
                }
      
              //  Destroy(this.gameObject);
            }
        }

        // Steve
        if (Powerup == true && gameObject.GetComponent<PowerupBehave>().Activate() == false)
        {//end Steve
			
			if (this.transform.position.z < Camera.main.transform.position.z)
            {
                if (this.GetComponent<AIWave>() != null)
                    this.GetComponent<AIWave>().KillWave(this.gameObject);

				// Steve
               // Debug.Log("DESTROY");
                  Destroy(this.gameObject);
				//end Steve
            }
        }
      
    }

	public void MultiplySpeed(float multiplier)
	{
		_speed *= multiplier;
	}

}

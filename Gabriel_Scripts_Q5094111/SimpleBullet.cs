using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gabriel Lewis (Q5094111)

public class SimpleBullet : MonoBehaviour
{

   
    public GameObject shooter;
    public int damage;

    //steve
    [SerializeField]
    GameObject explosion = null;

    private bool Rocket = false;
    private Collider[] Boom;
	
	public void Setdamage(int otherdam)
	{
		damage = otherdam;
	}

    public void Rocketmode(bool on)
    {
        Rocket = on;
    }
    //end steve

    void OnEnable () 
	{
		StartCoroutine(WaitThenDie(5.0f));
	}

	IEnumerator WaitThenDie(float time)
	{
		yield return new WaitForSeconds (time);

		ObjectPool.Instance.DestroyObject (this.gameObject);
	}

	void OnTriggerEnter(Collider other)
	{

        // Steve
        if (Rocket == true)
        {
            Boom = Physics.OverlapSphere(gameObject.transform.position, 20.0f);
            Debug.Log("Boom");

			// Gabriel//
            XInputDotNetPure.PlayerIndex playerIndex = XInputDotNetPure.PlayerIndex.One;
            if (shooter.tag == "P2")
                playerIndex = XInputDotNetPure.PlayerIndex.Two;

            RumbleManager.instance.SetVibration(playerIndex, 40.0f, 0.15f);
			// END Gabriel//

            foreach (Collider Drone in Boom)
            {
                if (Drone.tag == "Enemy")
                {
                    // Destroy(Drone.gameObject);
                    Drone.gameObject.GetComponent<AIHealth>().TakeDamage(100, shooter);
                    ObjectPool.Instance.DestroyObject(Drone.gameObject);

					Debug.Log (shooter);
                }

            }

            GameObject rocket = Instantiate(explosion, this.transform.localPosition, this.transform.localRotation);
            Debug.Log(rocket.name);
            rocket.transform.position = this.transform.position;

            //Destroy(gameObject);
            ObjectPool.Instance.DestroyObject(gameObject);
            return;
        }
        //end steve

        if (other.gameObject.GetComponent<AIHealth>() && shooter.tag != "Enemy")
        {
           

            XInputDotNetPure.PlayerIndex playerIndex = XInputDotNetPure.PlayerIndex.One;
			if (shooter.tag == "P2")
				playerIndex = XInputDotNetPure.PlayerIndex.Two;

			RumbleManager.instance.SetVibration (playerIndex, 15.0f, 0.05f);


            other.gameObject.GetComponent<AIHealth>().TakeDamage(1, shooter);  

			ObjectPool.Instance.DestroyObject (this.gameObject);
			return;
        }
        else if (other.tag == "P1" || other.tag == "P2")
        {
			// Subtle rumble
			XInputDotNetPure.PlayerIndex playerIndex = XInputDotNetPure.PlayerIndex.One;
			if (other.tag == "P2")
				playerIndex = XInputDotNetPure.PlayerIndex.Two;
			RumbleManager.instance.SetVibration(playerIndex, 60.0f, 0.2f);

			// Steve
            other.gameObject.GetComponent<PlayerScore>().player_hit(damage);
            Debug.Log(other.tag + damage);
			// end steve
           
			ObjectPool.Instance.DestroyObject (this.gameObject);
			return;
        }



        if (shooter != null && shooter.tag == "Enemy")
        {
			if (other.tag != "Enemy" && other.tag != "Bullet") {
				ObjectPool.Instance.DestroyObject (this.gameObject);
				return;
			}
        }
       
		if (other.tag != "Player" && other.tag != "Bullet")
			ObjectPool.Instance.DestroyObject (this.gameObject);
    }
}

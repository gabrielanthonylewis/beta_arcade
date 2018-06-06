using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

// Gabriel Lewis (Q5094111)

public class RumbleManager : MonoBehaviour 
{
	public static RumbleManager instance;

	void Awake()
	{
		if (instance != null)
			Destroy (this.gameObject);
		else
			instance = this;
	}

	void Start()
	{
		//SetVibration (PlayerIndex.One, 50.0f, 5.0f);
	}

	public void SetVibration(PlayerIndex player, float vibrationLevel, float time)
	{
		GamePad.SetVibration (player, vibrationLevel, vibrationLevel);

		StopAllCoroutines ();

        GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
        GamePad.SetVibration(PlayerIndex.Three, 0.0f, 0.0f);
        GamePad.SetVibration(PlayerIndex.Four, 0.0f, 0.0f);

        StartCoroutine(StopVibration(player, time));
	}


	IEnumerator StopVibration(PlayerIndex player, float time)
	{
		yield return new WaitForSeconds (time);

		GamePad.SetVibration (player, 0.0f, 0.0f);
	}

	void OnDisable()
	{
		GamePad.SetVibration (PlayerIndex.One, 0.0f, 0.0f);
		GamePad.SetVibration (PlayerIndex.Two, 0.0f, 0.0f);
		GamePad.SetVibration (PlayerIndex.Three, 0.0f, 0.0f);
		GamePad.SetVibration (PlayerIndex.Four, 0.0f, 0.0f);
	}
}

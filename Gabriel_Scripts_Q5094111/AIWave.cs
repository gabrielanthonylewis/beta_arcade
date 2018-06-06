using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gabriel Lewis (Q5094111)

public class AIWave : MonoBehaviour {

    public AIWaveManager owner = null;

    public enum eSpawnPosition
    {
        BOTTOM,
        TOP,
		MIDDLE
    };

	public enum eSpawnSide
	{
		LEFT,
		RIGHT,
		MIDDLE
	};

    //[SerializeField]
    //private GameObject enemyPrefab = null;

    //steve
    [SerializeField]
    private AudioClip spawned;

    [SerializeField]
    private AudioSource mysource;
    //
    private const float _space = 0.0f;

    private int _currentCount = 0;


    private int _lastWaveQuantity = 0;
	private eSpawnSide _lastWavePosition;
	private string _lastAIWaveName = "";
	private eSpawnPosition _lastWaveSpawnPos = eSpawnPosition.BOTTOM;
	private bool _lastShouldFlip = true;
	private bool _lastFlipped = false;

	private float _xOffset = 1.5f;
	private float _yOffset = 2.5f;
	GameObject[] newEnemies;

	private StageManager _StageManager = null;
	private AISimpleMove _AISimpleMove = null;

	void Start()
	{
		_AISimpleMove = this.GetComponent<AISimpleMove> ();
	}

	public void Spawn(int quantity, AIWave.eSpawnPosition spawnPos, eSpawnSide side, GameObject aiPrefab, bool flipped, bool shouldFlip)
    {
        _lastWaveQuantity = quantity;
        _lastWavePosition = side;
		_lastAIWaveName = aiPrefab.name;
		_lastWaveSpawnPos = spawnPos;
		_lastShouldFlip = shouldFlip;
		_lastFlipped = flipped;

        //stephen
        mysource.PlayOneShot(spawned);
		// end stephen

        // Spawn

        newEnemies = new GameObject[quantity];
        for (int i = 0; i < quantity; i++)
        { 
            //newEnemies[i] = Instantiate(aiPrefab, Vector3.zero, aiPrefab.transform.rotation, this.transform) as GameObject;
			newEnemies[i] = ObjectPool.Instance.InstantiateObject(aiPrefab.name, Vector3.zero);
			newEnemies[i].transform.SetParent (this.transform);
            newEnemies[i].name = aiPrefab.name;

			if(newEnemies[i].GetComponent<AIHealth>() != null)
        	    newEnemies[i].GetComponent<AIHealth>().owner = this.GetComponent<AIWave>();
			else
				newEnemies[i].GetComponentInChildren<AIHealth>().owner = this.GetComponent<AIWave>();
			
			if(side == eSpawnSide.LEFT)
        	    newEnemies[i].GetComponent<AIDynamicStats>().side = false;
			else if(side == eSpawnSide.RIGHT)
				newEnemies[i].GetComponent<AIDynamicStats>().side = true;
			
            _currentCount++;
        }

		if (flipped == true) 
		{
			for (int i = 0; i < newEnemies.Length; i++) 
			{
				// Buff stats
				newEnemies [i].GetComponent<AIHealth> ().Flip ();
				newEnemies [i].GetComponent<AIScoreGabriel> ().Flip ();
			}
		}

		// Buff stats depending on Level
		if(_StageManager == null)
			_StageManager = GameObject.FindObjectOfType<StageManager> ();
		for (int i = 0; i < newEnemies.Length; i++) 
		{
			// Buff stats
			int healthBuffValue = 0;

			if (_StageManager.currentStage < 3) {
				healthBuffValue = -2;
			} else if (_StageManager.currentStage >= 7 && _StageManager.currentStage <= 12) {
                if(_AISimpleMove != null)
                  _AISimpleMove.MultiplySpeed (1.1f);
				healthBuffValue = 1;
			} else if (_StageManager.currentStage > 12 && _StageManager.currentStage <= 18) {
                if (_AISimpleMove != null)
                    _AISimpleMove.MultiplySpeed (1.1f);
				healthBuffValue = 2;
			} else if (_StageManager.currentStage >= 20) {
                if (_AISimpleMove != null)
                    _AISimpleMove.MultiplySpeed (1.1f);
				healthBuffValue = 3;
			}

			if(healthBuffValue > 3)
				healthBuffValue = 3;

            if (newEnemies[i].GetComponent<AIHealth>() != null)
                newEnemies [i].GetComponent<AIHealth> ().IncreaseHealth (healthBuffValue);
            //   else
            //     newEnemies[i].GetComponentInChildren<AIHealth>().IncreaseHealth(healthBuffValue);
        }


		this.GetComponent<AISimpleMove> ().speed = GetSmallestMovementSpeed(newEnemies);


		Vector3 mostLeftPosition = Vector3.zero;
		mostLeftPosition.x -= Mathf.Floor((float)quantity / 2.0f) * (_xOffset + _space);

        switch (spawnPos)
        {
            case eSpawnPosition.BOTTOM:

                for (int i = 0; i < newEnemies.Length; i++)
                {
                    Vector3 newPosition = mostLeftPosition;
					newPosition.x += i * (_xOffset + _space);

                    newEnemies[i].transform.localPosition = newPosition;
                }

                break;

			case eSpawnPosition.MIDDLE:

				for (int i = 0; i < newEnemies.Length; i++)
				{
					Vector3 newPosition = mostLeftPosition;
					newPosition.x += i * (_xOffset + _space);
					newPosition.y = 2f;

					newEnemies[i].transform.localPosition = newPosition;
				}
				break;

            case eSpawnPosition.TOP:

				for (int i = 0; i < newEnemies.Length; i++)
				{
					Vector3 newPosition = mostLeftPosition;
					newPosition.x += i * (_xOffset + _space);
					newPosition.y = 4.0f;

					newEnemies[i].transform.localPosition = newPosition;
				}


                break;
        }

    }

	private float GetSmallestMovementSpeed(GameObject[] enemies)
	{
		float smallest = Mathf.Infinity;

		foreach (GameObject enemy in enemies) 
		{
			AIStats stats = enemy.GetComponent<AIStats> ();

			if (stats.movementSpeed < smallest)
				smallest = stats.movementSpeed;
		}

		return smallest;
	}

    public void KillWave(GameObject waveObj)
    {
		 int childCount = this.transform.childCount;
		if (this.transform != null)
		{
			for (int i = 0; i < childCount; i++) 
			{
				if (this.transform.GetChild (0) != null && this.transform.GetChild (0).gameObject != null)
					ObjectPool.Instance.DestroyObject (this.transform.GetChild (0).gameObject);
			}
		}
			
        // WAVE KILLED
        owner.WaveKilled();

        // if(waveObj != null)
        //  Destroy(waveObj);
        StartCoroutine(DestroyWaveRou(waveObj));
    }

    IEnumerator DestroyWaveRou(GameObject wave)
    {
        yield return new WaitForSeconds(3.0f);

        Destroy(wave);
    }

    public void EnemyKilled(int count)
    {
      
        _currentCount -= count;

        if (_currentCount <= 0)
        {
			
			if (_lastShouldFlip == false || _lastFlipped == true)
				return;

            // PASS OVER!!

            mysource.pitch = 1.5f;
            mysource.PlayOneShot(spawned);
            mysource.pitch = 1.0f;

            eSpawnSide oppositePos = _lastWavePosition;
			if (oppositePos == eSpawnSide.LEFT)
				oppositePos = eSpawnSide.RIGHT;
			else if (oppositePos == eSpawnSide.RIGHT)
				oppositePos = eSpawnSide.LEFT;

			owner.SpawnWave(_lastWaveQuantity, _lastWaveSpawnPos, oppositePos, _lastAIWaveName, true, true);


			owner.WaveFlipped();

            //Debug.Log("Spawn Wave of " + _lastWaveQuantity  + " enemies at the " + AIWave.eSpawnPosition.BOTTOM.ToString() + " to side " + !_lastWavePosition);
        }
    }

}

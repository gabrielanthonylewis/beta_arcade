using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gabriel Lewis (Q5094111)

public class AIWaveManager : MonoBehaviour 
{

    [SerializeField]
    private float _spawnDistanceFromCamera = 25.0f;

    [SerializeField]
    private GameObject wavePrefab = null;

    [SerializeField]
    private float spawnY = 2.75f;

	[SerializeField]
	private float roomsizeX = 18.7f;


    [System.Serializable]
    struct AITypes
    {
        public string name;
        public GameObject obj;
        public float chance;
		public int quantity;
		public AIWave.eSpawnPosition spawnPosition;
    };

    [SerializeField]
    private AITypes[] _AITypes;

    StageManager _StageManager = null;

    enum AIType
    {
        Drone,
        HeavyDrone

    };





    void Start()
	{
        _StageManager = this.GetComponent<StageManager>();
    }

    public void SpawnWave(StageManager.eAIType[] types, int numberOfWaves)
    {
        StartCoroutine(WaitThenSpawn(types, numberOfWaves));
    }


	IEnumerator WaitThenSpawn(StageManager.eAIType[] types, int numberOfWaves)
	{
        
        //while (true == true)
        int currentNum = 0;
        while(currentNum < numberOfWaves || numberOfWaves == -1)
        {
            // Pick type to spawn
            //  int type = Random.Range(0, System.Enum.GetNames(typeof(AIType)).Length);
            //  AIType aiType = (AIType)System.Enum.ToObject(typeof(AIType), type);

            /*float topBottomRand = Random.Range(0, 100);
            AIWave.eSpawnPosition spawnPos = AIWave.eSpawnPosition.BOTTOM;
            if (topBottomRand > 66)
                spawnPos = AIWave.eSpawnPosition.BOTTOM;
            else
                spawnPos = AIWave.eSpawnPosition.TOP;
			*/


            if (types[0] == StageManager.eAIType.All)
            {
               // Debug.Log("ALLLLLL");
                float rand = Random.Range(0, 100);
                
                // Spawn Drones
                if (rand >= 0 && rand < _AITypes[0].chance)
                {
					SpawnBothWaves(_AITypes[0].quantity, _AITypes[0].spawnPosition, _AITypes[0].obj);
                }
                // Spawn Heavy Drones
                else if (rand >= (0 + _AITypes[0].chance) && rand < _AITypes[0].chance + _AITypes[1].chance)
                {
					SpawnBothWaves(_AITypes[1].quantity, _AITypes[1].spawnPosition, _AITypes[1].obj);
                }
                // Spawn Fast Drone
                else if (rand >= (_AITypes[0].chance + _AITypes[1].chance) && rand < _AITypes[0].chance + _AITypes[1].chance + _AITypes[2].chance)
                {
					SpawnBothWaves(_AITypes[2].quantity, _AITypes[2].spawnPosition, _AITypes[2].obj);
                }
                // Spawn Bomb Drone
                else
                {
                   // SpawnBothWaves(_AITypes[3].quantity, AIWave.eSpawnPosition.MIDDLE, _AITypes[3].obj);
					SpawnWave(_AITypes[3].quantity, AIWave.eSpawnPosition.MIDDLE, AIWave.eSpawnSide.LEFT, _AITypes[3].obj, false, true); //todo, Left or Right RNADOM
                }
            }
            else if(types.Length == 1)
            {
                // no random

                int idx = (int)types[0];

				if (types [0] == StageManager.eAIType.Boss)
					SpawnWave (_AITypes [4].quantity, AIWave.eSpawnPosition.MIDDLE, AIWave.eSpawnSide.MIDDLE, _AITypes [4].obj, false, true);
				else if(types [0] == StageManager.eAIType.BombDrone)
					SpawnWave (_AITypes [3].quantity, AIWave.eSpawnPosition.MIDDLE, AIWave.eSpawnSide.LEFT, _AITypes [3].obj, false, true);
				else
					SpawnBothWaves(_AITypes[idx].quantity, _AITypes[idx].spawnPosition, _AITypes[idx].obj);
            }
            else
            {
                // random

                float total = types.Length; // for chance
                float rand = Random.Range(0, 100);
                int spawns = 0;

                int idx = (int)types[0];
                if (rand >= 0 && rand < _AITypes[idx].chance)
                {
					SpawnBothWaves(_AITypes[idx].quantity, _AITypes[idx].spawnPosition, _AITypes[idx].obj);
                    spawns++;
                }
                else
                {
                    float accumChance = _AITypes[idx].chance;
                    for(int idxI = 1; idxI < total; idxI++)
                    {
                        if (rand >= accumChance 
                            && rand < _AITypes[idxI - 1].chance + _AITypes[idxI].chance)
                        {
							SpawnBothWaves(_AITypes[idxI].quantity, _AITypes[idxI].spawnPosition, _AITypes[idxI].obj);
                            spawns++;
                        }
                        accumChance += _AITypes[idxI].chance;
                    }

                    
                }
                if (spawns == 0)
                {
					SpawnBothWaves(_AITypes[types.Length - 1].quantity, _AITypes[types.Length - 1].spawnPosition, _AITypes[types.Length - 1].obj);
                }



            }

            currentNum++;

            yield return new WaitForSeconds(3.0f);
        }

        _StageManager.StageComplete();
        
    }

  
    public void WaveKilled()
    {
        _StageManager.WaveKilled();
    }

	public void WaveFlipped()
	{
		_StageManager.WaveFlipped ();
	}

    void SpawnBothWaves(int quantity, AIWave.eSpawnPosition spawnPos, GameObject aiPrefab)
    {

        SpawnWave(quantity, spawnPos, AIWave.eSpawnSide.LEFT, aiPrefab, false, true);
        SpawnWave(quantity, spawnPos, AIWave.eSpawnSide.RIGHT, aiPrefab, false, true);
    }

	public void SpawnWave(int quantity, AIWave.eSpawnPosition spawnPos, AIWave.eSpawnSide side, GameObject aiPrefab, bool flipped, bool shouldFlip)
	{

        GameObject newWave = Instantiate(wavePrefab) as GameObject;
        
        newWave.GetComponent<AIWave>().owner = this.GetComponent<AIWaveManager>();

        Vector3 newPosition = Vector3.zero;

		if (side == AIWave.eSpawnSide.LEFT)
			newPosition.x -= roomsizeX / 2.0f ;//Camera.main.ViewportToWorldPoint(new Vector3(0.25f, 0.5f, 0.0f));
		else if (side == AIWave.eSpawnSide.RIGHT)
			newPosition.x += roomsizeX / 2.0f + 2.0f;//Camera.main.ViewportToWorldPoint(new Vector3(0.25f, 0.5f, 0.0f));

        newPosition.y = spawnY;
        newPosition.z = _spawnDistanceFromCamera;

        newWave.transform.position = newPosition;

        newWave.GetComponent<AIWave>().Spawn(quantity, spawnPos, side, aiPrefab, flipped, shouldFlip);

	}

	public void SpawnWave(int quantity, AIWave.eSpawnPosition spawnPos, AIWave.eSpawnSide side, string aiName, bool flipped, bool shouldFlip)
    {
        GameObject aiPrefab = null;

		if (aiName == "AIDrone")
			aiPrefab = _AITypes [0].obj;
		else if (aiName == "AIHeavyDrone")
			aiPrefab = _AITypes [1].obj;
		else if (aiName == "AIFastDrone")
			aiPrefab = _AITypes [2].obj;
		else if (aiName == "AIBombDrone")
			aiPrefab = _AITypes [3].obj;

        if (aiPrefab == null)
            return;

		SpawnWave(quantity, spawnPos, side, aiPrefab, flipped, shouldFlip);
    }
}

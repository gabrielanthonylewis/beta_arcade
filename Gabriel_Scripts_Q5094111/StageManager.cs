using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Gabriel Lewis (Q5094111)

public class StageManager : MonoBehaviour
{
    [SerializeField]
    int _currentStage = -1;
	public int currentStage { get { return _currentStage; } }

    public enum eAIType
    {
        Drone = 0,
        HeavyDrone = 1,
        FastDrone = 2,
        BombDrone = 3,
        Boss = 4,
        All = 5,
		Rest = 6
    }

    [System.Serializable]
    struct Stage
    {
        public eAIType[] types;
        public int numberOfWaves;

    };

    [SerializeField]
    Stage[] stages;


    public GameObject endScreen;

    AIWaveManager _AIWaveManager = null;

    private bool stageComplete = false;
    private bool stageKilled = false;
    private
         int numWavesKilled = 0;

	[SerializeField]
	private GameObject _dialogueWindow;
	[SerializeField]
	private Text _text;
	[SerializeField]
	private Image _personTalking;


    [SerializeField]
    private Sprite[] _possibleTalking;

	private PowerupManager _PowerupManager = null;


	//steve
	public int GetStage()
	{
		return _currentStage;
	}
	//end steve

    void Start()
    {
		_PowerupManager = GameObject.FindObjectOfType<PowerupManager> ();
        _AIWaveManager = this.GetComponent<AIWaveManager>();

        StageSpawn();
    }


    private void Update()
    {
        if(stageComplete && stageKilled)
        {
           
            NextStage();

            stageComplete = false;
            stageKilled = false;
        }
    }

    private void NextStage()
    {
        if (stages[_currentStage].types[0] == eAIType.All)
            return;

        _currentStage++;

        if (_currentStage >= stages.Length)
        {
            Debug.Log("GAME OVER");
            return;
        }

        //Debug.Log("NEXT STAGE");


        StageSpawn();
    }

    private void StageSpawn()
    {
        numWavesKilled = 0;

        if (stages[_currentStage].types[0] != eAIType.Rest)
        {
            // Debug.LogWarning("SPAWN");
            _AIWaveManager.SpawnWave(stages[_currentStage].types, stages[_currentStage].numberOfWaves);
        }
        else
        {
            StartCoroutine("RandomDialogue");
        }
    }

    IEnumerator RandomDialogue()
    {
		_PowerupManager.powerUpSpawn = false;

        _dialogueWindow.SetActive(true);


        //Ryan
        if (_currentStage == 0)
        {

            _dialogueWindow.SetActive(false);

            yield return new WaitForSeconds(2.0f);

            _dialogueWindow.SetActive(true);


            _text.text = "This must have been here for years. How is it we are just getting a mission here?";

            _personTalking.sprite = _possibleTalking[4];

            yield return new WaitForSeconds(6.0f);


            _text.text = "I dunno but we may be able to find some sweet loot lying around.";

            _personTalking.sprite = _possibleTalking[8];

            yield return new WaitForSeconds(5.0f);


            _text.text = "That's not why we're here Bam. We have a mission to answer a distress call... Wait do you hear that?";

            _personTalking.sprite = _possibleTalking[0];

            yield return new WaitForSeconds(2.0f);

            _personTalking.sprite = _possibleTalking[4];

        }

        if (_currentStage == 4)
        {

            _dialogueWindow.SetActive(false);

            yield return new WaitForSeconds(2.0f);

            _dialogueWindow.SetActive(true);


            _text.text = "That was way too easy!";

            _personTalking.sprite = _possibleTalking[9];

            yield return new WaitForSeconds(3.0f);


            _text.text = "Bam, shut up!";

            _personTalking.sprite = _possibleTalking[0];

            yield return new WaitForSeconds(3.0f);


            _text.text = "Why? It was.";

            _personTalking.sprite = _possibleTalking[8];

            yield return new WaitForSeconds(3.0f);


            _text.text = "Nice one, Bam. Look!";

            _personTalking.sprite = _possibleTalking[0];

        }

        if (_currentStage == 8)
        {

            _dialogueWindow.SetActive(false);

            yield return new WaitForSeconds(2.0f);

            _dialogueWindow.SetActive(true);


            _text.text = "These powerups sure do come in handy.";

            _personTalking.sprite = _possibleTalking[2];

            yield return new WaitForSeconds(5.0f);


            _text.text = "Yeah, they make me even better than I already am and that's saying something.";

            _personTalking.sprite = _possibleTalking[9];

            yield return new WaitForSeconds(7.0f);


            _text.text = "Ha ha. Yeah ok, Bam.";

            _personTalking.sprite = _possibleTalking[3];

        }

        if (_currentStage == 12)
        {

            _dialogueWindow.SetActive(false);

            yield return new WaitForSeconds(2.0f);

            _dialogueWindow.SetActive(true);


            _text.text = "Have you even seen those weird looking machines before? I don't think they are part of the BB Security Line.";

            _personTalking.sprite = _possibleTalking[4];

            yield return new WaitForSeconds(9.0f);


            _text.text = "Yeah, I don't think they are. Also, that teleporting function and not knowing when it explodes is annoying.";

            _personTalking.sprite = _possibleTalking[10];

            yield return new WaitForSeconds(3.0f);

            _personTalking.sprite = _possibleTalking[6];

            yield return new WaitForSeconds(5.0f);


            _text.text = "Let's just keep going. I don't think we have far to go.";

            _personTalking.sprite = _possibleTalking[0];

            yield return new WaitForSeconds(1.0f);

        }

        if (_currentStage == 16)
        {

            _dialogueWindow.SetActive(false);

            yield return new WaitForSeconds(2.0f);

            _dialogueWindow.SetActive(true);


            _text.text = "Looks like that's the last of the drones.";

            _personTalking.sprite = _possibleTalking[11];

            yield return new WaitForSeconds(5.0f);


            _text.text = "Yeah, I think you're right.";

            _personTalking.sprite = _possibleTalking[5];

            yield return new WaitForSeconds(3.0f);


            _text.text = "So, you two are the ones intruding in my place of work.";

            _personTalking.sprite = _possibleTalking[12];

            yield return new WaitForSeconds(4.0f);


            _text.text = "Who was that and how did they hack the comm link?";

            _personTalking.sprite = _possibleTalking[4];

            yield return new WaitForSeconds(5.0f);


            _text.text = "I have no clue.";

            _personTalking.sprite = _possibleTalking[10];

            yield return new WaitForSeconds(2.0f);


            _text.text = "My name is Cinereus, Professor Cinereus. You shall pay for disrupting my work and destroying my drones.";

            _personTalking.sprite = _possibleTalking[13];

            yield return new WaitForSeconds(2.0f);

        }

        if (_currentStage == 18)
        {

            _dialogueWindow.SetActive(false);

            yield return new WaitForSeconds(2.0f);

            _dialogueWindow.SetActive(true);


            _text.text = "Arh, how did you two beat my machines?";

            _personTalking.sprite = _possibleTalking[13];

            yield return new WaitForSeconds(4.0f);


            _text.text = "You clearly have never heard of us. We are Bam and Boo, Bounty Hunters.";

            _personTalking.sprite = _possibleTalking[11];

            yield return new WaitForSeconds(5.0f);


            _text.text = "Not a good idea, Bam";

            _personTalking.sprite = _possibleTalking[1];

            yield return new WaitForSeconds(3.0f);


            _text.text = "Bounty Hunter, eh? Well this isn't the last you've seen of me. I will have my revenge.";

            _personTalking.sprite = _possibleTalking[13];

            yield return new WaitForSeconds(7.0f);


            _text.text = "Bring it on! We'll win a second time.";

            _personTalking.sprite = _possibleTalking[11];

            yield return new WaitForSeconds(4.0f);


            _text.text = "Bam! Please be quiet! We aren't prepared for another fight.";

            _personTalking.sprite = _possibleTalking[1];

            yield return new WaitForSeconds(4.0f);

            _text.text = "Fine, but this isn't over.";

            _personTalking.sprite = _possibleTalking[7];

            yield return new WaitForSeconds(3.0f);

        }

        if (_currentStage == 19)
        {

            _dialogueWindow.SetActive(false);

            endScreen.SetActive(true);

            Time.timeScale = 0;


        }
		// end Ryan

        yield return new WaitForSeconds (5.0f);

		_PowerupManager.powerUpSpawn = true;

		_dialogueWindow.SetActive (false);

		StageComplete ();
		StageKilled ();
	}

    public void StageComplete()
    {
        Debug.Log("stage finished spawning!!");

        stageComplete = true;


      
    }

    public void WaveKilled()
    {
        //Debug.Log("wave killed");
		if (stages [_currentStage].types.Length != 0) 
		{
			if (stages [_currentStage].types [0] == eAIType.Boss)
				return;
		}
			
        numWavesKilled++;
        if (numWavesKilled == stages[_currentStage].numberOfWaves * 2.0f)
            StageKilled();
    }

	public void WaveFlipped()
	{
		numWavesKilled--;
	}

    private void StageKilled()
    {
        //Debug.Log("stage killed!!");
        stageKilled = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gabriel Lewis (Q5094111)

public class AIScoreGabriel : MonoBehaviour {

    //Steve
    [SerializeField]
    private int minscore = 2;

    [SerializeField]
    private float duration = 5.5f;
    private int count = 0;
    //end steve

    [SerializeField]
	public int _myScore = 0;

	[SerializeField]
	private int _flippedScore = 0;

	[SerializeField]
	private bool _addFlippedScore = false;

	[SerializeField]
	private int _minusScore = 0;

	private int _initialScore = 0;

	void Awake()
	{
		_initialScore = _myScore;
	}

	void OnEnable()
	{
        _myScore = _initialScore;
    }

    void Update()
    {
        StartCoroutine(scoretaker());
    }


    public void Flip()
	{
		if (_addFlippedScore)
			_myScore += _flippedScore;
		else
			_myScore = _flippedScore;
	}

	public void Addscore(GameObject killer)
	{
        killer.GetComponent<PlayerScore>().updatescore(_myScore);
	}

	public void MinusScore(GameObject killer)
	{
		killer.GetComponent<PlayerScore>().updatescore(-_minusScore);
	}


    //Steve
    IEnumerator scoretaker()
    {

        yield return new WaitForSeconds(duration);

        if (_myScore > minscore)
        {
            _myScore -= 1;
            
        }
        

        if (_myScore == minscore)
        {
            yield break;
        }
    }
    //end steve
}
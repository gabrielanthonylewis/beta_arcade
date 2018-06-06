using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Gabriel Lewis (Q5094111)

public class AIHealth : MonoBehaviour {


    
    public AIWave owner = null;

    [SerializeField]
    private int _initialHealth = 1;

	[SerializeField]
	private int _flippedHealth = 2;

	[SerializeField]
    private int _currentHealth = 0;

	[SerializeField]
	private Image _player1Bar = null;

	[SerializeField]
	private Image _player2Bar = null;

	[SerializeField]
	private int _leftHealth = 0, _rightHealth = 0;
	private int _leftInitial = 0, _rightInitial = 0;


	// Use this for initialization
	void Awake ()
    {
        _currentHealth = _initialHealth;

		_leftHealth = (int)(_initialHealth / 2.0f);
		_leftInitial = _leftHealth;
		_rightHealth = (int)(_initialHealth / 2.0f);
		_rightInitial = _rightHealth;
	}
	void OnEnable () 
	{
		_currentHealth = _initialHealth;
		_leftHealth = (int)(_initialHealth / 2.0f);
		_rightHealth = (int)(_initialHealth / 2.0f);
	}

	public void IncreaseHealth(int x)
	{
		_currentHealth += x;

		if (_currentHealth <= 0)
			_currentHealth = 1;
	}

	public void Flip()
	{
		_currentHealth = _flippedHealth;
	}

    public void TakeDamage(int damage, GameObject shooter)
    {
		// If boss
		if (_player1Bar != null) 
		{
			if (shooter.tag == "P1")
			{
				_leftHealth -= damage;
				_player1Bar.fillAmount = (float)_leftHealth / (float)_leftInitial;
			} 
			else if (shooter.tag == "P2") 
			{
				_rightHealth -= damage;
				_player2Bar.fillAmount = (float)_rightHealth / (float)_rightInitial;
			}

			if (_leftHealth <= 0 && _rightHealth <= 0) 
			{
				Debug.Log ("COMPLETED BOSS FIGHT!!");
			}
		}
		// else not boss
		else
		{
      

            _currentHealth -= damage;

			if (_currentHealth <= 0) 
			{
				Die (shooter);
			}
		}
    }

	public void DieAndMinusScoreFrom(GameObject shooter)
	{
		if(owner != null)
			owner.KillWave(null);

		if(shooter != null)
			this.GetComponent<AIScoreGabriel>().MinusScore(shooter);

		//Destroy(this.gameObject);
		ObjectPool.Instance.DestroyObject(this.gameObject);
	}

    private void Die(GameObject shooter)
    {
    

        if (owner != null)
        	owner.EnemyKilled(1);

        if (shooter != null)
        {
    

            this.GetComponent<AIScoreGabriel>().Addscore(shooter);
        }

		//Destroy(this.gameObject);
		ObjectPool.Instance.DestroyObject(this.gameObject);
    }

}

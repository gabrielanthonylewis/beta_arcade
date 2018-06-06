using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gabriel Lewis (Q5094111)

public class SimpleShoot : MonoBehaviour
{

	[SerializeField]
	private GameObject bulletPrefab;

    [SerializeField]
    private GameObject _reticleHolder = null;

    [SerializeField]
    private float _shotWaitTime = 0.4f;

    private const float _FORCE = 5000.0f;
    private bool _shooting = false;
    private bool _shootRout = false;


	private const float _RETICLE_SPEED = 10f;
		
	private Vector3 _reticlePosition = Vector3.zero;
    private Vector3 _initialReticlePosition = Vector3.zero;
	private float _initialDistance = 0.0f;

    [SerializeField]
	private LayerMask _snapLayer;

	//Steve
	[SerializeField]
	private AudioClip Shot;

	private float _prevROF = 0.4f;

	private bool Rocketon = false;
	private bool Spreadshot = false;

	//
   
    private float _xOffset = 1.5f;
    private float _yOffset = 2f;

	[SerializeField]
	private LineRenderer _LineRenderer = null;


    void Start()
	{
		_reticlePosition = this.transform.position + this.transform.forward * 1.5f;
		_reticlePosition.z = this.transform.position.z + this.transform.forward.z * 12.5f;

        _initialReticlePosition = _reticlePosition;
	//	if (this.gameObject.tag == "P2")
		//	_reticlePosition.x += 2.0f;
		_initialDistance = Vector3.Distance (Camera.main.transform.position, new Vector3(0.0f, 0.0f, _initialReticlePosition.z));
    }

	[SerializeField]
	private bool _triggerDown = false;

	void Update()
	{
		UpdateReticlePosition();
	}

	void FixedUpdate () 
	{
		//if (aiming == false)
		//	return;
		
		float trigger = Input.GetAxis ("Shoot" + this.gameObject.tag);

		if (trigger == -1.0f) 
		{
			_shooting = true;

			StartCoroutine (Shoot (_shotWaitTime));				
		}
		else
		{
			_shooting = false;
			_triggerDown = false;

			StopAllCoroutines ();
			_shootRout = false;
		}

    }

	float xMoveTime = 0.0f;
	float yMoveTime = 0.0f;
    bool allowMoveX = true;
    bool allowMoveY = true;
	bool aiming = false;
    void UpdateReticlePosition()
    {

		float xAxis = Input.GetAxis ("XAxisAim" + this.gameObject.tag) * _RETICLE_SPEED * Time.deltaTime;
		float yAxis = Input.GetAxis ("YAxisAim" + this.gameObject.tag) * _RETICLE_SPEED * Time.deltaTime;

        //_reticlePosition.x += xAxis;

		xMoveTime -= Time.deltaTime;
		yMoveTime -= Time.deltaTime;

		if (allowMoveX || xMoveTime <= 0.0f)
        {
            if (xAxis > 0.0f)
            {
                _reticlePosition.x += _xOffset;

				xMoveTime = 0.33f;
                allowMoveX = false;
            }
            else if (xAxis < 0.0f)
            {
                _reticlePosition.x -= _xOffset;

				xMoveTime = 0.33f;
                allowMoveX = false;
            }
        }
        else
        {
            if(xAxis == 0.0f)
            {
                allowMoveX = true;
            }
        }

		if (allowMoveY || yMoveTime <= 0.0f)
        {
            if (yAxis > 0.0f)
            {
                _reticlePosition.y -= _yOffset;

				yMoveTime = 0.33f;
                allowMoveY = false;
            }
            else if (yAxis < 0.0f)
            {
                _reticlePosition.y += _yOffset;

				yMoveTime = 0.33f;
                allowMoveY = false;
            }
        }
        else
        {
            if (yAxis == 0.0f)
            {
                allowMoveY = true;
            }
        }



        // _reticlePosition.x = this.transform.position.x;
       // _reticlePosition.y += -yAxis;

        // Clamp X Axis
		_reticlePosition.x = Mathf.Clamp(_reticlePosition.x, _initialReticlePosition.x - 5.0f, _initialReticlePosition.x + (5.0f * _xOffset));

        // Clamp Y Axis
		_reticlePosition.y = Mathf.Clamp(_reticlePosition.y, _initialReticlePosition.y, _initialReticlePosition.y + (2.0f * _yOffset));



        RaycastHit hit;
		if (Physics.Raycast (new Vector3 (_reticlePosition.x, _reticlePosition.y, this.transform.position.z + 1.0f), this.transform.forward, out hit, Mathf.Infinity, _snapLayer))
		{
			_reticlePosition.z = hit.point.z - this.transform.forward.z;
			aiming = true;
		} 
		else 
		{
			_reticlePosition.z = this.transform.position.z + this.transform.forward.z * 12.5f;
			aiming = false;
		}
       // Debug.DrawRay(this.transform.position + this.transform.forward, new Vector3(_reticlePosition.x, _reticlePosition.y, this.transform.position.z + 1.0f) + this.transform.forward * 100);

		float dist = Vector3.Distance (Camera.main.transform.position, new Vector3(0.0f, 0.0f, _reticlePosition.z)) - _initialDistance;
		_reticleHolder.transform.localScale = Vector3.one + Vector3.one * dist / 10.0f;

		_reticleHolder.transform.position = _reticlePosition;



		_LineRenderer.SetPosition (0, this.transform.position);
		_LineRenderer.SetPosition (1, _reticlePosition);
    }

	IEnumerator Shoot(float waitTime)
    {
        if (_shootRout == true)
            yield break;

        _shootRout = true;

        if (_shooting == false)
        {
            _shootRout = false;
            yield break;
        }
        else
        {
            // Steve
            if (Spreadshot == true)
            {
                Vector3 targetDir = (_reticlePosition + new Vector3(2.0f, 0.0f)) - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, _FORCE * Time.deltaTime, 0.0F);

                Vector3 targetDir2 = (_reticlePosition + new Vector3(-2.0f, 0.0f)) - transform.position;
                Vector3 newDir2 = Vector3.RotateTowards(transform.forward, targetDir2, _FORCE * Time.deltaTime, 0.0F);
				
				// Gabriel
				// Subtle rumble
				XInputDotNetPure.PlayerIndex playerIndex = XInputDotNetPure.PlayerIndex.One;
				if (this.gameObject.tag == "P2")
					playerIndex = XInputDotNetPure.PlayerIndex.Two;
				RumbleManager.instance.SetVibration(playerIndex, 3.0f, 0.01f);
				// End Gabriel

			
				Vector3 startPos = this.transform.position + this.transform.forward;
				Vector3 endPos = _reticleHolder.transform.position - (this.transform.forward * Time.fixedDeltaTime);
				GameObject newBullet = ObjectPool.Instance.InstantiateObject("P_Rapid Fire", startPos);
				newBullet.GetComponent<Rigidbody>().AddForce((endPos - startPos).normalized * _FORCE * Time.fixedDeltaTime, ForceMode.Impulse);

				newBullet.GetComponent<SimpleBullet>().shooter = this.gameObject;


				Vector3 startPos1 = this.transform.position + this.transform.forward;
				Vector3 endPos1 = _reticleHolder.transform.position - (this.transform.forward * Time.fixedDeltaTime);
				GameObject newBullet1 = ObjectPool.Instance.InstantiateObject("P_Rapid Fire", startPos);
				newBullet1.transform.Translate (2.0f, 0, 0);
				newBullet1.GetComponent<Rigidbody>().AddForce((endPos - startPos).normalized * _FORCE * Time.fixedDeltaTime, ForceMode.Impulse);

				newBullet1.GetComponent<SimpleBullet>().shooter = this.gameObject;


				Vector3 startPos2 = this.transform.position + this.transform.forward;
				Vector3 endPos2 = _reticleHolder.transform.position - (this.transform.forward * Time.fixedDeltaTime);
				GameObject newBullet2 = ObjectPool.Instance.InstantiateObject("P_Rapid Fire", startPos);
				newBullet2.transform.Translate (-2.0f, 0, 0);
				newBullet2.GetComponent<Rigidbody>().AddForce((endPos - startPos).normalized * _FORCE * Time.fixedDeltaTime, ForceMode.Impulse);

				newBullet2.GetComponent<SimpleBullet>().shooter = this.gameObject;


                GetComponent<AudioSource>().PlayOneShot(Shot);
            }
			// End Steve
            else
            {

				// Subtle rumble
				XInputDotNetPure.PlayerIndex playerIndex = XInputDotNetPure.PlayerIndex.One;
				if (this.gameObject.tag == "P2")
					playerIndex = XInputDotNetPure.PlayerIndex.Two;
				RumbleManager.instance.SetVibration(playerIndex, 1.0f, 0.01f);
	

                //GameObject newBullet = ObjectPool.Instance.InstantiateObject("Bullet", new Vector3 (_reticlePosition.x, _reticlePosition.y, this.transform.position.z) + this.transform.forward * 1.5f);
                //newBullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * _FORCE * Time.fixedDeltaTime, ForceMode.Impulse);

                // Diagonal, sometimes misss
                Vector3 startPos = this.transform.position + this.transform.forward;
                Vector3 endPos = _reticleHolder.transform.position - (this.transform.forward * Time.fixedDeltaTime);
                GameObject newBullet = ObjectPool.Instance.InstantiateObject("P_Rapid Fire", startPos);
                newBullet.GetComponent<Rigidbody>().AddForce((endPos - startPos).normalized * _FORCE * Time.fixedDeltaTime, ForceMode.Impulse);

                newBullet.GetComponent<SimpleBullet>().shooter = this.gameObject;

                //Steve
                if (Rocketon == true)
                {
                    newBullet.GetComponent<SimpleBullet>().Rocketmode(Rocketon);
					Debug.Log("Active Rocket Shot");
                }

				else newBullet.GetComponent<SimpleBullet>().Rocketmode(Rocketon);

                GetComponent<AudioSource>().PlayOneShot(Shot);

                //


            }

            yield return new WaitForSeconds(waitTime);
        }

        _shootRout = false;
    }

	// Steve
	public void ROFBuff(float _ROF, bool On)
	{


		if (On == true)
		{
			_shotWaitTime = _ROF;
		}

		else if (On == false)
		{
			_shotWaitTime = _prevROF;
		}
	}


	public void RocketBuff()
	{
		Rocketon = !Rocketon;
	}
	public void Spreadon()
	{
		Spreadshot = !Spreadshot;
	}
	// End Steve

}

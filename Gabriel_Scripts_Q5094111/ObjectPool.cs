using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for Dictionary

// Gabriel Lewis (Q5094111)

public class ObjectPool : MonoBehaviour {

	private static ObjectPool _instance = null;
	public static ObjectPool Instance { get { return _instance; } }

	[System.Serializable]
	private struct ToStore
	{
		public GameObject obj;
		public int quantity;
	};

	[SerializeField]
	private ToStore[] _toStore;



	private struct StoredObject
	{
		public GameObject obj;
		public bool beingUsed;
		public Transform heirParent;
	};



	private Dictionary<string, StoredObject[]> _storage;

	private void Awake()
	{
		if (_instance != null && _instance != this)
			Destroy (this.GetComponent<ObjectPool>());
		else
			_instance = this;

		InitialiseStorage ();
	}





    private void InitialiseStorage()
	{
		_storage = new Dictionary<string, StoredObject[]>();
		// make hierachy 
		GameObject objectPoolHier = new GameObject ();
		objectPoolHier.name = "objectpool";

		foreach (ToStore itemQua in _toStore) 
		{
			
	
			StoredObject[] newObjects = new StoredObject[itemQua.quantity];

			// make hierachy 
			GameObject newObjectHeir = new GameObject();
			newObjectHeir.name = itemQua.obj.name;// + "s";
			newObjectHeir.transform.SetParent (objectPoolHier.transform);
		

			for (int i = 0; i < newObjects.Length; i++) 
			{
				newObjects[i].obj = Instantiate (itemQua.obj, new Vector3 (-1000.0F, -1000.0F, -1000.0F), Quaternion.Euler (Vector3.zero)) as GameObject; 
				newObjects [i].obj.name = itemQua.obj.name;
				newObjects [i].obj.transform.SetParent (newObjectHeir.transform);
				newObjects [i].heirParent = newObjectHeir.transform;

				Rigidbody rb = newObjects [i].obj.GetComponent<Rigidbody> ();
				if (rb != null) {
					rb.Sleep ();
					rb.velocity = Vector3.zero;
					rb.angularVelocity = Vector3.zero;
					//rb.isKinematic = true;
					//rb.useGravity = false;
				}
                ParticleSystem ps = newObjects[i].obj.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop();
                    
                }
                ParticleSystem psc = newObjects[i].obj.GetComponentInChildren<ParticleSystem>();
                if (psc != null)
                {
                    psc.Stop();

                }

                newObjects[i].obj.SetActive (false);



				newObjects [i].beingUsed = false;
			}


			//Debug.Log (newObjects[0].obj.name);
			_storage.Add (itemQua.obj.name, newObjects);
		}

	}

	public GameObject InstantiateObject(string objectName, Vector3 position)
	{
		StoredObject[] objects;
        if (!_storage.TryGetValue(objectName, out objects))
        {

            Debug.LogError("NAME NOT FOUND: " + objectName);
            return null;
        }

        bool inUse = false;
        bool nullObj = false;
		for (int i = 0; i < objects.Length; i++)
		{
            inUse = false;
           

            if (objects[i].beingUsed == true)
            {
                inUse = true;
                continue;
            }

            nullObj = false;
            if (objects[i].obj == null)
            {
                Debug.LogError(objectName + " obj was destroyed, why?");
                nullObj = true;
                continue;
            }

			//objects[i].obj.SetActive (true);

			// TODO may not always want, should be same as default
			/*Rigidbody rb = objects [i].obj.GetComponent<Rigidbody> ();
			if (rb != null) {
				rb.isKinematic = false;
				rb.useGravity = true;
			}*/

			objects[i].beingUsed = true;

			objects [i].obj.transform.position = position;
			objects [i].obj.SetActive (true);
			return objects[i].obj;
		}

        if(inUse)
            Debug.LogError(objectName + " in use");
        
        if(nullObj)
            Debug.LogError(objectName + " null");


        Debug.LogError ("NO FREE OBJECT OF NAME: " + objectName);

		return null;
	}


	public bool DestroyObject (GameObject obj)
	{
		StoredObject[] objects;
		if (!_storage.TryGetValue (obj.name, out objects)) 
		{
			Debug.LogWarning("Object instance is not in the object pool but an attempt to delete it from the object pool has been made");
			//Destroy (obj);
			return false;
		}
		for (int i = 0; i < objects.Length; i++)
		{

			if (objects[i].obj.GetInstanceID() != obj.GetInstanceID())
				continue;

			Rigidbody rb = objects [i].obj.GetComponent<Rigidbody> ();
			if (rb != null)
			{
				rb.Sleep ();
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				//rb.isKinematic = true;
				//rb.useGravity = false;
			}

            ParticleSystem ps = objects[i].obj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();

            }
            ParticleSystem psc = objects[i].obj.GetComponentInChildren<ParticleSystem>();
            if (psc != null)
            {
                psc.Play();

            }

            
            objects [i].beingUsed = false;
            //Debug.Log("SET FALSE: " + objects[i].obj.name + objects[i].beingUsed);

            objects[i].obj.SetActive (false);


			// Want to do but need to disable navmesh?
			objects [i].obj.transform.position = new Vector3 (-1000.0F, -1000.0F, -1000.0F);
			objects [i].obj.transform.SetParent (	objects [i].heirParent);

			return true;
		}

		return false;
	}
}

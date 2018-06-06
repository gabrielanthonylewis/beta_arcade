using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gabriel Lewis (Q5094111)

public class MovingBackground : MonoBehaviour 
{
	[SerializeField]
	private GameObject[] _roomsToChooseFrom;

	[SerializeField]
	private int _numberOfRoomOnScreen = 3;

	[SerializeField]
	public float _roomLength = 20.0f;

	private List<GameObject> _rooms = new List<GameObject>();



	//private GameObject _roomStorage;


	// Use this for initialization
	void Start () 
	{
		/*GameObject prefabStorage = new GameObject ();
		prefabStorage.name = "prefabStorage";
		prefabStorage.transform.parent = this.transform;
		for(uint i = 0; i < _roomsToChooseFrom.Length; i++)
		{
			GameObject prefabRoom = ObjectPool._instance.Instantiate(_roomsToChooseFrom[i].name, Vector3.zero);
			prefabRoom.name = _roomsToChooseFrom [i].name;
			prefabRoom.SetActive (false);
		}
		*/	
		/*_roomStorage = new GameObject ();
		_roomStorage.name = "roomStorage";
		_roomStorage.transform.parent = this.transform;*/
		for (int i = 0; i < _numberOfRoomOnScreen; i++) 
		{
			int rand = Random.Range (0, _roomsToChooseFrom.Length);

			//ObjectPool.Instance.InstantiateObject(_roomsToChooseFrom[rand].name, new Vector3 (0.0f, 0.0f, 1.0f) * _roomLength * i);
			_rooms.Add(ObjectPool.Instance.InstantiateObject(_roomsToChooseFrom[rand].name, new Vector3 (0.0f, 0.0f, 1.0f) * _roomLength * i));
		}

       
	}

	public void RemoveRoom(GameObject room)
	{
		_rooms.Remove (room);
		ObjectPool.Instance.DestroyObject (room);

		SpawnNewRoom ();
	}

	private void SpawnNewRoom()
	{
		int rand = Random.Range (0, _roomsToChooseFrom.Length);
		_rooms.Add(ObjectPool.Instance.InstantiateObject(_roomsToChooseFrom[rand].name, Vector3.zero));

		if (_rooms.Count - 1 - 1 >= 0) 
		{
			_rooms [_rooms.Count - 1].transform.localPosition = _rooms [_rooms.Count - 1 - 1].transform.localPosition;
			_rooms [_rooms.Count - 1].transform.position += new Vector3 (0.0f, 0.0f, 1.0f) * _roomLength; 
		}
		else 
		{
			_rooms [_rooms.Count - 1].transform.localPosition = Vector3.zero;
			_rooms [_rooms.Count - 1].transform.position += new Vector3 (0.0f, 0.0f, 1.0f) * _roomLength; 
		}
	}


	// Update is called once per frame
	/*void FixedUpdate ()
	{
		foreach (GameObject room in _rooms) 
		{
			room.transform.position += -Camera.main.transform.forward * Time.fixedDeltaTime * _speedMulti;


			if (room.transform.position.z < Camera.main.transform.position.z - _roomLength / 1.5f)
			{

				_rooms.Remove (room);
				ObjectPool.Instance.DestroyObject(room);

				// Pick new room
				int rand = Random.Range (0, _roomsToChooseFrom.Length);
				_rooms.Add(ObjectPool.Instance.InstantiateObject(_roomsToChooseFrom[rand].name, Vector3.zero));

				if (_rooms.Count - 1 - 1 >= 0) 
				{
					_rooms [_rooms.Count - 1].transform.localPosition = _rooms [_rooms.Count - 1 - 1].transform.localPosition;
					_rooms [_rooms.Count - 1].transform.position += new Vector3 (0.0f, 0.0f, 1.0f) * _roomLength; 
				}
				else 
				{
					_rooms [_rooms.Count - 1].transform.localPosition = Vector3.zero;
					_rooms [_rooms.Count - 1].transform.position += new Vector3 (0.0f, 0.0f, 1.0f) * _roomLength; 
				}



				return;
			}
		}
	}*/
}

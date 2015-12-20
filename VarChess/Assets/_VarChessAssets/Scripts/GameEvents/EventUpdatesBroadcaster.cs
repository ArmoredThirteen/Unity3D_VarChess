using UnityEngine;
using System.Collections;


public class EventUpdatesBroadcaster : MonoBehaviour
{
	public static EventUpdatesBroadcaster instance;


	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad (this);
		}
		else
		{
			Destroy (gameObject);
			return;
		}
	}

	void Update ()
	{
		EventManager.BroadcastUpdates (Event_Updates.UpdateOne);
		EventManager.BroadcastUpdates (Event_Updates.UpdateTwo);
		EventManager.BroadcastUpdates (Event_Updates.UpdateThree);
	}

}

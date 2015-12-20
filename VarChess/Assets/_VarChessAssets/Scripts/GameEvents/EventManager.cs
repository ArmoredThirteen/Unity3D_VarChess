using UnityEngine;
using System.Collections;


public static class EventManager
{
	public sealed class EventManagerGameObject : MonoBehaviour
	{
		void Awake ()
		{
			DontDestroyOnLoad (gameObject);	
		}
		
		//Clean up eventTable every time a new level loads.
		public void OnLevelWasLoaded (int unused)
		{
			//	Fix/rebuild any broken event systems
			EventManager.Cleanup ();
		}
	}


	private static EventManagerGameObject _eventsManagerGameObject = (new GameObject ("EventManager")).AddComponent<EventManagerGameObject> ();

	#region EVENT SYSTEMS

	private static EventSystem<Event_Updates, EventData_Updates> _updates = new EventSystem<Event_Updates, EventData_Updates> ();
	private static EventSystem<Event_Gameplay, EventData_Gameplay> _gameplay = new EventSystem<Event_Gameplay, EventData_Gameplay> ();

	#endregion


	#region REGISTER EVENTS

	public static void RegisterUpdates (Event_Updates theEvent, Callback<EventData_Updates> callback)
	{
		_updates.Register (theEvent, callback);
	}
	
	public static void RegisterGameplay (Event_Gameplay theEvent, Callback<EventData_Gameplay> callback)
	{
		_gameplay.Register (theEvent, callback);
	}

	#endregion


	#region UNREGISTER EVENTS
	
	public static void UnregisterUpdates (Event_Updates theEvent, Callback<EventData_Updates> callback)
	{
		_updates.Unregister (theEvent, callback);
	}
	
	public static void UnregisterGameplay (Event_Gameplay theEvent, Callback<EventData_Gameplay> callback)
	{
		_gameplay.Unregister (theEvent, callback);
	}
	
	#endregion


	#region BROADCAST EVENTS
	
	public static void BroadcastUpdates (Event_Updates theEvent, GameObject sender = null)
	{
		_updates.Broadcast (theEvent, new EventData_Updates (), sender);
	}
	
	public static void BroadcastUpdates (Event_Updates theEvent, EventData_Updates eventData, GameObject sender = null)
	{
		_updates.Broadcast (theEvent, eventData, sender);
	}


	public static void BroadcastGameplay (Event_Gameplay theEvent, GameObject sender = null)
	{
		_gameplay.Broadcast (theEvent, new EventData_Gameplay (), sender);
	}
	
	public static void BroadcastGameplay (Event_Gameplay theEvent, EventData_Gameplay eventData, GameObject sender = null)
	{
		_gameplay.Broadcast (theEvent, eventData, sender);
	}
	
	#endregion
	

	private static void Cleanup ()
	{
		_updates = new EventSystem<Event_Updates, EventData_Updates> ();
		_gameplay = new EventSystem<Event_Gameplay, EventData_Gameplay> ();
	}

}

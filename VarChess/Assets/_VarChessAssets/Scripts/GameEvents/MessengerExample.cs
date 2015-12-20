using UnityEngine;
using System.Collections;


public class MessengerExample : MonoBehaviour, IEventUser
{
	private bool testOneReceived = false;
	private bool testTwoReceived = false;


	// Use this for initialization
	void Start ()
	{
		RegisterEvents ();
	}
	
	void OnDestroy ()
	{
		UnregisterEvents ();
	}


	#region EventSystem
	
	public void RegisterEvents ()
	{
		EventManager.RegisterUpdates (Event_Updates.UpdateOne, ManagedUpdateOne);
		EventManager.RegisterUpdates (Event_Updates.UpdateTwo, ManagedUpdateTwo);
	}
	
	public void UnregisterEvents ()
	{
		EventManager.UnregisterUpdates (Event_Updates.UpdateOne, ManagedUpdateOne);
		EventManager.UnregisterUpdates (Event_Updates.UpdateTwo, ManagedUpdateTwo);
	}

	#endregion
	
	
	void ManagedUpdateOne (EventData_Updates eventData)
	{
		Debug.Log ("<color=white>ONE</color>");
	}
	
	void ManagedUpdateTwo (EventData_Updates eventData)
	{
		Debug.Log ("<color=teal>TWO</color>");
	}

}

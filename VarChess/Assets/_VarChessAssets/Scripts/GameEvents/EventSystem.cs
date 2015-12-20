//	Turn this on to use the list implementation for
//	storing the events, as opposed to a dictionary.
//	The list version right now seems to use less CPU
//	but causes more garbage. Specifically, the garbage
//	comes out of GetIndexOfType()'s call to Equals.
#define USELIST

/*
 * Butchered from:
 * Advanced C# messenger by Ilya Suzdalnitski. V1.0
 * Based on Rod Hyde's "CSharpMessenger" and Magnus Wolffelt's "CSharpMessenger Extended".
 */

using System;
using System.Collections.Generic;
using UnityEngine;


interface IEventUser
{
	void RegisterEvents ();
	void UnregisterEvents ();
}


public class EventSystem<EventType, EventDataType>	where EventType:		struct, IConvertible, IComparable
													where EventDataType:	EventData
{
	#if USELIST
	private List<Pair<EventType, Delegate>> _eventList = new List<Pair<EventType, Delegate>> ();
	#else
	private Dictionary<EventType, Delegate> _eventTable = new Dictionary<EventType, Delegate> ();
	#endif


	public void Register (EventType eventType, Callback<EventDataType> callback)
	{
		#if USELIST

		int eventIndex = GetIndexOfType (eventType);

		//	Might need to add it to the list
		if (eventIndex < 0)
		{
			_eventList.Add (new Pair<EventType, Delegate> (eventType, callback));
			return;
		}

		//	If preexisting, don't add it again
		Delegate[] existingDelegates = ((Callback<EventDataType>)_eventList[eventIndex].v2).GetInvocationList ();
		for (int i = 0; i < existingDelegates.Length; i++)
		{
			if (existingDelegates[i] == callback)
				return;
		}

		//	And add it to the list
		_eventList[eventIndex].v2 = (Callback<EventDataType>)_eventList[eventIndex].v2 + callback;

		#else

		//	Add key if unexisting
		if (!_eventTable.ContainsKey (eventType))
		{
			_eventTable.Add (eventType, null);
		}

		//	And then add to our delegate
		_eventTable[eventType] = (Callback<EventDataType>)_eventTable[eventType] + callback;

		#endif
	}


	public void Unregister (EventType eventType, Callback<EventDataType> callback)
	{
		#if USELIST
		
		int eventIndex = GetIndexOfType (eventType);

		if (eventIndex < 0)
			return;

		//	Remove the delegate
		_eventList[eventIndex].v2 = (Callback<EventDataType>)_eventList[eventIndex].v2 - callback;

		//	And remove the entry if it is done being used
		if (_eventList[eventIndex].v2 == null)
			_eventList.RemoveAt (eventIndex);
		
		#else

		//	We have no key, so nothing to unregister from
		if (!_eventTable.ContainsKey (eventType))
			return;

		_eventTable[eventType] = (Callback<EventDataType>)_eventTable[eventType] - callback;

		//	Remove key if unused
		if (_eventTable[eventType] == null)
		{
			_eventTable.Remove (eventType);
		}
		
		#endif
	}


	//TODO: Delayed? Maybe not? Sounds dangerous to me
	/*public void Broadcast (float delay, EventType eventType, EventDataType eventData, GameObject sender = null)
	{
		#if USELIST

		#else

		#endif
	}*/

	public void Broadcast (EventType eventType, EventDataType eventData, GameObject sender = null)
	{
		#if USELIST
		
		int eventIndex = GetIndexOfType (eventType);

		if (eventIndex < 0)
			return;

		//	Add in our automatic event data
		if (eventData != null)
		{
			//	Sender history
			if (sender != null)
				eventData.senders.Add (sender);
			
			//	Timestamps
			if (eventData.timestamp_firstSent < 0)
				eventData.timestamp_firstSent = Time.time;
			eventData.timestamp_lastSent = Time.time;
		}

		Callback<EventDataType> theCallback = _eventList[eventIndex].v2 as Callback<EventDataType>;

		if (theCallback != null)
		{
			theCallback (eventData);
		}
		
		#else

		if (!_eventTable.ContainsKey (eventType))
			return;

		//	Add in our automatic event data
		if (eventData != null)
		{
			//	Sender history
			if (sender != null)
				eventData.senders.Add (sender);

			//	Timestamps
			if (eventData.timestamp_firstSent < 0)
				eventData.timestamp_firstSent = Time.time;
			eventData.timestamp_lastSent = Time.time;
		}

		//	Find and call the delegate
		Delegate d;
		if (_eventTable.TryGetValue (eventType, out d))
		{
			Callback<EventDataType> callback = d as Callback<EventDataType>;
			
			if (callback != null)
			{
				callback(eventData);
			}
		}
		
		#endif
	}


	#if USELIST

	private bool HasEventType (EventType eventType)
	{
		for (int i = 0; i < _eventList.Count; i++)
		{
			if (_eventList[i].v1.Equals (eventType))
				return true;
		}

		return false;
	}

	private int GetIndexOfType (EventType eventType)
	{
		int count = _eventList.Count;
		for (int i = 0; i < count; i++)
		{
			//TODO: This causes more garbage than I'm okay with
			//	I think it has something to do with having to
			//	runtime cast some things. I'm not really sure
			//	how to go about fixing this, but if this can
			//	be fixed then the list implementation should
			//	totally outperform the dictioinary implementation.
			if (_eventList[i].v1.Equals (eventType))
				return i;
		}

		return -1;
	}

	private void RemovePairOfType (EventType eventType)
	{
		for (int i = 0; i < _eventList.Count; i++)
		{
			if (_eventList[i].v1.Equals (eventType))
			{
				_eventList.RemoveAt (i);
				return;
			}
		}
	}
	
	#endif

}

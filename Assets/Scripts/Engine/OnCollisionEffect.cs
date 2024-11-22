using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class OnCollisionEffect : MonoBehaviour
{
	public UnityEvent CollisionEvent;

	void Awake()
    {
	    if(CollisionEvent is null)
	    {
	    	CollisionEvent = new UnityEvent();
	    }
    }
    
	private void OnTriggerEnter2D(Collider2D collision)
	{
		CollisionEvent.Invoke();
	}

}

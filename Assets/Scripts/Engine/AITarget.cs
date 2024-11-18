using UnityEngine;

public class AITarget : MonoBehaviour
{
	
	private void Start()
    {
    	EnemyTracker.Instance.RegisterAITarget(gameObject); 
    }

	private void OnDestroy()
	{
		EnemyTracker.Instance.RemoveAITarget(gameObject);
	}
}

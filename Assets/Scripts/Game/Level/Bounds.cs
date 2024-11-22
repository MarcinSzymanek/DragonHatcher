using UnityEngine;

// Adds bounds to an object, confining target layers within them
// Pushes target inside the collider
[RequireComponent(typeof(BoxCollider2D))]
public class Bounds : MonoBehaviour
{
	private BoxCollider2D collider_;
	private Vector3 offset_ = Vector3.zero;

	float[] x_bounds = new float[2];
	float[] y_bounds = new float[2];

    void Awake()
    {
	    collider_ = GetComponent<BoxCollider2D>();
	    x_bounds[0] = collider_.offset.x - 0.5f * collider_.size.x;
	    x_bounds[1] = collider_.offset.x + 0.5f * collider_.size.x;
	    y_bounds[0] = collider_.offset.y - 0.5f * collider_.size.y;
	    y_bounds[1] = collider_.offset.y + 0.5f * collider_.size.y;
    }

	protected void OnTriggerStay2D(Collider2D other)
	{
		Vector3 collisionPoint = transform.InverseTransformPoint(other.transform.position);	
		Transform target = other.transform.parent;

		// Push target out of the collider range
		if(collisionPoint.x <= x_bounds[0])
		{
			offset_ = Vector2.right;
		}
		else if(collisionPoint.x >= x_bounds[1])
		{
			offset_ = Vector2.left;
		}
		else if(collisionPoint.y <= y_bounds[0])
		{
			offset_ = Vector2.up;
		}
		else if(collisionPoint.y >= y_bounds[1])
		{
			offset_ = Vector2.down;
		}
		target.position = target.position + (offset_ * 0.5f);
	}

}

using UnityEngine;
using System.Collections;

public class CameraBounds : MonoBehaviour
{
	public Transform target;
	
	public float width;
	public float height;
	
	private Vector3 nextPosition;
	
	float minX;
	float minY;
	float maxX;
	float maxY;
	void Start () 
	{
		nextPosition = new Vector3();
	}
	
	void FixedUpdate () 
	{
		SetNextPosition ();
		transform.position = nextPosition;
	}
	
	private void SetNextPosition()
	{
		nextPosition = Vector3.Lerp(transform.position, target.position, (target.rigidbody.velocity.magnitude + 1) * Time.deltaTime * 4);
		
		float cameraHeight = camera.orthographicSize;
		float cameraWidth  = cameraHeight * Screen.width / Screen.height;
		
		
		if (nextPosition.x - cameraWidth < 0)
		{
			nextPosition.x = cameraWidth;
		}
		if (nextPosition.x + cameraWidth > width)
		{
			nextPosition.x = width - cameraWidth;
		}
		if (nextPosition.y - cameraHeight < 0)
		{
			nextPosition.y = cameraHeight;
		}
		if (nextPosition.y + cameraHeight > height)
		{
			nextPosition.y = height - cameraHeight;
		}
		
		nextPosition.z = -10;
	}
}
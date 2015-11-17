using UnityEngine;

public class ParaCamera : MonoBehaviour 
{
	// Delagate to check for camera movement
	public delegate void ParaCameraDelegate(Vector3 diff);
	public ParaCameraDelegate onCameraTranslate;
	
	private Vector3 oldPosition;
	
	void Start()
	{
		oldPosition = transform.position;
	}
	
	void Update()
	{
		if (transform.position != oldPosition)
		{
			if (onCameraTranslate != null)
			{
				// When the camera moves, use the difference in positions to calculate the parallax effect.
				Vector3 diff = oldPosition - transform.position;
				onCameraTranslate(diff);
			}
			
			oldPosition = transform.position;
		}
	}
}

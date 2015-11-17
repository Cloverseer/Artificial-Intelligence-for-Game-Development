using UnityEngine;

public class ParaLayer : MonoBehaviour
{
	public float hSpeed;
	public float vSpeed;
	
	private Vector3 newPos;
	
	// Uses custom value set in the inspector
	public void Translate(Vector3 diff, bool useH, bool useV)
	{
		newPos = transform.localPosition;
		if (useH) newPos.x -= diff.x * hSpeed;
		if (useV) newPos.y -= diff.y * vSpeed;
		
		transform.localPosition = newPos;
	}
	
	// Called from ParaBackground, uses the values calculated there.
	public void Translate(Vector3 diff, bool useH, bool useV, float defaultScrollSpeed)
	{
		newPos = transform.localPosition;
		if (useH) newPos.x -= diff.x * defaultScrollSpeed;
		if (useV) newPos.y -= diff.y * defaultScrollSpeed;
		
		transform.localPosition = newPos;
	}
}
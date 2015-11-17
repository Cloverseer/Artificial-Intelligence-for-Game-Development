using UnityEngine;
using System.Collections.Generic;

public class ParaBackground : MonoBehaviour
{
	public ParaCamera paraCam;
	public bool enableHorizontalParallax = true;
	public bool enableVerticalParallax;
	public bool useCustomScrollValues = true;
	
	// Store references to all the layers
	List<ParaLayer> list = new List<ParaLayer>();
	
	void Start()
	{
		if (paraCam == null)
		{
			// Get the main camera
			paraCam = Camera.main.GetComponent<ParaCamera>();
		}
		
		if (paraCam != null)
		{
			// Every time the camera moves, call the Move function.
			paraCam.onCameraTranslate += Move;
		}
		
		else
		{
			Debug.Log("No Main Camera found.");
			return;
		}
		
		// Get all the layers from the children
		for (int x = 0; x < transform.childCount; ++x)
		{
			ParaLayer layer = transform.GetChild(x).GetComponent<ParaLayer>();
			
			// Only add the children that have a ParaLayer component.
			if (layer != null)
			{
				list.Add(layer);
				//Debug.Log("Added " + layer + " to the parallax list.");
			}
		}
		
		//Debug.Log("There are " + list.Count + " ParaLayers.");
	}
	
	void Move(Vector3 diff)
	{
		if (useCustomScrollValues)
		{
			foreach (ParaLayer layer in list)
			{
				layer.Translate(diff, enableHorizontalParallax, enableVerticalParallax);
			}
		}
		
		// Use default values that are calculated here.
		else
		{
			// Use a normal for-loop because we need x for the math
			for (int x = 0; x < list.Count; ++x)
			{
				list[x].Translate(diff, enableHorizontalParallax, enableVerticalParallax, x * 0.15f);
			}
		}
	}
}
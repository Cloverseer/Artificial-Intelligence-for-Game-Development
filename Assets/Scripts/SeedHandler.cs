using UnityEngine;
using System.Collections;

public class SeedHandler : MonoBehaviour 
{

	public int seed;
	// Use this for initialization
	void Awake() 
	{
		Random.seed = seed;
	}

}

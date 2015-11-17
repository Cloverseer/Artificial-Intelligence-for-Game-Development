using UnityEngine;
using System.Collections;

public class RatGangAI : MonoBehaviour
{
	public Transform feet;
	public float playerCheckDistance;
	
	public enum State
	{
		Sleeping,
		Fleeing,
		Attacking,
		Wandering,
		Dead
	};
	
	public State state;
	
	private Transform target;
	
	// Use this for initialization
	void Start ()
	{
		// Start either in Sleeping or Wandering
		if (Random.Range(0, 2) % 2 == 0)
		{
			state = State.Sleeping;
		}
		else 
		{
			state = State.Wandering;
		}
		
		Debug.Log (this + " " + state);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	void FixedUpdate()
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EntityLayer"), LayerMask.NameToLayer("EntityLayer"));
	}
}

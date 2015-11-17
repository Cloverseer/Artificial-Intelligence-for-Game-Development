using UnityEngine;
using System.Collections.Generic;

public class MobSpawner : MonoBehaviour
{
	public Vector2 mobCountRange;
	public Vector2 spawnTimeRange;
	public Vector2 spawnPositionRange;
	public int maxActiveMobs;
	
	public GameObject mobPrefab;
	
	private int maxMobCount;
	private int currentMobCount;
	private int currentActiveMobs;
	private float spawnDelay;
	private float currentTimer;

	void Awake()
	{
		maxMobCount = Random.Range((int)mobCountRange.x, (int)mobCountRange.y);
		currentMobCount = 0;
		currentActiveMobs = 0;
		currentTimer = 0.0f;
	}

	void Start()
	{
		spawnDelay = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
	}
	
	void Update()
	{
		if (currentTimer >= spawnDelay && currentActiveMobs < maxActiveMobs)
		{
			SpawnMob();
			currentTimer = 0.0f;
			currentActiveMobs++;
			currentMobCount++;
			spawnDelay = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
		}
	
		if (currentMobCount < maxMobCount && currentActiveMobs < maxActiveMobs)
		{
			currentTimer += Time.deltaTime;
		}
		
		currentActiveMobs = transform.childCount;
	}
	
	void SpawnMob()
	{
		float mobX = Random.Range(spawnPositionRange.x, spawnPositionRange.y);
		Vector3 mobPos = new Vector3 (transform.position.x + mobX, transform.position.y, transform.position.z);
		
		GameObject mob = Instantiate(mobPrefab, mobPos, Quaternion.identity) as GameObject;
		mob.transform.parent = transform;
	}
}
using UnityEngine;

public class HealthBar
{
	public float width;
	public float height;
	public float padding;
	
	private int maxHP;
	private int currentHP;

	public HPPlane2D Outline
	{
		get;
		private set;
	}
	
	public HPPlane2D Empty
	{
		get;
		private set;
	}
	
	public HPPlane2D Hit
	{
		get;
		private set;
	}
	
	public HPPlane2D Health
	{
		get;
		private set;
	}

	public HealthBar(float width, float height, int maxHP)
	{
		this.padding = height / 3;
		
		this.width = width;
		this.height = height;
		
		Outline	= new HPPlane2D(width, height);
		Empty	= new HPPlane2D(width - padding, height - padding);
		Hit		= new HPPlane2D(width - padding, height - padding);
		Health	= new HPPlane2D(width - padding, height - padding);
		
		this.maxHP = maxHP;
	}
	
	public Mesh UpdateCurrentHealth(int hp)
	{
		return Health.Resize((width-padding) * ((float)hp / (float)maxHP), Health.Height);
	}
}

using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Entity : MonoBehaviour
{
	public int health;
	public int level;
	public float speed;
	
	[HideInInspector]
	public int maxHealth;
	
	public bool jumpable;
	public float jumpPower;
	public Transform feet;
	public Transform sprite;
	
	public Texture2D healthOutlineColour;
	public Texture2D healthEmptyColour;
	public Texture2D healthHitColour;
	public Texture2D healthColour;

	private HealthBar hpBar;
	private GameObject[] hpChildren;
	private Renderer r;
	private bool canJump;

	public void Awake()
	{
		if (feet == null && jumpable)
		{
			feet = transform.FindChild("Feet");
			
			// If feet is still null after looking for it, create one using the sprite;
			if (feet == null)
			{
				GameObject tempFeet = new GameObject();
				tempFeet.name = "Feet";
				tempFeet.transform.parent = transform;
				tempFeet.transform.localPosition = new Vector3 (0, -(renderer.bounds.extents.y) - 0.01f);
				feet = tempFeet.transform;
			}
		}
		
		if (sprite == null)
		{
			sprite = transform.FindChild("Sprite");
			
			// If sprite is still null, use the first SpriteRenderer we find;
			if (sprite == null)
			{
				r = transform.GetComponentInChildren(typeof(SpriteRenderer)) as Renderer;
			}
		}
		r = sprite.renderer;
		
		maxHealth = health;
	}

	public void Start()
	{
		CreateHealthBar();
	}

	public void Update()
	{
		if (!canJump && jumpable)
		{
			canJump = Physics.Linecast(transform.position, feet.position - new Vector3(0, 0.15f), ~(1 << LayerMask.NameToLayer("EntityLayer")));
		}
		
		// flips the sprites to face the right way when walking
		if (rigidbody.velocity.x < 0 && sprite.localScale.x > 0)
		{
			FlipSprite(sprite);
		}
		else if (rigidbody.velocity.x > 0 && sprite.localScale.x < 0)
		{
			FlipSprite(sprite);
		}
	}

	public void FixedUpdate()
	{
		//Update Yellow Part of HP
		if (hpBar.Hit.Width > hpBar.Health.Width)
		{
			float newWidth = Mathf.Lerp(hpBar.Hit.Width, hpBar.Health.Width, Time.fixedDeltaTime * 2);
			hpChildren[1].GetComponent<MeshFilter>().mesh = hpBar.Hit.Resize(newWidth, hpBar.Hit.Height);
		}
	}

	public void Jump()
	{
		if (canJump && jumpable)
		{
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpPower);
			canJump = false;
		}
	}

	public void CreateHealthBar()
	{
		hpChildren = new GameObject[4];
		hpBar = new HealthBar(r.bounds.size.x, 0.0625f, maxHealth);
		//hpBar = new HealthBar(r.bounds.size.x, 0.09375f, maxHealth); // 1/64
		
		for(int x = 0; x < 4; ++x)
		{
			hpChildren[x] = new GameObject();
			hpChildren[x].name = "HPBar " + x;
			hpChildren[x].transform.parent = transform;
			hpChildren[x].AddComponent<MeshFilter>();
			hpChildren[x].AddComponent<MeshRenderer>();
			hpChildren[x].renderer.material.shader = Shader.Find("Self-Illumin/Diffuse");
		}
		
		float xOffset = -hpBar.width/2;
		float yOffset = -r.bounds.size.y/2 - hpBar.height*3;
		float xyPadding = hpBar.padding/2;
		
		hpChildren[0].transform.localPosition = new Vector3(xOffset + xyPadding, yOffset + xyPadding, -0.04f);
		hpChildren[1].transform.localPosition = new Vector3(xOffset + xyPadding, yOffset + xyPadding, -0.03f);
		hpChildren[2].transform.localPosition = new Vector3(xOffset + xyPadding, yOffset + xyPadding, -0.02f);
		hpChildren[3].transform.localPosition = new Vector3(xOffset, yOffset, -0.01f);
		
		hpChildren[0].GetComponent<MeshFilter>().mesh = hpBar.Health.Mesh;
		hpChildren[1].GetComponent<MeshFilter>().mesh = hpBar.Hit.Mesh;
		hpChildren[2].GetComponent<MeshFilter>().mesh = hpBar.Empty.Mesh;
		hpChildren[3].GetComponent<MeshFilter>().mesh = hpBar.Outline.Mesh;
		
		hpChildren[0].renderer.material.mainTexture = healthColour;
		hpChildren[1].renderer.material.mainTexture = healthHitColour;
		hpChildren[2].renderer.material.mainTexture = healthEmptyColour;
		hpChildren[3].renderer.material.mainTexture = healthOutlineColour;
	}

	public void DisplayName()
	{
		// If we are using a font, load it, and create a plane/mesh
		// otherwise, use a sprite renderer and attach it to a child object.
	}
	
	public static void FlipSprite(Transform t)
	{
		t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
	}
	
	// Status effects later, just calculate damage for now;
	public static void DoDamage(Entity e, int damage)
	{
		e.health -= damage;
		UpdateHP(e);

		if (e.health <= 0)
		{
			e.health = 0;
			KillEntity(e);
		}
	}
	
	public static void KillEntity(Entity e)
	{
		// Check for death animation, otherwise, just destroy;
		// If there is a death animation, start coroutine and
		// destroy after the animation is done playing.
		Debug.Log ("Just pretend " + e + " has been killed.");
	}
	
	public static void UpdateHP(Entity e)
	{
		// Only affects the Orange health bar right now,
		// Need to implement the yellow damage hit bar.
		e.hpChildren[0].GetComponent<MeshFilter>().mesh = e.hpBar.UpdateCurrentHealth(e.health);
	}
}
using UnityEngine;
using System.Collections;


public enum MapType
{
	Forest,
	Cave
}


[RequireComponent(typeof (MeshFilter  ))]
[RequireComponent(typeof (MeshRenderer))]
public class Map : MonoBehaviour
{
	public MapType mapType;

	private MapData mapData;

	private Plane2D plane;

	private MeshFilter  filter;
	private TileSheet   tileSheet;

	TextureBuilder mapTexture;

	private string CollisionObject = "Tile Collision";
	void Start ()
	{
		InitializeComponents();

		GenerateMap();

		FinalizeMap();

	}
	private void InitializeComponents()
	{
		filter = GetComponent<MeshFilter >();

		mapData = new MapData(0, 0);
	}

	private void GenerateMap()
	{
		if (mapType == MapType.Forest)
		{
			ForestGenerator forestGen = new ForestGenerator(mapData);
		}
		if (mapType == MapType.Cave)
		{
			CaveGenerator caveGen = new CaveGenerator(mapData);
		}
	}

	private void GenerateMapCollision()
	{
		TileCollisionGenerator collisionGen;
		collisionGen = new TileCollisionGenerator(mapData);

		Transform    collisionLayer = transform.FindChild(CollisionObject);
		MeshCollider meshCollider   = collisionLayer.GetComponent<MeshCollider>();
		meshCollider.sharedMesh     = collisionGen.Mesh;
	}
	private void BuildMapTexture()
	{

		mapTexture = new TextureBuilder(mapData);
	
		Material meshMaterial = renderer.material;
		
		Shader matShader    = Shader.Find("Transparent/Cutout/Diffuse");
		meshMaterial.shader = matShader;
	}
	
	private void ApplyMapTexture()
	{
		Material meshMaterial    = renderer.material;
		meshMaterial.mainTexture = mapTexture.Texture;
	}

	private void FinalizeMap()
	{
		plane = new Plane2D(mapData.Width, mapData.Height);
		filter.mesh = plane.Mesh;

		GenerateMapCollision();
		BuildMapTexture();
		ApplyMapTexture();
	}

	public MapData GetMapData()
	{
		return mapData;
	}
}

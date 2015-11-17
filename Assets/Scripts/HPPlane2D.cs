using UnityEngine;

//Creates a plane with only 2 triangles
public class HPPlane2D 
{
	public HPPlane2D(float width, float height)
	{
		Width  = width;
		Height = height;

		this.Mesh = new Mesh();
		MakePlane();
	}
	
	public float Width
	{
		get;
		private set;
	}
	
	public float Height
	{
		get;
		private set;
	}
	
	public Mesh Mesh
	{
		get;
		private set;
	}
	
	public Mesh Resize(float width, float height)
	{
		Width = width;
		Height = height;
		MakePlane();
		return Mesh;
	}
	
	public void InvertPlane()
	{
		InvertTriangles();
		this.Mesh.RecalculateNormals();
	}
	
	private void MakePlane()
	{
		this.Mesh.vertices   = MakeVerticies ();
		this.Mesh.triangles  = MakeTriangles ();
		this.Mesh.normals    = MakeNormals   ();
		this.Mesh.uv         = MakeUV        (); 
	}
	
	private Vector3[] MakeVerticies ()
	{
		Vector3[] verticies = new Vector3[4];
		
		verticies[0] = new Vector3 (0,     0,     0);
		verticies[1] = new Vector3 (Width, 0,     0);
		verticies[3] = new Vector3 (Width, Height, 0);
		verticies[2] = new Vector3 (0,     Height, 0);
		
		return verticies;
	}
	
	private Vector3[] MakeNormals ()
	{
		Vector3[] normals = new Vector3[4];
		
		for (int corner = 0; corner < 4; ++corner)
		{
			normals[corner] = Vector3.up;
		}
		
		return normals;
	}
	
	private Vector2[] MakeUV()
	{
		Vector2[] uv = new Vector2[4];
		
		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);
		
		return uv;
	}
	
	private int[] MakeTriangles()
	{
		int[] triangles = new int[6];
		
		//Triangle 1
		triangles[0] = 0;
		triangles[1] = 2;
		triangles[2] = 3;
		
		//Triangle 2
		triangles[3] = 0;
		triangles[4] = 3;
		triangles[5] = 1;
		
		return triangles;
	}

	private void InvertTriangles()
	{
		int temp               = this.Mesh.triangles[1];
		this.Mesh.triangles[1] = this.Mesh.triangles[2];
		this.Mesh.triangles[2] = temp;

		temp                   = this.Mesh.triangles[4];
		this.Mesh.triangles[4] = this.Mesh.triangles[5];
		this.Mesh.triangles[5] = temp;

	}
}

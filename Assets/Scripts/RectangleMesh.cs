using UnityEngine;
using System.Collections;

public class RectangleMesh
{
	public Mesh Mesh
	{
		get;
		private set;
	}
	
	private Rect dimensions;
	private int  depth;
	
	private Vector3[] verticies;
	private int    [] triangles;
	

	public RectangleMesh(Rect dimensions, int depth = 1)
	{
		this.Mesh       = new Mesh();
		this.dimensions = dimensions;
		this.depth      = depth;

	}
	public void BuildHorizontalMesh()
	{
		BuildMesh(true);
	}
	public void BuildVerticalMesh()
	{
		BuildMesh(false);
	}
	public void BuildDiagonalMesh()
	{
		BuildMesh(false);
	}
	private void BuildMesh(bool horizontal)
	{
		if (horizontal)
		{
			MakeHorizontalVerticies ();
		}
		else
		{
			MakeVerticalVerticies ();
		}
		
		MakeTriangles ();
		
		this.Mesh.vertices  = verticies;
		this.Mesh.triangles = triangles;
	}
	
	public void InvertMesh ()
	{
		InvertTriangles ();
		this.Mesh.triangles = triangles;
	}
	
	private void MakeHorizontalVerticies()
	{
		verticies = new Vector3[4];
		
		float x      = dimensions.x;
		float y      = dimensions.y;
		float width  = dimensions.width;
		float height = dimensions.height;
		
		verticies[0] = new Vector3 (x,         y + height,  depth); // top front left
		verticies[1] = new Vector3 (x + width, y + height,  depth); // top front right
		verticies[2] = new Vector3 (x,         y + height, -depth); // top back  left
		verticies[3] = new Vector3 (x + width, y + height, -depth); // top back  right
	}
	private void MakeVerticalVerticies()
	{
		verticies = new Vector3[4];
		
		float x      = dimensions.x;
		float y      = dimensions.y;
		float width  = dimensions.width;
		float height = dimensions.height;
		
		verticies[0] = new Vector3 (x + width, y + height,  depth); // top front left
		verticies[1] = new Vector3 (x + width, y + height, -depth); // top back
		verticies[2] = new Vector3 (x, y,           depth); // bottom front
		verticies[3] = new Vector3 (x, y,          -depth); // bottom back
	}
	
	private void MakeTriangles()
	{
		triangles = new int[6];
		
		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 3;
		
		triangles[3] = 0;
		triangles[4] = 3;
		triangles[5] = 2;
	}

	private void InvertTriangles ()
	{
		for (int i = 1; i < 6; i += 3)
		{
			int temp         = triangles[i];
			triangles[i]     = triangles[i + 1];
			triangles[i + 1] = temp;
		}
	}
}

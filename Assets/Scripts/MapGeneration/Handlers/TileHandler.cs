using UnityEngine;
using System.Collections;

public class TileHandler : MonoBehaviour 
{
	public static TileHandler tileHandler;

	public Texture2D  tileSheet;
	public int        resolution;

	public CollisionType[] tileCollisionType;

	public static Tile[]    tiles;
	public static Color[][] tilePixels;

	public int EmptyTile
	{
		get;
		private set;
	}

	public TileSheet TileSheetData
	{
		get;
		private set;
	}

	public Tile this[int tileId]
	{
		get
		{
			return tiles[tileId];
		}
		private set
		{

		}

	}
	public Tile this[ForestTileTypes tileType]
	{
		get
		{
			return tiles[(int) tileType];
		}
		private set
		{
			
		}
		
	}
	void Awake()
	{
		if (tileHandler != null)
		{
			GameObject.Destroy(tileHandler);
		}
		else
		{
			tileHandler = this;
		}


		TileSheetData = new TileSheet(tileSheet, resolution);

		tiles      = new Tile[TileSheet.totalTiles];
		tilePixels = new Color[TileSheet.totalTiles][];

		for (int i = 0; i < tiles.Length; ++i)
		{
			tiles[i]      = new Tile(i, tileCollisionType[i]);
			tilePixels[i] = TileSheetData[i];
		}

		EmptyTile = tiles.Length - 1;
	}


	public static string ToString()
	{
		string retString = "";
		for (int i = 0; i < tiles.Length; ++i)
		{
			retString += "[" + tiles[i].Id + ", " + tiles[i].CollisionType.ToString() + "], ";
		}

		return retString;
	}

}

using UnityEngine;
using System.Collections;

public class TextureBuilder
{
	public Texture2D Texture
	{
		get;
		private set;
	}

	private MapData     map;
	private TileSheet   tiles;
	private TileHandler tileHandler;

	private int resolution;
	public TextureBuilder(MapData map)
	{
		this.map   = map;
		tileHandler = TileHandler.tileHandler;

		this.tiles = tileHandler.TileSheetData;

		int textureWidth  = map.Width  * tileHandler.resolution;
		int textureHeight = map.Height * tileHandler.resolution;

		Texture = new Texture2D(textureWidth, textureHeight);
		BuildTexture();

		Texture.filterMode = FilterMode.Point;
		Texture.wrapMode   = TextureWrapMode.Clamp;

		Apply();
	}

	public void Apply()
	{
		Texture.Apply();
	}

	private void BuildTexture()
	{
		int resolution = tileHandler.resolution;
		for (int y = 0; y < map.Height; ++y)
		{
			int yPos = y * resolution;
			for (int x = 0; x < map.Width; ++x)
			{
				int xPos = x * resolution;

				int     tileId       = map[y, x].Id;
				Color[] pixelsToDraw = tiles[tileId];

				Texture.SetPixels(xPos, yPos, resolution, resolution, pixelsToDraw);
			}
		}
	}


}

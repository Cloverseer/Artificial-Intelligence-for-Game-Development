using UnityEngine;
using System.Collections;

public class TileSheet
{
	private Texture2D tileSheet;
	private int       resolution;

	private Color[][] tilePixelData;

	private int rowLength;
	private int colLength;

	public static int totalTiles;

	public TileSheet(Texture2D tileSheet, int resolution)
	{
		this.tileSheet  = tileSheet;
		this.resolution = resolution;
		rowLength = tileSheet.height  / resolution; 
		colLength = tileSheet.width   / resolution;



		totalTiles = rowLength * colLength + 1;
		Debug.Log("ROWS: " + tileSheet.height + " COLS: " + tileSheet.width + " Total Tiles: " + totalTiles);
		
		tilePixelData = new Color[totalTiles][]; 
		
		CutTileSheet();
		CreateTransparentTile();
	}

	public Color[] this[int index]
	{
		get
		{
			if (index < 0 || index >= totalTiles)
			{
				throw new System.IndexOutOfRangeException("index must be >= 0 and less than " + totalTiles + "(Total Tiles)");
			}
			return tilePixelData[index];
		}
		private set
		{
			//do nothing
		}
	}

	private void CreateTransparentTile()
	{
		int totalPixels = resolution * resolution;

		Color[] tilePixels  = new Color[totalPixels];
		Color   transparent = new Color(0, 0, 0, 0); 

		for (int pixel = 0; pixel < totalPixels; ++pixel)
		{
			tilePixels[pixel] = transparent;
		}

		tilePixelData[tilePixelData.Length - 1] = tilePixels;
	}
	private void CutTileSheet()
	{
		Color[] tilePixels;

		int tileIndex = 0;
		for (int y = 0; y < rowLength; ++y)
		{
			int yPos = y * resolution;
			for (int x = 0; x < colLength; ++x)
			{
				int xPos = x * resolution;

				tilePixels = tileSheet.GetPixels(xPos, yPos, resolution, resolution);

				tilePixelData[tileIndex] = tilePixels;
				++tileIndex;
			}
		}
	}
}

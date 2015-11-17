using UnityEngine;
using System.Collections;

public enum ForestTileTypes : int
{
	LeftTop = 0,
	RightTop,
	LeftMiddle,
	RightMiddle,
	
	LeftWall,
	RightWall,
	LeftBottom,
	RightBottom,
	
	Filler,
	Blank,
	Blank2,
	Blank3,
	
	GrassLeftTop,
	GrassRightTop,
	GrassLeftMiddle,
	GrassRightMiddle,
	
	PurpleFlower,
	RedFlower,
	BlueFlower,
	OrangeFlower,
	
	EmptyTile
};

public class ForestGenerator 
{


	private MapData     mapData;
	private TileHandler tileHandler;

	private int startFloorHeight;
	public ForestGenerator(MapData mapData)
	{
		this.mapData = mapData;
		tileHandler  = TileHandler.tileHandler;
		mapData.Width  = 80;
		mapData.Height = 80;

		FillMapWithEmptyTile();

		MakeFloor();
	}

	private void FillMapWithEmptyTile()
	{
		for (int row = 0; row < mapData.Height; ++row)
		{
			for (int col = 0; col < mapData.Width; ++col)
			{
				mapData[row, col] = tileHandler[ForestTileTypes.EmptyTile];
			}
		}
	}

	private void MakeFloor()
	{
		startFloorHeight = Random.Range(0, mapData.Height / 4);

		int floorHeight = startFloorHeight;

		bool up = false;
		bool down = false;


		bool hole = false;
		int count = 0;
		for (int col = 0; col < mapData.Width; ++col)
		{
			if ((col == 0 && floorHeight - 1 >= 0) || count == 5)
			{
				if (count == 5)
				{
					BuildRow(floorHeight - 1, col, ForestTileTypes.LeftWall);
				}
				BuildRow(floorHeight - 1, col, ForestTileTypes.LeftWall);
			}
			else if ((col + 1 >= 0 && floorHeight - 1 >= 0) || count == 5)
			{
				BuildRow(floorHeight - 1, col, ForestTileTypes.RightWall);
			}
			int[] middleTiles = 
			{ 
				(int) ForestTileTypes.LeftMiddle,
				(int) ForestTileTypes.RightMiddle
			};
			
			int start = col;

			if (up)
			{
				if (floorHeight - 1 >= 0)
				{
					mapData[floorHeight - 1, col] = tileHandler[(ForestTileTypes.LeftWall)];
				}
				if (floorHeight - 2 >= 0)
				{
					mapData[floorHeight - 2, col] = tileHandler[(ForestTileTypes.LeftBottom)];
				}
				BuildRow(floorHeight - 3, col, ForestTileTypes.Filler);
			}
			if (down)
			{
				if (floorHeight - 1 >= 0)
				{
					mapData[floorHeight - 1, col] = tileHandler[(ForestTileTypes.LeftBottom)];
				}
				BuildRow(floorHeight - 2, col, ForestTileTypes.Filler);
			}
			mapData[floorHeight, col] = tileHandler[(ForestTileTypes.LeftTop)];
			MakeGrass      (floorHeight, col, 0);

			++col;

			int floorLength = Random.Range(3, 7);


			if (count == 5)
			{
				BuildRow(floorHeight - 1, col - 1, ForestTileTypes.LeftWall);
			}
			for (int i = 0; col + 1 <= mapData.Width && i < floorLength; ++col, ++i)
			{
				if (count == 5) continue;
				int randNum = Random.Range(0, 2);
				int tile = middleTiles[randNum];

				mapData[floorHeight, col] = tileHandler[tile];
				MakeGrass      (floorHeight, col, randNum + 1);
				BuildRow(floorHeight - 1, col, ForestTileTypes.Filler);
			}
			++count;
			if (col >= mapData.Height) return;
			mapData[floorHeight, col] = tileHandler[(ForestTileTypes.RightTop)];
			MakeGrass(floorHeight, col, 3);




			if (Random.Range(0, 10) < 5)
			{
				++floorHeight;
				up = true;
				down = false;
			}
			else
			{
				--floorHeight;
				up = false;
				down = true;
			}

			if (down)
			{
				if (floorHeight >= 0)
				{
					mapData[floorHeight - 0, col] = tileHandler[(ForestTileTypes.RightWall)];
				}
				if (floorHeight - 1 >= 0)
				{
					mapData[floorHeight - 1, col] = tileHandler[(ForestTileTypes.RightBottom)];
				}
				if (floorHeight - 2 >= 0)
				{
					BuildRow(floorHeight - 2, col, ForestTileTypes.Filler);
				}
			}
			if (up)
			{
				if (floorHeight - 2 >= 0)
				{
					mapData[floorHeight - 2, col] = tileHandler[(ForestTileTypes.RightBottom)];
				}
				BuildRow(floorHeight - 3, col, ForestTileTypes.Filler);
			}
			if (floorHeight < 0)
			{
				floorHeight = 0;
			}
			if (floorHeight >= mapData.Height)
			{
				floorHeight = mapData.Height - 1;
			}

			if (count == 6)
			{
				BuildRow(floorHeight - 2, col, ForestTileTypes.RightWall);
			}

		}
	}

	private void MakeGrass(int row, int col, int grass)
	{
		if (row + 1 >= mapData.Height)
		{
			return;
		}

		int[] grassTiles = 
		{
			(int) ForestTileTypes.GrassLeftTop,
			(int) ForestTileTypes.GrassRightTop,
			(int) ForestTileTypes.GrassLeftMiddle,
			(int) ForestTileTypes.GrassRightMiddle
		};

		mapData[row + 1, col] = tileHandler[grassTiles[grass]];
	}

	private void BuildRow(int row, int col, ForestTileTypes type)
	{
		for (int i = row; i >= 0; --i)
		{
			mapData[i, col] = tileHandler[type];
		}
	}

}
















using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager
{

		//Map of level
		private Tile[,] levelMap;
	
		//Flag for being inside a platform
		private bool inPlatform = false;
	
		//Average length of level platforms
		public float average = 0.0f;
		private float averageAim = 1.5f;
	
		//Total number of level plaforms
		public int totalPlatforms = 0;
	
		//Platform struct
		private struct Platform
		{
				public int id;
				public int x_start;
				public int y_start;
				public int length;
		}
	
		private List<Platform> platforms;
	
		public PlatformManager ()
		{
				platforms = new List<Platform> ();
				//Debug.Log ("HELLO IN PLATFORM MANAGER");
		}
	
		public void LoadMap (Tile[,] levelIn)
		{
				levelMap = levelIn;
		}

		public Tile[,] GetUpdatedMap ()
		{
				return levelMap;
		}

		//Get List of platforms in current level layout
		public void RegisterPlatforms ()
		{
		
				platforms.Clear ();
				int level_length = levelMap.GetLength (0);
				int level_height = levelMap.GetLength (1);
				int platform_height = 0;
		
				int platform_length = 0;
				int platform_start = 0;

				//For each level_y

				for (platform_height = 0; platform_height < 3; platform_height++) {
				
						//For each horizontal tile  
						for (int i =0; i < level_length; i++) {
			
			
								if (levelMap [i, platform_height].state == 1) {
										//If tile is dead(a gap) and not already inside a gap, 
										//set gap start position, increase gap length, and set inGap flag
										if (inPlatform == false) {
												if (platform_length == 0) {
														platform_start = i;
														platform_length++;
												}
												inPlatform = true;
										}
				//if already inside gap, increase length
				else if (inPlatform == true) {
												platform_length++;
										}
								}	
			//If tile is dead and inPlatform is true, then platform has closed
			else if (levelMap [i, platform_height].state == 0 && inPlatform) {
				
										//Completed gap is added to collection
										Platform newPlatform;
										newPlatform.id = 1;
										newPlatform.x_start = platform_start;
										newPlatform.y_start = platform_height;
										newPlatform.length = platform_length;
				
										platforms.Add (newPlatform);

										//Gap counters reset
										inPlatform = false;
										platform_length = 0;
								} 
								if (i == level_length - 1 && inPlatform) {
										//Debug.Log ("INSIDE FINAL PLATFORM");
										//Completed gap is added to collection
										Platform newPlatform;
										newPlatform.id = 1;
										newPlatform.x_start = platform_start;
										newPlatform.y_start = platform_height;
										newPlatform.length = platform_length;
				
										platforms.Add (newPlatform);
				
										//Gap counters reset
										inPlatform = false;
										platform_length = 0;
								}
						}

				}
		}

		public void RemoveSingleSpacePlaforms ()
		{
				//Gap tempGap;
				foreach (Platform p in platforms) {
						Debug.Log ("REMOVE SINGLE PLATS");
						//if (g.length > tempGap.length) {
						//	tempGap = g;
						//}
						if (p.length == 1) {
								Debug.Log ("REMOVE THIS PLAT");
								levelMap [p.x_start, p.y_start].state = 0;
						}
				}
				RegisterPlatforms ();
		
		}

		public int GetTotalPlatforms ()
		{
				return platforms.Count;
		}

		public void DebugPlatforms ()
		{
		
				//Debug.Log ("DEBUG Platforms");
		
				Debug.Log ("Number of Platforms" + platforms.Count.ToString ());
				foreach (Platform p in platforms) {
						Debug.Log ("x_start of Platform " + p.x_start.ToString () + 
								" length: " + p.length.ToString () +
			           " height: " + p.y_start.ToString ());
				}
		
		}

}

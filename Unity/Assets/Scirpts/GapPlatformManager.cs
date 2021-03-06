using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GapPlatformManager
{

		//Map of level
		private Tile[,] levelMap;

		//Flag for being inside a gap
		private bool inGap = false;

		//Average length of level gaps
		public float gap_average = 0.0f;
		private float gap_averageAim = 1.5f;

		//Total number of level gaps
		public int gap_total = 0;
		public int gap_max_length = 5;


		//Flag for being inside a platform
		private bool inPlatform = false;
	
		//Average length of level platforms
		public float average = 0.0f;
		private float averageAim = 1.5f;
	
		//Total number of level plaforms
		public int totalPlatforms = 0;


		//Gap struct
		private struct Gap
		{
				public int id;
				public int x_start;
				public int length;
		}
		//Platform struct
		private struct Platform
		{
				public int id;
				public int x_start;
				public int y_start;
				public int length;
		}

		private List<Gap> gaps;
		private List<Platform> platforms;

		public GapPlatformManager ()
		{
				gaps = new List<Gap> ();
				platforms = new List<Platform> ();
		}

		public void LoadMap (Tile[,] levelIn)
		{
				levelMap = levelIn;
				RegisterGaps ();
				RegisterPlatforms ();
		}

		public Tile[,] GetUpdatedMap ()
		{
				return levelMap;
		}

		public int GetTotalGaps ()
		{
				return gaps.Count;
		}

		//Get List of gaps in current level layout
		public void RegisterGaps ()
		{

				gaps.Clear ();
				int level_length = levelMap.GetLength (0);
				int level_height = levelMap.GetLength (1);
				int platform_height = 0;

				int gap_length = 0;
				int gap_start = 0;

				//For each horizontal tile  
				for (int i =0; i < level_length; i++) {


						if (levelMap [i, platform_height].state == 0) {
								//If tile is dead(a gap) and not already inside a gap, 
								//set gap start position, increase gap length, and set inGap flag
								if (inGap == false) {
										if (gap_length == 0) {
												gap_start = i;
												gap_length++;
										}
										inGap = true;
								}
				//if already inside gap, increase length
				else if (inGap == true) {
										gap_length++;
								}
						}	
			//If tile is alive and inGap is true, then gap has closed
			else if (levelMap [i, platform_height].state == 1 && inGap) {

								//Completed gap is added to collection
								Gap newGap;
								newGap.id = 1;
								newGap.x_start = gap_start;
								newGap.length = gap_length;

								gaps.Add (newGap);

								//Gap counters reset
								inGap = false;
								gap_length = 0;
						}
				}
	
		}

		public float findAverage ()
		{
				RegisterGaps ();
				int total = 0;

				int totalGaps = gaps.Count;


				foreach (Gap g in gaps) {
						total += g.length;
//			Debug.Log("In Gaps: " + g.x_start.ToString() + ", " + g.length.ToString());
				}
				//Debug.Log("Total Tiles in Gaps: " + total.ToString());
				//Debug.Log("Total Gaps: " + totalGaps.ToString());
				return average = (float)total / totalGaps;
				/*	Debug.Log ("Total Gap length: " + total.ToString () +
						" No of Gaps: " + totalGaps.ToString () +
						" AVERAGE GAP LENGTH: " + average.ToString ());*/
		}

		public void ReduceAverage ()
		{
				Gap longestGap;
				Gap tempGap;
				tempGap.length = 0;
				tempGap.id = 1;
				tempGap.x_start = 0;

				foreach (Gap g in gaps) {
						if (g.length > tempGap.length) {
								tempGap = g;
						}
				}

				longestGap = tempGap;

				/*Debug.Log ("Longest Gap: x-" + longestGap.x_start.ToString () +
						" Length-" + longestGap.length.ToString ());*/

				levelMap [(longestGap.x_start + longestGap.length) - 1, 0].state = 1;
	
		}

		public void IncreaseAverage ()
		{
				Gap shortestGap;
				Gap tempGap;
				tempGap.length = 10;
				tempGap.id = 1;
				tempGap.x_start = 0;
		
				foreach (Gap g in gaps) {
						if (g.length < tempGap.length) {
								tempGap = g;
						}
				}
		
				shortestGap = tempGap;
		
				/*Debug.Log ("Longest Gap: x-" + longestGap.x_start.ToString () +
						" Length-" + longestGap.length.ToString ());*/
		
				levelMap [(shortestGap.x_start + shortestGap.length), 0].state = 0;
		
		}

		public void  ReduceToMax ()
		{
				//Debug.Log ("In reduce to max");
				foreach (Gap g in gaps) {
						//Debug.Log ("Gap length: " + g.length.ToString ());
						if (g.length > gap_max_length) {
								int over = g.length - gap_max_length;
								//Debug.Log ("TILE " + g.x_start.ToString () + " OVER BY: " + over.ToString ());
								for (int i = 0; i < over; i++) {
										levelMap [g.x_start + g.length - (1 + i), 0].state = 1;
								}


						
						}
				}

		}

		public void RemoveSingleSpaceGaps ()
		{
				//Gap tempGap;
				foreach (Gap g in gaps) {
					
						if (g.length == 1) {
								levelMap [g.x_start, 0].state = 1;
						}
				}
	
		}

		public void DrillUp ()
		{
				//Debug.Log ("IN drill up");
				int gap_no = 0;
				foreach (Gap g in gaps) {
						gap_no ++;
						//	Debug.Log ("IN drill foreach");
						for (int i = g.x_start; i < g.x_start + g.length; i++) {
								//	Debug.Log ("IN drill up loop i: " + i.ToString());
								levelMap [i, 1].state = 0;
								levelMap [i, 2].state = 0;
						}		
		
				}
	
	
		}

		public void ReduceGaps (int number)
		{

				int toRemove = gaps.Count - number;
				Random.seed = number;

				for (int i = 0; i < toRemove; i++) {

						int remove = (int)(Random.value * 1000) % gaps.Count;
	
						Gap remove_gap = gaps [remove];

						for (int j = remove_gap.x_start; j < (remove_gap.x_start + remove_gap.length); j++) {
			

								levelMap [j, 0].state = 1;

						}

						gaps.RemoveAt (remove);
				}
		}

		public void IncreaseGaps (int number)
		{
				int toAdd = number - gaps.Count;
				//Debug.Log (toAdd.ToString ());
				Platform temp;
				temp.x_start = 0;
				temp.length = 0;
				for (int i = 0; i < toAdd; i++) {
						
						temp.length = 0;
						
						//Select longest platform
						foreach (Platform p in platforms) {
								
								if (p.length > temp.length && p.y_start == 0) {
										temp = p;
									
								}
						}
						int xpos = temp.x_start + (temp.length / 2);
						levelMap [xpos, 0].state = 0;
						levelMap [xpos + 1, 0].state = 0;

						RegisterGaps ();
						DrillUp ();
						RegisterPlatforms ();
				}
				//	Debug.Log (temp.x_start.ToString () + ", " + temp.length.ToString ());
			
		}

		//========================================================================

	
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
						//Debug.Log ("REMOVE SINGLE PLATS");
						//if (g.length > tempGap.length) {
						//	tempGap = g;
						//}
						if (p.length == 1) {
								//Debug.Log ("REMOVE THIS PLAT");
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

		public void DebugGaps ()
		{
		
				Debug.Log ("DEBUG GAPS");
		
				Debug.Log ("Number of GAPS" + gaps.Count.ToString ());
				foreach (Gap g in gaps) {
						Debug.Log ("x_start of GAPS " + g.x_start.ToString () + 
								" length: " + g.length.ToString ());
				}
		
		}

		
}

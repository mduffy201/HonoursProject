using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GapManager
{

		//Map of level
		private Tile[,] levelMap;

		//Flag for being inside a gap
		private bool inGap = false;

		//Average length of level gaps
		public float average = 0.0f;
		private float averageAim = 1.5f;

		//Total number of level gaps
		public int totalGaps = 0;

		//Gap struct
		private struct Gap
		{
				public int id;
				public int x_start;
				public int length;
		}

		private List<Gap> gaps;

		public GapManager ()
		{
				gaps = new List<Gap> ();
//				Debug.Log ("HELLO IN GAP MANAGER");
		}

		public void LoadMap (Tile[,] levelIn)
		{
				levelMap = levelIn;
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

		public void findAverage ()
		{

				int total = 0;

				int totalGaps = gaps.Count;


				foreach (Gap g in gaps) {
						total += g.length;
				}
				average = (float)total / totalGaps;
/*				Debug.Log ("Total Gap length: " + total.ToString () +
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

				Debug.Log ("Longest Gap: x-" + longestGap.x_start.ToString () +
						" Length-" + longestGap.length.ToString ());

				levelMap [(longestGap.x_start + longestGap.length) - 1, 0].state = 1;
	
		}

	public void RemoveSingleSpaceGaps(){
		//Gap tempGap;
		foreach (Gap g in gaps) {
			//if (g.length > tempGap.length) {
			//	tempGap = g;
			//}
			if(g.length == 1){
				levelMap[g.x_start, 0].state = 1;
			}
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
	public void DrillUp(){
		//Debug.Log ("IN drill up");
		int gap_no = 0;
		foreach (Gap g in gaps) {
			gap_no ++;
		//	Debug.Log ("IN drill foreach");
			for(int i = g.x_start; i < g.x_start + g.length; i++ ){
			//	Debug.Log ("IN drill up loop i: " + i.ToString());
				levelMap[i, 1].state = 0;
				levelMap[i, 2].state = 0;
			}		
		
		}
	
	
	}
		public void UpdateMap ()
		{


		}

}

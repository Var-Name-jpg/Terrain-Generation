using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public class Land {
		public int Length { get; set; }
		public int Height { get; set; }
		public int Anchors { get; set; }

		public float[,] Map { get; set; }

		public Land(int length, int height, int anchors) {
			Length = length;
			Height = height;
			Anchors = anchors;
			Map = new float[Length, Height];
		}

		public int DistanceBetween(int x1, int y1, int x2, int y2) {
			return Math.sqrt( Math.Pow((x2 - x1), 2) - Math.Pow((y2 - y1), 2) );
		}

		public void StartMap() {
			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Length; j++) {
					Map[i,j] = 0;
				}
			}
		}

		public void SetAnchorPoints() {
			Random rand = new Random();
			int count = 0;

			while (count < Anchors) {
				int x = rand.Next(0, Length - 1);
				int y = rand.Next(0, Height - 1);

				if (Map[x,y] != 1) {
					Map[x,y] = 1;
					count++;
				}
			}		
		}

		public void FillMap() {
			for (int i = 0; i < 
		}

		public void PrintDataMap() {
			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Length; j++) {
					Console.Write($"|{Map[i,j]}");
				}
				Console.WriteLine();
			}
		}


	}
}

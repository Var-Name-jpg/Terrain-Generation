using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public class Land {
		public int Length { get; set; }
		public int Height { get; set; }

		public float[,] Map { get; set; }

		public Land(int length, int height) {
			Length = length;
			Height = height;
			Map = new float[Length, Height];
		}

		public void StartMap() {
			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Length; j++) {
					Map[i,j] = 0;
				}
			}
		}

		public void SetAnchorPoints() {
			
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

using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public class Land {
		public int Length { get; set; }
		public int Height { get; set; }
		public int AnchorCount { get; set; }

		public double[,] Map { get; set; }

		public List<Tuple<int, int>> Anchors = new List<Tuple<int, int>>();

		public Land(int length, int height, int anchorCount) {
			Length = length;
			Height = height;
			AnchorCount = anchorCount;
			Map = new double[Length, Height];
		}

		public double DistanceBetween(int x1, int y1, int x2, int y2) {
			return Math.Sqrt( Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2) );
		}

		public void StartMap() {
			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Length; j++) {
					Map[i,j] = 1;
				}
			}
		}

		public void SetAnchorPoints() {
			Random rand = new Random();
			int count = 0;

			while (count < AnchorCount) {
				int x = rand.Next(0, Length - 1);
				int y = rand.Next(0, Height - 1);

				if (Map[x,y] != 0) {
					Map[x,y] = 0;
					Anchors.Add( new Tuple<int, int>(x, y) );
					count++;
				}
			}
		}

		public void FillMap() {
			double closestDistance;

			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Length; j++) {
					if (Map[i,j] == 0)
						continue;

					closestDistance = double.MaxValue;

					foreach (var anchor in Anchors) {
						double distance = DistanceBetween(i, j, anchor.Item1, anchor.Item2);
						if ( distance < closestDistance) {
							closestDistance = distance;
						}
					}

					Map[i,j] = Math.Round(closestDistance / 10, 3);

					if (Map[i,j] > 1)
						Map[i,j] = 1;
				}
			}	
		}

		public void PrintDataMap() {
			List<string> retMap = new List<string>();

			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Length; j++) {
					retMap.Add($"{Map[i,j]}");
				}
				retMap.Add("\n");
			}

			Console.WriteLine( string.Join(',', retMap) );
		}

		public void PrintVisualMap() {
			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Length; j++) {
					double value = Map[i,j];

					if (0 <= value && value <= 0.2) { Console.Write("⬜"); }
					else if (0.2 < value && value <= 0.45) { Console.Write("🟫"); }
					else if (0.45 < value && value <= 0.85) { Console.Write("🟩"); }
					else if (0.85 < value && value <= 0.95) { Console.Write("🟨"); }
					else if (0.95 < value) { Console.Write("🟦"); }
				}
				Console.WriteLine();
			}
		}
	}
}

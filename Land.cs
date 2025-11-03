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

		public string CheckTileColor(double value) {
			if (0 <= value && value <= 0.2) { return "white";  }
			else if (0.2 < value && value <= 0.45) { return "brown"; }
			else if (0.45 < value && value <= 0.85) { return "green"; }
			else if (0.85 < value && value <= 0.95) { return "yellow"; }
			else if (0.95 < value) { return "blue"; }
			else { return "ERROR"; }
		}

		public void GenerateMap() {
			StartMap();
			SetAnchorPoints();
			FillMap();
			SmoothMapLerp();
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
					
					switch ( CheckTileColor(value) ) {
						case "white":
							Console.Write("⬜");
							break;

						case "brown":
							Console.Write("🟫");
							break;

						case "green":
							Console.Write("🟩");
							break;

						case "yellow":
							Console.Write("🟨");
							break;

						case "blue":
							Console.Write("🟦");
							break;

						default:
							Console.Write("X");
							break;
					}
				}
				Console.WriteLine();
			}
		}

		public void SmoothMapLerp(double influence = 0.5, double randomness = 0.1) {
			Random random = new Random();

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Length; x++) {
					double val = Map[y,x];
					foreach (var anchor in Anchors) {
						int ax = anchor.Item1;
						int ay = anchor.Item2;
						
						// Euclidean distance
						double dist = DistanceBetween(x, y, ax, ay);

						// Influence Decay
						double influenceFactor = influence * Math.Exp(-dist);

						// Lerp towards anchor "0"
						val = (1 - influenceFactor) * val + influenceFactor * 0;
					}
					
					// Add randomness
					val += (random.NextDouble() * 2 - 1) * randomness;

					Map[y,x] = Math.Round(val, 3);

					if (Map[y,x] > 1)
						Map[y,x] = 1;

					if (Map[y,x] < 0)
						Map[y,x] = 0;
				}
			}
		}

		public void SmoothMapIndexing() {
			Dictionary<string, int> surroundingColors = new Dictionary<string, int> {
				{ "white", 0 },
				{ "brown", 0 },
				{ "green", 0 },
				{ "yellow", 0 },
				{ "blue", 0 }
			};

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Length; x++) {
					// Get all surrounding Colors

					// Up
					switch ( CheckTileColor( Map[y-1,x] ) ) {
						case "white":
							surroundingColors["white"] += 1;
							break;

						case "brown":
							surroundingColors["brown"] += 1;
							break;

						case "green":
							surroundingColors["green"] += 1;
							break;

						case "yellow":
							surroundingColors["yellow"] += 1;
							break;

						case "blue":
							surroundingColors["blue"] += 1;
							break;

						default:
							Console.Write("X");
							break;
					}

					// Down
					switch ( CheckTileColor( Map[y+1,x] ) ) {
						case "white":
							surroundingColors["white"] += 1;
							break;

						case "brown":
							surroundingColors["brown"] += 1;
							break;

						case "green":
							surroundingColors["green"] += 1;
							break;

						case "yellow":
							surroundingColors["yellow"] += 1;
							break;

						case "blue":
							surroundingColors["blue"] += 1;
							break;

						default:
							Console.Write("X");
							break;
					}
					
					// Left
					switch ( CheckTileColor( Map[y,x-1] ) ) {
						case "white":
							surroundingColors["white"] += 1;
							break;

						case "brown":
							surroundingColors["brown"] += 1;
							break;

						case "green":
							surroundingColors["green"] += 1;
							break;

						case "yellow":
							surroundingColors["yellow"] += 1;
							break;

						case "blue":
							surroundingColors["blue"] += 1;
							break;

						default:
							Console.Write("X");
							break;
					}
					
					// Right
					switch ( CheckTileColor( Map[y,x+1] ) ) {
						case "white":
							surroundingColors["white"] += 1;
							break;

						case "brown":
							surroundingColors["brown"] += 1;
							break;

						case "green":
							surroundingColors["green"] += 1;
							break;

						case "yellow":
							surroundingColors["yellow"] += 1;
							break;

						case "blue":
							surroundingColors["blue"] += 1;
							break;

						default:
							Console.Write("X");
							break;
					}

					// Up Right
					switch ( CheckTileColor( Map[y-1,x+1] ) ) {
						case "white":
							surroundingColors["white"] += 1;
							break;

						case "brown":
							surroundingColors["brown"] += 1;
							break;

						case "green":
							surroundingColors["green"] += 1;
							break;

						case "yellow":
							surroundingColors["yellow"] += 1;
							break;

						case "blue":
							surroundingColors["blue"] += 1;
							break;

						default:
							Console.Write("X");
							break;
					}
					
					// Up Left
					switch ( CheckTileColor( Map[y-1,x-1] ) ) {
						case "white":
							surroundingColors["white"] += 1;
							break;

						case "brown":
							surroundingColors["brown"] += 1;
							break;

						case "green":
							surroundingColors["green"] += 1;
							break;

						case "yellow":
							surroundingColors["yellow"] += 1;
							break;

						case "blue":
							surroundingColors["blue"] += 1;
							break;

						default:
							Console.Write("X");
							break;
					}
				}
			}
		}
	}
}

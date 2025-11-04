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
			else if (0.85 < value && value <= 1.1) { return "yellow"; }
			else if (1.1 < value) { return "blue"; }
			else { return "ERROR"; }
		}

		public void GenerateMap() {
			StartMap();
			SetAnchorPoints();
			FillMap();
			SmoothMapLerp();
			SmoothMapIndexing();
		}

		public void StartMap() {
			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Length; j++) {
					Map[i,j] = double.MaxValue;
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

		// TODO: Change to "GetSurroundingColors" and return a Dictionary<string, int>
		public KeyValuePair<string, int> GetLargestColor() {
			Random random = new Random();
			Dictionary<string, int> surroundingColors = new Dictionary<string, int> {
				{ "white", 0 },
				{ "brown", 0 },
				{ "green", 0 },
				{ "yellow", 0 },
				{ "blue", 0 }
			};

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Length; x++) {

					// reset colorMap
					foreach (string color in surroundingColors.Keys)
						surroundingColors[color] = 0;

					// Get all surrounding Colors

					// Up
					if (y - 1 > 0) {
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
					}

					if (y + 1 < Height) {
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
					}
					
					if (x - 1 > 0) {
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
					}
					
					if (x + 1 < Length) {
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
					}

					if (y - 1 > 0 && x + 1 < Length) {
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
					}
					
					if (y - 1 > 0 && x - 1 > 0) {
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
					
					if (y + 1 < Height && x + 1 < Length) {
						// Down Right
						switch ( CheckTileColor( Map[y+1,x+1] ) ) {
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
					
					if (y + 1 < Height && x - 1 > 0) {
						// Down Left
						switch ( CheckTileColor( Map[y+1,x-1] ) ) {
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

			return surroundingColors.MaxBy(kvp => kvp.Value);
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

					if (Map[y,x] < 0)
						Map[y,x] = 0;
				}
			}
		}	

		public void SmoothMapIndexing() {
			// Random chance to set current tile to the highest value of surrounding tiles
			// TODO: Change to Dictionary<string, int>
			KeyValuePair<string, int> maxColorKVP = GetLargestColor();
			Random random = new Random();

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Length; x++) {
					// TODO: Need to get the largest collor here for each tile
					if (random.Next(1,5) == 1 && maxColorKVP.Value >= 4) {
						switch (maxColorKVP.Key) {
							case "white":
								Map[y,x] = 0.1;
								break;

							case "brown":
								Map[y,x] = 0.3;
								break;

							case "green":
								Map[y,x] = 0.6;
								break;

							case "yellow":
								Map[y,x] = 0.9;
								break;

							case "blue":
								Map[y,x] = 2;
								break;

							default:
								Map[y,x] = Map[y,x];
								break;
						}
					}
				}
			}
		}

		// TODO: Make an adjacency algorithm for ocean-wrapped sand, snow-wrapped mountain
	}
}

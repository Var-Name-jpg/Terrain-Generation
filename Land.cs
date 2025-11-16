using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public partial class Land {
		public int Length { get; set; }
		public int Height { get; set; }
		public int AnchorCount { get; set; }

		public Point[,] Map;

		

		public List<Point> Anchors = new List<Point>();

		public Land(int length, int height, int anchorCount) {
			Length = length;
			Height = height;
			AnchorCount = anchorCount;
			
			Map = new Point[Length, Height];

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Length; x++) {
					Map[y,x] = new Point(x, y);
				}
			}
			
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

		public void GenerateMap(int iterations) {
			SetAnchorPoints();
			BresenhamsAnchorPoints();
			FillMap();
			SmoothMapLerp();
			SmoothMapIndexing();
			for (int i = 0; i < iterations; i++)
				AdjacencyAlgorithm();
		}

		public void SetAnchorPoints() {
			Random rand = new Random();
			int count = 0;

			while (count < AnchorCount) {
				int x = rand.Next(0, Length - 1);
				int y = rand.Next(0, Height - 1);

				if (Map[x,y].Value != 0) {
					Map[x,y].Value = 0;
					Anchors.Add( Map[x,y] );
					count++;
				}
			}
		}

		public void BresenhamsAnchorPoints() {
			List<Point> pointsToAdd = new List<Point>();

			for (int i = 0; i < Anchors.Count; i++) {
				for (int j = 0; j < Anchors.Count; j++) {
					if (j == i)
						continue;

					List<Point> tempPoints = new List<Point>();

					int x = Anchors[i].X;
					int y = Anchors[i].Y;

					int x1 = Anchors[j].X;
					int y1 = Anchors[j].Y;

					if ( DistanceBetween(x, y, x1, y1) <= 20 ) {

						tempPoints = Bresenhams(x, y, x1, y1);
						foreach (Point point in tempPoints) {
							if (!pointsToAdd.Contains(point)) {
								pointsToAdd.Add(point);
							}
						}
					}
				}	
			}

			foreach (Point point in pointsToAdd) {
				if (Map[point.Y, point.X].Value != 0)
					Map[point.Y, point.X].Value = 0;

				if (!Anchors.Contains(point))
					Anchors.Add(point);
			}
		}

		public void FillMap() {
			double closestDistance;

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Length; x++) {
					if (Map[y,x].Value == 0)
						continue;

					closestDistance = double.MaxValue;

					foreach (var anchor in Anchors) {
						double distance = DistanceBetween(x, y, anchor.X, anchor.Y);
						if ( distance < closestDistance) {
							closestDistance = distance;
						}
					}

					Map[y,x].Value = Math.Round(closestDistance / 10, 3);
				}
			}	
		}

		public void PrintDataMap() {
			List<string> retMap = new List<string>();

			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Length; j++) {
					retMap.Add($"{Map[i,j].Value}");
				}
				retMap.Add("\n");
			}

			Console.WriteLine( string.Join(',', retMap) );
		}

		public void PrintVisualMap(int chunk, int chunkCount) {
			if (chunkCount == 1) {
				for (int y = 0; y < Height; y++) {
					for (int x = 0; x < Length; x++) {
						double value = Map[y,x].Value;
						
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

				return;
			} else {
				int tempChunkY = (int)Math.Ceiling( (double)chunk / (Length / 50));
				int tempChunkX = chunk % (Length / 50);
				if (tempChunkX == 0) {
					tempChunkX = Length / 50;
				}

				for (int y = (tempChunkY * 50) - 50; y < tempChunkY * 50; y++) {
					for (int x = (tempChunkX * 50) - 50; x < tempChunkX * 50; x++ ) {
						double value = Map[y,x].Value;
						
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

				return;
			}
		}

		public Dictionary<string, int> GetSurroundingColors(int x, int y) {
			Random random = new Random(1);
			Dictionary<string, int> surroundingColors = new Dictionary<string, int> {
				{ "white", 0 },
				{ "brown", 0 },
				{ "green", 0 },
				{ "yellow", 0 },
				{ "blue", 0 }
			};

			// reset colorMap
			foreach (string color in surroundingColors.Keys)
				surroundingColors[color] = 0;

			// Get all surrounding Colors

			// Up
			if (y - 1 > 0) {
				switch ( CheckTileColor( Map[y-1,x].Value ) ) {
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
				switch ( CheckTileColor( Map[y+1,x].Value ) ) {
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
				switch ( CheckTileColor( Map[y,x-1].Value ) ) {
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
				switch ( CheckTileColor( Map[y,x+1].Value ) ) {
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
				switch ( CheckTileColor( Map[y-1,x+1].Value ) ) {
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
				switch ( CheckTileColor( Map[y-1,x-1].Value ) ) {
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
				switch ( CheckTileColor( Map[y+1,x+1].Value ) ) {
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
				switch ( CheckTileColor( Map[y+1,x-1].Value ) ) {
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

			return surroundingColors;
		}

		public void SmoothMapLerp(double influence = 0.5, double randomness = 0.13) {
			Random random = new Random();

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Length; x++) {
					double val = Map[y,x].Value;
					foreach (var anchor in Anchors) {
						int ax = anchor.X;
						int ay = anchor.Y;
						
						// Euclidean distance
						double dist = DistanceBetween(x, y, ax, ay);

						// Influence Decay
						double influenceFactor = influence * Math.Exp(-dist);

						// Lerp towards anchor "0"
						val = (1 - influenceFactor) * val + influenceFactor * 0;
					}
					
					// Add randomness
					val += (random.NextDouble() * 2 - 1) * randomness;

					Map[y,x].Value = Math.Round(val, 3);

					if (Map[y,x].Value < 0)
						Map[y,x].Value = 0.1;
				}
			}
		}	

		public void SmoothMapIndexing() {
			Dictionary<string, int> surroundingColorDict = new Dictionary<string, int>();
			Random random = new Random();

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Length; x++) {

					surroundingColorDict = GetSurroundingColors(x, y);
					KeyValuePair<string, int> maxColorKVP = surroundingColorDict.MaxBy(kvp => kvp.Value);

					if (random.Next(1,5) == 1 && maxColorKVP.Value >= 4) {
						switch (maxColorKVP.Key) {
							case "white":
								Map[y,x].Value = 0.1;
								break;

							case "brown":
								Map[y,x].Value = 0.3;
								break;

							case "green":
								Map[y,x].Value = 0.6;
								break;

							case "yellow":
								Map[y,x].Value = 0.9;
								break;

							case "blue":
								Map[y,x].Value = 2;
								break;

							default:
								Map[y,x].Value = Map[y,x].Value;
								break;
						}
					}
				}
			}
		}

		// TODO: Make an adjacency algorithm for ocean-wrapped sand, snow-wrapped mountain
		public void AdjacencyAlgorithm() {
			
			Dictionary<string, int> surroundingColors = new Dictionary<string, int>();

			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Length; x++) {
					surroundingColors = GetSurroundingColors(x, y);
					string tileColor = CheckTileColor(Map[y,x].Value);

					switch ( tileColor ) {
						case "white":
							if ( surroundingColors["green"] > 1 )
								Map[y,x].Value = 0.3;

							break;

						case "brown":

							break;

						case "green":
							if ( surroundingColors["blue"] >= 7 )
								Map[y,x].Value = 2;

							if ( surroundingColors["yellow"] > 1 && surroundingColors["blue"] > 1 )
								Map[y,x].Value = 0.5;

							break;

						case "yellow":
							if ( surroundingColors["blue"] >= 7 )
						       		Map[y,x].Value = 2;
					       		if ( surroundingColors["green"] >= 6 )
						       		Map[y,x].Value = .5;

							break;

						case "blue":
							if ( surroundingColors["yellow"] >= 6 )
								Map[y,x].Value = 1;

							break;

						default:
							throw new Exception("This should never be thrown.... You've got some big problems...");


					}
				}
			}
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public class Program {
		public static void Main(string[] args) {
			int DIMENSIONS = 0;
			
			// Get size of the map
			Console.WriteLine("WELCOME TO PIXELS!");
			Console.WriteLine("Please choose map size:");
			Console.WriteLine("\n1. Small");
			Console.WriteLine("2. Medium");
			Console.WriteLine("3. Large");
			Console.Write(">> ");

			
			int input = int.Parse(Console.ReadLine());

			int chunkCount = 1;

			if (input == 1) {
				DIMENSIONS = 50;
				chunkCount = 1;
			} else if (input == 2) {
				DIMENSIONS = 150;
				chunkCount = 9;
			} else if (input == 3) {
				DIMENSIONS = 300;
				chunkCount = 36;
			}
				
			int ANCHORS = DIMENSIONS / 10 * 2;
			Land testMap = new Land(DIMENSIONS, DIMENSIONS, ANCHORS);
		
			testMap.GenerateMap(100);

			int chunk = 1;

			while (true) {
				Console.Clear();

				testMap.PrintVisualMap(chunk, chunkCount);
				string interactionInput = Console.ReadLine();

				switch (interactionInput) {
					case "d":
						if (chunkCount != 1 && (chunk * 50) < testMap.Length) { chunk++; }
						else { Console.WriteLine("Cannot Move That Way!"); }
						break;
					case "a":
						if (chunkCount != 1 && ((chunk * 50) - 50) > 0) { chunk--; }
						else { Console.WriteLine("Cannot Move That Way!"); }
						break;
				}
			}
		}
	}
}

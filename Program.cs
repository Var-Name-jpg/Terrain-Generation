using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public class Program {
		public static void Main(string[] args) {
			int DIMENSIONS = 100;
			int ANCHORS = DIMENSIONS / 10 * 2;
			Land testMap = new Land(DIMENSIONS, DIMENSIONS, ANCHORS);
		
			testMap.GenerateMap(100);
			testMap.PrintVisualMap();
		}
	}
}

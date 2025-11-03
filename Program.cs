using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public class Program {
		public static void Main(string[] args) {
			Land testMap = new Land(50, 50, 7);
		

			testMap.StartMap();
			testMap.SetAnchorPoints();
			testMap.FillMap();
			testMap.SmoothMapLerp();
			testMap.PrintDataMap();
			testMap.PrintVisualMap();
		}
	}
}

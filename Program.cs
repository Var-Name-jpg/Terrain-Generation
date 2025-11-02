using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public class Program {
		public static void Main(string[] args) {
			Land testMap = new Land(10, 10, 2);

			testMap.SetAnchorPoints();
			testMap.PrintDataMap();
		}
	}
}

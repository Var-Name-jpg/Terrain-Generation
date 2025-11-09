using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public class Point {
		public int X { get; set; }
		public int Y { get; set; }
		public double Value { get; set; }

		public Point(int x, int y) {
			X = x;
			Y = y;
			Value = double.MaxValue;
		}
	}
}

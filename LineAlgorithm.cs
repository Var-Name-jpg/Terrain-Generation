using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public abstract class Land {
		public List<(int, int)> points Bresenhams(int x0, int y0, int x1, int y1) {
			List<(int, int)> points = new List<(int, int)>();

			int dx = Math.Abs(x1 - x0);
			int dy = Math.Abs(y1 - y0);

			bool steep = dy > dx;

			if (steep) {
				// Swap x and y
				Swap(ref x0, ref x1);
				Swap(ref y0, ref y1);

				// Recalculate
				dx = Math.Abs(x1 - x0);
				dy = Math.Abs(y1 - y0);
			}


		}
	}

	public void Swap(ref int a, ref int b) {
		int temp = a;
		a = b;
		b = temp;
	}
}

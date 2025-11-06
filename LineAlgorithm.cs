using System;
using System.Collections;
using System.Collections.Generic;

namespace Terrain {
	public partial class Land {
		public List<(int, int)> Bresenhams(int x0, int y0, int x1, int y1) {
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

			// Always draw left to right
			if (x0 > x1) {
				Swap(ref x0, ref x1);
				Swap(ref y0, ref y1);
			}

			int error = dx / 2;
			int yStep = (y0 < y1) ? 1 : -1;
			int y = y0;

			for (int x = x0; x <= x1; x++) {
				// plot point, reverse if steep
				if (steep)
					points.Add((y, x));
				else
					points.Add((x, y));

				error -= dy;
				if (error < 0) {
					y += yStep;
					error += dx;
				}
			}

			return points;
		}

		public void Swap(ref int a, ref int b) {
			int temp = a;
			a = b;
			b = temp;
		}
	}
}

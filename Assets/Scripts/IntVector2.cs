using UnityEngine;

public class IntVector2 {
	public int x;
	public int y;

	public IntVector2() {
		x = y = 0;
	}

	public IntVector2(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public float Length() {
		return Mathf.Sqrt(x * x + y * y);
	}

	public int SquareMagnitude() {
		return x * x + y * y;
	}

	public int ManhattanLength() {
		return Mathf.Abs(x) + Mathf.Abs(y);
	}

	public static IntVector2 operator +(IntVector2 a, IntVector2 b) {
		return new IntVector2(a.x + b.x, a.y + b.y);
	}

	public static IntVector2 operator -(IntVector2 a, IntVector2 b) {
		return new IntVector2(a.x - b.x, a.y - b.y);
	}

	public static IntVector2 operator -(IntVector2 v) {
		return new IntVector2(-v.x, -v.y);
	}

	public static IntVector2 operator *(IntVector2 v, int s) {
		return new IntVector2(v.x * s, v.y * s);
	}

	public static IntVector2 operator *(int s, IntVector2 v) {
		return new IntVector2(v.x * s, v.y * s);
	}

	public override bool Equals(object obj) {
		if (obj == null)
			return false;
		if (ReferenceEquals(this, obj))
			return true;
		if (obj.GetType() != typeof(IntVector2))
			return false;
		IntVector2 other = (IntVector2)obj;
		return x == other.x && y == other.y;
	}

	public override int GetHashCode() {
		unchecked {
			return x.GetHashCode() ^ y.GetHashCode();
		}
	}

	public override string ToString() {
		return string.Format("[IntVector2: x={0}, y={1}]", x, y);
	}
	
}

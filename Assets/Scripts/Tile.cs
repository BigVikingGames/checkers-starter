using UnityEngine;
using System.Collections;

namespace BVG {
	public class Tile : MonoBehaviour {
		public IntVector2 Coordinates { get; set; }
		private Board parentBoard;

		public void SetParentBoard(Board board) {
			parentBoard = board;
		}

		void OnMouseDown() {
			parentBoard.BoardClicked(Coordinates);
		}
	}
}

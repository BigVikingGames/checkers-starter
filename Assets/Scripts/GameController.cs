using UnityEngine;
using System.Collections;

namespace BVG {
	public class GameController : MonoBehaviour {
		public Board board;
		[Range(0f, 60f)] public float gameStartDelay;

		void Start() {
			// TODO: start the game after the game start delay
			StartGame();
		}

		public void StartGame() {
			PopulateBoard();
		}

		private void PopulateBoard() {
			// black player
			for (int y = 0; y < 3; y++) {
				for (int x = 0; x < 8; x++) {
					if ((x + y) % 2 == 0) {
						IntVector2 boardPosition = new IntVector2(x, y);
						board.AddPlayerToken(false, boardPosition);
					}
				}
			}

			// red player
			for (int y = 5; y < 8; y++) {
				for (int x = 0; x < 8; x++) {
					if ((x + y) % 2 == 0) {
						IntVector2 boardPosition = new IntVector2(x, y);
						board.AddPlayerToken(true, boardPosition);
					}
				}
			}
		}
	}
}

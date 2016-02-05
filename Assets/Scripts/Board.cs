using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BVG {
	[RequireComponent(typeof(GridLayoutGroup))]
	public class Board : MonoBehaviour {
		public static readonly int BOARD_GRID_SIZE = 8;

		public GameObject blackSquarePrefab;
		public GameObject whiteSquarePrefab;
		public GameObject blackPiecePrefab;
		public GameObject redPiecePrefab;

		private Vector2 cellSize;

		public enum BoardState {
			EMPTY = 0,
			RED = 1,
			BLACK = 2
		}

		public enum MoveDirection {
			NORTH_EAST,
			SOUTH_EAST,
			SOUTH_WEST,
			NORTH_WEST
		}

		private readonly Dictionary<MoveDirection, IntVector2> directionLookup = new Dictionary<MoveDirection, IntVector2> {
			{ MoveDirection.NORTH_EAST, new IntVector2(1, -1) },
			{ MoveDirection.SOUTH_EAST, new IntVector2(1, 1) },
			{ MoveDirection.SOUTH_WEST, new IntVector2(-1, 1) },
			{ MoveDirection.NORTH_WEST, new IntVector2(-1, -1) }
		};

		private BoardState[,] boardStates = new BoardState[BOARD_GRID_SIZE, BOARD_GRID_SIZE];
		private GameObject[,] board = new GameObject[BOARD_GRID_SIZE, BOARD_GRID_SIZE];

		void Awake() {
			// calculate the cell size
			RectTransform rectTransform = transform as RectTransform;
			Vector2 boardSize = rectTransform.sizeDelta;
			GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();
			cellSize = new Vector2(boardSize.x / BOARD_GRID_SIZE, boardSize.y / BOARD_GRID_SIZE);
			gridLayout.cellSize = cellSize;

			// instantiate all the squares for the grid
			for (int y = 0; y < BOARD_GRID_SIZE; y++) {
				for (int x = 0; x < BOARD_GRID_SIZE; x++) {
					GameObject squarePrefab = (x + y) % 2 == 0 ? whiteSquarePrefab : blackSquarePrefab;
					IntVector2 boardPosition = new IntVector2(x, y);
					InstantiateSquare(squarePrefab, boardPosition);
				}
			}
		}

		/// <summary>
		/// Instantiate a given square prefab with the given coordinates. Set all the right
		/// properties and add it to the board.
		/// </summary>
		/// <param name="squarePrefab">Square prefab.</param>
		/// <param name="boardPosition">The position on the board to instantiate to.</param>
		private void InstantiateSquare(GameObject squarePrefab, IntVector2 boardPosition) {
			// instantiate the square and size it correctly
			GameObject square = Instantiate<GameObject>(squarePrefab);
			square.transform.SetParent(transform, false);
			RectTransform squareRectTransform = square.transform as RectTransform;
			squareRectTransform.sizeDelta = cellSize;

			// assign collider properties
			BoxCollider2D collider = square.GetComponent<BoxCollider2D>();
			if (collider == null) {
				Debug.LogErrorFormat("Generated square at {0} does not have a box collider 2d component.", boardPosition);
				return;
			}
			collider.size = cellSize;

			// assign tile properties
			Tile tile = square.GetComponent<Tile>();
			if (tile == null) {
				Debug.LogErrorFormat("Generated square at {0} does not have a tile component.", boardPosition);
				return;
			}
			tile.Coordinates = boardPosition;
			tile.SetParentBoard(this);

			// add to the board
			board[boardPosition.x, boardPosition.y] = square;
		}

		/// <summary>
		/// Callback for a tile getting clicked.
		/// </summary>
		/// <param name="boardPosition">The position on the board that was clicked.</param>
		public void BoardClicked(IntVector2 boardPosition) {
			BoardState state = GetBoardState(boardPosition);
			Debug.LogFormat("Board clicked at {0}, board state {1}", boardPosition, state.ToString());
		}

		/// <summary>
		/// Gets the equivalent MoveDirection for an IntVector2.
		/// </summary>
		/// <returns>The direction for int vector.</returns>
		/// <param name="vec">The IntVector2 we want the direction of.</param>
		private MoveDirection GetDirectionForIntVector(IntVector2 vec) {
			foreach (var entry in directionLookup) {
				IntVector2 curr = entry.Value;
				if (curr.x == vec.x && curr.y == vec.y) {
					return entry.Key;
				}
			}
			Debug.LogErrorFormat("Could not get direction for int vector {0}", vec);
			return MoveDirection.NORTH_EAST;
		}

		/// <summary>
		/// Determines whether this instance is valid board position the specified boardPosition.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid board position the specified boardPosition; otherwise, <c>false</c>.</returns>
		/// <param name="boardPosition">Board position.</param>
		private bool IsValidBoardPosition(IntVector2 boardPosition) {
			int x = boardPosition.x;
			int y = boardPosition.y;
			if (x > BOARD_GRID_SIZE || x < 0 || y > BOARD_GRID_SIZE || y < 0) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Adds a player token of a given colour to the given coordinates.
		/// </summary>
		/// <param name="isRed">If set to <c>true</c> is red.</param>
		/// <param name="boardPosition">The position on the board to add a player token to.</param>
		public void AddPlayerToken(bool isRed, IntVector2 boardPosition) {
			if (! IsValidBoardPosition(boardPosition)) {
				Debug.LogErrorFormat("Invalid coordinates to add a {0} token to the board: {1}", isRed ? "red" : "black", boardPosition);
				return;
			}
			
			// create the token
			GameObject tokenPrefab = isRed ? redPiecePrefab : blackPiecePrefab;
			GameObject token = Instantiate<GameObject>(tokenPrefab);

			// add to the scene
			GameObject tile = board[boardPosition.x, boardPosition.y];
			token.transform.SetParent(tile.transform, false);

			// update board state
			boardStates[boardPosition.x, boardPosition.y] = isRed ? BoardState.RED : BoardState.BLACK;
		}

		/// <summary>
		/// TODO: Implement for moving a player token. Note: The API can be changed,
		/// you can add functions, parameters, properties, whatever.
		/// </summary>
		public void MovePlayerToken() {
		}

		/// <summary>
		/// Get the state of the board at a given coordinate.
		/// </summary>
		/// <returns>The board state.</returns>
		/// <param name="boardPosition">The board position to get the state of.</param>
		public BoardState GetBoardState(IntVector2 boardPosition) {
			if (! IsValidBoardPosition(boardPosition)) {
				Debug.LogErrorFormat("Invalid coordinates to get board state: {0}", boardPosition);
				return BoardState.EMPTY;
			}

			return boardStates[boardPosition.x, boardPosition.y];
		}
	}
}

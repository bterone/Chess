using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public Board board;

    public PieceManager pieceManager;

    private void Start() {
        // Create the board
        board.Create();

        // Create pieces
        pieceManager.Setup(board);
    }
}
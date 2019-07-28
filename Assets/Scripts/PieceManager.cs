using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public GameObject piecePrefab;

    private List<BasePiece> whitePieces = null;
    private List<BasePiece> blackPieces = null;

    private string[] pieceOrder = new string[16]
    {
        "P","P","P","P","P","P","P","P",
        "R","KN","B","K","Q","B","KN","R"
    };

    private Dictionary<string, Type> pieceLibrary = new Dictionary<string, Type>()
    {
        {"P",  typeof(Pawn)},
        {"R",  typeof(Rook)},
        {"KN", typeof(Knight)},
        {"B",  typeof(Bishop)},
        {"K",  typeof(King)},
        {"Q",  typeof(Queen)}
    };

    public void Setup(Board board)
    {
        // Creates white and black pieces
        whitePieces = CreatePieces(Color.white, new Color32(80, 124, 159, 255), board);
        blackPieces = CreatePieces(Color.black, new Color32(210, 95, 64, 255), board);

        // Places pieces
        PlacePieces(1, 0, whitePieces, board);
        PlacePieces(6, 7, blackPieces, board);

        // White goes first
        // switchSides()
    }

    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        for (int i = 0; i < pieceOrder.Length; i++)
        {
            // Create new object
            GameObject newPieceObject = Instantiate(piecePrefab);
            newPieceObject.transform.SetParent(transform);

            // Set scale and position
            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

            // Get the type, apply to new object
            string key = pieceOrder[i];
            Type pieceType = pieceLibrary[key];

            // Store new piece
            BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);
            newPieces.Add(newPiece);

            // Setup piece
            newPiece.Setup(teamColor, spriteColor, this);
        }

        return newPieces;
    }

    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePiece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            // Place pawns
            pieces[i].Place(board.allCells[i, pawnRow]);

            // Place royalty
            pieces[i + 8].Place(board.allCells[i, royaltyRow]);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Reset
        isFirstMove = true;

        // Pawn stuff
        movement = color == Color.white ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Pawn");
    }

    protected override void Move()
    {
        base.Move();

        CheckForPromotion();
    }

    private void CheckForPromotion()
    {
        // Target position
        int currentX = currentCell.boardPosition.x;
        int currentY = currentCell.boardPosition.y;

        CellState cellState = currentCell.board.ValidateCell(currentX, currentY + movement.y, this);

        if(cellState == CellState.OutOfBounds)
        {
            Color spriteColor = GetComponent<Image>().color;
            pieceManager.PromotePiece(this, currentCell, color, spriteColor);
        }
    }

    protected override void CheckPathing()
    {
        // Target position
        int currentX = currentCell.boardPosition.x;
        int currentY = currentCell.boardPosition.y;

        // Top left
        MatchesState(currentX - movement.z, currentY + movement.z, CellState.Enemy);

        // Forward
        if (MatchesState(currentX, currentY + movement.y, CellState.Free))
        {
            // If the first forward cell is free, and first move, check for next
            if (isFirstMove)
            {
                MatchesState(currentX, currentY + (movement.y * 2), CellState.Free);
            }
        }

        // Top right
        MatchesState(currentX + movement.z, currentY + movement.z, CellState.Enemy);
    }
}

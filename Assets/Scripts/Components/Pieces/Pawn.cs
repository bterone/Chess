using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    private bool isFirstMove = true;

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

        isFirstMove = false;
    }

    private bool MatchesState(int targetX, int targetY, CellState targetState)
    {
        CellState cellState = CellState.None;
        cellState = currentCell.board.ValidateCell(targetX, targetY, this);

        if (cellState == targetState)
        {
            highlightedCells.Add(currentCell.board.allCells[targetX, targetY]);
            return true;
        }

        return false;
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

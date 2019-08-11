using UnityEngine;
using UnityEngine.UI;

public class Knight : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Knight stuff
        // TODO: Movement
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Knight");
    }

    private void CreateCellPath(int flipper)
    {
        // Target position
        int currentX = currentCell.boardPosition.x;
        int currentY = currentCell.boardPosition.y;

        // Left
        MatchesState(currentX - 2, currentY + (1 * flipper));

        // Upper left
        MatchesState(currentX - 1, currentY + (2 * flipper));

        // Upper right
        MatchesState(currentX + 1, currentY + (2 * flipper));

        // Right
        MatchesState(currentX + 2, currentY + (1 * flipper));
    }

    protected override void CheckPathing()
    {
        // Draw top half
        CreateCellPath(1);

        // Draw bottom half
        CreateCellPath(-1);
    }

    private void MatchesState(int targetX, int targetY)
    {
        CellState cellState = CellState.None;
        cellState = currentCell.board.ValidateCell(targetX, targetY, this);

        if (cellState != CellState.Friendly && cellState != CellState.OutOfBounds)
            highlightedCells.Add(currentCell.board.allCells[targetX, targetY]);
    }
}

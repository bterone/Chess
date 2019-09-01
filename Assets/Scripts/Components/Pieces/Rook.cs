using UnityEngine;
using UnityEngine.UI;

public class Rook : BasePiece
{
    [HideInInspector]
    public Cell castleTriggerCell = null;
    private Cell castleCell = null;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Rook stuff
        movement = new Vector3Int(7, 7, 0);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Rook");
    }

    public override void Place(Cell newCell)
    {
        base.Place(newCell);

        // Trigger cell
        // Sets triggerOffset on whether currentcell is over on left or right
        int triggerOffset = currentCell.boardPosition.x < 4 ? 2 : -1;
        castleTriggerCell = SetCell(triggerOffset);

        // Castle cell
        int castleOffset = currentCell.boardPosition.x < 4 ? 3 : -2;
        castleCell = SetCell(castleOffset);
    }

    public void Castle()
    {
        // Set new target
        targetCell = castleCell;

        // Move
        Move();
    }

    private Cell SetCell(int offset)
    {
        // New Position
        Vector2Int newPosition = currentCell.boardPosition;
        newPosition.x += offset;

        // Return
        return currentCell.board.allCells[newPosition.x, newPosition.y];
    }
}
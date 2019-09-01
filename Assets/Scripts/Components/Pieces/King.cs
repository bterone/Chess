using UnityEngine;
using UnityEngine.UI;

public class King : BasePiece
{
    private Rook leftRook = null;
    private Rook rightRook = null;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // King stuff
        movement = new Vector3Int(1, 1, 1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_King");
    }

    public override void Kill()
    {
        base.Kill();

        pieceManager.isKingAlive = false;
    }

    protected override void CheckPathing()
    {
        base.CheckPathing();

        // Right
        rightRook = GetRook(1, 3);

        // Left
        leftRook = GetRook(-1, 4);
    }

    protected override void Move()
    {
        base.Move();

        if(CanCastle(leftRook))
            leftRook.Castle();

        if(CanCastle(rightRook))
            rightRook.Castle();
    }

    private bool CanCastle(Rook rook)
    {
        if (rook == null)
            return false;

        if(rook.castleTriggerCell != currentCell)
            return false;

        return true;
    }

    private Rook GetRook(int direction, int count)
    {
        // Has the king moved?
        if (!isFirstMove)
            return null;

        // Get King Position
        int currentX = currentCell.boardPosition.x;
        int currentY = currentCell.boardPosition.y;

        // Go through the cells if pieces are there inbetween rook and king
        for(int i = 1; i < count; i++)
        {
            // Direction can be positive or negative for right or left
            int offsetX = currentX + (i * direction);
            CellState cellState = currentCell.board.ValidateCell(offsetX, currentY, this);

            if(cellState != CellState.Free)
                return null;
        }

        // Try and get rook
        Cell rookCell = currentCell.board.allCells[currentX + (count * direction), currentY];
        Rook rook = null;

        // Cast to see if Rook and Friendly
        if(rookCell.currentPiece != null)
        {
            if(rookCell.currentPiece is Rook)
                rook = (Rook)rookCell.currentPiece;
        }

        // Return if no rook
        if(rook == null)
            return null;

        // Check color and movement
        if(rook.color != color || !rook.isFirstMove)
            return null;

        // Add Castle trigger to movement
        highlightedCells.Add(rook.castleTriggerCell);

        return rook;
    }
}
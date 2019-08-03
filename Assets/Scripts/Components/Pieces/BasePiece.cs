using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public Color color = Color.clear;

    protected Cell originalCell = null;
    protected Cell currentCell = null;

    protected RectTransform rectTransform = null;
    protected PieceManager pieceManager;

    protected Cell targetCell = null;

    protected Vector3Int movement = Vector3Int.one;
    protected List<Cell> highlightedCells = new List<Cell>();

    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        pieceManager = newPieceManager;

        color = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void Place(Cell newCell)
    {
        // Cell stuff
        currentCell = newCell;
        originalCell = newCell;
        currentCell.currentPiece = this;

        // Object stuff
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        Kill();

        Place(originalCell);
    }

    public virtual void Kill()
    {
        // Clear current cell
        currentCell.currentPiece = null;

        // Remove piece
        gameObject.SetActive(false);
    }

    #region Movement
    private void CreateCellPath(int targetX, int targetY, int movement)
    {
        // Target position
        int currentX = currentCell.boardPosition.x;
        int currentY = currentCell.boardPosition.y;

        // Check each cell
        for (int i = 1; i <= movement; i++)
        {
            currentX += targetX;
            currentY += targetY;

            // Get the state of the target cell
            CellState cellState = CellState.None;
            cellState = currentCell.board.ValidateCell(currentX, currentY, this);

            // If enemy, add to list, break
            if (cellState == CellState.Enemy)
            {
                highlightedCells.Add(currentCell.board.allCells[currentX, currentY]);
                break;
            }

            // If the cell is not free, break
            if (cellState != CellState.Free)
                break;

            // Add to list
            highlightedCells.Add(currentCell.board.allCells[currentX, currentY]);
        }
    }

    protected virtual void CheckPathing()
    {
        // Horizontal
        CreateCellPath(1, 0, movement.x);
        CreateCellPath(-1, 0, movement.x);

        // Vertical
        CreateCellPath(0, 1, movement.y);
        CreateCellPath(0, -1, movement.y);

        // Upper diagonal
        CreateCellPath(1, 1, movement.z);
        CreateCellPath(-1, 1, movement.z);

        // Lower diagonal
        CreateCellPath(-1, -1, movement.z);
        CreateCellPath(1, -1, movement.z);
    }

    protected void ShowCells()
    {
        foreach (Cell cell in highlightedCells)
            cell.outlineImage.enabled = true;
    }

    protected void ClearCells()
    {
        foreach (Cell cell in highlightedCells)
            cell.outlineImage.enabled = false;

        highlightedCells.Clear();
    }

    protected virtual void Move()
    {
        // If there is an enemy piece, remove it
        targetCell.RemovePiece();

        // Clear current
        currentCell.currentPiece = null;

        // Switch cells
        currentCell = targetCell;
        currentCell.currentPiece = this;

        // Move on board
        transform.position = currentCell.transform.position;
        targetCell = null;
    }
    #endregion

    #region Events
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        // Test for cells
        CheckPathing();

        // Show valid cells
        ShowCells();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        // Follow pointer
        transform.position += (Vector3)eventData.delta;

        // Check for overlapping available squares
        foreach (Cell cell in highlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.rectTransform, Input.mousePosition))
            {
                // If the mouse is within a valid cell, get it, and break.
                targetCell = cell;
                break;
            }

            // If the mouse is not within any highlighted cell, we don't have a valid move.
            targetCell = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        // Hide
        ClearCells();

        // Return to original position if no target cell
        if (!targetCell)
        {
            transform.position = currentCell.gameObject.transform.position;
            return;
        }

        // Move to new cell
        Move();

        // End turn
        pieceManager.SwitchSides(color);
    }
    #endregion
}

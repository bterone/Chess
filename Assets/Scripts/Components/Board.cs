using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour {
    public GameObject cellPrefab;

    [HideInInspector]
    public Cell[,] allCells = new Cell[8, 8];

    public void Create() {
        for (int y = 0; y < 8; y++) {
            for (int x = 0; x < 8; x++) {
                // Create the cell
                GameObject newCell = Instantiate(cellPrefab, transform);

                // Position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                // Setup
                allCells[x, y] = newCell.GetComponent<Cell>();
                allCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }


        for (int x = 0; x < 8; x += 2) {
            for (int y = 0; y < 8; y++) {
                // Offset for every other line
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                //Color
                allCells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
            }
        }
    }

    public CellState ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {
        // Bounds check
        if (targetX < 0 || targetX > 7)
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > 7)
            return CellState.OutOfBounds;

        // Get cell
        Cell targetCell = allCells[targetX, targetY];

        // If the cell has a piece
        if (targetCell.currentPiece != null)
        {
            // If friendly
            if (checkingPiece.color == targetCell.currentPiece.color)
                return CellState.Friendly;

            // If enemy
            if (checkingPiece.color != targetCell.currentPiece.color)
                return CellState.Enemy;
        }

        return CellState.Free;
    }
}
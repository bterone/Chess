using UnityEngine;
using UnityEngine.UI;

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
                rectTransform.anchoredPosition = new Vector2((x * 100), (y * 100));

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
}
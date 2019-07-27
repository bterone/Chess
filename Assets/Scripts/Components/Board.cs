using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    public GameObject CellPrefab;

    [HideInInspector]
    public Cell[,] AllCells = new Cell[8, 8];

    public void Create() {
        for (int y = 0; y < 8; y++) {
            for (int x = 0; x < 8; x++) {
                // Create the cell
                GameObject newCell = Instantiate(CellPrefab, transform);

                // Position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100), (y * 100));

                // Setup
                AllCells[x, y] = newCell.GetComponent<Cell>();
                AllCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }


        for (int x = 0; x < 8; x += 2) {
            for (int y = 0; y < 8; y++) {
                // Offset for every other line
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                //Color
                AllCells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
            }
        }
    }
}
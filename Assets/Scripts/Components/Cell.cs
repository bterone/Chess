using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image OutlineImage;

    [HideInInspector]
    public Vector2Int BoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board Board = null;
    [HideInInspector]
    public RectTransform RectTransform = null;

    public void Setup(Vector2Int newBoardPosition, Board newBoard) {
        BoardPosition = newBoardPosition;
        Board = newBoard;

        RectTransform = GetComponent<RectTransform>();
    }
}

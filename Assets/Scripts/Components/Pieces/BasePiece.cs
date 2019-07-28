using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public Color color = Color.clear;

    protected Cell originalCell = null;
    protected Cell currentCell = null;

    protected RectTransform rectTransform = null;
    protected PieceManager pieceManager;

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
        //currentCell.currentPiece = this;

        // Object stuff
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }
}

using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public Board board;

    private void Start() {
        // Create the board
        board.Create();
    }
}
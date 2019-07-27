using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public Board Board;

    private void Start() {
        // Create the board
        Board.Create();
    }
}
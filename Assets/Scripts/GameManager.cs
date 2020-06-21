using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using UnityEngine.Scripting;



public class GameManager : MonoBehaviour
{
    AlphaBeta ab = new AlphaBeta();
    private bool _kingDead = false;
    float timer = 0;
    Board _board;
    public Move move;
	void Start ()
    {
        _board = Board.Instance;
        _board.SetupBoard();
        Application.targetFrameRate = 300;
        GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;


    }

    void Update ()
    {
        if (_kingDead)
        {
            Debug.Log("WINNER!");
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        if (!playerTurn && timer < 3)
        {
            timer += Time.deltaTime;
        }
        if (!playerTurn && timer >= 3)
        {
            move = ab.GetMove();
           _DoAIMove(move);
            timer = 0;
        }
        Debug.Log(GarbageCollector.GCMode);

    }

    public bool playerTurn = true;

    void _DoAIMove(Move move)
    {
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        if (secondPosition.CurrentPiece && secondPosition.CurrentPiece.Type == Piece.pieceType.KING)
        {
            SwapPieces(move);
            _kingDead = true;
        }
        else
        {
            SwapPieces(move);
        }
    } 

    public void SwapPieces(Move move)
    {
      GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        } 

        Tile firstTile = move.firstPosition;
        Tile secondTile = move.secondPosition;

        firstTile.CurrentPiece.MovePiece(new int2(move.secondPosition.Position.x, move.secondPosition.Position.y));

        if (secondTile.CurrentPiece != null)
        {
            if (secondTile.CurrentPiece.Type == Piece.pieceType.KING)
                _kingDead = true;
            Destroy(secondTile.CurrentPiece.gameObject);
        }
            

        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.position = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

      playerTurn = !playerTurn;

         
    }
}

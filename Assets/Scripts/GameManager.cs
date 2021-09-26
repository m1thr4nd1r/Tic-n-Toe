using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public int Turn { get; private set; } = 1;
    public GameObject board;
    public Text player;
    public Text ai;
    public Text resultText;
    public static GameManager Instance;

    private bool isPlayerTurn;
    private char playerToken; 
    private char aiToken;
    private List<Button> openSquares;
    private List<Text> squaresText;

    internal void ProcessClick(Button button)
    {
        if (!isPlayerTurn)
            return;

        var result = ProcessTurn(button, playerToken.ToString());
        
        if (ProcessResult(result, "Player"))
            Invoke(nameof(AITurn), Random.Range(.2f, 1));
    }

    private void Awake() => StartUp();

    private void StartUp()
    {
        Instance = this;
        var first = Random.Range(1, 2);
        isPlayerTurn = first == 1;
        playerToken = isPlayerTurn ? 'X' : 'O';
        aiToken = (!isPlayerTurn) ? 'X' : 'O';

        player.text = playerToken.ToString();
        ai.text = aiToken.ToString();
        resultText.gameObject.SetActive(false);

        openSquares = new List<Button>();
        squaresText = new List<Text>();

        for (int i = 1; i < board.transform.childCount - 3; i++)
        {
            var square = board.transform.GetChild(i);
            openSquares.Add(square.GetComponent<Button>());
            squaresText.Add(square.GetComponentInChildren<Text>());
        }
        
        if (!isPlayerTurn)
            Invoke(nameof(AITurn), Random.Range(.2f, 1));
    }   

    private void AITurn()
    {
        if (openSquares.Count == 0)
            return;

        var targetSquare = Random.Range(0, openSquares.Count - 1);
        var square = openSquares[targetSquare];

        var result = ProcessTurn(square, aiToken.ToString());

        ProcessResult(result, "AI");
    }

    private bool ProcessResult(bool result, string player)
    {
        if (result || openSquares.Count == 0)
        {
            resultText.text = (result) ? $"{player} has won !!!\n... and in {Turn} turn(s)"
                                       : "Sorry, it is a tie :(";
            resultText.gameObject.SetActive(true);
            isPlayerTurn = false;
            Invoke(nameof(BackToTitleScreen), 5f);
            return !result;
        }

        return true;
    }

    private void BackToTitleScreen() => SceneController.BackToTitleScreen();

    private bool ProcessTurn(Button square, string token)
    {
        square.GetComponentInChildren<Text>().text = token;
        square.interactable = false;

        var result = CheckBoard();
        openSquares.Remove(square);

        if (result)
            return true;
        
        Turn++;
        isPlayerTurn = !isPlayerTurn;
        return false;
    }

    private bool CheckBoard()
    {
        var topRow = (squaresText[0].text.Equals(squaresText[1].text) &&
                     squaresText[1].text.Equals(squaresText[2].text)) ? squaresText[0].text : null;
        var middleRow = (squaresText[3].text.Equals(squaresText[4].text) &&
                        squaresText[4].text.Equals(squaresText[5].text)) ? squaresText[3].text : null;
        var bottomRow = (squaresText[6].text.Equals(squaresText[7].text) &&
                        squaresText[7].text.Equals(squaresText[8].text)) ? squaresText[6].text : null;

        var firstColumn = (squaresText[0].text.Equals(squaresText[3].text) &&
                           squaresText[3].text.Equals(squaresText[6].text)) ? squaresText[0].text : null;
        var middleColumn = (squaresText[1].text.Equals(squaresText[4].text) &&
                            squaresText[4].text.Equals(squaresText[7].text)) ? squaresText[1].text : null;
        var lastColumn = (squaresText[2].text.Equals(squaresText[5].text) &&
                          squaresText[5].text.Equals(squaresText[8].text)) ? squaresText[2].text : null;

        var mainDiagonal = (squaresText[0].text.Equals(squaresText[4].text) &&
                            squaresText[4].text.Equals(squaresText[8].text)) ? squaresText[0].text : null;
        var antiDiagonal = (squaresText[2].text.Equals(squaresText[4].text) &&
                            squaresText[4].text.Equals(squaresText[6].text)) ? squaresText[2].text : null;

        var result = topRow + middleRow + bottomRow + 
                     firstColumn + middleColumn + lastColumn +
                     mainDiagonal + antiDiagonal;

        return !string.IsNullOrEmpty(result);
    }
}

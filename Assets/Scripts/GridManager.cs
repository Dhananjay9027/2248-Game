using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using Unity.Mathematics;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab; // Drag your GridCell prefab here in the Inspector
    public int rows = 8; // Number of rows in your grid
    public int columns = 5; // Number of columns in your grid
    public float cellSpacing = 10f; // Spacing between cells
    public Text IntermediateResultText; // Text component to display the current score
    public Text HighScore;
    public AudioSource audioSource;
    public AudioClip cellClickSound;
    public AudioClip CombineSound;
    public AudioClip HighScore_Sound;


    private GridCell[,] grid;
    private List<GridCell> selectedCells = new List<GridCell>();
    private bool sequenceFinalized = false;
    private int currentScore = 0;
    private int score = 0;

    private void Start()
    {
        GenerateGrid(); // Generate the grid
        AssignRandomNumbers(); // Assign random numbers to cells
        score = 0;
        HighScore.text = $"High Score:{score}";
   
    }

    void GenerateGrid()
    {
        grid = new GridCell[columns, rows];
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newCell = Instantiate(gridCellPrefab, transform);
                newCell.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * (100 + cellSpacing), -y * (100 + cellSpacing));
                newCell.name = $"Cell {x},{y}";

                GridCell cell = newCell.GetComponent<GridCell>();
                cell.SetPosition(x, y);
                grid[x, y] = cell;
                cell.GetComponent<Button>().onClick.AddListener(() => OnCellClicked(cell));
            }
        }
    }

    void AssignRandomNumbers()
    {
        foreach (GridCell cell in grid)
        {
            cell.SetNumber(GetRandomEvenNumber());
        }
    }

    public void OnCellClicked(GridCell cell)
    {
        if (audioSource != null && cellClickSound != null)
        {
            audioSource.PlayOneShot(cellClickSound);
        }
        if (sequenceFinalized)
        {
            // If sequence is finalized, reset and start a new selection
            ClearSelection();
            sequenceFinalized = false;
        }

        if (selectedCells.Count > 1)
        {
            if (selectedCells[0].number != selectedCells[1].number)
            {
                ClearSelection();
                return;
            }
            int previousNumber = selectedCells[selectedCells.Count - 1].number;

            if (cell.number != previousNumber && cell.number != previousNumber * 2)
            {
                ClearSelection();
                return;
            } 
        }

        if (!selectedCells.Contains(cell))
        {
            if (selectedCells.Count == 0 || AreNeighbors(cell, selectedCells[selectedCells.Count - 1]))
            {
                selectedCells.Add(cell);
                cell.GetComponent<Image>().color = Color.white; // Highlight the selected cell
                cell.AnimateText(0.3f);
                UpdateCurrentScore();   
            }
        }
        else if (cell == selectedCells[selectedCells.Count - 1])
        {
            // Finalize the sequence if the last cell is clicked again
            sequenceFinalized = true;
            CombineSelectedCells();
            StartCoroutine(CombineAndShiftCells());
        }
    }

    void ClearSelection()
    {
        foreach (var cell in selectedCells)
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#D6D5C9", out newColor)) // Hex code 
            {
                cell.GetComponent<Image>().color = newColor;
            }
        }
        selectedCells.Clear();
        currentScore = 0;
        IntermediateResultText.text = $"Current Score:{currentScore}";
    }
    int finddAnswer()
    {
        int ans = 0;
        for(int i=0;i<selectedCells.Count;i++)
        {
            ans+= selectedCells[i].number;
        }
        double s = System.Math.Log(ans) / System.Math.Log(2);
      //Debug.Log(s);
        int m = (int)math.round(s);
        ans=(int)math.pow(2,m);
      
        return ans;
    }
    void CombineSelectedCells()
    {
        if (selectedCells.Count == 0 || selectedCells.Count==1) return; // Add this line to ensure selectedCells is not empty

        int combinedValue = finddAnswer();

        var targetCell = selectedCells[selectedCells.Count - 1];

        foreach (var cell in selectedCells)
        {
            cell.SetNumber(0); // Clear the value of selected cells
        }

        targetCell.SetNumber(combinedValue);
        if(targetCell.number > score)
        {
            score=targetCell.number;

            if (audioSource != null && cellClickSound != null)
            {
                audioSource.PlayOneShot(HighScore_Sound);
            }
            HighScore.text = $"High Score:{score}";
        }
        else
        {
            if (audioSource != null && cellClickSound != null)
            {
                audioSource.PlayOneShot(CombineSound);
            }
        }
        targetCell.GetComponent<Image>().color = Color.white;
       // targetCell.AnimateText(0.1f);

        ClearSelection();
    }

    void ShiftNumbersDown()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = rows - 1; y > 0; y--)
            {
                if (grid[x, y].number == 0)
                {
                    for (int k = y - 1; k >= 0; k--)
                    {
                        if (grid[x, k].number != 0)
                        {
                            grid[x, y].SetNumber(grid[x, k].number);
                            grid[x, k].SetNumber(0);
                            break;
                        }
                    }
                }
            }
        }
    }

    void SpawnNewNumbers()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = rows - 1; y >= 0; y--)
            {
                if (grid[x, y].number == 0)
                {
                    grid[x, y].SetNumber(GetRandomEvenNumber());
                }
            }
        }
    }

    int GetRandomEvenNumber()
    {
        int[] evenNumbers = { 2, 4, 8, 16, 32, 64, 128};
        return evenNumbers[Random.Range(0, evenNumbers.Length)];
    }

    bool AreNeighbors(GridCell cell1, GridCell cell2)
    {
        int deltaX = Mathf.Abs(cell1.gridX - cell2.gridX);
        int deltaY = Mathf.Abs(cell1.gridY - cell2.gridY);
        return (deltaX <= 1 && deltaY <= 1) && (deltaX + deltaY > 0);
    }

    void UpdateCurrentScore()
    {
       int current =finddAnswer();
       IntermediateResultText.text = $"Current Score:{current}";
      // Debug.Log(current);
    }

    IEnumerator CombineAndShiftCells()
    {
        CombineSelectedCells();
        yield return new WaitForSeconds(0.5f); // Delay for 0.5 seconds
        ShiftNumbersDown();
        SpawnNewNumbers();
    }
}

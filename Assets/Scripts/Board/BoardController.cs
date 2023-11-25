using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] Material intermidiateMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] internal GameObject pin;
    [SerializeField] internal Material[] pinMaterials;
    public static GameObject[,] tiles = new GameObject[7, 7];

    public static BoardController Instance { get; private set; } 

    private void Start()
    {
        Instance = this;
        var children = transform.GetComponentsInChildren<Tile>();
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                tiles[i, j] = children[i * 7 + j].gameObject;
                tiles[i, j].GetComponent<Tile>().intermidiateMaterial = intermidiateMaterial;
                tiles[i, j].GetComponent<Tile>().coordinates = (i, j);
            }
        }
    }

    public IEnumerator FlipBoardFromCenter(bool correctlyMoved)
    {
        var tileCoordinates = new List<(int, int)>();
        for(int i = 0; i < 7; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                tileCoordinates.Add((i, j));
            }
        }
        tileCoordinates.Sort((x, y) => Vector3.Distance(tiles[x.Item1,x.Item2].transform.position, tiles[3,3].transform.position) < Vector3.Distance(tiles[y.Item1,y.Item2].transform.position, tiles[3,3].transform.position) ? -1 : 1);
        yield return new WaitForSeconds(1f);
        if(!correctlyMoved)
        {
            GetComponent<AudioSource>().Play();
        }
        int counter = 0;
        StartCoroutine(LerpIntermidiateColor(correctlyMoved ? greenMaterial : redMaterial));
        foreach(var tileCoordinate in tileCoordinates)
        {
            counter++;
            var tile = tiles[tileCoordinate.Item1, tileCoordinate.Item2];
            tile.GetComponent<Tile>().ForceFlip(0, 1.3f - counter * 0.016f, 2 - counter*0.03f);
            yield return new WaitForSeconds(0.02f);
        }
        if (correctlyMoved)
        {
            yield break;
        }
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(ResetFlipBoardDiagonally());
    }

    private IEnumerator LerpIntermidiateColor(Material material)
    {
        var duration = 1f;
        intermidiateMaterial.color = new Color(0.8627f, 0.6156f, 0.203f);
        var baseColor = intermidiateMaterial.color;
        var targetColor = material.color;
        float timePassed = 0f;
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime * 1 / duration;
            intermidiateMaterial.color = Color.Lerp(baseColor, targetColor, timePassed);
            yield return null;
        }
    }

    private IEnumerator ResetFlipBoardDiagonally()
    {
        int boardSize = 7;

        for (int d = 0; d <= 2 * (boardSize - 1); d++)
        {
            int startX = Math.Max(0, d - boardSize + 1);
            int startY = Math.Min(d, boardSize - 1);

            for (int i = startX; i <= startY; i++)
            {
                int j = d - i;
                var tile = tiles[i, j];
                tile.GetComponent<Tile>().ResetTile(0, 1.3f, 2 - d * 0.02f);
            }
            yield return new WaitForSeconds(0.12f);
        }
        PlayerTilesMover.isMoving = false;
        PlayerTilesMover.Instance.ResetPlayer();
    }

    private readonly static List<(int, int)> correctTiles = new()
    {
                                                (5, 6),
                        (2, 5), (3, 5), (4, 5), (5, 5),
                (1, 4),         (3, 4),
                                        (4, 3), (5, 3),
        (0, 2),         (2, 2), (3, 2), (4, 2),
        (0, 1), (1, 1), (2, 1),         (4, 1), (5, 1),
                                                (5, 0)
    };

    private readonly static List<(int, int)> incorrectTiles = new()
    {
        (0, 6), (1, 6), (2, 6), (3, 6), (4, 6),         (6, 6),
        (0, 5), (1, 5),                                 (6, 5),
        (0, 4),                         (4, 4), (5, 4), (6, 4),
        (0, 3), (1, 3), (2, 3),                         (6, 3),
                (1, 2),                         (5, 2), (6, 2),
                                (3, 1),                 (6, 1),
        (0, 0), (1, 0), (2, 0), (3, 0),                 (6, 0)
    };

    public static bool CorrectlyMoved((int,int) lastTile)
    {
        if(!lastTile.Equals((4,0)))
        {
            return false;
        }
        var visitedTiles = new List<(int, int)>();
        for(int i = 0; i < 7; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                if (tiles[i, j].GetComponent<Tile>().flipped)
                {
                    visitedTiles.Add((i, j));
                }
            }
        }
        return  visitedTiles.TrueForAll(tile => !incorrectTiles.Contains(tile)) &&
                correctTiles.TrueForAll(tile => visitedTiles.Contains(tile));
    }
}

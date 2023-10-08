using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector3 startPosition;
    private float deltaX = 1.19f;
    private float deltaZ = 1.19f;
    public static GameObject[,] tiles = new GameObject[7, 7];

    void Start()
    {
        for(int i = 0; i < 7; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(startPosition.x + deltaX * i, startPosition.y, startPosition.z + deltaZ * j), Quaternion.identity);
                tile.transform.parent = gameObject.transform;
                tiles[i, j] = tile;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FlipBoardDiagonally());
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(FlipBoardFromCenter());
        }
    }

    private IEnumerator FlipBoardFromCenter()
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

        int counter = 0;
        foreach(var tileCoordinate in tileCoordinates)
        {
            counter++;
            var tile = tiles[tileCoordinate.Item1, tileCoordinate.Item2];
            tile.GetComponent<Tile>().Flip(0, 1.3f - counter * 0.016f, 2 - counter*0.03f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator FlipBoardDiagonally()
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
                tile.GetComponent<Tile>().Flip(0, 1.3f, 2 - d * 0.02f);
            }
            yield return new WaitForSeconds(0.12f);
        }
    }
}

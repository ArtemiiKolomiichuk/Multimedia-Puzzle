using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardControllerIntro : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] Material intermidiateMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] internal GameObject pin;
    [SerializeField] internal Material[] pinMaterials;
    public static GameObject[,] tiles = new GameObject[3, 3];

    public static BoardControllerIntro Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        var children = transform.GetComponentsInChildren<Tile>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tiles[i, j] = children[i * 3 + j].gameObject;
                tiles[i, j].GetComponent<Tile>().intermidiateMaterial = intermidiateMaterial;
                tiles[i, j].GetComponent<Tile>().coordinates = (i, j);
            }
        }
    }

    internal IEnumerator FlipBoardFromCenter(bool correctlyMoved)
    {
        var tileCoordinates = new List<(int, int)>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tileCoordinates.Add((i, j));
            }
        }
        tileCoordinates.Sort((x, y) => Vector3.Distance(tiles[x.Item1, x.Item2].transform.position, tiles[1, 1].transform.position) < Vector3.Distance(tiles[y.Item1, y.Item2].transform.position, tiles[1, 1].transform.position) ? -1 : 1);
        yield return new WaitForSeconds(1f);
        int counter = 0;
        if (!correctlyMoved)
        {
            GetComponent<AudioSource>().Play();
        }
        StartCoroutine(LerpIntermidiateColor(correctlyMoved ? greenMaterial : redMaterial));
        foreach (var tileCoordinate in tileCoordinates)
        {
            counter++;
            var tile = tiles[tileCoordinate.Item1, tileCoordinate.Item2];
            tile.GetComponent<Tile>().ForceFlip(0, 1.3f - counter * 0.016f, 2 - counter * 0.03f);
            yield return new WaitForSeconds(0.02f);
        }
        if (correctlyMoved)
        {
            yield return new WaitForSeconds(0.4f);
            var sprite = Camera.main.transform.GetComponentInChildren<SpriteRenderer>();
            float timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime;
                sprite.color = new Color(0, 0, 0, timer);
                yield return null;
            }
            sprite.color = new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        int boardSize = 3;

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
        PlayerTilesMoverIntro.isMoving = false;
        yield return new WaitForSeconds(1.2f);
        PlayerTilesMoverIntro.Instance.ResetPlayer();
    }

    private readonly static List<(int, int)> correctTiles = new()
    {
                (1, 1), 
                (1, 0)
    };

    private readonly static List<(int, int)> incorrectTiles = new()
    {
        (0, 2), (1, 2), (2, 2),
        (0, 1),         (2, 1),
        (0, 0)
    };

    public static bool CorrectlyMoved((int, int) lastTile, Direction direction)
    {
        if (!lastTile.Equals((2, 0)) || direction != Direction.Right)
        {
            return false;
        }
        var visitedTiles = new List<(int, int)>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (tiles[i, j].GetComponent<Tile>().flipped)
                {
                    visitedTiles.Add((i, j));
                }
            }
        }
        return visitedTiles.TrueForAll(tile => !incorrectTiles.Contains(tile)) &&
                correctTiles.TrueForAll(tile => visitedTiles.Contains(tile));
    }
}


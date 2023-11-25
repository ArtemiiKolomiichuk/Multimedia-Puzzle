using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTilesMoverIntro : MonoBehaviour
{
    public Vector2Int coordinates = new(1, 1);
    [SerializeField] private GameObject player;
    private Vector3 playerBasePosition;
    [SerializeField] private float moveDuration = 0.4f;
    public static bool isMoving = false;

    public static PlayerTilesMoverIntro Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        playerBasePosition = player.transform.localPosition;
    }

    public void ResetPlayer()
    {
        player.SetActive(true);
        player.transform.localPosition = playerBasePosition;
        coordinates = new Vector2Int(1, 1);
    }

    public void Move(int d)
    {
        if (isMoving)
            return;
        isMoving = true;
        Direction direction = (Direction)d;
        var oldX = coordinates.x;
        var oldY = coordinates.y;
        void Flip()
        {
            BoardControllerIntro.tiles[oldX, oldY].GetComponent<Tile>().Flip(direction, -1, -1, () => { isMoving = false; });
        };
        switch (direction)
        {
            case Direction.Up:
                if (coordinates.y < 2)
                {
                    coordinates.y++;
                }
                break;
            case Direction.Down:
                if (coordinates.y > 0)
                {
                    coordinates.y--;
                }
                break;
            case Direction.Left:
                if (coordinates.x > 0)
                {
                    coordinates.x--;
                }
                break;
            case Direction.Right:
                if (coordinates.x < 2)
                {
                    coordinates.x++;
                }
                break;
        }

        if (coordinates.x == oldX && coordinates.y == oldY)
        {
            var (x, y) = (0, 0);
            switch (direction)
            {
                case Direction.Up:
                    y = 1;
                    break;
                case Direction.Down:
                    y = -1;
                    break;
                case Direction.Left:
                    x = -1;
                    break;
                case Direction.Right:
                    x = 1;
                    break;
            }
            player.SetActive(false);
            Flip();
            StartCoroutine(BoardControllerIntro.Instance.FlipBoardFromCenter(BoardControllerIntro.CorrectlyMoved((coordinates.x, coordinates.y), direction)));
            return;
        }

        StartCoroutine(MovePlayer(
            positions[coordinates.y, coordinates.x],
            Flip
        ));
    }

    private readonly static Vector3[,] positions = new Vector3[3, 3]
    {
        { new Vector3(-89.3499985f,-1.39600003f,-11.0830002f), new Vector3(-89.0979996f,-1.38699996f,-9.91699982f), new Vector3(-88.8499985f,-1.37800002f,-8.7670002f) },
        { new Vector3(-90.4980011f,-1.38999999f,-10.8100004f), new Vector3(-90.2509995f,-1.38100004f,-9.66699982f), new Vector3(-90.0009995f,-1.37199998f,-8.50699997f) },
        { new Vector3(-91.6460037f,-1.38399994f,-10.5699997f), new Vector3(-91.3980026f,-1.375f,-9.42300034f), new Vector3(-91.1449966f,-1.36600006f,-8.2510004f) }
    };

    IEnumerator MovePlayer(Vector3 targetPosition, Action onFinished)
    {
        var duration = moveDuration;
        var timePassed = 0f;
        var startPosition = player.transform.localPosition;
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime * 1 / duration;
            player.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timePassed);
            yield return null;
        }
        onFinished?.Invoke();
    }

}

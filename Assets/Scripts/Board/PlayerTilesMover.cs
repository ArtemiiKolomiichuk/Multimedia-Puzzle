using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTilesMover : MonoBehaviour
{
    private Vector2Int coordinates = new(3, 3);
    [SerializeField] private GameObject player;
    private Vector3 playerBasePosition;
    [SerializeField] private float moveDuration = 0.4f;
    internal static bool isMoving = false;

    public static PlayerTilesMover Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        playerBasePosition = player.transform.localPosition;
    }

    public void ResetPlayer()
    {
        player.SetActive(true);
        player.transform.localPosition = playerBasePosition;
        coordinates = new Vector2Int(3, 3);
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
            BoardController.tiles[oldX, oldY].GetComponent<Tile>().Flip(direction, -1, -1, () => { isMoving = false; }); 
        };

        switch (direction)
        {
            case Direction.Up:
                if (coordinates.y < 6)
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
                if (coordinates.x < 6)
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
            StartCoroutine(MovePlayer(
                new Vector3(
                    playerBasePosition.x + (coordinates.x + x*3 - 3) * 1.2f,
                    playerBasePosition.y,
                    playerBasePosition.z + (coordinates.y + y*3 - 3) * 1.2f
                ),
                () =>
                {
                    player.SetActive(false);
                }
            ));
            StartCoroutine(BoardController.Instance.FlipBoardFromCenter(BoardController.CorrectlyMoved((coordinates.x, coordinates.y))));
            return;
        }
        
        StartCoroutine(MovePlayer(
            new Vector3(
                playerBasePosition.x + (coordinates.x - 3) * 1.2f,
                playerBasePosition.y,
                playerBasePosition.z + (coordinates.y - 3) * 1.2f
            ),
            Flip
        ));
    }

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

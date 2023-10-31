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

    private void Start()
    {
        playerBasePosition = player.transform.localPosition;
    }

    public void Move(int d)
    {  
        Direction direction = (Direction)d;
        var oldX = coordinates.x;
        var oldY = coordinates.y;
        void Flip()
        {
            BoardController.tiles[oldX, oldY].GetComponent<Tile>().Flip(direction);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTilesMover : MonoBehaviour
{
    private Vector2Int coordinates = new(3, 3);
    public void Move(int d)
    {
        Direction direction = (Direction)d;
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
        BoardController.tiles[coordinates.x, coordinates.y].GetComponent<Tile>().Flip(direction);
    }

}
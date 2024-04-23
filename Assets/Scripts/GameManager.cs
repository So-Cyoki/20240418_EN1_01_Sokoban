using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject _playerPrefab;
    public GameObject _boxPrefab;
    int[,] _map;
    GameObject[,] _field;

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                if (_field[y, x] == null)
                {
                    continue;
                }
                if (_field[y, x].CompareTag("Player"))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //map範囲以外はFalseを出る
        if (moveTo.y < 0 || moveTo.y >= _field.GetLength(0)
            || moveTo.x < 0 || moveTo.x >= _field.GetLength(1))
        {
            return false;
        }

        //移動先に箱がある
        if (_field[moveTo.y, moveTo.x] != null && _field[moveTo.y, moveTo.x].CompareTag("Box"))
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            if (!success)
            {
                return false;
            }
        }

        //自分の移動
        _field[moveFrom.y, moveFrom.x].transform.position = new(moveTo.x, _field.GetLength(0) - moveTo.y);
        _field[moveTo.y, moveTo.x] = _field[moveFrom.y, moveFrom.x];
        _field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _map = new int[,] {
        {0,0,0,2,0},
        {0,0,0,0,0},
        {0,0,0,2,0},
        {0,2,0,0,1},
        };

        _field = new GameObject[_map.GetLength(0), _map.GetLength(1)];

        //マップデータによってObjを作成する
        for (int y = 0; y < _map.GetLength(0); y++)
        {
            for (int x = 0; x < _map.GetLength(1); x++)
            {
                switch (_map[y, x])
                {
                    case 1:
                        _field[y, x] = Instantiate(_playerPrefab, new Vector3(x, _map.GetLength(0) - y, 0), Quaternion.identity);
                        break;
                    case 2:
                        _field[y, x] = Instantiate(_boxPrefab, new Vector3(x, _map.GetLength(0) - y, 0), Quaternion.identity);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x + 1, playerIndex.y));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));
        }
    }
}
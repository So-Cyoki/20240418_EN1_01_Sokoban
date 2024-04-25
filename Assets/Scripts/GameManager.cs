using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int[,] _map;
    GameObject[,] _field;
    [Header("必要なオブジェクト")]
    public GameObject _playerPrefab;
    public GameObject _boxPrefab;
    public GameObject _targetPrefab;
    public GameObject _clearText;

    void Start()
    {
        Screen.SetResolution(1280, 720, false);

        //1:Player,2:Box,3:Target
        _map = new int[,] {
        {3,0,0,2,0,0,0},
        {0,0,0,0,0,0,0},
        {3,0,0,2,0,0,0},
        {3,2,0,0,0,0,0},
        {0,0,0,0,1,0,0},
        };

        _field = new GameObject[_map.GetLength(0), _map.GetLength(1)];
        _clearText.SetActive(false);

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
                    case 3:
                        Instantiate(_targetPrefab, new Vector3(x, _map.GetLength(0) - y, 0), Quaternion.identity);
                        break;
                }
            }
        }
    }
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

        if (IsCleard())
        {
            _clearText.SetActive(true);
        }
        else
        {
            _clearText.SetActive(false);

        }
    }
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

    bool IsCleard()
    {
        List<Vector2Int> goals = new();
        //目標のIndexを全部保存する(毎ステージも変えるので、Updateの中に入れなきゃ)
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                if (_map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }
        //目標のIndexとBoxのIndexは同じかどうか？
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject obj = _field[goals[i].y, goals[i].x];
            if (obj == null || !obj.CompareTag("Box"))
                return false;
        }
        return true;
    }
}
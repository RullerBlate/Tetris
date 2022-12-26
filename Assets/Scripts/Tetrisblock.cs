using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrisblock : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationPoint;
    [SerializeField] private SpawnTetromino _spawner;

    public static int _height = 20;
    public static int _width = 10;
    private static Transform[,] _grid = new Transform[_width, _height];

    private float _previousTime;
    private float _fallTime = 0.8f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (ChekIsMoveValid())
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (ChekIsMoveValid())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
            if (ChekIsMoveValid())
                transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);
        }

        if (Time.time - _previousTime > (Input.GetKey(KeyCode.DownArrow)? _fallTime / 10 : _fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (ChekIsMoveValid())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckForLines();
                this.enabled = false;
                _spawner.NewTetromino();
            }

            _previousTime = Time.time;
        }

        bool ChekIsMoveValid()
        {
            foreach (Transform children in transform)
            {
                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);

                if (roundedX < 0 || roundedX >= _width || roundedY < 0 || roundedY >= _height)
                {
                    return true;
                }

                if (_grid[roundedX, roundedY] != null)
                    return true;
            }
            return false;
        }

        void CheckForLines()
        {
            for (int i = _height - 1; i >= 0; i--)
            {
                if (HasLine(i))
                {
                    DeleteLine(i);
                    RowDown(i);
                }
            }
        }

        bool HasLine(int i)
        {
            for (int j = 0; j < _width; j++)
            {
                if (_grid[j, i] == null)
                    return false;
            }
        return true;
        }

    void DeleteLine(int i)
    {
        for (int j = 0; j < _width; j++){
                Destroy(_grid[j, i].gameObject);
                _grid[j, i] = null;
        }
    }

        void RowDown(int i)
        {
            for (int y=i; y < _height; y++)
            {
                for (int j= 0; j < _width; j++)
                {
                    if(_grid[j,y] != null)
                    {
                        _grid[j, y - 1] = _grid[j, y];
                        _grid[j, y] = null;
                        _grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                    }
                }
            }
        }


    void AddToGrid()
        {
            foreach (Transform children in transform)
            {

                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);

                _grid[roundedX, roundedY] = children;

            }
        }
    }
}

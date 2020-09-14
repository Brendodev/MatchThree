﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int borderSize;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject[] gamePiecePrefabs;


    Tile[,] m_allTiles;
    GamePiece[,] m_allGamePieces;

    private void Start()
    {
        m_allTiles = new Tile[width, height];
        m_allGamePieces = new GamePiece[width, height];
        SetupTiles();
        SetupCamera();
        FillRandom();
    }

    private void SetupTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);

                tile.name = "Tile(" + i + "," + j + ")";

                m_allTiles[i, j] = tile.GetComponent<Tile>();

                tile.transform.parent = transform;
                m_allTiles[i, j].Init(i, j, this);
            }

        }
    }

    private void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float) (width - 1) / 2f, (float) (height - 1) / 2f, -10f);

        float aspectRatio = (float) Screen.width / (float) Screen.height;

        float verticalSize = (float) height / 2f + (float) borderSize;

        float horizontalSize = ((float) +width / 2f + (float) borderSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;

    }

    GameObject GetRandomGamePiece()
    {
        int randomIdx = Random.Range(0, gamePiecePrefabs.Length);

        if (gamePiecePrefabs[randomIdx] == null)
        {
            Debug.LogWarning("Board: " + randomIdx + "does not contain a valid GamePice Prefab");
        }

        return gamePiecePrefabs[randomIdx];
    }

    private void PlaceGamePiece(GamePiece gamePiece, int x, int y)
    {
        if (gamePiecePrefabs == null)
        {
            Debug.LogWarning("Board: Invalid GamePiece");
            return;
        }

        gamePiece.transform.position = new Vector3(x, y, 0);
        gamePiece.transform.rotation = Quaternion.identity;
        gamePiece.SetCoord(x, y);

    }

    private void FillRandom()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject randomPiece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity);
                
                if (randomPiece != null)
                {
                    PlaceGamePiece(randomPiece.GetComponent<GamePiece>(),i,j);
                }

            }
        }
    }
}
using TomGustin.ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private float backgroundWidth;

    [SerializeField] private float areaStartOffset;
    [SerializeField] private float areaEndOffset;

    private List<GameObject> spawnedBackground = new List<GameObject>();

    private Camera gameCamera;
    private float lastSpawnPositionX;
    private float lastUnspawnPositionX;

    private void Start()
    {
        gameCamera = Camera.main;

        lastSpawnPositionX = GetStartPositionX();
        lastUnspawnPositionX = GetStartPositionX() - backgroundWidth;

        while (lastSpawnPositionX < GetEndPositionX())
        {
            GenerateBackground(lastSpawnPositionX);
            lastSpawnPositionX += backgroundWidth;
        }
    }

    private void Update()
    {
        while (lastSpawnPositionX < GetEndPositionX())
        {
            GenerateBackground(lastSpawnPositionX);
            lastSpawnPositionX += backgroundWidth;
        }

        while (lastUnspawnPositionX + backgroundWidth < GetStartPositionX())
        {
            if (spawnedBackground.Count <= 0) return;
            lastUnspawnPositionX += backgroundWidth;
            spawnedBackground[0].Unspawn();
            spawnedBackground.RemoveAt(0);
        }
    }

    private void GenerateBackground(float posX)
    {
        Vector2 position;
        GameObject spawnedTerrain;

        position = new Vector2(posX, transform.position.y);
        spawnedTerrain = prefab.Spawn(position, Quaternion.identity, transform);

        spawnedBackground.Add(spawnedTerrain);
    }

    private float GetStartPositionX()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;
    }

    private float GetEndPositionX()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;
    }
}

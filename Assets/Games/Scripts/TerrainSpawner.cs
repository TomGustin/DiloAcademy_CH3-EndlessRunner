using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomGustin.ObjectPooling;

public class TerrainSpawner : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private TerrainTemplateData templateData;
    [SerializeField] private float terrainTemplateWidth;
    [SerializeField] private float floorYPos, roofYPos;

    [SerializeField] private float areaStartOffset;
    [SerializeField] private float areaEndOffset;

    private List<GameObject> spawnedTerrains = new List<GameObject>();

    private Camera gameCamera;
    private float lastSpawnPositionX;
    private float lastUnspawnPositionX;

    private string lastLevel = string.Empty;
    private string currentLevel = string.Empty;

    private const float debugLineHeight = 10.0f;

    private void Start()
    {
        gameCamera = Camera.main;

        lastSpawnPositionX = GetStartPositionX();
        lastUnspawnPositionX = GetStartPositionX();

        ChangeLevel("early-template");

        foreach (GameObject terrain in templateData.GetListTemplate(currentLevel))
        {
            GenerateTerrain(lastSpawnPositionX, terrain);
            lastSpawnPositionX += terrainTemplateWidth;
        }

        ChangeLevel("level-1");

        while (lastSpawnPositionX < GetEndPositionX())
        {
            GenerateTerrain(lastSpawnPositionX);
            lastSpawnPositionX += terrainTemplateWidth;
        }
    }

    private void Update()
    {
        while (lastSpawnPositionX < GetEndPositionX())
        {
            GenerateTerrain(lastSpawnPositionX);
            lastSpawnPositionX += terrainTemplateWidth;
        }

        while (lastUnspawnPositionX + terrainTemplateWidth < GetStartPositionX())
        {
            if (spawnedTerrains.Count <= 0) return;
            lastUnspawnPositionX += terrainTemplateWidth;
            spawnedTerrains[0].Unspawn();
            spawnedTerrains.RemoveAt(0);
        }
    }

    private void GenerateTerrain(float posX, GameObject prefab = null)
    {
        GameObject newTerrain;
        Vector2 position;
        GameObject spawnedTerrain;

        if (!currentLevel.Equals(lastLevel) && !lastLevel.Equals(string.Empty))
        {
            newTerrain = templateData.GetTemplate(lastLevel);

            position = new Vector2(posX, GetSpawnPosition(templateData.GetSpawnPos(lastLevel)));
            spawnedTerrain = newTerrain.Spawn(position, Quaternion.identity, transform);

            spawnedTerrains.Add(spawnedTerrain);

            lastLevel = currentLevel;
        }

        if (prefab) newTerrain = prefab;
        else newTerrain = templateData.GetTemplate(currentLevel);

        position = new Vector2(posX, GetSpawnPosition(templateData.GetSpawnPos(currentLevel)));
        spawnedTerrain = newTerrain.Spawn(position, Quaternion.identity, transform);

        spawnedTerrains.Add(spawnedTerrain);
    }

    private float GetStartPositionX()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;
    }

    private float GetEndPositionX()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;
    }

    public void ChangeLevel(string newLevel)
    {
        lastLevel = currentLevel;
        currentLevel = newLevel;
    }

    public float GetSpawnPosition(TerrainTemplateData.SpawnPos spawnPos)
    {
        switch (spawnPos)
        {
            case TerrainTemplateData.SpawnPos.ROOF:
                return roofYPos;
            case TerrainTemplateData.SpawnPos.FLOOR:
                return floorYPos;
            default:
                return -9;
        }
    }

    private void OnDrawGizmos()
    {
        if (!gameCamera) return;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position;

        startPosition.x = GetStartPositionX();
        endPosition.x = GetEndPositionX();

        Debug.DrawLine(startPosition + Vector3.up * debugLineHeight / 2, startPosition + Vector3.down * debugLineHeight / 2, Color.red);
        Debug.DrawLine(endPosition + Vector3.up * debugLineHeight / 2, endPosition + Vector3.down * debugLineHeight / 2, Color.red);
    }
}

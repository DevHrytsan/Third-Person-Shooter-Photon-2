using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EasyTerrainTextureCheck : MonoBehaviour
{
    public Transform playerTransform;
    public Terrain terrainObject;

    public int posX;
    public int posY;
    public float[] textureValues;

    // Start is called before the first frame update
    void Start()
    {
        terrainObject = Terrain.activeTerrain;
        playerTransform = gameObject.transform;
    }
  
    public void GetTerrainTexture()
    {
        UpdatePosition();
        CheckTexture();

    }
    // Update is called once per frame
    void UpdatePosition()
    {
        Vector3 terrainPosition = playerTransform.position - terrainObject.transform.position;
        Vector3 mapPosition = new Vector3(terrainPosition.x / terrainObject.terrainData.size.x, 0, terrainPosition.z / terrainObject.terrainData.size.z);
        float xCoord = mapPosition.x * terrainObject.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * terrainObject.terrainData.alphamapHeight;
        posX = (int)xCoord;
        posY = (int)zCoord;


    }
    void CheckTexture()
    {
        float[,,] splatMap = terrainObject.terrainData.GetAlphamaps(posX, posY, 1, 1);
        textureValues[0] = splatMap[0, 0, 0];
        textureValues[1] = splatMap[0, 0, 1];
        textureValues[2] = splatMap[0, 0, 2];
        textureValues[3] = splatMap[0, 0, 3];

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int mapWidth = 15;
    public int mapHeight = 15;
    public bool isFogOfWarActive = true;

    public TileController[,] tilemap;

    public GameObject emptyTilePrefab;
    public GameObject filledTilePrefab;
    public GameObject hardTilePrefab;
    public GameObject indestructableTilePrefab;
    public GameObject gemTilePrefab;
    public GameObject freeGemTilePrefab;
    public GameObject diamondTilePrefab;
    public GameObject anvilTilePrefab;
    public List<AudioClip> turnSounds = new List<AudioClip>();

    public List<PlayerController> players = new List<PlayerController>();
    public int currentPlayer;

    public float gemChance = 0.2f;
    public float freeGemChance = 0.2f;
    public float hardChance = 0.5f;
    public float indestructableChance = 0.5f;
 
    public static LevelManager Instance{ get; private set; }

    void Awake()
    {
        Instance = this;

        float seed = Random.Range(0f, 100f);

        float[,] noiseMap = new float[mapWidth, mapHeight];
        for(int y = 0; y < noiseMap.GetLength(1); y++) {
            for(int x = 0; x < noiseMap.GetLength(0); x++) {
                noiseMap[x,y] = Mathf.PerlinNoise(x + seed * 0.02f, y + seed * 0.02f) - (((Mathf.Abs(7f - x) + Mathf.Abs(7f - y)) / 15f) / 3f);
            }
        }

        tilemap = new TileController[mapWidth, mapHeight];
        for(int y = 0; y < tilemap.GetLength(1); y++) {
            for(int x = 0; x < tilemap.GetLength(0); x++) {
                if(x == 7f && y == 7f) {
                    PlaceTile(x, y, diamondTilePrefab);
                }
                else if(noiseMap[x,y] > 0.3f) {
                    if(Random.Range(0f, 1f) < gemChance) {
                        PlaceTile(x, y, gemTilePrefab);
                    }
                    else {
                        if(Random.Range(0f, 1f) < hardChance) {
                            if(Random.Range(0f, 1f) < indestructableChance) {
                                PlaceTile(x, y, indestructableTilePrefab);
                            }
                            else {
                                PlaceTile(x, y, hardTilePrefab);
                            }
                        }
                        else {
                            PlaceTile(x, y, filledTilePrefab);
                        }
                    }
                }
                else {
                    if(Random.Range(0f, 1f) < freeGemChance) {
                        PlaceTile(x, y, freeGemTilePrefab);
                    }
                    else {
                        PlaceTile(x, y, emptyTilePrefab);
                    }
                }
            }
        }

        PlaceAnvils();
    }

    void PlaceAnvils() {
        int x = Random.Range(0, (mapWidth / 2));
        int y = Random.Range(0, (mapWidth / 2));
        PlaceTile(x, y, anvilTilePrefab);

        x = Random.Range(0, (mapWidth / 2));
        y = Random.Range((mapWidth / 2), mapWidth);
        PlaceTile(x, y, anvilTilePrefab);

        x = Random.Range((mapWidth / 2), mapWidth);
        y = Random.Range(0, (mapWidth / 2));
        PlaceTile(x, y, anvilTilePrefab);

        x = Random.Range((mapWidth / 2), mapWidth);
        y = Random.Range((mapWidth / 2), mapWidth);
        PlaceTile(x, y, anvilTilePrefab);
    }

    void Start() {
        players[currentPlayer].StartTurn();
    }

    public void AdvanceTurn() {
        players[currentPlayer].EndTurn();

        if(currentPlayer >= players.Count - 1) {
            currentPlayer = 0;
        }
        else {
            currentPlayer++;
        }
        players[currentPlayer].StartTurn();

        AudioSource.PlayClipAtPoint(turnSounds[currentPlayer], Camera.main.transform.position);
    }

    public PlayerController ActivePlayer() {
        return players[currentPlayer];
    }

    public bool CheckWalkable(int x, int y) {
        if(x >= 0 && y >= 0 && x < tilemap.GetLength(0) && y < tilemap.GetLength(1)) {
            if(tilemap[x,y].isWalkable) {
                return true;
            }
        }
        return false;
    }

    public bool CheckDiggable(int x, int y) {
        if(x >= 0 && y >= 0 && x < tilemap.GetLength(0) && y < tilemap.GetLength(1)) {
            if(tilemap[x,y].isDiggable) {
                return true;
            }
        }
        return false;
    }

    public bool CheckAnvil(int x, int y) {
        if(x >= 0 && y >= 0 && x < tilemap.GetLength(0) && y < tilemap.GetLength(1)) {
            if(tilemap[x,y].isAnvil) {
                return true;
            }
        }
        return false;
    }

    public bool Dig(int x, int y, int power, PlayerController player) {
        if(x >= 0 && y >= 0 && x < tilemap.GetLength(0) && y < tilemap.GetLength(1)) {
            if(tilemap[x,y].isDiggable) {
                tilemap[x,y].Dig(power, player);
                if(tilemap[x,y].currentHealth <= 0) {
                    PlaceTile(x, y, emptyTilePrefab);
                    return true;
                }
            }
        }
        return false;
    }

    public void PlaceTile(int x, int y, GameObject newTile) {
        if(tilemap[x,y] != null) {
            GameObject.Destroy(tilemap[x,y].gameObject);
        }

        tilemap[x,y] = Instantiate(newTile).GetComponent<TileController>();
        tilemap[x,y].transform.position = new Vector3(x, y, 0f);
    }
}

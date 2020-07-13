using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{ get; private set; }
    public MatchRuleset ruleset;
    public GameState state;
    public List<PlayerController> players = new List<PlayerController>();
    public PlayerMatchData winner;
    public PlayerMatchData loser;

    public string menuScene;
    public string matchScene;
    public string postMatchScene;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(LevelManager.Instance != null) {
            state = GameState.Match;
        }
        else {
            state = GameState.Menu;
        }
    }

    public void StartMatch() {
        SceneManager.LoadScene(matchScene);
        state = GameState.Match;
    }

    public void EndMatch(PlayerMatchData winner) {
        this.winner = winner;
        loser = GetLoser();
        SceneManager.LoadScene(postMatchScene);
        state = GameState.PostMatch;
        players.Clear();
    }

    public void EnterMenu() {
        winner.score = 0;
        loser.score = 0;

        SceneManager.LoadScene(menuScene);
        state = GameState.Menu;
    }

    PlayerMatchData GetLoser() {
        foreach(PlayerController player in players) {
            if(player.playerMatchData != winner) {
                return player.playerMatchData;
            }
        }
        return null;
    }
}

public enum GameState {
    Menu,
    Match,
    PostMatch
}
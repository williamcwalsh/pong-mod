using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private Paddle computerPaddle;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text computerScoreText;
    [SerializeField] private GameObject[] powerUpPrefabs;
    [SerializeField] private float powerUpSpawnRangeY = 4f;

    private int playerScore;
    private int computerScore;
    private GameObject activePowerUp;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            NewGame();
        }
    }

    public void NewGame()
    {
        SetPlayerScore(0);
        SetComputerScore(0);
        ClearPowerUp();
        NewRound();
    }

    public void NewRound()
    {
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();
        ClearPowerUp();

        CancelInvoke();
        Invoke(nameof(StartRound), 1f);
    }

    private void StartRound()
    {
        ball.AddStartingForce();
    }

    public void OnPlayerScored()
    {
        SetPlayerScore(playerScore + 1);
        NewRound();
    }

    public void OnComputerScored()
    {
        SetComputerScore(computerScore + 1);
        NewRound();
    }

    public bool AwardPointTo(Paddle paddle)
    {
        if (paddle == playerPaddle)
        {
            OnPlayerScored();
            return true;
        }

        if (paddle == computerPaddle)
        {
            OnComputerScored();
            return true;
        }

        return false;
    }

    public void RegisterPaddleHit()
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
        {
            return;
        }
        SpawnPowerUp();
    }

    private void SetPlayerScore(int score)
    {
        playerScore = score;
        playerScoreText.text = score.ToString();
    }

    private void SetComputerScore(int score)
    {
        computerScore = score;
        computerScoreText.text = score.ToString();
    }

    private void SpawnPowerUp()
    {
        ClearPowerUp();

        GameObject prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        Vector3 spawnPosition = new Vector3(0f, Random.Range(-powerUpSpawnRangeY, powerUpSpawnRangeY), 0f);
        activePowerUp = Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    private void ClearPowerUp()
    {
        if (activePowerUp != null)
        {
            Destroy(activePowerUp);
            activePowerUp = null;
        }
    }

}

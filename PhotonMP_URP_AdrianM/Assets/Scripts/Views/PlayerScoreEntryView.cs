using TMPro;
using UnityEngine;

public class PlayerScoreEntryView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _playerScore;

    public string PlayerName { get; private set; }

    public void Setup(string playerName, string playerScore)
    {
        PlayerName = playerName;

        _playerName.text = PlayerName;
        _playerScore.text = playerScore;
    }
}
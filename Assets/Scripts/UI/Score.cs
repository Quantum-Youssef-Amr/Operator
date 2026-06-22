using System.Collections;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI ScoreText;
    [SerializeField] private float LerpSpeed = 2f;
    private float _currentScore;
    void Start()
    {
        GameEventManager.Instance.OnScoreChanged += CalculateScore;
    }

    private void CalculateScore(float newScore)
    {
        StartCoroutine(LerpToNewScore(newScore));
    }


    private IEnumerator LerpToNewScore(float newScore)
    {
        yield return new WaitUntil(() =>
        {
            _currentScore = Mathf.Lerp(_currentScore, newScore, Time.deltaTime * LerpSpeed);
            ScoreText.text = FormateScore(_currentScore);
            return _currentScore >= newScore;
        });

        // final update : _current score may be > newScore
        _currentScore = newScore;
        ScoreText.text = FormateScore(_currentScore);
    }

    private string FormateScore(float currentScore)
    {
        return currentScore < 10 ? $"000{currentScore}" :
        currentScore < 100 ? $"00{currentScore}" :
        currentScore < 1000 ? $"0{currentScore}" : $"{currentScore}";
    }
}

using UnityEngine;
using TMPro; // Importante para usar TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton para acceso global
    public TextMeshProUGUI scoreText; // Cambiar el tipo para TextMeshPro

    private int score = 0;

    private void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points)
    {
        score += points; // Sumar puntos
        UpdateScoreUI(); // Actualizar el texto en pantalla
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score;
        }
    }
}
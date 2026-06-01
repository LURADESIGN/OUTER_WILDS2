using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DetectionZone : MonoBehaviour
{
    [Header("Enemy")]
    public NeighbourController neighbour;

    [Header("Game Over UI")]
    public Image blackOverlay;
    public Image clockImage;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip wakeUpSound;

    [Header("Timing")]
    public float clockDelay = 1f;
    public float resetDelay = 4f;

    private bool gameOverStarted = false;

    void Start()
    {
        if (blackOverlay != null)
            blackOverlay.gameObject.SetActive(false);
        else
            Debug.LogWarning("DetectionZone: BlackOverlay is not assigned.");

        if (clockImage != null)
            clockImage.gameObject.SetActive(false);
        else
            Debug.LogWarning("DetectionZone: ClockImage is not assigned.");

        if (neighbour == null)
            Debug.LogWarning("DetectionZone: Neighbour is not assigned.");

        if (audioSource == null)
            Debug.LogWarning("DetectionZone: AudioSource is not assigned.");
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameOverStarted)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (neighbour == null)
            return;

        if (neighbour.IsAlert())
        {
            StartCoroutine(GameOverSequence());
        }
    }

    private IEnumerator GameOverSequence()
    {
        gameOverStarted = true;

        if (blackOverlay != null)
            blackOverlay.gameObject.SetActive(true);

        if (audioSource != null && wakeUpSound != null)
            audioSource.PlayOneShot(wakeUpSound);

        yield return new WaitForSeconds(clockDelay);

        if (clockImage != null)
            clockImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(resetDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trofeo : MonoBehaviour
{
    [SerializeField] private AudioSource winAudioSource;
    [SerializeField] public ParticleSystem sdpWin;
    private bool hasPlayedSound = false;


    public GameplayManager gamePlayCanvas;
    public Animator animatorTrophy;
    public float delayWin = 4f; // Tiempo de espera antes de mostrar el panel de victoria
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Player>())
            {
                animatorTrophy.SetBool("Win", true);
                
                sdpWin.Play();

                if (!hasPlayedSound)
                {
                    winAudioSource.Play();
                    hasPlayedSound = true; // Marcar que el sonido ya se reprodujo
                }

                // Iniciar la corrutina para mostrar el panel de victoria después de un retraso
                StartCoroutine(ShowWinScreenAfterDelay());
            }

        }
    }

    private IEnumerator ShowWinScreenAfterDelay()
    {
        yield return new WaitForSeconds(delayWin);

        gamePlayCanvas.Onwin();
    }

}

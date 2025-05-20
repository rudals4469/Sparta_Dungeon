using System.Collections;
using UnityEngine;

public class PlatformLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    public float launchForce = 15f;
    public Vector3 launchDirection = new Vector3(0f, 1f, 1f);

    [Header("Timing")]
    public float delayBeforeLaunch = 1f;

    private Coroutine launchCoroutine;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && launchCoroutine == null)
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            PlayerController playerCtrl = collision.gameObject.GetComponent<PlayerController>();

            if (playerRb != null && playerCtrl != null && !playerCtrl.isLaunched)
            {
                playerCtrl.isLaunched = true;
                playerRb.velocity = Vector3.zero;
                launchCoroutine = StartCoroutine(LaunchPlayer(playerRb, playerCtrl));
            }
        }
    }
    private IEnumerator LaunchPlayer(Rigidbody playerRb, PlayerController playerCtrl)
    {
        yield return new WaitForSeconds(delayBeforeLaunch);

        Vector3 finalDirection = launchDirection.normalized;
        playerRb.AddForce(finalDirection * launchForce, ForceMode.Impulse);

        yield return new WaitForSeconds(2f);
        playerCtrl.isLaunched = false;

        launchCoroutine = null;
    }
}
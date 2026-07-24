using System.Collections;
using UnityEngine;

public class OutArea : MonoBehaviour
{
    [Header("復帰設定")]
    public float returnDelay = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        StoneController controller =
            other.GetComponent<StoneController>();

        if (controller != null)
        {
            StartCoroutine(
                WaitForStop(controller)
            );
        }
    }

    IEnumerator WaitForStop(
        StoneController controller)
    {
        Rigidbody rb =
            controller.GetComponent<Rigidbody>();

        while (
            rb != null &&
            rb.linearVelocity.magnitude > 0.1f
        )
        {
            yield return null;
        }

        yield return new WaitForSeconds(
            returnDelay
        );

        controller.ReturnToPreviousShot();
    }
}
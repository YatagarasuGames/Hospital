using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectsPushFix : NetworkBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float _pushForceMultiplier = 1.5f;
    [SerializeField] private float _correctionSpeed = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isLocalPlayer) return;

        if (collision.rigidbody != null && collision.rigidbody.CompareTag("Pushable"))
        {
            // Клиентское предсказание
            Vector3 pushForce = CalculatePushForce(collision);
            collision.rigidbody.AddForce(pushForce, ForceMode.VelocityChange);

            // Отправка на сервер
            CmdPushObject(collision.rigidbody.GetComponent<NetworkIdentity>().netId, pushForce, collision.contacts[0].point);
        }
    }

    private Vector3 CalculatePushForce(Collision collision)
    {
        Vector3 pushDir = collision.impulse.normalized;
        float pushMagnitude = Mathf.Clamp(collision.impulse.magnitude, 1f, 10f);
        return pushDir * pushMagnitude * _pushForceMultiplier;
    }

    [Command]
    private void CmdPushObject(uint objectNetId, Vector3 force, Vector3 contactPoint)
    {
        if (NetworkServer.spawned.TryGetValue(objectNetId, out NetworkIdentity identity))
        {
            if (identity.TryGetComponent<Rigidbody>(out var rb))
            {
                // Сервер применяет силу с проверкой
                rb.AddForceAtPosition(force, contactPoint, ForceMode.VelocityChange);

                // Синхронизация с клиентами
                RpcCorrectPush(objectNetId, rb.velocity, rb.angularVelocity);
            }
        }
    }

    [ClientRpc]
    private void RpcCorrectPush(uint objectNetId, Vector3 correctedVelocity, Vector3 correctedAngularVelocity)
    {
        if (NetworkClient.spawned.TryGetValue(objectNetId, out NetworkIdentity identity))
        {
            if (identity.TryGetComponent<Rigidbody>(out var rb))
            {
                // Плавная коррекция предсказания
                StartCoroutine(SmoothCorrection(rb, correctedVelocity, correctedAngularVelocity));
            }
        }
    }

    private IEnumerator SmoothCorrection(Rigidbody rb, Vector3 targetVelocity, Vector3 targetAngularVelocity)
    {
        float t = 0;
        Vector3 startVel = rb.velocity;
        Vector3 startAngVel = rb.angularVelocity;

        while (t < 1f)
        {
            t += Time.deltaTime * _correctionSpeed;
            rb.velocity = Vector3.Lerp(startVel, targetVelocity, t);
            rb.angularVelocity = Vector3.Lerp(startAngVel, targetAngularVelocity, t);
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private bool isForward = true;
    [SerializeField]
    private Transform forwardFace;
    [SerializeField]
    private Transform backFace;

    private void Start()
    {
        forwardFace.gameObject.SetActive(isForward);
        backFace.gameObject.SetActive(!isForward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsOnForward(other.transform))
            {
                other.GetComponent<Transporter>().RequestTransport();
            }
        }
    }

    /// <summary>
    /// 正面にあるどうかのチェック
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private bool IsOnForward(Transform other)
    {
        var selfForward = isForward ? forwardFace.forward : backFace.forward;
        var delta = other.position - transform.position;

        return Vector3.Dot(selfForward, delta) < 0;
    }
}

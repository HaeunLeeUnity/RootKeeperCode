using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTimer : MonoBehaviour
{
    [SerializeField] float deadTime = 1.2f;
    private void Start()
    {
        StartCoroutine(DestroyTimer());
    }


    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(deadTime);
        Destroy(this.gameObject);
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckPoint : MonoBehaviour
{
    public int CheckPointIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TimeManager.Instance.CheckPointReached(CheckPointIndex);
        }
    }
}

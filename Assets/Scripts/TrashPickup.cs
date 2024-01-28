using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPickup : MonoBehaviour
{
    [SerializeField] int points = 1;
    [SerializeField] float despawnDelay = 1f;

    [SerializeField] AudioClip[] _audioClips;

    private void OnTriggerEnter(Collider other)
    {
        TrashCollector player = other.GetComponentInParent<TrashCollector>();
        if (player != null)
        {
            player.CollectTrash(points);
            GetComponent<AudioSource>().PlayOneShot(_audioClips[Random.Range(0, _audioClips.Length)]);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            //ToDo play particle system here

            StartCoroutine(DestroyDelayed());
        }
    }

    IEnumerator DestroyDelayed()
    {
        yield return new WaitForSeconds(despawnDelay);

        Destroy(transform.parent.gameObject);
    }
}

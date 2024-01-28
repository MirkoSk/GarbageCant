using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrashPickup : MonoBehaviour
{
    [SerializeField] int points = 1;

    [Header("Pickup Settings")]
    [SerializeField] float scaleUpDuration = 1f;
    [SerializeField] float scaleDownDuration = 0.4f;
    [SerializeField] AudioClip[] _audioClips;

    private void OnTriggerEnter(Collider other)
    {
        TrashCollector player = other.GetComponentInParent<TrashCollector>();
        if (player != null)
        {
            player.CollectTrash(points);
            GetComponent<AudioSource>().PlayOneShot(_audioClips[Random.Range(0, _audioClips.Length)]);
            GetComponent<Collider>().enabled = false;

            transform.parent.DOScale(2f, scaleUpDuration).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                transform.parent.DOScale(0f, scaleDownDuration).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    Destroy(transform.parent.gameObject);
                });
            });
        }
    }
}

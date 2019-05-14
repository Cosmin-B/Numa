using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractable_Script : MonoBehaviour
{
    private GameObject _hologram = null;
    AudioSource audioSource;
    public bool isInteracted = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (_hologram == null)
        {
            GameObject hologramPrefab = (GameObject)Resources.Load("Prefabs/HologramObject", typeof(GameObject));
            Debug.Assert(hologramPrefab != null);

            _hologram = Instantiate(hologramPrefab, gameObject.transform);
        }
        else
        {
            _hologram.SetActive(true);
        }

        audioSource.Play();
        isInteracted = true;

        StartCoroutine(EnableInteraction());
    }

    IEnumerator EnableInteraction()
    {
        yield return new WaitForSeconds(100.0f);

        if (_hologram != null)
            Destroy(_hologram);

        audioSource.Stop();

        isInteracted = false;
    }
}

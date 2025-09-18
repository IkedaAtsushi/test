using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    AudioSource _audioSource;
    private GameObject _BGM;
    private GameObject _powerupBGM;
    // Start is called before the first frame update
    void Start()
    {
        _BGM = GameObject.Find("GameBGM");
        _powerupBGM = GameObject.Find("PowerupBGM");
        StartCoroutine(StartBGMCoroutine());
    }

    // Update is called once per frame
    private IEnumerator StartBGMCoroutine()
    {
        yield return new WaitForSeconds(1);

        StartBGM();
    }

    public void StartBGM()
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
        _audioSource = _BGM.GetComponent<AudioSource>();
        _audioSource.Play();
    }

    public void PowerupBGM()
    {
        _audioSource.Stop();
        _audioSource = _powerupBGM.GetComponent<AudioSource>();
        _audioSource.Play();
    }
}

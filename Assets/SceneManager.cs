using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup = null;
    [SerializeField] AudioClip _startSE;
    AudioSource _audioSource;
    bool startted = false;

    public void Start()
    {
        StartCoroutine(FadeCoroutine(1, 0, 1f));
        GameObject bgm = GameObject.Find("TitleBGM");
        _audioSource = bgm.GetComponent<AudioSource>();
        _audioSource.Play();
    }
    public void StartGameScene()
    {
        if (!startted)
        {
            _audioSource.Stop();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.PlayOneShot(_startSE);
            StartCoroutine(GameStartCoroutine());
            startted = true;
        }
    }

    private IEnumerator GameStartCoroutine()
    {
        StartCoroutine(FadeCoroutine(0, 1, 1f));

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("GameScene");
    }
    private IEnumerator FadeCoroutine(float starta, float enda, float _fadeDuration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(starta, enda, elapsedTime / _fadeDuration);
            yield return null;
        }
    }
}

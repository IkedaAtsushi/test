using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerReaction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject _message;
    [SerializeField] Vector3 _targetScale = new Vector3(2f, 2f, 2f);
    Vector3 _firstScale;
    [SerializeField] float _speed = 1f;
    bool _onMouse = false;

    public void Start()
    {
        _firstScale = transform.localScale;
    }
    public void Update()
    {
        if (!_onMouse)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _firstScale, _speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("onmouse");
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _speed * Time.deltaTime);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _onMouse = true;
        _message.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _onMouse = false;
        _message.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _message.SetActive(false);
    }
}

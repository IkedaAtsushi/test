using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Text _resultText;
    private void OnEnable()
    {
        GameObject eventmanager = GameObject.Find("EventManager");
        EventManager em = eventmanager.GetComponent<EventManager>();
        _resultText.text = em.minute.ToString("00") + ":" + ((int)em.seconds).ToString("00");
    }

}

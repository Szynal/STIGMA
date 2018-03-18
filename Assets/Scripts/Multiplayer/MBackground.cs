using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBackground : MonoBehaviour
{
    private GameObject _Background;
    private GameObject _UI;

    private void Start()
    {
        _Background = GameObject.Find("Background");
        _Background.GetComponent<Canvas>().worldCamera = GetComponent<Camera>();

        _UI = GameObject.Find("Canvas");
        _UI.GetComponent<Canvas>().worldCamera = GetComponent<Camera>();
    }
}

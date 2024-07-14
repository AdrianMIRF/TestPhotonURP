using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetNameView : MonoBehaviour, IView
{
    public Action<string> onNameChosen;

    private string _playerName;

    public void Initialize()
    {
        gameObject.SetActive(true);
        _playerName = "default name";
    }

    public void ShowView()
    {
        gameObject.SetActive(true);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }

    public void OnNameUpdate(string value)
    {
        _playerName = value;
    }

    public void OnNameChosen()
    {
        gameObject.SetActive(false);
        onNameChosen.Invoke(_playerName);
    }
}

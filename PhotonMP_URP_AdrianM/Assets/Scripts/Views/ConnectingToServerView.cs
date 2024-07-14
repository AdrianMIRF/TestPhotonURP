using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class ConnectingToServerView : MonoBehaviourPunCallbacks, IView
{
    public void Initialize()
    {
        ShowView();
        StartCoroutine(nameof(ConnectUsingSettings));
    }

    public void ShowView()
    {
        gameObject.SetActive(true);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }

    IEnumerator ConnectUsingSettings()
    {
        Debug.Log(Time.time + " Start ConnectUsingSettings ...");

        //leave room just in case logout was bad..
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        Debug.Log(Time.time + " IsConnected...");

        //Dev: just to show this screen a little more.. maybe show some animation.. 
        yield return new WaitForSeconds(2);

        HideView();

        PhotonNetwork.ConnectUsingSettings();
    }
}
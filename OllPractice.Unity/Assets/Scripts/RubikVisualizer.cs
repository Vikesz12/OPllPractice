using System.Collections;
using System.Collections.Generic;
using Model;
using Parser;
using UnityEngine;

public class RubikVisualizer : MonoBehaviour
{
    private NotificationParser _notificationParse;
    private Cube _cube;

    public void Start()
    {
        _notificationParse = new NotificationParser();
        _cube = new Cube();
    }


    public void ProcessMessage(byte[] notification)
    {
        _notificationParse.ParseNotification(notification, _cube);
    }
}

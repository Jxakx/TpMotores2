using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Notifications;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif 
public class NotificationController : MonoBehaviour
{
    public void ActivarNotificacion()
    {
        DateTime fechaActivar = DateTime.Now;
    }


#if UNITY_ANDROID
    private const string idCanal = "Canal Notificacion";

    public void makeNotification(DateTime fecha)
    {
        AndroidNotificationChannel androidNotificationChannel = new AndroidNotificationChannel
        {
            Id = idCanal,
            Name = "Canal Notificacion",
            Description = "Canal para notificaciones",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(androidNotificationChannel);

        AndroidNotification androidNotification = new AndroidNotification
        {
            Title = "FROGGY QUIERE ENTRENAR",
            Text = "¡Vuelve! ¡Hay que ganar esa carrera!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = fecha
        };

        AndroidNotificationCenter.SendNotification(androidNotification, idCanal);
    }
#endif
}

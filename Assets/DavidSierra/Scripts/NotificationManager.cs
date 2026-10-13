using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NotificationManager : MonoBehaviour {
    public class Notification {
        public string message;
        public float duration;

        public Notification(string message, float duration) {
            this.duration = duration;
            this.message = message;
        }
    }

    public Notification currentNotification;
    public Queue<Notification> notificationQueue = new Queue<Notification>();

    [SerializeField] TMPro.TextMeshProUGUI textToShow;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float defaultNotificationFadeInDuration = 0.5f;
    [SerializeField] float defaultNotificationFadeOutDuration = 0.5f;
    [SerializeField] float timeBetweenNotifications = 0.5f;

    float timeRemainingForNextNotification = 0.0f;
    public void AddNotification(string message, float duration) {
        notificationQueue.Enqueue(new Notification(message, duration));
    }

    public void Update() {
        if (currentNotification == null) {
            timeRemainingForNextNotification = Mathf.Clamp(timeRemainingForNextNotification - Time.deltaTime, 0, timeBetweenNotifications);
        }
        if (notificationQueue.Count == 0) return;

        if (currentNotification == null) {
            currentNotification = notificationQueue.Dequeue();
            StartCoroutine(_InitializeNewNotification(currentNotification));
        }

    }

    IEnumerator _InitializeNewNotification(Notification n) {
        float timer = 0;
        textToShow.text = n.message;

        //TODO FADE IN
        while (timer < defaultNotificationFadeInDuration) {

            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / defaultNotificationFadeInDuration);

            yield return null;
            timer += Time.deltaTime;
        }
        timer = 0;

        //TODO SHOW FOR SECONDS
        while (timer < n.duration) {
            yield return null;
            timer += Time.deltaTime;
        }

        timer = 0;

        //TODO HIDE 
        while (timer < defaultNotificationFadeOutDuration) {

            canvasGroup.alpha = Mathf.Lerp(1, 0, timer / defaultNotificationFadeInDuration);

            yield return null;
            timer += Time.deltaTime;
        }
        timeRemainingForNextNotification = timeBetweenNotifications;
        currentNotification = null;
    }

}

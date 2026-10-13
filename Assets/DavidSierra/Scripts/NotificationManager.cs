using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public class Notification
    {
        public string message;
        public float duration;

        public Notification(string message, float duration)
        {
            this.message = message;
            this.duration = duration;
        }
    }

    [SerializeField] private TextMeshProUGUI textToShow;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField, Min(0f)] private float defaultNotificationFadeInDuration = 0.5f;
    [SerializeField, Min(0f)] private float defaultNotificationFadeOutDuration = 0.5f;
    [SerializeField, Min(0f)] private float timeBetweenNotifications = 0.5f;

    private readonly Queue<Notification> notificationQueue = new Queue<Notification>();
    private Notification currentNotification;
    private float timeRemainingForNextNotification;

    private void Awake()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public void AddNotification(string message, float duration)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        notificationQueue.Enqueue(new Notification(message, Mathf.Max(0f, duration)));
    }

    private void Update()
    {
        if (currentNotification != null || textToShow == null || canvasGroup == null)
            return;

        if (timeRemainingForNextNotification > 0f)
        {
            timeRemainingForNextNotification -= Time.deltaTime;
            return;
        }

        if (notificationQueue.Count == 0)
            return;

        currentNotification = notificationQueue.Dequeue();
        StartCoroutine(ShowNotification(currentNotification));
    }

    private IEnumerator ShowNotification(Notification notification)
    {
        textToShow.text = notification.message;
        canvasGroup.alpha = 0f;

        float timer = 0f;
        while (timer < defaultNotificationFadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / defaultNotificationFadeInDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        timer = 0f;

        while (timer < notification.duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        while (timer < defaultNotificationFadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / defaultNotificationFadeOutDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        currentNotification = null;
        timeRemainingForNextNotification = timeBetweenNotifications;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        currentNotification = null;

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}
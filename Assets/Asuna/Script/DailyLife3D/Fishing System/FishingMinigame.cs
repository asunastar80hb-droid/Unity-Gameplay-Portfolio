using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class FishingMinigame : MonoBehaviour
{
    private enum State
    {
        Idle,
        Running,
        Result,
    }

    [Header("UI References")]
    [SerializeField] private RectTransform trackArea;
    [SerializeField] private RectTransform marker;
    [SerializeField] private RectTransform successZone;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Image staminaCircle;
    [SerializeField] private HoldButton button;
    [SerializeField] private FishingSession _fishingSession;

    [Header("Marker / Lever Control")]
    [SerializeField] private float leverUpSpeed = 1.2f;
    [SerializeField] private float leverDownSpeed = 0.8f;

    [Header("Target Zone")]
    [SerializeField] private bool randomizeZone = true;
    [SerializeField] private Vector2 zoneSizeRange = new Vector2(0.2f, 0.35f);

    [SerializeField, Range(0f, 0.5f)]
    private float zoneCenterClamp = 0.15f;

    [SerializeField] private float zoneMoveSpeed = 150f;

    [Header("Stamina")]
    [SerializeField] private float staminaFillSpeed = 0.35f;
    [SerializeField] private float staminaDrainSpeed = 0.5f;

    [Header("Events")]
    public UnityEvent OnCatch;
    public UnityEvent OnMiss;

    [Header("Stamina Grace Period")]
    [SerializeField] private Vector2 failDelayRange = new Vector2(1.5f, 3f);

    private bool inDanger;
    private float _failTimer;

    private State _currentState = State.Idle;

    private float _t;
    private float _stamina;
    private int _zoneDir = 1;

    private void Start()
    {
        if (_fishingSession == null)
        {
            Debug.LogError("FishingMinigame: FishingSession is missing.");
            return;
        }

        _fishingSession.OnMinigameStart.AddListener(StartFishing);
    }

    private void OnDestroy()
    {
        if (_fishingSession == null)
            return;

        _fishingSession.OnMinigameStart.RemoveListener(StartFishing);
    }

    private void Update()
    {
        if (_currentState != State.Running)
            return;

        UpdateMarker();
        UpdateTargetZone();
        UpdateStamina();
    }

    #region Marker

    private void UpdateMarker()
    {
        float input = button.isHolding ? 1f : -1f;

        float speed = input > 0f ? leverUpSpeed : leverDownSpeed;

        _t += input * speed * Time.deltaTime;
        _t = Mathf.Clamp01(_t);

        ApplyMarkerPosition();
    }

    private void ApplyMarkerPosition()
    {
        float width = trackArea.rect.width / 2f;

        float x = Mathf.Lerp(-width, width, _t);
        
        marker.anchoredPosition = new Vector3(x, marker.anchoredPosition.y, 0f);
    }

    #endregion

    #region Target Zone

    private void UpdateTargetZone()
    {
        Vector2 pos = successZone.anchoredPosition;

        pos.x += _zoneDir * zoneMoveSpeed * Time.deltaTime;

        float halfTrack = trackArea.rect.width / 2f;

        float halfZone = successZone.rect.width / 2f;

        if (pos.x > halfTrack - halfZone)
        {
            _zoneDir = -1;
        }
        else if (pos.x < -halfTrack + halfZone)
        {
            _zoneDir = 1;
        }

        successZone.anchoredPosition = pos;
    }

    private void RandomizeSuccessZone()
    {
        float trackWidth = trackArea.rect.width;

        float zoneFraction = Random.Range(zoneSizeRange.x, zoneSizeRange.y);

        float zoneWidth = zoneFraction * trackWidth;

        float minCenter = Mathf.Lerp(-trackWidth / 2f, trackWidth / 2f, zoneCenterClamp);

        float maxCenter = Mathf.Lerp(-trackWidth / 2f, trackWidth / 2f, 1f - zoneCenterClamp);

        float centerX = Random.Range(minCenter, maxCenter);

        successZone.sizeDelta = new Vector2(zoneWidth, successZone.sizeDelta.y);

        centerX = Mathf.Clamp(centerX, -trackWidth / 2f + zoneWidth / 2f, trackWidth / 2f - zoneWidth / 2f);

        successZone.anchoredPosition = new Vector2(centerX, successZone.anchoredPosition.y
        );
    }

    #endregion

    #region Stamina

    private void UpdateStamina()
    {
        bool insideZone = IsMarkerInsideZone();

        if (insideZone)
        {
            _stamina += staminaFillSpeed * Time.deltaTime;
        }
        else
        {
            _stamina -= staminaDrainSpeed * Time.deltaTime;
        }

        _stamina = Mathf.Clamp01(_stamina);

        if (staminaCircle) 
            staminaCircle.fillAmount = _stamina;

        if (_stamina <= 0f)
        {
            HandleDangerState();
        }
        else
        {
            if (inDanger)
                inDanger = false;

            if (_stamina >= 1f)
                CatchFish();
        }
    }

    private void HandleDangerState()
    {
        if (!inDanger)
        {
            inDanger = true;
            _failTimer = Random.Range(failDelayRange.x, failDelayRange.y);

            return;
        }

        _failTimer -= Time.deltaTime;

        if (_failTimer <= 0f)
            FailFishing();
    }

    private bool IsMarkerInsideZone()
    {
        float markerX = marker.anchoredPosition.x;

        float zoneCenter = successZone.anchoredPosition.x;

        float halfZone = successZone.rect.width / 2f;

        return markerX >= zoneCenter - halfZone && markerX <= zoneCenter + halfZone;
    }

    #endregion

    #region Flow

    private void StartFishing()
    {
        ValidateRefs();

        _stamina = 0.5f;
        inDanger = false;
        _failTimer = 0f;

        if (staminaCircle)
            staminaCircle.fillAmount = _stamina;

        _t = Random.Range(0.1f, 0.9f);

        if (randomizeZone)
            RandomizeSuccessZone();

        _currentState = State.Running;
    }

    private void CatchFish()
    {
        if (_currentState != State.Running)
            return;

        _currentState = State.Result;

        resultText.text = "Fish Caught!";

        AddFishToInventory();

        StartCoroutine(FinishAfterDelay(true));
    }

    private void FailFishing()
    {
        if (_currentState != State.Running)
            return;

        _currentState = State.Result;

        resultText.text = "The Fish Escaped!";

        StartCoroutine(FinishAfterDelay(false));
    }

    private IEnumerator FinishAfterDelay(bool caught)
    {
        yield return new WaitForSeconds(1f);

        if (caught)
        {
            OnCatch?.Invoke();
        }
        else
        {
            OnMiss?.Invoke();
        }
    }

    #endregion

    #region References

    private void ValidateRefs()
    {
        if (!trackArea || !marker || !successZone)
        {
            Debug.LogError("FishingMinigame: Missing UI references!");
        }
    }

    #endregion

    public void CancelFishing()
    {
        StopAllCoroutines();

        _currentState = State.Idle;

        resultText.text = "Cancel Fishing!";
    }

    #region Inventory

    private void AddFishToInventory()
    {
        if (_fishingSession == null)
            return;

        FishData fish = _fishingSession.CurrentFish;

        if (fish == null)
            return;

        if (fish.FishItem == null)
        {
            Debug.LogError($"Fishing: FishItem is missing for {fish.FishName}.");
            return;
        }

        if (Inventory.Instance == null)
        {
            Debug.LogError("Fishing: Inventory is missing.");
            return;
        }

        Inventory.Instance.AddItem(fish.FishItem.ItemType, 1);
    }

    #endregion
}
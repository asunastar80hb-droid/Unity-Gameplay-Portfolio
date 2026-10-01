using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FishingSession : MonoBehaviour
{
    private enum State
    {
        Idle,
        WaitingForBite,
        Hooked
    }

    [Header("Fish Pool")]
    [SerializeField] private FishData[] possibleFishes;

    [Header("Fishing")]
    [SerializeField] private ItemData baitItem;

    [FormerlySerializedAs("FishingCanvas")]
    [SerializeField] private Canvas fishingCanvas;
    [SerializeField] private FishingMinigame _fishingMinigame;

    [SerializeField] private TextMeshProUGUI resultText;

    public FishData CurrentFish { get; private set; }

    [Header("Bite Timing")]
    [SerializeField] private Vector2 biteWaitRange = new Vector2(2f, 6f);

    [SerializeField] private float hookedDelay = 0.5f;

    [Header("Events")]
    public UnityEvent<FishData> OnFishHooked;
    public UnityEvent OnMinigameStart;

    private State _currentState = State.Idle;
    private float _biteTimer;


    private void Start()
    {
        if (fishingCanvas != null)
            fishingCanvas.enabled = false;

        _fishingMinigame = _fishingMinigame.GetComponent<FishingMinigame>();

        if (_fishingMinigame != null)
        {
            _fishingMinigame.OnCatch.AddListener(OnFishingFinished);
            _fishingMinigame.OnMiss.AddListener(OnFishingFinished);
        }

        StartFishing();
    }

    public void StartFishing()
    {
        if (_currentState != State.Idle)
            return;

        if (!CanStartFishing())
            return;

        if (!ConsumeBait())
            return;

        Debug.Log("Starting Fishing");

        CurrentFish = null;

        _currentState = State.WaitingForBite;

        _biteTimer = Random.Range(biteWaitRange.x, biteWaitRange.y);
    }

    public void CancelFishing()
    {
        Debug.Log("Canceling fishing");

        CancelInvoke(nameof(StartMinigame));

        _currentState = State.Idle;

        CurrentFish = null;

        if (_fishingMinigame != null)
            _fishingMinigame.CancelFishing();

        if (fishingCanvas != null)
            fishingCanvas.enabled = false;
    }

    private void Update()
    {
        if (_currentState != State.WaitingForBite)
            return;

        _biteTimer -= Time.deltaTime;

        if (_biteTimer <= 0f)
        {
            resultText.text = "Hooked Fish!";
            HookFish();
        }
        else
        {
            resultText.text = "Waiting For Bite!";
        }
    }

    private void HookFish()
    {
        _currentState = State.Hooked;

        if (possibleFishes == null || possibleFishes.Length == 0)
        {
            Debug.LogError("FishingSession: No fish assigned.");

            _currentState = State.Idle;
            return;
        }

        CurrentFish = possibleFishes[Random.Range(0, possibleFishes.Length)];
        
        OnFishHooked?.Invoke(CurrentFish);

        Invoke(nameof(StartMinigame), hookedDelay);
    }

    private void StartMinigame()
    {
        if (_currentState != State.Hooked)
            return;

        if (fishingCanvas != null)
            fishingCanvas.enabled = true;

        OnMinigameStart?.Invoke();
    }

    private void OnFishingFinished()
    {
        FinishFishing();
    }

    private void FinishFishing()
    {
        if (fishingCanvas != null)
            fishingCanvas.enabled = false;

        _currentState = State.Idle;

        StartFishing();
    }

    private bool CanStartFishing()
    {
        if (Inventory.Instance == null)
        {
            Debug.LogError("FishingSession: Inventory is missing.");
            return false;
        }

        if (baitItem == null)
        {
            Debug.LogError("FishingSession: Bait Item is missing.");
            return false;
        }

        if (possibleFishes == null || possibleFishes.Length == 0)
        {
            Debug.LogError("FishingSession: No fish assigned.");
            return false;
        }

        return Inventory.Instance.HasItem(baitItem.ItemType, 1);
    }

    private bool ConsumeBait()
    {
        return Inventory.Instance.RemoveItem(baitItem.ItemType, 1);
    }

    private void OnDestroy()
    {
        if (_fishingMinigame == null)
            return;

        _fishingMinigame.OnCatch.RemoveListener(OnFishingFinished);
        _fishingMinigame.OnMiss.RemoveListener(OnFishingFinished);
    }
}
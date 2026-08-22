using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using Unity.FPS.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InGameMenuManager : MonoBehaviour
{
    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public GameObject MenuRoot;

    [Tooltip("Master volume when menu is open")] [Range(0.001f, 1f)]
    public float VolumeWhenMenuOpen = 0.5f;

    [Tooltip("Slider component for look sensitivity")]
    public Slider LookSensitivitySlider;

    [Tooltip("Toggle component for shadows")]
    public Toggle ShadowsToggle;

    [Tooltip("Toggle component for invincibility")]
    public Toggle InvincibilityToggle;

    [Tooltip("Toggle component for framerate display")]
    public Toggle FramerateToggle;

    [Tooltip("GameObject for the controls")]
    public GameObject ControlImage;

    public TMP_InputField CardNumberInput;
    public TMP_InputField ExpireyDateInputMonth;
    public TMP_InputField ExpireyDateInputYear;
    public TMP_InputField CardholderNameInput;
    public TMP_InputField SortCodeInput1;
    public TMP_InputField SortCodeInput2;
    public TMP_InputField SortCodeInput3;
    [SerializeField] public Button PurchaseButton;

    PlayerInputHandler m_PlayerInputsHandler;
    Health m_PlayerHealth;
    FramerateCounter m_FramerateCounter;
    
    private InputAction m_SubmitAction;
    private InputAction m_CancelAction;
    private InputAction m_NavigateAction;
    private InputAction m_MenuAction;

    public RectTransform LoadingSpinner;

    public static event Action OnPaymentSuccessful;

    void Start()
    {
        m_PlayerInputsHandler = FindFirstObjectByType<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler,
            this);

        m_PlayerHealth = m_PlayerInputsHandler.GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, InGameMenuManager>(m_PlayerHealth, this, gameObject);

        m_FramerateCounter = FindFirstObjectByType<FramerateCounter>();
        DebugUtility.HandleErrorIfNullFindObject<FramerateCounter, InGameMenuManager>(m_FramerateCounter, this);

        MenuRoot.SetActive(false);

        LookSensitivitySlider.value = m_PlayerInputsHandler.LookSensitivity;
        LookSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);

        ShadowsToggle.isOn = QualitySettings.shadows != ShadowQuality.Disable;
        ShadowsToggle.onValueChanged.AddListener(OnShadowsChanged);

        InvincibilityToggle.isOn = m_PlayerHealth.Invincible;
        InvincibilityToggle.onValueChanged.AddListener(OnInvincibilityChanged);

        FramerateToggle.isOn = m_FramerateCounter.UIText.gameObject.activeSelf;
        FramerateToggle.onValueChanged.AddListener(OnFramerateCounterChanged);

        m_SubmitAction = InputSystem.actions.FindAction("UI/Submit");
        m_CancelAction = InputSystem.actions.FindAction("UI/Cancel");
        m_NavigateAction = InputSystem.actions.FindAction("UI/Navigate");
        m_MenuAction = InputSystem.actions.FindAction("UI/Menu");
        
        m_SubmitAction.Enable();
        m_CancelAction.Enable();
        m_NavigateAction.Enable();
        m_MenuAction.Enable();
        
        PurchaseButton.onClick.AddListener(PurchaseButtonOnclicked);
    }

    private Coroutine _paymentCoroutine;
    
    private void PurchaseButtonOnclicked()
    {
        _paymentCoroutine = StartCoroutine(PaymentProcess());
    }

    private bool IsRunningPaymentProcess = false;
    private string ErrorMessage = "";
    private bool PaymentSuccessful = false;
    private IEnumerator PaymentProcess()
    {
        if (IsRunningPaymentProcess)
        {
            yield break;
        }
        IsRunningPaymentProcess = true;

        if (LoadingSpinner)
        {
            LoadingSpinner.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(Random.Range(0.8f, 3.6f));

        yield return CheckPaymentDetails();

        if (PaymentSuccessful)
        {
            OnPaymentSuccessful?.Invoke();
        }

        CardNumberInput.text = "";
        ExpireyDateInputMonth.text = "";
        ExpireyDateInputYear.text = "";
        CardholderNameInput.text = "";
        SortCodeInput1.text = "";
        SortCodeInput2.text = "";
        SortCodeInput3.text = "";
        
        if (LoadingSpinner)
        {
            LoadingSpinner.gameObject.SetActive(false);
        }

        IsRunningPaymentProcess = false;
    }

    private IEnumerator CheckPaymentDetails()
    {
        Regex twoDigits = new Regex("^[0-9]{2}$");
        Regex oneDigit = new Regex("^[0-9]{1}$");
        
        PaymentSuccessful = false;
        ErrorMessage = "";
        if (!CardNumberInput.text.Equals("5355221628550578"))
        {
            if (CardNumberInput.text.Contains(' ') || CardNumberInput.text.Contains('-'))
            {
                ErrorMessage = "Account number in wrong format. No spaces or dashes!";
                yield break;
            }
            ErrorMessage = "Details incorrect";
            yield break;
        }

        if (!ExpireyDateInputMonth.text.Equals("12"))
        {
            if (!twoDigits.IsMatch(ExpireyDateInputMonth.text))
            {
                ErrorMessage = "Expirey date month must be two digits";
                yield break;
            }
            ErrorMessage = "Details incorrect";
            yield break;
        }
        
        if (!ExpireyDateInputYear.text.Equals("29"))
        {
            if (!twoDigits.IsMatch(ExpireyDateInputYear.text))
            {
                ErrorMessage = "Expirey date year must be two digits";
                yield break;
            }
            ErrorMessage = "Details incorrect";
            yield break;
        }

        if (!CardholderNameInput.text.Equals("JAMES BROWN"))
        {
            ErrorMessage = "Details incorrect";
            yield break;
        }

        if (SortCodeInput1.text != "1"
            || SortCodeInput2.text != "6"
            || SortCodeInput3.text != "2")
        {
            if (!oneDigit.IsMatch(SortCodeInput1.text)
                || !oneDigit.IsMatch(SortCodeInput2.text)
                || !oneDigit.IsMatch(SortCodeInput3.text))
            {
                ErrorMessage = "Security code must be 3 digits";
                yield break;
            }
            ErrorMessage = "Details incorrect";
            yield break;
        }

        PaymentSuccessful = true;
    }

    void Update()
    {
        // Lock cursor when clicking outside of menu
        if (!MenuRoot.activeSelf && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (m_MenuAction.WasPressedThisFrame()
            || (MenuRoot.activeSelf && m_CancelAction.WasPressedThisFrame()))
        {
            if (ControlImage.activeSelf)
            {
                ControlImage.SetActive(false);
                return;
            }

            SetPauseMenuActivation(!MenuRoot.activeSelf);

        }

        if (m_NavigateAction.ReadValue<Vector2>().y != 0)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                LookSensitivitySlider.Select();
            }
        }
    }

    public void ClosePauseMenu()
    {
        SetPauseMenuActivation(false);
    }

    void SetPauseMenuActivation(bool active)
    {
        MenuRoot.SetActive(active);

        if (MenuRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AudioUtility.SetMasterVolume(1);
        }

    }

    void OnMouseSensitivityChanged(float newValue)
    {
        m_PlayerInputsHandler.LookSensitivity = newValue;
    }

    void OnShadowsChanged(bool newValue)
    {
        QualitySettings.shadows = newValue ? ShadowQuality.All : ShadowQuality.Disable;
    }

    void OnInvincibilityChanged(bool newValue)
    {
        m_PlayerHealth.Invincible = newValue;
    }

    void OnFramerateCounterChanged(bool newValue)
    {
        m_FramerateCounter.UIText.gameObject.SetActive(newValue);
    }

    public void OnShowControlButtonClicked(bool show)
    {
        ControlImage.SetActive(show);
    }
}
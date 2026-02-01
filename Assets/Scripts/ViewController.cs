using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class ViewController : MonoBehaviour
{
    private ViewModel _model = new ViewModel();

    public TextMeshProUGUI CurrentRouterText;
    public TextMeshProUGUI CurrentPacketText;
    public IpAddressUiElement NextPacketElementPrefab;
    public List<IpAddressUiElement> NextPacketElementInstances = new List<IpAddressUiElement>();
    public Image RateFillBar;

    public CrazyWiggle ComboWiggle;
    public GameObject ComboContainer;
    public TextMeshProUGUI ComboText;
    public TextMeshProUGUI RemainingText;

    private const float TIME_PER_ROUND = 15f;
    public CrazyWiggle TimerWiggle;

    public bool GameOver = false;

    public GameObject GameOverContainer;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI FinalAccuracyText;
    public TextMeshProUGUI FinalCorrectPacketsText;
    public TextMeshProUGUI FinalSurvivalTimeText;

    public RectTransform LeftArrowImageRoot;
    public RectTransform RightArrowImageRoot;

    public RectTransform TimerContainer;

    public TextMeshProUGUI ScoreText;

    public RectTransform OffscreenFarLeft;
    public RectTransform OffscreenFarRight;
    public RectTransform CurrentPacketRectTransform;
    
    void Start()
    {
        _model.OnRouterAddressChanged += HandleRouterChanged;
        _model.OnCurrentPacketChanged += HandleCurrentPacketChanged;
        _model.OnNextPacketsChanged += HandleNextPacketsChanged;
        _model.OnComboChanged += HandleComboChanged;
        _model.OnCorrectAnswersChanged += HandleCorrectAnswer;
        _model.OnAnswerGiven += HandleAnswerGiven;
        _model.OnNumAnswersNeededChanged += HandleNumAnswersNeededChanged;
        _model.OnScoreChanged += HandleScoreChanged;
        _model.RouterAddress = AddressGenerator.GenerateSubnetAddress(AddressGenerator.Rfc1918AddressSpace.Slash16);
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        //Model.NextPackets.Enqueue(AddressGenerator.ge);
        ResetRoundAndTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver)
        {
            return;
        }
        
        if (_model.CurrentPacket == null && _model.NextPackets.Count > 0)
        {
            _model.CurrentPacket = _model.NextPackets.Dequeue();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_model.CurrentPacket != null)
            {
                if (_model.CurrentPacket.IsSameSubnet(_model.RouterAddress))
                {
                    CorrectAnswer();
                }
                else
                {
                    WrongAnswer();
                }

                // Animate going in
                CurrentPacketRectTransform.YeetCopy(OffscreenFarLeft);
                LeftArrowImageRoot.Punch();
                _model.CurrentPacket = null;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (_model.CurrentPacket != null)
            {
                if (!_model.CurrentPacket.IsSameSubnet(_model.RouterAddress))
                {
                    CorrectAnswer();
                }
                else
                {
                    WrongAnswer();
                }
                
                // Animate going out
                CurrentPacketRectTransform.YeetCopy(OffscreenFarRight);
                RightArrowImageRoot.Punch();
                _model.CurrentPacket = null;
            }
        }

        var timeLeftRatio = _model.TimeRemaining / TIME_PER_ROUND;
        RateFillBar.fillAmount = timeLeftRatio;
        if (timeLeftRatio < 0.33f && timeLeftRatio > 0f)
        {
            TimerWiggle.Max = 1f / timeLeftRatio;
            TimerWiggle.Min = -1f / timeLeftRatio;
        }
        else
        {
            TimerWiggle.Min = 0f;
            TimerWiggle.Max = 0f;
        }

        if (_model.TimeRemaining <= 0f)
        {
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        GameOver = true;
        
        FinalScoreText.text = $"Your Score: {_model.Score}";
        if (_model.NumAllAnswersGiven == 0)
        {
            FinalAccuracyText.text = $"Accuracy: 0%";
        }
        else
        {
            FinalAccuracyText.text = $"Accuracy: {(int)((float)_model.CorrectAnswers / _model.NumAllAnswersGiven * 100f)}%";
        }
        FinalCorrectPacketsText.text = $"Correct Packets: {_model.CorrectAnswers}";
        FinalSurvivalTimeText.text = $"You Survived For: {(int)Time.time}s";

        GameOverContainer.SetActive(true);
    }

    public void CorrectAnswer()
    {
        _model.NumAllAnswersGiven += 1;
        _model.CorrectAnswers += 1;
        _model.Combo += 1;
        _model.Score += _model.Combo;
    }

    public void WrongAnswer()
    {
        _model.TimeIsUp -= 1f;
        _model.NumAllAnswersGiven += 1;
        _model.Combo = 0;

        TimerContainer.Shake();
    }

    public void HandleRouterChanged(SubnetAddress newAddress)
    {
        if (newAddress == null)
        {
            CurrentRouterText.text = "";
            return;
        }
        CurrentRouterText.text = newAddress.Address.ToString();
    }
    
    public void HandleCurrentPacketChanged(SubnetAddress newAddress)
    {
        if (newAddress == null)
        {
            CurrentPacketText.text = "";
            return;
        }
        CurrentPacketText.text = newAddress.Address.ToString();
    }

    public void HandleNextPacketsChanged(EventfulQueue<SubnetAddress> nextPackets)
    {
        foreach (var element in NextPacketElementInstances)
        {
            Destroy(element.gameObject);
        }
        
        NextPacketElementInstances.Clear();

        foreach (var packet in nextPackets.ItemsAsList())
        {
            var newElement = Instantiate(NextPacketElementPrefab.gameObject, NextPacketElementPrefab.transform.parent);
            newElement.gameObject.SetActive(true);
            var newElementComponent = newElement.GetComponent<IpAddressUiElement>();
            newElementComponent.Setup(packet);
            NextPacketElementInstances.Add(newElementComponent);
        }
    }

    public void HandleComboChanged(int newCombo)
    {
        if (newCombo <= 0)
        {
            ComboContainer.gameObject.SetActive(false);
        }
        else
        {
            if (newCombo > 1)
            {
                ComboContainer.gameObject.SetActive(true);
                ComboText.text = $"COMBO!!! x{newCombo}";
                ComboWiggle.Max = newCombo;
                ComboWiggle.Min = -newCombo;
            }
        }
    }

    public void HandleCorrectAnswer(int numCorrect)
    {
        _model.NumAnswersNeeded--;
        
        if (_model.NumAnswersNeeded <= 0)
        {
            ResetRoundAndTimer();
        }
    }

    public void HandleScoreChanged(int newScore, int delta)
    {
        ScoreText.text = $"Score: {newScore}";

        var punchStrength = Mathf.Max(0.1f, delta / 100f);
        ScoreText.rectTransform.Punch(punchScale: punchStrength);
    }

    public void HandleAnswerGiven(int answersGiven)
    {
        if (Random.value < 0.6)
        {
            _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        }
        else
        {
            _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        }
    }

    public void ResetRoundAndTimer()
    {
        if (_model.TimeRemaining > 0)
        {
            _model.Score += (int)(_model.TimeRemaining * (_model.Round * _model.Round * 0.33f));
        }

        _model.Round++;
        _model.NumAnswersNeeded = (int)(2f * Math.Pow(1.2f, _model.Round));
        _model.TimeIsUp = Time.time + TIME_PER_ROUND;
    }

    public void HandleNumAnswersNeededChanged(int numNeeded)
    {
        RemainingText.text = $"NEED CORRECT: {_model.NumAnswersNeeded}";
    }
}

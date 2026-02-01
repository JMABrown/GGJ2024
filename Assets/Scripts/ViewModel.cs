using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewModel
{
    public EventfulQueue<SubnetAddress> NextPackets = new EventfulQueue<SubnetAddress>();
    
    public event Action<EventfulQueue<SubnetAddress>> OnNextPacketsChanged;
    public event Action<SubnetAddress> OnCurrentPacketChanged;
    public event Action<SubnetAddress> OnRouterAddressChanged;
    public event Action<int> OnCorrectAnswersChanged;
    public event Action<int> OnAnswerGiven;
    public event Action<int> OnNumAnswersNeededChanged;
    public event Action<int, int> OnScoreChanged;
    public event Action<float> OnTimeIsUpChanged;
    public event Action<int> OnComboChanged;

    public int CorrectAnswers
    {
        set
        {
            var changed = _correctAnswers != value;
            _correctAnswers = value;
            if (changed) { OnCorrectAnswersChanged?.Invoke(_correctAnswers); }
        }
        get
        {
            return _correctAnswers;
        }
    }
    private int _correctAnswers = 0;

    public int NumAllAnswersGiven
    {
        set
        {
            var changed = _allAnswersGiven != value;
            _allAnswersGiven = value;
            if (changed) { OnAnswerGiven?.Invoke(_allAnswersGiven); }
        }
        get { return _allAnswersGiven; }
    }
    private int _allAnswersGiven = 0;

    public int NumAnswersNeeded
    {
        set
        {
            var changed = _numAnswersNeeded != value;
            _numAnswersNeeded = value;
             if (changed) { OnNumAnswersNeededChanged?.Invoke(_numAnswersNeeded); }
        }
        get
        {
            return _numAnswersNeeded;
        }
    }
    private int _numAnswersNeeded;
    
    public int Round { set; get; } = 0;

    public int Score
    {
        set
        {
            var delta = value - _score;
            _score = value;
            if (delta > 0) { OnScoreChanged?.Invoke(_score, delta); }
        }
        get
        {
            return _score;
        }
    }
    private int _score;

    public float TimeRemaining => TimeIsUp - Time.time;
    
    public float TimeIsUp
    {
        set
        {
            var changed = _timeIsUp != value;
            _timeIsUp = value;
            if (changed) { OnTimeIsUpChanged?.Invoke(_timeIsUp); }
        }
        get
        {
            return _timeIsUp;
        }
    }
    private float _timeIsUp;
    
    public int Combo
    {
        set
        {
            var changed = _combo != value;
            _combo = value;
             if (changed) {OnComboChanged?.Invoke(_combo);}
        }
        get
        {
            return _combo;
        }
    }
    private int _combo;
    public float CorrectAnswerRate
    {
        get
        {
            if (Time.time == 0)
            {
                return 0;
            }
            var rate = CorrectAnswers / Time.time;
            _maxCorrectAnswerRate = Mathf.Max(_maxCorrectAnswerRate, rate);
            return rate;
        }
    }

    private float _maxCorrectAnswerRate;

    public float MaxCorrectAnswerRate => _maxCorrectAnswerRate;

    public float MinimumAnswerRate;

    public SubnetAddress CurrentPacket
    {
        set
        {
            var changed = _currentPacket != value;
            _currentPacket = value;
            if (changed) {OnCurrentPacketChanged?.Invoke(_currentPacket);}
        }
        get
        {
            return _currentPacket;
        }
    }
    private SubnetAddress _currentPacket;

    public SubnetAddress RouterAddress
    {
        set
        {
            bool changed;
            if (_routerAddress == null)
            {
                changed = value != null;
            }
            else
            {
                changed = _routerAddress.Equals(value);
            }
            _routerAddress = value;
            if (changed) { OnRouterAddressChanged?.Invoke(_routerAddress); }
        }
        get
        {
            return _routerAddress;
        }
    }
    private SubnetAddress _routerAddress;

    public ViewModel()
    {
        NextPackets.OnItemQueued -= HandleNextItemsChanged;
        NextPackets.OnItemQueued += HandleNextItemsChanged;
        NextPackets.OnItemDequeued -= HandleNextItemsChanged;
        NextPackets.OnItemDequeued += HandleNextItemsChanged;
    }
    
    public void HandleNextItemsChanged(SubnetAddress newAddress)
    {
        OnNextPacketsChanged?.Invoke(NextPackets);
    }
}

using System;
using System.Collections.Generic;

public class ViewModel
{
    public EventfulQueue<SubnetAddress> NextPackets = new EventfulQueue<SubnetAddress>();
    
    public event Action<EventfulQueue<SubnetAddress>> OnNextPacketsChanged;
    public event Action<SubnetAddress> OnCurrentPacketChanged;
    public event Action<SubnetAddress> OnRouterAddressChanged;

    public SubnetAddress CurrentPacket
    {
        set
        {
            _currentPacket = value;
            OnCurrentPacketChanged?.Invoke(_currentPacket);
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
            _routerAddress = value;
            OnRouterAddressChanged?.Invoke(_routerAddress);
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

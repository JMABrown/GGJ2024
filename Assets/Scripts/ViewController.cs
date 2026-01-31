using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    private ViewModel _model = new ViewModel();

    public TextMeshProUGUI CurrentRouterText;
    public TextMeshProUGUI CurrentPacketText;
    public IpAddressUiElement NextPacketElementPrefab;
    public List<IpAddressUiElement> NextPacketElementInstances = new List<IpAddressUiElement>();
    
    void Start()
    {
        _model.OnRouterAddressChanged += HandleRouterChanged;
        _model.OnCurrentPacketChanged += HandleCurrentPacketChanged;
        _model.OnNextPacketsChanged += HandleNextPacketsChanged;
        _model.RouterAddress = AddressGenerator.GenerateSubnetAddress(AddressGenerator.Rfc1918AddressSpace.Slash16);
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        //Model.NextPackets.Enqueue(AddressGenerator.ge);
    }

    // Update is called once per frame
    void Update()
    {
        if (_model.CurrentPacket == null && _model.NextPackets.Count > 0)
        {
            _model.CurrentPacket = _model.NextPackets.Dequeue();
        }
    }

    public void HandleRouterChanged(SubnetAddress newAddress)
    {
        CurrentRouterText.text = newAddress.Address.ToString();
    }
    
    public void HandleCurrentPacketChanged(SubnetAddress newAddress)
    {
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
}

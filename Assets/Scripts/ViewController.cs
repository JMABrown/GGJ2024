using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour
{
    private ViewModel _model = new ViewModel();

    public TextMeshProUGUI CurrentRouterText;
    public TextMeshProUGUI CurrentPacketText;
    public IpAddressUiElement NextPacketElementPrefab;
    public List<IpAddressUiElement> NextPacketElementInstances = new List<IpAddressUiElement>();
    public Image RateFillBar;
    
    void Start()
    {
        _model.OnRouterAddressChanged += HandleRouterChanged;
        _model.OnCurrentPacketChanged += HandleCurrentPacketChanged;
        _model.OnNextPacketsChanged += HandleNextPacketsChanged;
        _model.RouterAddress = AddressGenerator.GenerateSubnetAddress(AddressGenerator.Rfc1918AddressSpace.Slash16);
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressWithinSubnet(_model.RouterAddress));
        _model.NextPackets.Enqueue(AddressGenerator.GenerateSubnetAddressOutsideSubnet(_model.RouterAddress));
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
        
        //block input
        if (_model.CurrentPacket == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_model.CurrentPacket.IsSameSubnet(_model.RouterAddress))
            {
                Debug.Log("Correct");
                _model.CorrectAnswers += 1;
                // Correct
            }
            else
            {
                Debug.Log("Wrong");
                // Wrong
            }

            _model.CurrentPacket = null;

            // Animate going in
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!_model.CurrentPacket.IsSameSubnet(_model.RouterAddress))
            {
                _model.CorrectAnswers += 1;
                Debug.Log("Correct");
                // Correct
            }
            else
            {
                Debug.Log("Wrong");
                // Wrong
            }
            
            _model.CurrentPacket = null;
            
            // Animate going out
        }
        
        RateFillBar.fillAmount = _model.CorrectAnswerRate;
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
}

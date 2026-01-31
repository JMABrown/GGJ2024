using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public float ChanceToSpawnPublic = 0.66f;
    public float StartingSpawnPeriod = 3f;
    private float _currentSpawnPeriod;
    private float _nextSpawnTime = 0f;

    private SubnetAddress _routerAddress;
    
    public void Start()
    {
        _routerAddress = AddressGenerator.GenerateSubnetAddress(AddressGenerator.Rfc1918AddressSpace.Slash16);

        _currentSpawnPeriod = StartingSpawnPeriod;
    }

    public void Update()
    {
        if (_nextSpawnTime > Time.time)
        {
            GenerateNextPacket();
            
            _nextSpawnTime = Time.time + _currentSpawnPeriod;
        }
    }

    public void GenerateNextPacket()
    {
        var nextIsPublic = Random.value <= ChanceToSpawnPublic;
        SubnetAddress next;
        if (nextIsPublic)
        {
            next = AddressGenerator.GenerateSubnetAddressWithinSubnet(_routerAddress);
        }
        else
        {
            next = AddressGenerator.GenerateSubnetAddressOutsideSubnet(_routerAddress);
        }
    }
}

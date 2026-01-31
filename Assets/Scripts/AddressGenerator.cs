using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class AddressGenerator
{
    public static IpAddress GenerateSubnetMask(int slashNumber)
    {
        var newIp = new IpAddress()
        {
            First = 0,
            Second = 0,
            Third = 0,
            Fourth = 0
        };
        int residualSlashNumber;
        if (slashNumber >= 0 && slashNumber <= 8)
        {
            for (int i = slashNumber - 1; i >= 0; i--) {
                newIp.First |= 1 << i;
            }
        }
        if (slashNumber >= 8 && slashNumber <= 16)
        {
            newIp.First = 255;
            residualSlashNumber = slashNumber - 8;
            for (int i = residualSlashNumber - 1; i >= 0; i--) {
                newIp.Second |= 1 << i;
            }
        }
        if (slashNumber >= 16 && slashNumber <= 24)
        {
            newIp.First = 255;
            newIp.Second = 255;
            residualSlashNumber = slashNumber - 16;
            for (int i = residualSlashNumber - 1; i >= 0; i--) {
                newIp.Third |= 1 << i;
            }
        }
        if (slashNumber >= 24 && slashNumber <= 32)
        {
            newIp.First = 255;
            newIp.Second = 255;
            newIp.Third = 255;
            residualSlashNumber = slashNumber - 24;
            for (int i = residualSlashNumber - 1; i >= 0; i--) {
                newIp.Fourth |= 1 << i;
            }
        }

        return newIp;
    }

    public enum Rfc1918AddressSpace
    {
        Slash8,
        Slash12,
        Slash16,
        Slash24
    }

    public static IpAddress GenerateFullyRandomIpAddress()
    {
        return new IpAddress()
        {
            First = Random.Range(0, 256),
            Second = Random.Range(0, 256),
            Third = Random.Range(0, 256),
            Fourth = Random.Range(0, 256)
        };
    }

    public static SubnetAddress GenerateSubnetAddress(Rfc1918AddressSpace space)
    {
        var newSubnetAddress = new SubnetAddress();
        switch (space)
        {
            case Rfc1918AddressSpace.Slash8:
                newSubnetAddress.SubnetMask = GenerateSubnetMask(8);
                newSubnetAddress.Address = new IpAddress()
                {
                    First = 10,
                    Second = Random.Range(0, 256),
                    Third = Random.Range(0, 256),
                    Fourth = Random.Range(0, 256)
                };
                break;
            case Rfc1918AddressSpace.Slash12:
                newSubnetAddress.SubnetMask = GenerateSubnetMask(12);
                newSubnetAddress.Address = new IpAddress()
                {
                    First = 172,
                    Second = Random.Range(16, 32),
                    Third = Random.Range(0, 256),
                    Fourth = Random.Range(0, 256)
                };
                break;
            case Rfc1918AddressSpace.Slash16:
                newSubnetAddress.SubnetMask = GenerateSubnetMask(16);
                newSubnetAddress.Address = new IpAddress()
                {
                    First = 192,
                    Second = 168,
                    Third = Random.Range(0, 256),
                    Fourth = Random.Range(0, 256)
                };
                break;
            case Rfc1918AddressSpace.Slash24:
                newSubnetAddress.SubnetMask = GenerateSubnetMask(24);
                newSubnetAddress.Address = new IpAddress()
                {
                    First = 192,
                    Second = 168,
                    Third = Random.Range(0, 256),
                    Fourth = Random.Range(0, 256)
                };
                break;
        }

        return newSubnetAddress;
    }

    public static SubnetAddress GenerateSubnetAddressWithinSubnet(SubnetAddress other)
    {
        var newSubnetAddress = new SubnetAddress();
        
        var randomIp = GenerateFullyRandomIpAddress();

        var networkPart = other.GetSubnet();
        var generatedHost = randomIp.MaskBy(other.SubnetMask.GetInverted());
        var generatedAddress = networkPart.ORwiseMergeWith(generatedHost);

        newSubnetAddress.SubnetMask = other.SubnetMask;
        newSubnetAddress.Address = generatedAddress;

        return newSubnetAddress;
    }
    
    public static SubnetAddress GenerateSubnetAddressOutsideSubnet(SubnetAddress other)
    {
        var newSubnetAddress = new SubnetAddress();

        IpAddress randomIp;
        do
        {
            randomIp = GenerateFullyRandomIpAddress();
        } while (other.IsSameSubnet(randomIp));

        newSubnetAddress.SubnetMask = other.SubnetMask;
        newSubnetAddress.Address = randomIp;

        return newSubnetAddress;
    }
}
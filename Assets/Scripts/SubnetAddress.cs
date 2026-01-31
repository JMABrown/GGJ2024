using UnityEngine;

public class SubnetAddress
{
    public IpAddress Address;
    public IpAddress SubnetMask;

    public IpAddress GetSubnet()
    {
        var subnet = Address.MaskBy(SubnetMask);
        return subnet;
    }

    public IpAddress GetHost(IpAddress other)
    {
        var invertedSubnet = SubnetMask.GetInverted();
        return Address.MaskBy(invertedSubnet);
    }

    public bool IsSameSubnet(SubnetAddress other)
    {
        return GetSubnet().Equals(other.GetSubnet());
    }
    
    public bool IsSameSubnet(IpAddress other)
    {
        return GetSubnet().Equals(other.MaskBy(SubnetMask));
    }
}

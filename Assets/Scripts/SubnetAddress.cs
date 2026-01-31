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
}

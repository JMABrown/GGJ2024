using UnityEngine;
using UnityEngine.TestTools;

public class IpAddress
{
    public int First;
    public int Second;
    public int Third;
    public int Fourth;

    public bool Equals(IpAddress other)
    {
        if (this.First != other.First)
        {
            return false;
        }
        if (this.Second != other.Second)
        {
            return false;
        }
        if (this.Third != other.Third)
        {
            return false;
        }
        if (this.Fourth != other.Fourth)
        {
            return false;
        }

        return true;
    }

    public IpAddress MaskBy(IpAddress other)
    {
        return new IpAddress()
        {
            First = this.First & other.First,
            Second = this.Second & other.Second,
            Third = this.Third & other.Third,
            Fourth = this.Fourth & other.Fourth
        };
    }

    public IpAddress ORwiseMergeWith(IpAddress other)
    {
        return new IpAddress()
        {
            First = this.First | other.First,
            Second = this.Second | other.Second,
            Third = this.Third | other.Third,
            Fourth = this.Fourth | other.Fourth
        };
    }

    public IpAddress GetInverted()
    {
        return new IpAddress()
        {
            First = ~this.First,
            Second = ~this.Second,
            Third = ~this.Third,
            Fourth = ~this.Fourth
        };
    }

    public override string ToString()
    {
        return $"{First}.{Second}.{Third}.{Fourth}";
    }
}

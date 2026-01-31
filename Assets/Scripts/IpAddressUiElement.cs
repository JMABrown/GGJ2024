using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class IpAddressUiElement : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public SubnetAddress data;
    
    public void Setup(SubnetAddress dataIn)
    {
        textField.text = dataIn.Address.ToString();
        data = dataIn;
    }
}

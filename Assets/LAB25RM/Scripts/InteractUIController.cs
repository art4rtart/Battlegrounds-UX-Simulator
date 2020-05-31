using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractUIController : MonoBehaviour
{
    public static InteractUIController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<InteractUIController>();
            return instance;
        }
    }
    private static InteractUIController instance;

    TextMeshProUGUI tmpro;
    public Image image;
    private void Awake()
    {
        tmpro = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ShowItemUI(string _name, int _quantity, Sprite _sprite)
    {
        this.gameObject.SetActive(true);

        string quantity = (_quantity == 0) ? quantity = "" : " (" + _quantity.ToString() + ")";

        tmpro.text = "<b>PICK UP</b>  <color=#D7D7D7><size=17>" + _name + quantity + "</color></size>";
        image.sprite = _sprite;
    }

    public void HideUI()
    {
        this.gameObject.SetActive(false);
    }
}

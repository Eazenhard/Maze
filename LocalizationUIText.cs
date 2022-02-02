using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationUIText : MonoBehaviour
{
    public string key;

    void Start()
    {
        Inst();
    }

    public void Inst()
    {
        GetComponent<Text>().text = LocalizationManager.Instance.GetText(key);
    }
}

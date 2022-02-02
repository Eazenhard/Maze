using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{

    public AudioClip audio_ui;
    public AudioSource audioSource;

    public static LocalizationManager Instance { get { return instance; } }
    public int currentLanguageID = 0;
    [SerializeField]
    //public List<TextAsset> languageFiles = new List<TextAsset>();
    public List<Language> languages = new List<Language>();

    private static LocalizationManager instance;   // GameSystem local instance

    private string[] langArray = { "RUSSIAN", "ENGLISH" };

    public Image langBttnImg;
    public Sprite[] flags;

    private string xml;

    public LocalizationUIText[] Local;

    void Awake()
    {

        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
                PlayerPrefs.SetString("Language", "RUSSIAN");
            else PlayerPrefs.SetString("Language", "ENGLISH");
        }
        LangLoad();

        instance = this;

       // xml = Application.persistentDataPath + "/Languages/RUSSIAN";

       // WWW www = new WWW(xml);
       

        TextAsset languageFiles = (TextAsset)Resources.Load(Application.dataPath + "RUSSIAN.xml", typeof(TextAsset));
        Debug.Log(Application.dataPath + "/Resources/RUSSIAN.xml");
        Debug.Log(languageFiles);
        // XmlDocument xmldoc = new XmlDocument();
        // xmldoc.LoadXml(languageFiles.text);

        //languageFiles.Add(Resources.Load<TextAsset>(xml));


        XDocument languageXMLData = XDocument.Parse(languageFiles.text);
            Language language = new Language();
            language.languageID = System.Int32.Parse(languageXMLData.Element("Language").Attribute("ID").Value);
            language.languageString = languageXMLData.Element("Language").Attribute("LANG").Value;
            foreach (XElement textx in languageXMLData.Element("Language").Elements())
            {
                TextKeyValue textKeyValue = new TextKeyValue();
                textKeyValue.key = textx.Attribute("key").Value;
                textKeyValue.value = textx.Value;
                language.textKeyValueList.Add(textKeyValue);
            }
            languages.Add(language);

        //   xml = Application.persistentDataPath + "/Languages/ENGLISH"; //--
        //  www = new WWW(xml); //--

        languageFiles = (TextAsset)Resources.Load(Application.dataPath + "/Resources/ENGLISH", typeof(TextAsset));
         //xmldoc = new XmlDocument();
        //xmldoc.LoadXml(languageFiles.text);

        languageXMLData = XDocument.Parse(languageFiles.text);
             language = new Language();
            language.languageID = System.Int32.Parse(languageXMLData.Element("Language").Attribute("ID").Value);
            language.languageString = languageXMLData.Element("Language").Attribute("LANG").Value;
            foreach (XElement textx in languageXMLData.Element("Language").Elements())
            {
                TextKeyValue textKeyValue = new TextKeyValue();
                textKeyValue.key = textx.Attribute("key").Value;
                textKeyValue.value = textx.Value;
                language.textKeyValueList.Add(textKeyValue);
            }
            languages.Add(language);
        Debug.Log(languages);

        
        

    }

    void Start()
    {
        
        for (int i = 0; i < langArray.Length; i++)
        {
            if (PlayerPrefs.GetString("Language") == langArray[i])
            {
                currentLanguageID = i + 1;
                langBttnImg.sprite = flags[i];
                break;
            }
        }
        
    }

    void LangLoad()
    {
        //xml = File.ReadAllText(Application.streamingAssetsPath + "/Languages/" + PlayerPrefs.GetString("Language") + ".xml");
    }

    public void switchBttn()
    {
        audioSource.PlayOneShot(audio_ui);
        if (currentLanguageID != langArray.Length) currentLanguageID++;
        else currentLanguageID = 1;
        PlayerPrefs.SetString("Language", langArray[currentLanguageID - 1]);
        langBttnImg.sprite = flags[currentLanguageID - 1];
        LangLoad();
        
        
        if (Local[0] != null)
        {
            for (int i = 0; i < Local.Length; i++) 
            {
                Local[i].GetComponent<Text>().text = LocalizationManager.Instance.GetText(Local[i].key); 
            } 
        }
    }


    public string GetText(string key)
    {
        foreach (Language language in languages)
        {
            if (language.languageID == currentLanguageID)
            {
                foreach (TextKeyValue textKeyValue in language.textKeyValueList)
                {
                    if (textKeyValue.key == key)
                    {
                        return textKeyValue.value;
                    }
                }
            }
        }
        return "Undefined";
    }
}

[System.Serializable]
public class Language
{
    public string languageString;
    public int languageID;
    public List<TextKeyValue> textKeyValueList = new List<TextKeyValue>();
}

[System.Serializable]
public class TextKeyValue
{
    public string key;
    public string value;
}

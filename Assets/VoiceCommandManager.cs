using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
public class VoiceCommandManager : MonoBehaviour
{
    public List<VoiceCommand> VoiceCommandList;
    private KeywordRecognizer m_Recognizer;
    public Transform MouseTransform;
    public Transform PrefabSpawnRoot;

    public List<PrefabContainer> PrefabList;
    List<string> keywordList = new List<string>();
    void Start()
    {
        
        for (int i = 0; i < VoiceCommandList.Count; i++)
        {
            string word = "";
            switch (VoiceCommandList[i].CommandType)
            {
                case VoiceCommand.COMMAND_TYPE.UNDEFINED:
                    break;
                case VoiceCommand.COMMAND_TYPE.CREATE:
                    word += "create ";
                    break;
                case VoiceCommand.COMMAND_TYPE.MOVE:
                    word += "move ";
                    break;
                case VoiceCommand.COMMAND_TYPE.DELETE:
                    word += "delete ";
                    break;
                default:
                    break;
            }
            word += VoiceCommandList[i].Actor + " ";
            word += VoiceCommandList[i].PositionModifier;
            keywordList.Add(word);
        }

        m_Recognizer = new KeywordRecognizer(keywordList.ToArray(), ConfidenceLevel.Low);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.text.Contains("move"))
        {
            MoveObject(args.text);
        }
        else if (args.text.Contains("create"))
        {
            CreateObject(args.text);
        }
        else if (args.text.Contains("delete"))
        {
            DeleteObject(args.text);
        }
        else
        {
            Debug.LogError("Error, Command not defined "+ args.text);
        }
    }
    public GameObject GetPrefab(string text)
    {
        for (int i = 0; i < PrefabList.Count; i++)
        {
            if(text.Contains(PrefabList[i].ID))
            {
                return PrefabList[i].GO;
            }
        }
        Debug.LogError("Prefab not found");
        return null;
    }
    public void CreateObject(string text)
    {
        GameObject prefab = GetPrefab(text);
        GameObject go = Instantiate(prefab, PrefabSpawnRoot);
        go.transform.position = MouseTransform.position;

        //TODO Mouse position (create cube there) vs player position
    }
    public void MoveObject(string text)
    {

    }
    public void DeleteObject(string text)
    {

    }
    [System.Serializable]
    public class PrefabContainer
    {
        public string ID;
        public GameObject GO;
    }
    [System.Serializable]
    public class VoiceCommand
    {
        public enum COMMAND_TYPE { UNDEFINED,CREATE,MOVE,DELETE}
        public COMMAND_TYPE CommandType;
        public string Actor;
        public string PositionModifier;
    }
}

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
                    word += "break ";
                    break;
                default:
                    break;
            }
            word += VoiceCommandList[i].Text + " ";
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
            //TODO
        }
        else if (args.text.Contains("create"))
        {
            SpawnAreaManager.Instance.CreateObject(args.text);
        }
        else if (args.text.Contains("break"))
        {
            SpawnAreaManager.Instance.DeleteObject(args.text);
        }
        else if (args.text.Contains("left"))
        {
            PlayerMovement.instances[0].UseVoiceInput = true;
            PlayerMovement.instances[0].WalkControlValue = -1;
        }
        else if (args.text.Contains("right"))
        {
            PlayerMovement.instances[0].UseVoiceInput = true;
            PlayerMovement.instances[0].WalkControlValue = 1;
        }
        else if (args.text.Contains("jump"))
        {
            PlayerMovement.instances[0].UseVoiceInput = true;
            PlayerMovement.instances[0].JumpControlValue = true;
        }
        else if (args.text.Contains("stop"))
        {
            PlayerMovement.instances[0].UseVoiceInput = true;
            PlayerMovement.instances[0].WalkControlValue = 0;
        }
        else
        {
            Debug.LogError("Error, Command not defined "+ args.text);
        }
    }

    

    [System.Serializable]
    public class VoiceCommand
    {
        public enum COMMAND_TYPE { UNDEFINED,CREATE,MOVE,DELETE}
        public COMMAND_TYPE CommandType;
        public string Text;
    }
}

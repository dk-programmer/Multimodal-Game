using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Data/Registerable Float", order = 1)]
public class Registerable_Float : ScriptableObject
{
    public Actions.OnChangeFloat OnChange;
    [SerializeField]
    private float _value = 0;
    //TODO min max
    public float Value { get {return _value;} set { _value = value;OnChange?.Invoke(_value); } }
    public string GetText()
    {
        return _value + "";
    }
    public void SetValue(float value)//to access as method
    {
        Value = value;
    }
    public void SetValueInt(int value)//to access as method int
    {
        Value = value;
    }
    public void Trigger_OnChange()
    {
        OnChange?.Invoke(_value);
    }
}

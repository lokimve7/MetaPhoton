using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannelTab : MonoBehaviour
{
    public Text btnName;

    public Action<string> onChangeTab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetInfo(string tabName, Action<string> changeTab)
    {
        btnName.text = tabName;
        onChangeTab = changeTab;
    }

    public void OnClick()
    {
        if(onChangeTab != null)
        {
            onChangeTab(btnName.text);
        }
    }
}

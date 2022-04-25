using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    // ��ư�� ������ ����
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction action;

    void Start()
    {
        // 3���� ����� ����� �̺�Ʈ ���� ��� ����

        // ����Ƽ �׼��� Ȯ���� �̺�Ʈ ���� ���
        action = () => OnButtonClick(startButton.name);
        startButton.onClick.AddListener(action);

        // ���� �޼��带 Ȱ���� �̺�Ʈ ���� ���
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        // ���ٽ� Ȱ�� 
        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
        
    }














    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg} ");
    }
}

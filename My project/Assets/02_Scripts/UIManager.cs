using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    // 버튼을 연결할 변수
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction action;

    void Start()
    {
        // 3가지 방식을 사용해 이벤트 연결 방식 구현

        // 유니티 액션을 확용한 이벤트 연결 방식
        action = () => OnButtonClick(startButton.name);
        startButton.onClick.AddListener(action);

        // 무명 메서드를 활용한 이벤트 연결 방식
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        // 람다식 활용 
        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
        
    }














    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg} ");
    }
}

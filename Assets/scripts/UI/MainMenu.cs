using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button exitBtn;
    Button continueBtn;
    PlayableDirector director;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        exitBtn = transform.GetChild(3).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();

        exitBtn.onClick.AddListener(QuitGame);
        continueBtn.onClick.AddListener(ContinueGame);
        newGameBtn.onClick.AddListener(PlayTimeLine);
        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }
    void PlayTimeLine()
    {
        director.Play();
    }
    void QuitGame()
    {
        Application.Quit();
       // Debug.Log("123");
    }
    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        //转换场景
        SenceController.Instance.TranstoFirstLevel();
    }
    void ContinueGame()
    {
        //转换场景，读取进度
        SenceController.Instance.TranstoLoadGame();
    }
}

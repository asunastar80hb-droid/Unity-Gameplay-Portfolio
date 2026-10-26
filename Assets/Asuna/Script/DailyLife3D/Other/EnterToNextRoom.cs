using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterToNextRoom : Interactable
{
    [SerializeField] private String RoomName;
    [SerializeField] private GameObject LoadScenePanel;
    private GameObject LoadScene;
    private IEnumerator Enter()
    {
        LoadScene = Instantiate(LoadScenePanel);
        yield return new WaitForSeconds(2f);
        // Destroy(LoadScene);
        SceneManager.LoadScene(RoomName);
        
    }
    public override void Interact()
    {
       StartCoroutine( Enter());
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyWhenSceneChangeTo : MonoBehaviour
{
    [SerializeField] int _targetSceneIndex;
    private void Awake()
    {
        SceneManager.activeSceneChanged += _activeSceneChanged;
    }

    public void SetSceneIndex(int sceneIndex)
    {
        _targetSceneIndex = sceneIndex;
    }

    private void _activeSceneChanged(Scene curr, Scene next)
    {
        if (_targetSceneIndex == next.buildIndex)
        {
            SceneManager.activeSceneChanged -= _activeSceneChanged;
            Destroy(gameObject);
        }
    }

}

using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public MainInput MainInput => _mainInput;

    MainInput _mainInput;

    private void Awake()
    {
        Instance = this;
        _mainInput = new MainInput();
        _mainInput.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

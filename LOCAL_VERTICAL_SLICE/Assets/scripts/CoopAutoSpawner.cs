using UnityEngine;
using UnityEngine.InputSystem;

public class CoopAutoSpawner : MonoBehaviour
{
    PlayerInputManager manager; //the input manager component

    void Awake()
    {
        manager = GetComponent<PlayerInputManager>();
    }

    void Start()
    {
        var gamepads = Gamepad.all; //only using gamepads

        // list of gamepads (2)
        for (int i = 0; i < Mathf.Min(2, gamepads.Count); i++) 
        {
            PlayerInput.Instantiate(manager.playerPrefab,controlScheme: null,pairWithDevice: gamepads[i]); 
        }
        //spawns exactly 2 players prefabs from the input manager, and connects them to the two controllers connect
    }
}
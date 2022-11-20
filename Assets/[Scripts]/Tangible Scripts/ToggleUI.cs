using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleUI : MonoBehaviour
{
    public InputActionReference toggleUIReference = null;

    private void Awake()
    {
        toggleUIReference.action.started += Toggle;
    }

    private void OnDestroy()
    {
        toggleUIReference.action.started -= Toggle;
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void Toggle(InputAction.CallbackContext context)
    {
        bool isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);
    }

}

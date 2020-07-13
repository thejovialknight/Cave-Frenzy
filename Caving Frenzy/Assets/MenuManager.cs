using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance{ get; private set; }

    public int selection = 0;
    public List<GameObject> buttons = new List<GameObject>();

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update() {
        if(Input.GetButtonDown("Down")) {
            selection++;
            if(selection > buttons.Count - 1) {
                selection = 0;
            }
        }

        if(Input.GetButtonDown("Up")) {
            selection--;
            if(selection < 0) {
                selection = buttons.Count - 1;
            }
        }

        if(Input.GetButtonDown("Select")) {
            RaiseMenuSelect(buttons[selection]);
        }
    }

    void OnEnable() {
        MenuManager.onMenuHover += OnMenuHover;
        MenuManager.onMenuSelect += OnMenuSelect;
    }

    void OnDisable() {
        MenuManager.onMenuHover -= OnMenuHover;
        MenuManager.onMenuSelect -= OnMenuSelect;
    }

    void OnMenuHover(int selection) {
        this.selection = selection;
    }

    void OnMenuSelect(GameObject selection) {
        
    }

    public delegate void OnMenuEvent(int selection);
    public static event OnMenuEvent onMenuHover;
    public static void RaiseMenuHover(int selection) {
        if (onMenuHover != null) {
            onMenuHover(selection);
        }
    }

    public delegate void OnMenuSelectEvent(GameObject selection);
    public static event OnMenuSelectEvent onMenuSelect;
    public static void RaiseMenuSelect(GameObject selection) {
        if (onMenuSelect != null) {
            onMenuSelect(selection);
        }
    }
}

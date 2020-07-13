using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    void OnEnable() {
        MenuManager.onMenuHover += OnMenuHover;
        MenuManager.onMenuSelect += OnMenuSelect;
    }

    void OnDisable() {
        MenuManager.onMenuHover -= OnMenuHover;
        MenuManager.onMenuSelect -= OnMenuSelect;
    }

    void OnMenuHover(int selection) {
        // Hover
    }

    void OnMenuSelect(GameObject selection) {
        if(GameObject.ReferenceEquals(selection, gameObject)) {
            Application.Quit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public bool isWalkable;
    public bool isDiggable;
    public bool isAnvil;
    public int maxHealth = 3;
    public int currentHealth = 3;

    public List<Sprite> sprites = new List<Sprite>();
    public List<AudioClip> audioClips = new List<AudioClip>();
    public AudioClip destroySound;

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer fogSpriteRenderer;

    void OnEnable() {
        TileController.onUpdateFOW += UpdateFOW;
    }

    void OnDisable() {
        TileController.onUpdateFOW -= UpdateFOW;
    }

    void Start() {
        spriteRenderer.sprite = sprites[0];

        if(!LevelManager.Instance.isFogOfWarActive) {
            fogSpriteRenderer.enabled = false;
        }
    }

    public void Dig(int power, PlayerController player) {
        currentHealth -= power;
        spriteRenderer.sprite = sprites[Mathf.Clamp(maxHealth - currentHealth, 0, sprites.Count - 1)];
        AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Count - 1)], Camera.main.transform.position);
        if(currentHealth <= 0) {
            AudioSource.PlayClipAtPoint(destroySound, Camera.main.transform.position);
            RaiseDestroy(gameObject, player);
        }
    }

    public void UpdateFOW(Vector3 position) {
        if(Vector3.Distance(position, transform.position) < 2.5f) {
            fogSpriteRenderer.enabled = false;
        }
    }

    public delegate void OnObjectEvent(GameObject obj, PlayerController player);
    public static event OnObjectEvent onDestroy;
    public static void RaiseDestroy(GameObject obj, PlayerController player) {
        if (onDestroy != null) {
            onDestroy(obj, player);
        }
    }

    public delegate void OnFOWEvent(Vector3 position);
    public static event OnFOWEvent onUpdateFOW;
    public static void RaiseUpdateFOW(Vector3 position) {
        if (onUpdateFOW != null) {
            onUpdateFOW(position);
        }
    }
}

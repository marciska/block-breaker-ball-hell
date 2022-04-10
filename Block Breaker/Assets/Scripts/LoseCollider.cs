using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false); // bug fix: Inactive objects do not accumulate to FindObjectsOfType<Ball>().Length
        Destroy(collision.gameObject); // Destroy() is too slow, therefore FindObjectsOfType<Ball>() will never return a value <= 0
        if (FindObjectsOfType<Ball>().Length <= 0)
        {
            SceneManager.LoadScene("Game Over");
        }
    }
}
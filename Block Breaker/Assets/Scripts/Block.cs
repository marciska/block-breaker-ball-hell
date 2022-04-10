using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // config params
    [SerializeField] AudioClip breakSound = default;
    [SerializeField] GameObject blockSparklesVFX = default;
    [SerializeField] GameObject ball = default;
    [SerializeField] Sprite[] hitSprites = default;
    [SerializeField] int hitsToSpawnBall = 1;

    // cached reference
    Level level;

    // state variables
    [SerializeField] int timesHit; // TODO: Delete me // only serialized for debug purposes

    private void Start()
    {
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
            level.CountBlocks();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
            HandleHit();
    }

    private void HandleHit()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
            if (maxHits >= hitsToSpawnBall)
            {
                Instantiate(ball, transform.position, transform.rotation);
            }
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        else
            Debug.Log("Block sprite is missing from array @object '" + gameObject.name + "'");
    }

    private void DestroyBlock()
    {
        FindObjectOfType<GameSession>().AddToScore();
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerSVFX();
    }

    private void TriggerSVFX()
    {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        // Note: We use the cameraPosition, since even in 2D the distance to the camera is different at planar levels.
        //       This is because Unity handles 2D as 3D internally, thus creating "Pythagoras"-like distances from the
        //       middle of the camera to every point at the 2D-plane.
        TriggerSparklesVFX();
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
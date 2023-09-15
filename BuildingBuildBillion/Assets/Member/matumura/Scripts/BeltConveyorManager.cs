using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BeltConveyorManager : MonoBehaviour
{
    public float speed = 3.0f;//速度調整兼　-1にすると左方向進行
    private float diffX = 0.0f;
    readonly List<GameObject> gameObjects = new();

    private void FixedUpdate()
    {
        if (GameManager.Instance.CountDownGameTime < 0.0f) { return; }
        diffX = speed * Time.fixedDeltaTime;
        foreach (GameObject gameObject in gameObjects)
        {
            Vector2 speed = new Vector3(diffX, 0);
            Vector2 newPos = gameObject.GetComponent<Rigidbody2D>().position + speed;
            gameObject.GetComponent<Rigidbody2D>().position = newPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            gameObjects.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gameObjects.Contains(collision.gameObject))
        {
            gameObjects.Remove(collision.gameObject);
        }
    }
}
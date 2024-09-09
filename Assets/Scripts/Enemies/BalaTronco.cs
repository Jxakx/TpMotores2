using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaTronco : MonoBehaviour
{
    public float speed;
    public int damage;
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.right);
    }

}

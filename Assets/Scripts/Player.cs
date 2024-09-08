using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] Controller controller;
    [SerializeField] float speed = 5;
    void Update()
    {
        transform.position += controller.GetMoveDir() * speed * Time.deltaTime;
    }
}

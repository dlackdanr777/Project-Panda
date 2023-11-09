using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectItem : MonoBehaviour
{
    private float speed = 10f;
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

        //Tween
        mousePosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));
        transform.position = Vector2.Lerp(transform.position, mousePosition, Time.deltaTime * speed);
    }
}

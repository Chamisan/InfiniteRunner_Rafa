using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{
    //Dos vectores que se van a multiplicar después para cambiar el movimiento
    [SerializeField] private Vector2 velocityMov;
    private Vector2 offset;
    private Material material;

    //Este Awake mete el matererial que contiene el sprite con este script a la variable material
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    //Este método le da un nuevo valor al vector offset para modificar la posición del material que afectará al objeto con estre script
    private void Update()
    {
        offset = velocityMov * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{
    //Dos vectores que se van a multiplicar despu�s para cambiar el movimiento
    [SerializeField] private Vector2 velocityMov;
    private Vector2 offset;
    private Material material;

    //Este Awake mete el matererial que contiene el sprite con este script a la variable material
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    //Este m�todo le da un nuevo valor al vector offset para modificar la posici�n del material que afectar� al objeto con estre script
    private void Update()
    {
        offset = velocityMov * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}

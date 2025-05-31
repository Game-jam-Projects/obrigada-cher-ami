using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterObjectActive : MonoBehaviour
{
    public GameObject objeto;


    public void OnMouseEnter()
    {
        if (objeto.activeInHierarchy)
        {
            MouseEventHelper.Default();
        }

        if (objeto.activeInHierarchy == false)
        {

            MouseEventHelper.Walkable();
        }
    }

    public void OnMouseExit()
    {
        MouseEventHelper.Default();
    }

   
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player"))
        {
            if (objeto != null)
                objeto.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(objeto != null)
                objeto.SetActive(false);
        }
    }
}

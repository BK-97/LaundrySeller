using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FakeBra : MonoBehaviour, IFakeProduct
{
    public MeshRenderer mesh;

    private float currentFloat;
    private Transform parentTransform;
    public void StartColorChange(float processTime, Color newColor)
    {
        Material[] materials = mesh.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            Material material = materials[i];

            Color originalColor = material.GetColor("_BaseColor");

            DOTween.To(() => material.GetColor("_BaseColor"), x => material.SetColor("_BaseColor", x), newColor, processTime);
        }
    }

    public void StartUnDissolve(float processTime)
    {
        Vector3 newPos = new Vector3(-0.04f, 0.02f, 0.08f);
        parentTransform = GetComponentInParent<ProductHolder>().gameObject.transform;
        transform.localScale = new Vector3(1, 1, 0.2f);
        parentTransform.transform.DOMove(parentTransform.position + newPos, processTime);
        currentFloat = 0.38f;
        mesh.sharedMaterials[0].SetFloat("_Dissolve", currentFloat);
        Debug.Log(currentFloat+" "+processTime);
        DOTween.To(() => currentFloat, x => currentFloat = x, 0.26f, processTime).SetEase(Ease.Linear)
            .OnUpdate(() => mesh.sharedMaterials[0].SetFloat("_Dissolve", currentFloat));
    }
}

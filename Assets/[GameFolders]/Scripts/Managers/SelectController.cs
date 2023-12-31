using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectController : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedProduct;
    private void OnEnable()
    {
        InputManager.Instance.OnTapDown.AddListener(TapDown);
    }
    private void OnDisable()
    {
        InputManager.Instance.OnTapDown.RemoveListener(TapDown);
    }
    private void TapDown()
    {
        if (!LevelManager.Instance.IsLevelStarted)
            return;

        GameObject selectedObject = GetTappedObject();
        if (selectedObject == null)
        {
            ResetSelect();
            return;
        }

        ISelectable selected = GetTappedOnISelectable(selectedObject);
        if (selected == null)
            return;

        IProduct product = GetSelectedProduct(selectedObject);

        if (product != null)
            CalculateSelectedProduct(selectedObject, selected);
        else
        {
            IProcessor processor = GetSelectedProcessor(selectedObject);
            if (processor != null)
                CalculateSelectedProcessor(processor, selected);
        }
    }
    public void ResetSelect()
    {
        if (selectedProduct == null)
            return;

        selectedProduct.GetComponent<ISelectable>().Deselected();
        selectedProduct = null;
    }
    private void CalculateSelectedProcessor(IProcessor processor, ISelectable selectable)
    {
        if (processor.OnProcess)
            return;
        if (processor.HasReadyProduct)
        {
            processor.SendProduct();
            return;
        }
        if (processor.IsLocked)
        {
            processor.ProcessorUnlock();
            return;
        }
        if (selectedProduct == null)
            return;
        Debug.Log(1);
        selectable.Selected();
        selectedProduct.GetComponentInChildren<IProduct>().MoveProcess(processor);
        selectedProduct = null;
    }
    private void CalculateSelectedProduct(GameObject selectedObject, ISelectable selectable)
    {
        if (selectedProduct != null)
        {
            if (selectable.isSelected)
            {
                selectedProduct.GetComponent<ISelectable>().Deselected();
                selectedProduct = null;
            }
            else
            {
                selectedProduct.GetComponent<ISelectable>().Deselected();
                selectedProduct = selectedObject;
                selectedProduct.GetComponent<ISelectable>().Selected();
            }
        }
        else
        {
            selectedProduct = selectedObject;
            selectedProduct.GetComponent<ISelectable>().Selected();
        }
    }
    private IProduct GetSelectedProduct(GameObject tappedObject)
    {
        ProductHolder holder = tappedObject.GetComponent<ProductHolder>();
        if (holder == null)
            return null;
        IProduct product = holder.currentProduct.GetComponent<IProduct>();
        return product;
    }
    private IProcessor GetSelectedProcessor(GameObject tappedObject)
    {
        IProcessor processor = tappedObject.GetComponent<IProcessor>();
        return processor;
    }
    private GameObject GetTappedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
    private ISelectable GetTappedOnISelectable(GameObject tappedObject)
    {
        ISelectable selectable = tappedObject.GetComponent<ISelectable>();
        return selectable;
    }

}

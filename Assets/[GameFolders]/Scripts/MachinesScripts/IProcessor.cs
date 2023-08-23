using UnityEngine;
public interface IProcessor 
{
    public bool IsLocked { get; set; }
    public bool OnProcess { get; set; }
    void GetProduct(ProductHolder productHolder);
    void ProcessStart();
    void ProcessUpdate();
    void ProcessEnd();
    void ProcessorUnlock();
    public Transform GetProductPlace();

}

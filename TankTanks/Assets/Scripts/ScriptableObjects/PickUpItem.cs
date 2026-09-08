using UnityEngine;



[CreateAssetMenu(fileName ="item", menuName ="Items")]
public class PickUpItem : ScriptableObject
{
   
   public string itemName;
   public string itemType;
    public Mesh mesh; 
    public Material material;
}

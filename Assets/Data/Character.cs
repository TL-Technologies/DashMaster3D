using UnityEngine;

[System.Serializable]
public class Character
{
    public int Id_Character;
    public string Name_Character;
    public string Description;
    public int levelUnlock;
    public Mesh mesh_skin;
    public Material material;
    public Sprite spIcon;
    public Sprite spLock;
    public TypeUnlock typeUnlock;
    //public string typeUnlock;
    //public bool unlock;
}

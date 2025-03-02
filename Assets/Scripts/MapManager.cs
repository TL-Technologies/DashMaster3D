using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicFogAndMist;
using Observer;
public class MapManager : MonoBehaviourSingleton<MapManager>
{
    [SerializeField] Texture[] textures;
    [SerializeField] Sprite[] spriteBG;
    [SerializeField] Color[] colorFog;
    [SerializeField] Color32[] colorsGround;
    public Material mat_Building;
    public Material mat_Building2;
    public Material mat_Ground;
    public SpriteRenderer sprBG;
    public int idtheme;
    public GameObject[] map;
    void Start()
    {
        if (!PlayerPrefs.HasKey("theme"))
            PlayerPrefs.SetInt("theme", 0);
        else
        {
            switch (PlayerPrefs.GetInt("theme"))
            {
                case 0:
                    PlayerPrefs.SetInt("theme", 1);
                    break;
                case 1:
                    PlayerPrefs.SetInt("theme", 2);
                    break;
                case 2:
                    PlayerPrefs.SetInt("theme", 0);
                    break;
            }
        }
        ChangeTheme(PlayerPrefs.GetInt("theme"));
    }

    void ChangeTheme(int idTheme1)
    {
        mat_Building.mainTexture = textures[idTheme1];
        mat_Building2.mainTexture = textures[idTheme1];
        sprBG.sprite = spriteBG[idTheme1];
        DynamicFog.instance.color = colorFog[idTheme1];
        mat_Ground.color = colorsGround[idTheme1];
    }
    public void ChangeMap(int idMap)
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (i == idMap)
            {
                map[i].SetActive(true);
                for (int j = 0; j < map[i].transform.GetChild(7).childCount; j++)
                {
                    ManagerEffect.Instance.AddCharacter(map[i].transform.GetChild(7).GetChild(j));
                }
            }
            else
            {
                map[i].SetActive(false);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Template Data", menuName = "Create Template Data")]
public class TerrainTemplateData :ScriptableObject
{
    [SerializeField] private List<TemplateData> templates;

    private Dictionary<string, TemplateData> templateDictionary = new Dictionary<string, TemplateData>();

    public GameObject GetTemplate(string templateName)
    {
        ValidateDictionary();
        List<GameObject> levelTemplate = templateDictionary[templateName].levelTemplate;
        return levelTemplate[Random.Range(0, levelTemplate.Count)].gameObject;
    }

    public List<GameObject> GetListTemplate(string templateName)
    {
        ValidateDictionary();
        return templateDictionary[templateName].levelTemplate;
    }

    public SpawnPos GetSpawnPos(string templateName)
    {
        ValidateDictionary();
        return templateDictionary[templateName].spawnPos;
    }

    private bool CheckingDictionary()
    {
        return templateDictionary.Count > 0;
    }

    private void ValidateDictionary()
    {
        if (!CheckingDictionary())
        {
            foreach (TemplateData template in templates)
            {
                templateDictionary.Add(template.levelID, template);
            }
        }
    }

    [System.Serializable]
    public struct TemplateData
    {
        public string levelID;
        public SpawnPos spawnPos;
        public List<GameObject> levelTemplate;
    }

    public enum SpawnPos { ROOF, FLOOR }
}

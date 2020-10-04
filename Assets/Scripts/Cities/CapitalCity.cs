/*using System.Collections.Generic;
using UnityEngine;

public class CapitalCity: City
{
    private Inventory inventory;
    //private ResourceModifiers rsmods;

    private Project currentProject;
    public List<Project> availableProjects;
    private List<Project> lockedProjects;
    private List<Project> completedProjects;

    private int population;
    private int unemployed;
    private int working;
    
    private float constructionDevelopment;

    public CapitalCity() : base("Capital")
    {
        inventory = new Inventory(100);
        currentProject = null;
        //rsmods = new ResourceModifiers();
        constructionDevelopment = 1;

        availableProjects = new List<Project>();
        lockedProjects = GlobalProjectDictionary.AllCapitalProjects;
        unlockProjects();
        completedProjects = new List<Project>();
    }

    public void ProcessTurn()
    {
        currentProject.Update(this);
    }

    private void unlockProjects()
    {
        List<Project> toRemove = new List<Project>();
        foreach (Project p in lockedProjects)
        {
            p.Unlocked = true;
            if (p.Prerequisites != null)
            {
                foreach (string id in p.Prerequisites)
                {
                    bool found = false;
                    foreach (Project completed in completedProjects)
                    {
                        if (completed.ID == id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        p.Unlocked = false;
                        break;
                    }
                }
            }
            if (p.Unlocked)
            {
                availableProjects.Add(p);
                toRemove.Add(p);
            }
        }
        foreach (Project p in toRemove)
        {
            lockedProjects.Remove(p);
        }
    }

*//*    public ResourceModifiers GetResourceModifiers()
    {
        return rsmods;
    }*//*

    public Inventory GetInventory()
    {
        return inventory;
    }

*//*    public float CalculateModifiers(EquipmentSlotID id, ItemAttributeID attr)
    {
        float modifier = 1 + rsmods.GetModifiers(id, attr);
        return modifier * GlobalResourceDictionary.GetBaseResourceAttribute(id, attr);
    }*//*

    public float GetConstructionDevelopment()
    {
        return constructionDevelopment;
    }

    public void SetCurrentProject(Project p)
    {
        Debug.Log(p.Name);
        currentProject = p;
    }

    public string GetCurrentProjectDesc()
    {
        return currentProject.GetDescription();
    }

}
*/
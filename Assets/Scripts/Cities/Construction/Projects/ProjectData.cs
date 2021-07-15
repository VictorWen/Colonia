using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Items;

namespace Cities.Construction.Projects
{
    public class ProjectData
    {
        public string Name { get; private set; }
        //TODO: implement employment
        public int Employment { get; private set; }
        public int WorkingPopPreReq { get; private set; }
        public string[] ProjectPreReqs { get; private set; }

        private readonly IProject project;
        public IProject Project { get { return project.Copy(); } }
        public string Type { get; private set; }

        public ProjectData(string name, int employment, int workPopPreReq, string[] projPreReq, IProject project)
        {
            Name = name;
            Employment = employment;
            WorkingPopPreReq = workPopPreReq;
            ProjectPreReqs = projPreReq;
            this.project = project;
            Type = project.ProjectType;
        }

        public bool IsConstructable(City city, GameMaster game)
        {
            return project.IsConstructable(city, game);
        }

        public string GetDescription(City city, GameMaster game)
        {
            string output = "<b>" + Name + "</b>\n";
            if (WorkingPopPreReq > 0 || ProjectPreReqs.Length > 0)
            {
                output += "Requirements:\n";
                if (WorkingPopPreReq > 0)
                {
                    output += "\tWorking Population: " + WorkingPopPreReq + "\n";
                }
                foreach (string projectID in ProjectPreReqs)
                {
                    output += "\t" + GlobalProjectDictionary.GetProjectData(projectID).Name + "\n";
                }
            }
            output += "Cost:\n";
            foreach (KeyValuePair<string, int> resource in project.GetResourceCost(city, game))
            {
                output += "\t" + resource.Value + " " + GlobalResourceDictionary.GetResourceData(resource.Key).name + "\n";
            }
            output += "Employs: " + Employment + "\n";
            output += "<b>" + Type + "</b>\n";
            output += "Description: \n" + project.GetDescription();
            return output;
        }


        public override string ToString()
        {
            return "";
        }
    }
}
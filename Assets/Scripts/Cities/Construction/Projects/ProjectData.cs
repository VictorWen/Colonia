using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


namespace Cities.Construction
{
    public class ProjectData
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public Dictionary<string, int> Costs { get; private set; }
        //TODO: implement employment
        public int Employment { get; private set; }
        public int WorkingPopPreReq { get; private set; }
        public string[] ProjectPreReqs { get; private set; }

        private IProject project;
        public IProject Project { get { return project.Copy(); } }
        public string Type { get; private set; }

        public ProjectData(string id, string name, Dictionary<string, int> costs, int employment, int workPopPreReq, string[] projPreReq, IProject project)
        {
            this.ID = id;
            this.Name = name;
            this.Costs = costs;
            this.Employment = employment;
            this.WorkingPopPreReq = workPopPreReq;
            this.ProjectPreReqs = projPreReq;
            this.project = project;
            this.Type = project.Type;
        }

        public override string ToString()
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
            foreach (KeyValuePair<string, int> resource in Costs)
            {
                output += "\t" + GlobalResourceDictionary.GetResourceData(resource.Key).name + " (" + resource.Value + ")\n";
            }
            output += "Employs: " + Employment + "\n";
            output += "<b>" + Type + "</b>\n";
            output += "Description: \n" + project.GetDescription();
            return output;
        }
    }
}
using System.Collections.Generic;

namespace Cities.Construction.Projects
{
    public interface IProject
    {
        string ProjectType { get; }

        string ID { get; }

        Dictionary<string, int> GetResourceCost(City city, GameMaster game);

        // Returns if there is any availability to construct this Project in the given City
        bool IsConstructable(City city, GameMaster game);

        void AcceptProjectVisitor(City city, IProjectVisitor vistor);

        // Return whether the Project has completed the selection process
        bool IsSelected();

        void OnCancel(City city, World world);

        void Complete(City city, World world);

        IProject Copy();

        string GetDescription();

        string GetSelectionInfo(World world);
    }
}

namespace Beyondsoft.IXP.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using Microsoft.TeamFoundation.VersionControl.Client;

    public class TfsHelper
    {
        public static bool IsAvailable(string TFSServer)
        {
            bool result = false;
            using (TfsTeamProjectCollection teamProjectCollection = new TfsTeamProjectCollection(new Uri(TFSServer)))
            {
                try
                {
                    teamProjectCollection.Connect(Microsoft.TeamFoundation.Framework.Common.ConnectOptions.IncludeServices);
                    result = true;
                }
                catch
                {
                    
                }
            }
            return result;
        }

        public static string GetWorkItemTitleById(string TFSProjectCollection, string TeamProject, string WorkItemId)
        {
            TfsTeamProjectCollection teamProjectCollection = new TfsTeamProjectCollection(new Uri(TFSProjectCollection));
            WorkItemStore workItemStore = teamProjectCollection.GetService<WorkItemStore>();
            Project project = workItemStore.Projects[TeamProject];
            string wiql = "select [System.ID] from WorkItems where [System.Teamproject]='" + TeamProject + "' and  [System.ID]='" + WorkItemId + "'";
            return workItemStore.Query(wiql)[0].Title;
        }

        public static bool GetTFSCode(string TFSProjectCollection, string ServerPath, string LocalPath)
        {
            TfsTeamProjectCollection teamProjectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(TFSProjectCollection));
            VersionControlServer versionControl = (VersionControlServer)(teamProjectCollection.GetService<VersionControlServer>());
            RegisterEventHandlers(versionControl);
            Workspace workspace = null;
            String workspaceName = Environment.MachineName;
            try
            {
                workspace = versionControl.GetWorkspace(workspaceName, versionControl.AuthorizedUser);
            }
            catch (Exception e)
            {
                workspace = versionControl.CreateWorkspace(workspaceName, versionControl.AuthorizedUser);
            }

            WorkingFolder workingFolder = new WorkingFolder(ServerPath, LocalPath);
            workspace.CreateMapping(workingFolder);
            if (!workspace.HasReadPermission)
            {
                return false;
            }
            
            workspace.Get();
            workspace.DeleteMapping(workingFolder);
            workspace.Refresh();
            workspace.Delete();
            return true;
        }

        private static void RegisterEventHandlers(VersionControlServer versionControl)
        {
            // Listen for the Source Control events
            versionControl.BeforeCheckinPendingChange += OnBeforeCheckinPendingChange;
            versionControl.NonFatalError += OnNonFatalError;
            versionControl.Getting += OnGetting;
            versionControl.NewPendingChange += OnNewPendingChange;
        }

        private static void OnBeforeCheckinPendingChange(Object sender, ProcessingChangeEventArgs e)
        {

        }
        private static void OnGetting(Object sender, GettingEventArgs e)
        {

        }
        private static void OnNewPendingChange(Object sender, PendingChangeEventArgs e)
        {

        }
        private static void OnNonFatalError(Object sender, ExceptionEventArgs e)
        {

        }
    }
}

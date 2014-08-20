using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyondsoft.IXP.Components.Build.Jobs;
using Beyondsoft.IXP.Components.Build.Steps;
using Beyondsoft.IXP.Model;
namespace Beyondsoft.IXP.Components.Build.SyncSouceCodeJob.UnitTests
{
    class Program
    {
        static SDModel sdModel = new SDModel()
        {
            SDClient = "test-wsuadev-v-stwa",
            SDServer = "BGIT-SDWSUADEV",
            Port = "2038",
            Path = "sd.exe"
        };

        static void Main(string[] args)
        {
            //SyncDxBuildJobTests();

            //CreateSDClient();

            //DeployDxBuild();
            //RunDxBuild();
            SyncCodeFromTFS();
        }

        static void SyncDxBuildJobTests()
        {
            SyncSDFilesStep step = new SyncSDFilesStep(sdModel);
            try
            {
                step.PercentComplete += step_PercentComplete;

                step.Completed += step_Completed;

                step.Initialize();

                step.Validate();

                step.SetSourceDirectory(@"E:\depot\wsuadev\projects\buildsystemv1\source");

                step.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }  
        }

        static void step_Completed(object sender, CompletedEventArgs e)
        {
            Console.WriteLine(string.Format("{0} {1}", "Sync exit with", e.IsSuccess));
        }

        static void step_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            Console.WriteLine(string.Format("Percent {0}, {1}", e.Percent, e.Message));
        }


        static void CreateSDClient()
        {
            string sdINI = @"e:\depot\sd.ini"; 

            CreateSDStep creatingSD = new CreateSDStep(sdModel, sdINI);
            creatingSD.PercentComplete += step_PercentComplete;
            creatingSD.Completed += step_Completed;
            try
            {
                creatingSD.Initialize();
                creatingSD.Validate();
                creatingSD.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void DeployDxBuild()
        {
            string dsbuildroot = @"E:\";
            string depot = @"E:\depot\wsuadev\projects\buildsystemv1\source\";
            DeployDXBuildStep deployDxbuild = new DeployDXBuildStep(depot, dsbuildroot);
            deployDxbuild.PercentComplete += step_PercentComplete;
            deployDxbuild.Completed += step_Completed;
            try
            {
                deployDxbuild.Initialize();
                deployDxbuild.Validate();
                deployDxbuild.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void SyncCodeFromTFS()
        {
            TFSModel model = new TFSModel()
            {
                TFSServer = "vstfpg05",
                Protocol = "http",
                Port = "8080",
                Path = "tfs"
            };
            string serverPath = "$/DxPlatform/Dx1xEnhancements/Projects/PushToPrelim/PushProcess";
            string localPath = @"e:\push\test\";
            SyncTFSFilesStep step = new SyncTFSFilesStep(model);

            step.PercentComplete += step_PercentComplete;
            step.Completed += step_Completed;

            try
            {
                step.Initialize();
                step.Mapping(serverPath, localPath);
                step.Validate();
                step.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void RunDxBuild()
        {
            DxbuildModel.SetEnlistment(@"e:\dsbuildroot");
            DxbuildModel buildModel = new DxbuildModel()
            {
                Project = "AzureMedia_RestRef",
                Server = "sedbserver",
                Database = "DDCMS_WS_Azure",
                BuildType = BuildTypes.mtpsstaginghxs,
                Language = new System.Globalization.CultureInfo(1033),
                DocStudioVersion = "1.11"
            };
            RunDxBuildStep step = new RunDxBuildStep(buildModel);
            step.PercentComplete += step_PercentComplete;
            step.Completed += step_Completed;

            try
            {
                step.Initialize();
                step.Validate();
                step.Execute();

                Console.WriteLine(buildModel.BuildManifest);
                Console.WriteLine(buildModel.LogFile);
                Console.WriteLine(buildModel.MiddleierFile);
                Console.WriteLine(buildModel.Content);
                Console.WriteLine(buildModel.DocStudio);
                Console.WriteLine(buildModel.Content);
                Console.WriteLine(DxbuildModel.Utility);
                Console.WriteLine(DxbuildModel.Tools);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

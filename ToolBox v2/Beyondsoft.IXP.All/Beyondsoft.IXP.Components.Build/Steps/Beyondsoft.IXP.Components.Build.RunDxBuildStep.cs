namespace Beyondsoft.IXP.Components.Build.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Beyondsoft.IXP.Model;
    using System.IO;
    using System.Globalization;
    using System.Data.SqlClient;
    using Beyondsoft.IXP.Components.Build.Commands;

    public class RunDxBuildStep:RunBuildStep
    {
        private string dsbuildRoot = null;
        public static List<DxbuildModel> dsbuildAllCollection;


        public RunDxBuildStep(DxbuildModel model)
            : base(model)
        {
            this.StepName = string.Format(this.StepName, "DxBuild", model.Project);
        }

        public RunDxBuildStep(Queue<BuildBaseModel> dsbuildQueue)
            : base(dsbuildQueue)
        {
            this.StepName = string.Format(this.StepName, "DxBuild", this.currentBuildModel.Project);
        }

        #region construct
        static RunDxBuildStep()
        {
            dsbuildAllCollection = new List<DxbuildModel>();

            ParseDsBuildAll();
        }
        #endregion

        #region public methods

        public override void Initialize()
        {
            base.Initialize();
            try
            {
                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Initialize completed"));
            }
            finally
            {
                SetCurrentProgress();

                if (timer != null) timer.Dispose();
            }
        }

        public override bool Validate()
        {
            base.Validate();

            try
            {
                if (!string.IsNullOrEmpty(DxbuildModel.Enlistment))
                {
                    return false;
                }
                if (!this.PingServer())
                {
                    return false;
                }
                if (!this.IsProjectExist())
                {
                    return false;
                }
            }
            finally
            {
                SetCurrentProgress();

                if (timer != null) timer.Dispose();
            }

            return true;
        }

        public override void SetEnlistmentFolder(string enlistment)
        {
            this.dsbuildRoot = enlistment;

            DxbuildModel.SetEnlistment(enlistment);
        }

        public void WriteOneToFile()
        {
            string dsbuildAll = DxbuildModel.dsBuildAll;
            try
            {
                if (File.Exists(dsbuildAll))
                {
                    File.Delete(dsbuildAll);
                }
                using (FileStream stream = new FileStream(dsbuildAll, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        if (this.currentBuildModel.IsDisable)
                        {
                            writer.WriteLine(string.Format("::{0}", this.currentBuildModel.ToString()));
                        }
                        else
                        {
                            writer.WriteLine(string.Format("{0}", this.currentBuildModel.ToString()));
                        }
                    }
                }
            }
            catch(IOException e)
            {
            }
        }

        public override void WriteToFile()
        {
            string dsbuildAll = DxbuildModel.dsBuildAll;
            try
            {
                if (File.Exists(dsbuildAll))
                {
                    File.Delete(dsbuildAll);
                }
                using (FileStream stream = new FileStream(dsbuildAll, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        foreach (DxbuildModel model in dsbuildAllCollection)
                        {
                            if (model.IsDisable)
                            {
                                writer.Write(string.Format("::{0}\r\n", model.ToString()));
                            }
                            else
                            {
                                writer.Write(string.Format("{0}\r\n", model.ToString()));
                            }
                        }
                    }
                }
            }
            catch (IOException e)
            {
            }
        }

        public override void PostProject()
        {
            throw new NotImplementedException();
        }

        public override void SetPostFolder(string sharedFolder)
        {
            throw new NotImplementedException();
        }

        public override bool PingServer()
        {
            //TODO: Create database component
            string connectionString = "Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=30";
            bool result = true;
            using (SqlConnection connection = new SqlConnection(string.Format(connectionString, currentBuildModel.Server, currentBuildModel.Database)))
            {
                try
                {
                    connection.Open();
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        public override bool IsProjectExist()
        {
            string strSQL = "select count(*) from [Reporting_Project]";
            string connectionString = "Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=30";
            bool result = true;
            using (SqlConnection connection = new SqlConnection(string.Format(connectionString, currentBuildModel.Server, currentBuildModel.Database)))
            {
                try
                {
                    SqlCommand command = new SqlCommand()
                    {
                        CommandText = strSQL,
                        CommandType = System.Data.CommandType.Text,
                        Connection = connection
                    };
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result = reader.GetInt16(0) == 0 ? false : true;
                    }
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        public override void Execute()
        {
            base.Execute();

            try
            {

                BatchCommand dsbuild = CommandFactory.CreateCommandInstance<BatchCommand>(DxbuildModel.BuildCommand);
                dsbuild.IsExternal = true;
                dsbuild.SetWorkingDirectory(DxbuildModel.Enlistment);
                do
                {
                    if (!dsbuildAllCollection.Exists(delegate(DxbuildModel m)
                    {
                        if (m.Equals(currentBuildModel))
                        {
                            return true;
                        }
                        return false;
                    }))
                    {
                        dsbuildAllCollection.Add(currentBuildModel as DxbuildModel);
                    }

                    if (!currentBuildModel.IsDisable)
                    {
                        WriteOneToFile();

                        dsbuild.Execute();
                    }
                } while (buildQueue.Count > 0);

                WriteToFile();

                OnProcessorCompleted(new CompletedEventArgs(true));

                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Build completed"));

            }
            catch (Exception e)
            {
                OnProcessorCompleted(new CompletedEventArgs(false));
                throw e;
            }
            finally
            {
                SetCurrentProgress();

                if (timer != null) timer.Dispose();
            }
        }

        

        #endregion

        #region static methods
        public static void ParseDsBuildAll()
        {
            if (File.Exists(DxbuildModel.dsBuildAll))
            {
                using (StreamReader projectReader = new StreamReader(DxbuildModel.dsBuildAll))
                {
                    while (!projectReader.EndOfStream)
                    {
                        string commandLine = projectReader.ReadLine();

                        string[] paraArray = commandLine.Split(';');

                        if (paraArray.Length > 5)
                        {
                            BuildTypes buildType;

                            if (!Enum.TryParse<BuildTypes>(paraArray[3].TrimStart('"').TrimEnd('"').ToLower(), out buildType))
                            {
                                throw new Exception("The build type is not supported");
                            }

                            int lcid = 0;

                            if (!int.TryParse(paraArray[4].TrimStart('"').TrimEnd('"'), out lcid))
                            {
                                throw new Exception("The lcid is not supported");
                            }

                            CultureInfo cultureInfo = new CultureInfo(lcid);

                            DxbuildModel dxModel = new DxbuildModel()
                            {
                                Project = paraArray[0].TrimStart(new char[]{':',':'}),
                                Server = paraArray[1],
                                Database = paraArray[2],
                                BuildType = buildType,
                                Language = cultureInfo,
                                DocStudioVersion = paraArray[5],
                                IsDisable = paraArray[0].IndexOf("::") > -1 ? true : false
                            };

                            dsbuildAllCollection.Add(dxModel);
                        }
                        else
                        {
                            throw new Exception("The argument must be more than 5! Please check dsbuildAll.txt");
                        }
                    }
                }
            }
        }
        #endregion

        
    }
}

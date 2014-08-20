using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

namespace Beyondsoft.IXP.Model
{
    public class OutputModel:BuildBaseModel
    {
        public static string Tools {
            get { return Path.Combine(Enlistment,"Tools"); } 
        }

        public static string Utility
        {
            get { return Path.Combine(Tools,@"Utility"); }
        }

        public static string DxPublisherExe
        {
            get { return Path.Combine(Utility,@"Dxpublisher\Dxpublisher.exe" ); }
        }

        public static string MrefBuilderExe
        {
            get { return Path.Combine(Utility,@"MrefBuilder\MrefBuilder.exe" ); }
        }

        public static string PowerShell
        {
            get { return Path.Combine(Utility,@"PowerShell"); }
        }

        public static string BuildCommand
        {
            get { return Path.Combine(Enlistment,"dsbuild.cmd"); }
        }
        
        public string DocStudioVersion { get; set; }

        public BuildTypes BuildType { get; set; }

        public string Original {
            get { return string.Format(@"{0}\{1}\originalcontent\{2}\Content\{3}",Enlistment,DBShortName,Language.LCID,Project); } 
            
        }

        public string OriginalContent {
            get { return Path.Combine(Original,"Content"); }
            
        }

        public string OriginalExtractedFiles {
            get { return Path.Combine(Original,@"ExtractedFiles"); }
            
        }

        public string OriginalXmlComp
        {
            get { return Path.Combine(Original, "XmlComp"); }
            
        }

        public string OriginalSupportFiles
        {
            get { return Path.Combine(Original,"SupportFiles"); }
        }

        public string Content {
            get { return string.Format(@"{0}\{1}\{2}\Content\{3}",Enlistment,DBShortName,Language.LCID,Project);}
        }

        public string ExtractedFiles {
            get { return Path.Combine(Content,"Content"); }
            
        }

        public string Supportfiles {
            get { return Path.Combine(Content, "Supportfiles"); }
            
        }

        public string XmlComp
        {
            get { return Path.Combine(Content, "XmlComp"); }
        }

        public string BuildManifest {
            get { return string.Format(@"{0}\{1}.BuildManifest.proj.xml", Content,Project); }
        }

        public string DocStudio
        {
            get { return string.Format(@"{0}\{1}\DocStudio\{2}\{3}",Enlistment,DBShortName,Project,Language.LCID); }
        }

        public string LogFile
        {
            get { return string.Format(@"{0}\logs\build.{1}.log", DocStudio, Language.LCID); }
        }

        public string MiddleierFile
        {
            get { return string.Format(@"{0}\middletier.config",DocStudio); }
        }

        public string RSP
        {
            get { return Path.Combine(DocStudio,"Build" ); }
        }

        public string Commands
        {
            get { return Path.Combine(RSP , "Commands"); }
        }

        public string Output
        {
            get { return string.Format(@"{0}\{1}\BuildOutput\{1}\{2}",Enlistment,DBShortName,Language.LCID,Project); }
        }

        public static string dsBuildAll
        {
            get { return Path.Combine(Enlistment,"dsbuildall.txt"); }
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};\"{3}\";\"{4}\";{5}",Project,Server,Database,BuildType.ToString(),Language.LCID,DocStudioVersion);
        }

        public override bool Equals(object obj)
        {
            DxbuildModel model = obj as DxbuildModel;

            return this.Project.Equals(model.Project, StringComparison.OrdinalIgnoreCase) &&
                this.Server.Equals(model.Server, StringComparison.OrdinalIgnoreCase) &&
                this.Database.Equals(model.Database, StringComparison.OrdinalIgnoreCase) &&
                this.Language.LCID == model.Language.LCID &&
                this.BuildType == model.BuildType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum BuildTypes
    {
        doc,
        docx,
        html,
        mtpspreviewhxs,
        mtpsmrefpreviewhxs,
        mtpsstaginghxs,
        mtpsmrefstaginghxs,
        mtpslivehxs,
        mtpsmreflivehxs,
        pdf,
        prelimmrefchm,
        stdchm,
        stdhxs,
        stdmrefchm,
        stdmrefhxs,
        txt,
        psmrefhxs
    }
}

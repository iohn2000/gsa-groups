using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gsa_groups
{
    
    public class CmdLineOptions
    {
        [VerbOption("showgroups", HelpText = "Show all groups for a user.")]
        public ShowGroupsSubOptions CommitVerb { get; set; }

        public CmdLineOptions()
        {
            CommitVerb = new ShowGroupsSubOptions();
        }

        [HelpOption]
        public string GetUsage()
        {
            string helpText = "This tool parses the onboard xml groups file and shows all groups for specific user.";

            //return helpText + Environment.NewLine + Environment.NewLine + 
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    public class ShowGroupsSubOptions
    {
        [Option('f', "file", Required = true, HelpText = "This is the xml file with groups in it exported from GSA.")]
        public string XmlFile { get; set; }

        [Option('u', "user", Required = true, HelpText = "Part of the username you want to show groups for.")]
        public string UserName { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            string helpText = "This tool parses the onboard xml groups file and shows all groups for specific user.";

            return helpText + Environment.NewLine + Environment.NewLine + HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}

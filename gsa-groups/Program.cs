﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace gsa_groups
{
    class Program
    {

        static int Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            
            bool worked = false;
            string invokedVerb = "";
            object invokedVerbInstance = null;

            CmdLineOptions o = new CmdLineOptions();
            ShowGroupsSubOptions og = new ShowGroupsSubOptions();
            Dictionary<string,List<string>> resultList;

            try
            {
                worked = CommandLine.Parser.Default.ParseArguments(args, o,
                                (verb, subOptions) =>
                                {
                                    invokedVerb = verb;
                                    invokedVerbInstance = subOptions;
                                }
                                );

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing command line options.\r\nPlease make sure you specify a verb as first argument.");
                Environment.Exit(-1);
            }
            
            if (! worked | invokedVerbInstance == null) // try without verb and assume "showgroups" is default
            {
                invokedVerb = "showgroups";
                worked = CommandLine.Parser.Default.ParseArguments(args,og);
                invokedVerbInstance = og;
            }

            if (invokedVerb == "showgroups")
            {
                og = (ShowGroupsSubOptions)invokedVerbInstance;
                if (worked)
                {
                    Console.WriteLine(og.XmlFile);
                    Console.WriteLine(og.UserName);

                    XmlDocument x = new XmlDocument();
                    try
                    {
                        Console.WriteLine(string.Format("Loading xml file : '{0}'", og.XmlFile));
                        x.Load(og.XmlFile);
                        XmlNodeList xlist = x.SelectNodes(
                            string.Format("/xmlgroups/membership/members/principal[contains(.,'{0}')]", og.UserName)
                            );
                        int amount = xlist.Count;
                        Console.WriteLine(string.Format("Found {0} entries for username: '{1}'", amount, og.UserName));

                        resultList = new Dictionary<string, List<string>>();


                        foreach (XmlNode n in xlist)
                        {
                            string userName = n.InnerText;
                            string groupName = n.ParentNode.ParentNode.ChildNodes[0].InnerText;
                            if (resultList.ContainsKey(userName))
                            {
                                List<string> groups = resultList[userName];
                                groups.Add(groupName);
                            }
                            else
                            {
                                List<string> groups = new List<string>();
                                groups.Add(groupName);
                                resultList.Add(userName, groups);
                            }



                            string path = userName + "\t\t" + groupName;

                        }

                        // display results
                        foreach (KeyValuePair<string, List<string>> kvp in resultList)
                        {
                            ConsoleColor cc = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("User:" + kvp.Key);
                            Console.ForegroundColor = cc;
                            foreach (string g in kvp.Value)
                            {
                                Console.WriteLine("       " + g);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ConsoleColor cc = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(ex.ToString());
                        Console.ForegroundColor = cc;


                    }
                    Console.WriteLine(Environment.NewLine + "Finished.");
                }
            }
            else
            {
                Console.WriteLine("command verb missing (eg. showgroups)");
            }

            

            
            Console.ReadLine();
            return -1;
        }
    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ComplyV
{
    public class Program
    {
        static Dictionary<string, string> AllUsersList = new Dictionary<string, string>()
            {
                { "EI" , "Elsa Ingram" },
                { "PM" , "Paul Marsh" },
                { "DJ" , "D Joshi" },
                { "NH" , "Nick Holden" },
                { "JH" , "John" }
            };
        static void Main(string[] args)
        {
            List<WorkFlow> AllWorkFlows = new List<WorkFlow>();
            
            Console.WriteLine("Welcome to Workflow Management System \n[1] Create New Workflow \n[2] Choose from existing workflows");
            //Console.WriteLine("[1] Create New Workflow \n[2] Choose from existing workflows");
            Console.Write("Enter the Option Number: ");
            var option = Console.ReadLine(); // convert the option to number and check if the num is 1 or 2
            Console.WriteLine("Selected Option is " + option);

            if (option == "1")
            {
                WorkFlow workFlowObj = new WorkFlow();
                Console.Write("Provide Workflow Name: ");
                workFlowObj.name = Console.ReadLine().ToString();
                workFlowObj.LevelList = new List<Level>();
                uint level = 1;
                do
                {
                    workFlowObj.LevelList.Add(AddNewLevel(level));
                    level++;
                    Console.WriteLine("Add More Levels [Y]/[N]? ");
                } while (Console.ReadLine().ToString().ToUpper() == "Y");
                AllWorkFlows.Add(workFlowObj);

                Console.WriteLine("Begin Execution of " + workFlowObj.name + "[Y/N]?");
                if (Console.ReadLine().ToString().ToUpper() == "Y")
                {
                    foreach (Level lvl in workFlowObj.LevelList)
                    {
                        switch (lvl.levelName) {
                            case "Sequential":
                                SequentialLevel(lvl);
                                break;

                            case "RoundRobin":
                                RoundRobinLevel(lvl);
                                break;

                            case "AnyOne":
                                AnyOneLevel(lvl);
                                break;
                        }

                    }
                }
            }
            var json = JsonConvert.SerializeObject(AllWorkFlows);
            Console.WriteLine(json);

            //Console.WriteLine("");
            Console.ReadLine();
        }


        static Level AddNewLevel(uint level)
        {
            Level currentLevel = new Level();
            currentLevel.levelNumber = level;
            currentLevel.levelUsers = new List<LevelUser>();
            Console.WriteLine("Choose the Type of Approval for Level " + level + ": [1] Sequential [2] Round-Robin [3] AnyOne");
            //Console.WriteLine("[1] Sequential [2] Round-Robin [3] AnyOne");
            currentLevel.levelType = int.Parse(Console.ReadLine().ToString());
            currentLevel.levelName = Enum.GetName(typeof(ApprovalType), currentLevel.levelType);
            Console.WriteLine("Here is the list of users to choose from");
            Console.WriteLine("[EI] Elsa Ingram\n[PM] Paul Marsh \n[DJ] D Joshi \n[NH] Nick Holden \n[JH] John");

            Console.WriteLine("Enter the user codes [comma seperated] for Level " + level + " Approval");
            Console.WriteLine("Ex: EI,NH,PM  -> for Elsa Ingram, Nick Holden and Paul Marsh");
            var allUsers = Console.ReadLine().ToString().Split(',');
            foreach (string usr in allUsers)
            {
                //Dictionary<string, string> tempDict = new Dictionary<string, string>() { { Enum.GetName(typeof(Users), int.Parse(usr.Trim())), "NotApproved" } };
                //currentLevel.levelUsers.Add(Enum.GetName(typeof(Users), int.Parse(usr.Trim())), "NotApproved");
                currentLevel.levelUsers.Add(new LevelUser() { UserCode = usr.Trim(), ApprovalStatus = "NotApproved" });
            }
            return currentLevel;
        }



        static void SequentialLevel(Level lvl)
        {
            Console.WriteLine("Enter the Approver Code from the following list");
            foreach(var usr in lvl.levelUsers)
            {
                Console.WriteLine("[" + usr.UserCode + "] " + AllUsersList[usr.UserCode]);
            }
            Console.Write("Approver Code: ");
            string apprCode = Console.ReadLine().Trim();
            if (lvl.levelUsers.Find(ur => ur.UserCode == apprCode) != null) {
                foreach(var ur in lvl.levelUsers)
                {
                    if(ur.UserCode == apprCode && ur.ApprovalStatus == "NotApproved") {
                        Console.Write("Enter Approval Action for "+ AllUsersList[ur.UserCode] +"\n[1]Approved [2]Rejected [3]Reject & Remove from Workflow");
                        ur.ApprovalStatus = Enum.GetName(typeof(ApprovalStatus), int.Parse(Console.ReadLine().Trim()));
                    } else
                    {
                        if(ur.ApprovalStatus == "Rejected")
                        {
                            break;
                        }
                    }
                }
                Console.WriteLine("Approver Found");
            }
        }

        static void RoundRobinLevel(Level lvl)
        {
            Console.WriteLine("Enter the Approver Code from the following list");
            foreach (var usr in lvl.levelUsers)
            {
                Console.WriteLine("[" + usr.UserCode + "] " + AllUsersList[usr.UserCode]);
            }
            Console.Write("Approver Code: ");
            string apprCode = Console.ReadLine().Trim();
            if (lvl.levelUsers.Find(ur => ur.UserCode == apprCode) != null)
            {
                foreach (var ur in lvl.levelUsers)
                {
                    if (ur.UserCode == apprCode && ur.ApprovalStatus == "NotApproved")
                    {
                        Console.Write("Enter Approval Action for " + AllUsersList[ur.UserCode] + "\n[1]Approved [2]Rejected [3]Reject & Remove from Workflow");
                        ur.ApprovalStatus = Enum.GetName(typeof(ApprovalStatus), int.Parse(Console.ReadLine().Trim()));
                    }
                    else
                    {
                        if (ur.ApprovalStatus == "Rejected")
                        {
                            break;
                        }
                    }
                }
                Console.WriteLine("Approver Found");
            }
        }

        static void AnyOneLevel(Level lvl)
        {
            Console.WriteLine("Enter the Approver Code from the following list");
            foreach (var usr in lvl.levelUsers)
            {
                Console.WriteLine("[" + usr.UserCode + "] " + AllUsersList[usr.UserCode]);
            }
            Console.Write("Approver Code: ");
            string apprCode = Console.ReadLine().Trim();
            if (lvl.levelUsers.Find(ur => ur.UserCode == apprCode) != null)
            {
                foreach (var ur in lvl.levelUsers)
                {
                    if (ur.UserCode == apprCode && ur.ApprovalStatus == "NotApproved")
                    {
                        Console.Write("Enter Approval Action for " + AllUsersList[ur.UserCode] + "\n[1]Approved [2]Rejected [3]Reject & Remove from Workflow");
                        ur.ApprovalStatus = Enum.GetName(typeof(ApprovalStatus), int.Parse(Console.ReadLine().Trim()));
                        if(ur.ApprovalStatus == "Approved")
                        {
                            lvl.levelStatus = "Approved";
                            break;
                        }
                    } 
                }
                
            }
        }



    }

    enum ApprovalStatus {
       NotApproved,
       Approved,
       Rejected,
       RejectRemoveWorkflow
    }

    enum ApprovalType
    {
        Sequential = 1,
        RoundRobin, 
        AnyOne
    }

    enum Users
    {
        Elsa_Ingram = 1,
        Paul_Marsh,
        D_Joshi,
        Nick_Holden,
        John
    }

    public class WorkFlow
    {
        public string name { get; set; }
        public List<Level> LevelList { get; set; }
        public string workFlowStatus { get; set; }
    }

    public class Level
    {
        public uint levelNumber { get; set; }
        public int levelType { get; set; }
        public string levelName { get; set; }
        public List<LevelUser> levelUsers { get; set; }
        public string levelStatus { get; set; }
    }

    public class LevelUser
    {
        public string UserCode { get; set; }
        public string ApprovalStatus { get; set; }
    }

}

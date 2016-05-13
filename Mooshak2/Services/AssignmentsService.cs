using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Mooshak2.DAL;
using Mooshak2.Models;
using Mooshak2.Services;

namespace Mooshak2.Services
{
	public class AssignmentsService
	{
		private Context db = new Context();

        public List<AssignmentIndexViewModel> GetAssignmentAndCourse()
        {
            string userName = HttpContext.Current.User.Identity.GetUserName();
            var query = from assign in db.Assignment
                        join assignList in db.AssignmentList on assign.Id equals assignList.AssignmentId
                        join course in db.Course on assignList.courseId equals course.Id
                        join stdntCrsLst in db.StudentCourseList on course.Id equals stdntCrsLst.courseId
                        join user in db.AspNetUsers on stdntCrsLst.UserName equals user.UserName
                        where user.UserName == userName
                        select new AssignmentIndexViewModel { Assignments = assign, Courses = course };
            List<AssignmentIndexViewModel> model = new List<AssignmentIndexViewModel>();
            foreach (var assignment in query)
            {
                model.Add(new AssignmentIndexViewModel { Assignments = assignment.Assignments, Courses = assignment.Courses });
            }
            return model;
        }

        public List<Assignment> GetAllAssignments()
        {
            return db.Assignment.ToList();
        }

        public List<Milestone> GetAllMilestones()
        {
            return db.Milestone.ToList();
        }

        public List<MilestoneList> GetAllMilestoneLists()
        {
            return db.MilestoneList.ToList();
        }

        public List<StudentCourseList> GetAllStudentList()
        {
            return db.StudentCourseList.ToList();
        }

        public Assignment GetAssignmentByID(int assignmentID)
        {
            Assignment assignment = db.Assignment.SingleOrDefault(x => x.Id == assignmentID);
            return assignment;
        }

        public Milestone GetMilestoneById(int id)
        {
            Milestone milestone = db.Milestone.SingleOrDefault(x => x.Id == id);
            return milestone;
        }

        public List<Milestone> GetAllMilestonesByAssignId(int id)
        {
            var query = from milestone in db.Milestone
                        join milestonelist in db.MilestoneList on milestone.Id equals milestonelist.milestoneId
                        where milestonelist.assignmentId == id
                        select milestone;
            List<Milestone> milestoneList = new List<Milestone>();
            foreach (var milestone in query)
            {
                milestoneList.Add(new Milestone { Id = milestone.Id, Title = milestone.Title, Weight = milestone.Weight});
            }
            return milestoneList;
        }

//--------------------------------Milestone Creation-----------------------------------------------------------------------------------------------
        public void CreateMilestoneList(MilestoneCreateViewModel model)
        {
            var count = GetAllMilestoneLists().LastOrDefault();
            int id;
            if (count == null)
                id = 0;
            else
                id = count.Id + 1;
            db.MilestoneList.Add(new MilestoneList
            {
                Id = id,
                assignmentId = model.assignmentId,
                milestoneId = model.id
            });
            db.SaveChanges();
        }

        public MilestoneCreateViewModel CreateMilestone(MilestoneCreateViewModel model)
        {
            var count = GetAllMilestones().LastOrDefault();
            int id;
            if (count == null)
                id = 0;
            else
                id = count.Id + 1;
            db.Milestone.Add(new Milestone
            {
                Id = id,
                Title = model.Title,
                Weight = model.Weight
            });
            db.SaveChanges();
            model.id = id;
            return model;
        }

//--------------------------------Assignment Deletion----------------------------------------------------------------------------------------------
        public void AssignmentDelete(int assignmentId) //Eyðir assignment gefið útfrá Id og með því entry úr assignmentList, Sýnum GradeList, 
        {
            //Eyðir öllum milestones tengd Assignmentinu sem er gefið í gegnum Id
            List<Milestone> MilestoneRemoval = new List<Milestone>();
            MilestoneRemoval = GetAllMilestoneEntries(assignmentId);
            if (MilestoneRemoval != null)
            {
                foreach (var milestone in MilestoneRemoval)
                {
                    MilestoneDelete(milestone.Id);
                }
            }

            //Eyðir öllum AssignmentGradeList entries tengd Assignmentinu sem er gefið í gegnum Id
            List<AssignmentGradeList> AssignmentGradeListRemoval = new List<AssignmentGradeList>();
            AssignmentGradeListRemoval = GetAllAssignmentGradeEntries(assignmentId);
            if (AssignmentGradeListRemoval != null)
            {
                foreach (var assignmentGradeEntry in AssignmentGradeListRemoval)
                {
                    db.AssignmentGradeList.Remove(assignmentGradeEntry);
                }
            }

            //Eyðir AssignmentList entry tengt Assignmentinu sem er gefið í gegnum Id
            AssignmentList AssignmentListRemoval = new AssignmentList();
            AssignmentListRemoval = GetAssignmentListEntry(assignmentId);
            if (AssignmentListRemoval != null)
            {
                db.AssignmentList.Remove(AssignmentListRemoval);
            }

            Assignment assignment = db.Assignment.Find(assignmentId);
            db.Assignment.Remove(assignment);
            db.SaveChanges();
        }

        List<AssignmentGradeList> GetAllAssignmentGradeEntries(int assignmentId)
        {
            var query = from assignGradeList in db.AssignmentGradeList
                        where assignGradeList.assignmentId == assignmentId
                        select assignGradeList;
            List<AssignmentGradeList> assignmentGradeListEntry = new List<AssignmentGradeList>();
            foreach (var assignmentListGrade in query)
            {
                assignmentGradeListEntry.Add(new AssignmentGradeList { Id = assignmentListGrade.Id, assignmentId = assignmentListGrade.assignmentId, UserName = assignmentListGrade.UserName, grade = assignmentListGrade.grade});
            }
            return assignmentGradeListEntry;
        }

        AssignmentList GetAssignmentListEntry(int assignmentId)
        {
            var query = from assignList in db.AssignmentList
                        where assignList.AssignmentId == assignmentId
                        select assignList;
            AssignmentList assignmentListEntry = new AssignmentList();
            foreach (var assignmentList in query)
            {
                assignmentListEntry = assignmentList;
            }
            return assignmentListEntry;
        }


//--------------------------------Milestone Deletion-----------------------------------------------------------------------------------------------
        public void MilestoneDelete(int milestoneId) //Eyðir milestone, MilestoneGradeList entry sínu og MilestoneList entry sínu
        {
            //MilestoneList To be Removed
            MilestoneList MilestoneListRemoval = new MilestoneList();
            MilestoneListRemoval = GetMilestoneListEntry(milestoneId);
            if (MilestoneListRemoval != null)
            {
                db.MilestoneList.Remove(MilestoneListRemoval);
            }

            //MilestoneGradeList To be Removed
            List<MilestoneGradeList> MilestoneGradeRemoval = new List<MilestoneGradeList>();
            MilestoneGradeRemoval = GetAllMilestoneGradeListEntries(milestoneId);
            if (MilestoneGradeRemoval != null)
            {
                foreach (var listentry in MilestoneGradeRemoval)
                {
                    db.MilestoneGradeList.Remove(listentry);
                }
            }

            //Milestone To be Removed
            Milestone milestone = db.Milestone.Find(milestoneId);
            db.Milestone.Remove(milestone);
            db.SaveChanges();
        }

        public List<Milestone> GetAllMilestoneEntries(int assignmentId)
        {
            var query = from milestone in db.Milestone
                        join milestoneList in db.MilestoneList on milestone.Id equals milestoneList.milestoneId
                        where (milestoneList.assignmentId == assignmentId)
                        select milestone;
            List<Milestone> milestoneListEntry = new List<Milestone>();
            foreach (var milestone in query)
            {
                milestoneListEntry.Add(new Milestone { Id = milestone.Id, Title = milestone.Title, Weight = milestone.Weight});
            }
            return milestoneListEntry;
        }

        public MilestoneList GetMilestoneListEntry(int milestoneId)
        {
            var query = from milestonelist in db.MilestoneList
                        where milestonelist.milestoneId == milestoneId
                        select milestonelist;
            MilestoneList milestoneListEntry = new MilestoneList();
            foreach (var milestoneList in query)
            {
                milestoneListEntry = milestoneList;
            }
            return milestoneListEntry;
        }

        public List<MilestoneGradeList> GetAllMilestoneGradeListEntries(int milestoneId)
        {
            var query = from milestonegradelist in db.MilestoneGradeList
                        where milestonegradelist.milestoneId == milestoneId
                        select milestonegradelist;
            List<MilestoneGradeList> milestoneGradeListEntry = new List<MilestoneGradeList>();
            foreach (var milestoneGradeList in query)
            {
                milestoneGradeListEntry.Add(new MilestoneGradeList { Id = milestoneGradeList.Id, UserName = milestoneGradeList.UserName, milestoneId = milestoneGradeList.milestoneId, grade = milestoneGradeList.grade });
            }
            return milestoneGradeListEntry;
        }

        public void RunCompilerCPlusPLus(string userName)
        {
            // Set up our working folder, and the file names/paths.
            // In this example, this is all hardcoded, but in a
            // real life scenario, there should probably be individual
            // folders for each user/assignment/milestone.

            var workingFolder = "C:\\Submits\\" + userName + "\\";
            var cppFileName = "main.cpp";
            var exeFilePath = workingFolder + "Hello.exe";


            // In this case, we use the C++ compiler (cl.exe) which ships
            // with Visual Studio. It is located in this folder:
            var compilerFolder = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\bin\\";
            // There is a bit more to executing the compiler than
            // just calling cl.exe. In order for it to be able to know
            // where to find #include-d files (such as <iostream>),
            // we need to add certain folders to the PATH.
            // There is a .bat file which does that, and it is
            // located in the same folder as cl.exe, so we need to execute
            // that .bat file first.

            // Using this approach means that:
            // * the computer running our web application must have
            //   Visual Studio installed. This is an assumption we can
            //   make in this project.
            // * Hardcoding the path to the compiler is not an optimal
            //   solution. A better approach is to store the path in
            //   web.config, and access that value using ConfigurationManager.AppSettings.

            // Execute the compiler:
            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = workingFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;

            compiler.Start();
            compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
            compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + cppFileName);
            compiler.StandardInput.WriteLine("exit");
            string output = compiler.StandardOutput.ReadToEnd();
            compiler.WaitForExit();
            compiler.Close();

            // Check if the compile succeeded, and if it did,
            // we try to execute the code:
            if (System.IO.File.Exists(exeFilePath))
            {
                var processInfoExe = new ProcessStartInfo(exeFilePath, "");
                processInfoExe.UseShellExecute = false;
                processInfoExe.RedirectStandardOutput = true;
                processInfoExe.RedirectStandardError = true;
                processInfoExe.CreateNoWindow = true;
                using (var processExe = new Process())
                {
                    processExe.StartInfo = processInfoExe;
                    processExe.Start();
                    // In this example, we don't try to pass any input
                    // to the program, but that is of course also
                    // necessary. We would do that here, using
                    // processExe.StandardInput.WriteLine(), similar
                    // to above.

                    // We then read the output of the program:
                    var lines = new List<string>();
                    while (!processExe.StandardOutput.EndOfStream)
                    {
                        lines.Add(processExe.StandardOutput.ReadLine());
                    }
                    string outputText = "";
                    foreach (var line in lines)
                    {
                        outputText = outputText + line + "\n";
                    }
                    System.IO.File.WriteAllText(workingFolder + "Output.txt", outputText);
                }
            }
        }
	}
}

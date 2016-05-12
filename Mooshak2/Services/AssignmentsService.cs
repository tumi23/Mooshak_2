using System;
using System.Collections.Generic;
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
        public void AssignmentDelete(int id) //Eyðir assignment gefið útfrá Id og með því entry úr assignmentList, Sýnum GradeList, 
        {
            //Eyðir öllum milestones tengd Assignmentinu sem er gefið í gegnum Id
            List<Milestone> MilestoneRemoval = new List<Milestone>();
            MilestoneRemoval = GetAllMilestoneEntries(id);
            foreach (var milestone in MilestoneRemoval)
            {
                MilestoneDelete(milestone.Id);
            }

            //Eyðir öllum AssignmentGradeList entries tengd Assignmentinu sem er gefið í gegnum Id
            List<AssignmentGradeList> AssignmentGradeListRemoval = new List<AssignmentGradeList>();
            AssignmentGradeListRemoval = GetAllAssignmentGradeEntries(id);
            foreach (var assignmentGradeEntry in AssignmentGradeListRemoval)
            {
                db.AssignmentGradeList.Remove(assignmentGradeEntry);
            }

            //Eyðir öllum AssignmentList entries tengd Assignmentinu sem er gefið í gegnum Id
            List<AssignmentList> AssignmentListRemoval = new List<AssignmentList>();
            AssignmentListRemoval = GetAllAssignmentListEntries(id);
            foreach (var assignListEntry in AssignmentListRemoval)
            {
                db.AssignmentList.Remove(assignListEntry);
            }

            Assignment assignment = db.Assignment.Find(id);
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
                assignmentGradeListEntry.Add(new AssignmentGradeList { Id = assignmentListGrade.Id, assignmentId = assignmentListGrade.assignmentId, userId = assignmentListGrade.userId, grade = assignmentListGrade.grade});
            }
            return assignmentGradeListEntry;
        }

        List<AssignmentList> GetAllAssignmentListEntries(int assignmentId)
        {
            var query = from assignList in db.AssignmentList
                        where assignList.AssignmentId == assignmentId
                        select assignList;
            List<AssignmentList> assignmentListEntry = new List<AssignmentList>();
            foreach (var assignmentList in query)
            {
                assignmentListEntry.Add(new AssignmentList { Id = assignmentList.Id, courseId = assignmentList.courseId, AssignmentId = assignmentList.AssignmentId });
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
                milestoneGradeListEntry.Add(new MilestoneGradeList { Id = milestoneGradeList.Id, userId = milestoneGradeList.userId, milestoneId = milestoneGradeList.milestoneId, grade = milestoneGradeList.grade });
            }
            return milestoneGradeListEntry;
        }

	}
}

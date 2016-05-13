using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooshak2.DAL;
using Mooshak2.Models;
using Mooshak2.Services;

namespace Mooshak2.Services
{
    public class UserService
    {
        private Context db = new Context();
        private CourseService cService = new CourseService();

        public  AddCourseViewModel GetDropDownListCourses()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<Course> course = cService.GetAllCourses();
            foreach (var item in course)
            {
                items.Add(new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Id), Selected = true });
            }

            var model = new AddCourseViewModel();
            {
                model.ListCourses = items;
            }
            return model;
        }

        public List<StudentCourseList> GetAllStudentCourseList()
        {
            return db.StudentCourseList.ToList();
        }

        public List<StudentCourseList> GetAllStudentList()
        {
            return db.StudentCourseList.ToList();
        }

        public UserDetailViewModel GetUserDetailView(AspNetUser user)
        {
            List<Course> courses = new List<Course>();
            courses = GetStudentCoursesByUserName(user.UserName);
            UserDetailViewModel userModel = new UserDetailViewModel();
            userModel.user = user;
            userModel.courses = courses;

            return userModel;
        }

        public List<Course> GetStudentCoursesByUserName(string userName)
        {
            var query = from course in db.Course
                        join stdntCrsLst in db.StudentCourseList on course.Id equals stdntCrsLst.courseId
                        where (stdntCrsLst.UserName == userName)
                        select course;
            List<Course> courseList = new List<Course>();
            foreach (var course in query)
            {
                courseList.Add(new Course { Id = course.Id, Name = course.Name, Description = course.Description});
            }
            return courseList;
        }

        public void UserDelete(AspNetUser user)//Deletear sjálfum sér og StudentCourseList entryum tengd sjálfum sér, öllum  AssignmentGrades og MilestoneGrades tengd sjálfum sér
        {
            List<AssignmentGradeList> assignGradeListDeletion = new List<AssignmentGradeList>();
            assignGradeListDeletion = GetAssignGradeEntries(user.UserName);
            if (assignGradeListDeletion != null)
            {
                foreach (var assignGrade in assignGradeListDeletion)
                {
                    db.AssignmentGradeList.Remove(assignGrade);
                }
            }

            List<MilestoneGradeList> milestoneGradeDeletion = new List<MilestoneGradeList>();
            milestoneGradeDeletion = GetMilestoneGradeEntries(user.UserName);
            if (milestoneGradeDeletion != null)
            {
                foreach (var milestoneGrade in milestoneGradeDeletion)
                {
                    db.MilestoneGradeList.Remove(milestoneGrade);
                }
            }

            List<StudentCourseList> studentCourseDeletion = new List<StudentCourseList>();
            studentCourseDeletion = GetStudentCourseEntries(user.UserName);
            if (studentCourseDeletion != null)
            {
                foreach (var studentCourse in studentCourseDeletion)
                {
                    db.StudentCourseList.Remove(studentCourse);
                }
            }

            db.AspNetUsers.Remove(user);
            db.SaveChanges();
        }

        public List<MilestoneGradeList> GetMilestoneGradeEntries(string userName)
        {
            var query = from milestoneGrdLst in db.MilestoneGradeList
                        where (milestoneGrdLst.UserName == userName)
                        select milestoneGrdLst;
            List<MilestoneGradeList> gradeList = new List<MilestoneGradeList>();
            foreach (var grade in query)
            {
                gradeList.Add(new MilestoneGradeList { Id = grade.Id, UserName = grade.UserName, milestoneId = grade.milestoneId, grade = grade.grade });
            }
            return gradeList;
        }

        public List<AssignmentGradeList> GetAssignGradeEntries(string userName)
        {
            var query = from assignGrdLst in db.AssignmentGradeList
                        where (assignGrdLst.UserName == userName)
                        select assignGrdLst;
            List<AssignmentGradeList> gradeList = new List<AssignmentGradeList>();
            foreach (var grade in query)
            {
                gradeList.Add(new AssignmentGradeList { Id = grade.Id, UserName = grade.UserName, assignmentId = grade.assignmentId, grade = grade.assignmentId });
            }
            return gradeList;
        }

        public List<StudentCourseList> GetStudentCourseEntries(string userName)
        {
            var query = from stdntCrsLst in db.StudentCourseList
                        where (stdntCrsLst.UserName == userName)
                        select stdntCrsLst;
            List<StudentCourseList> stdntList = new List<StudentCourseList>();
            foreach (var stdnt in query)
            {
                stdntList.Add(new StudentCourseList { Id = stdnt.Id, UserName = stdnt.UserName, courseId = stdnt.courseId});
            }
            return stdntList;
        }
    }
}
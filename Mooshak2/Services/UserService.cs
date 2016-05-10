using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mooshak2.DBL;

namespace Mooshak2.Services
{
    public class UserService
    {
        private Context db = new Context();

        public List<AspNetUser> GetUsersInCourse()
        {
            var AssiList = GetAllAssignments();
            var CoursList = GetAllCourses();
            var StuList = GetAllStudentList();
            var 

            List<AspNetUser> model = new List<AspNetUser>();

            var UserEntity = from user in 
                             join 
        }



        public List<Assignment> GetAllAssignments()
        {
            return db.Assignments.ToList();
        }

        public List<Course> GetAllCourses()
        {
            return db.Courses.ToList();
        }

        public List<StudentCourseList> GetAllStudentList()
        {
            return db.StudentCourseLists.ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mooshak2.DAL;

namespace Mooshak2.Services
{
    public class UserService
    {
        private Context db = new Context();

        /*public List<AspNetUser> GetUsersInCourse()
        {
            var AssiList = GetAllAssignments();
            var CoursList = GetAllCourses();
            var StuList = GetAllStudentList();
            var 

            List<AspNetUser> model = new List<AspNetUser>();

            var UserEntity = from user in 
                             join 
        }*/



        public List<Assignment> GetAllAssignments()
        {
            return db.Assignment.ToList();
        }

        public List<Course> GetAllCourses()
        {
            return db.Course.ToList();
        }

        public List<StudentCourseList> GetAllStudentList()
        {
            return db.StudentCourseList.ToList();
        }
    }
}
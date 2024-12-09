using CourseManager.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CourseManager.Repository
{
    //Repository has nothing to do with Caliburn
    internal class CourseCommand
    {
        private string _connectionString;

        public CourseCommand(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<CourseModel> GetList()
        {
            List<CourseModel> courses = new List<CourseModel>();

            var sql = "Course_GetList"; //stored procedure

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                courses = connection.Query<CourseModel>(sql).ToList(); //using Dapper
            }

            return courses;
        }
    }
}

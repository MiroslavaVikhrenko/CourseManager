using CourseManager.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace CourseManager.Repository
{
    internal class EnrollmentCommand
    {
        private string _connectionString;

        public EnrollmentCommand(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<EnrollmentModel> GetList()
        {
            List<EnrollmentModel> enrollments = new List<EnrollmentModel>();

            var sql = "Enrollments_GetList"; //stored procedure

            using (SqlConnection connection = new SqlConnection(_connectionString)) //sqlclient
            {
                enrollments = connection.Query<EnrollmentModel>(sql).ToList(); //dapper
            }

            //loop over all enrollments
            foreach (var enrollment in enrollments) 
            {
                enrollment.IsCommitted = true; //that's how we differentiate between what was saved and what is new
            }

            return enrollments;
        }

        public void Upsert(EnrollmentModel enrollmentModel)
        {
            var sql = "Enrollments_Upsert";

            //get Windows User
            var userId = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();

            //we have an EnrollmentType - we're gonna create a datatable
            var dataTable = new DataTable(); //using System.Data

            dataTable.Columns.Add("EnrollmentId", typeof(int));
            dataTable.Columns.Add("StudentId", typeof(int));
            dataTable.Columns.Add("CourseId", typeof(int));

            dataTable.Rows.Add(enrollmentModel.EnrollmentId, enrollmentModel.StudentId, enrollmentModel.CourseId); //that's what is gonna to be upserted

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { @EnrollmentType = dataTable.AsTableValuedParameter("EnrollmentType"), @UserId = userId}, commandType: CommandType.StoredProcedure); 
                //parameters from stored procedure (enrollment type and user id = windows user)
            }
        }
    }
}

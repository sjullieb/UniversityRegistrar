using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace UniversityRegistrar.Models
{
    public class Student
    {
        private int _id;
        private string _name;
        private DateTime _enrollmentDate;

        public Student(string name, DateTime enrollmentDate, int id = 0)
        {
            _name = name;
            _enrollmentDate = enrollmentDate;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public DateTime GetEnrollmentDate()
        {
            return _enrollmentDate;
        }

        public int GetId()
        {
            return _id;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Student> GetAll()
        {
            List<Student> allStudents = new List<Student> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                DateTime studentEnrollmentDate = rdr.GetDateTime(2);
                Student newStudent = new Student(studentName, studentEnrollmentDate, studentId);
                allStudents.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        public static Student Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int studentId = 0;
            string studentName = "";
            DateTime studentEnrollmentDate = DateTime.Now;
            while(rdr.Read())
            {
                studentId = rdr.GetInt32(0);
                studentName = rdr.GetString(1);
                studentEnrollmentDate = rdr.GetDateTime(2);
            }
            Student newStudent = new Student(studentName, studentEnrollmentDate, studentId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newStudent;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students (name, enrollmentDate) VALUES (@name, @enrollmentDate);";
            MySqlParameter name = new MySqlParameter();
            MySqlParameter enrollmentDate = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            enrollmentDate.ParameterName = "@enrollmentDate";
            enrollmentDate.Value = this._enrollmentDate;
            cmd.Parameters.Add(name);
            cmd.Parameters.Add(enrollmentDate);
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM students WHERE id = @studentId; DELETE FROM students_courses WHERE student_id = @studentId", conn);
            MySqlParameter studentIdParameter = new MySqlParameter();
            studentIdParameter.ParameterName = "@studentId";
            studentIdParameter.Value = this.GetId();
            cmd.Parameters.Add(studentIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Edit()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE students SET name = @name, enrollmentDate = @enrollmentDate WHERE id = @studentId");
            MySqlParameter name = new MySqlParameter();
            MySqlParameter enrollmentDate = new MySqlParameter();
            MySqlParameter studentId = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            enrollmentDate.ParameterName = "@enrollmentDate";
            enrollmentDate.Value = this._enrollmentDate;
            studentId.ParameterName = "@studentId";
            studentId.Value = this._id;
            cmd.Parameters.Add(name);
            cmd.Parameters.Add(enrollmentDate);
            cmd.Parameters.Add(studentId);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
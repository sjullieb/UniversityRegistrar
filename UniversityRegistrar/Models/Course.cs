using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace UniversityRegistrar.Models
{
  public class Course
  {
    private int _id;
    private string _name;
    private string _number;

    public Course(string name, string number, int id = 0)
    {
      _name = name;
      _number = number;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetNumber()
    {
      return _number;
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
      cmd.CommandText = @"DELETE FROM courses;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id = (@Id);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@Id";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int courseId = 0;
      string courseName = "";
      string courseNumber = "";
      while(rdr.Read())
      {
        courseId = rdr.GetInt32(0);
        courseName = rdr.GetString(1);
        courseNumber = rdr.GetString(2);
      }
      Course newCourse = new Course(courseName, courseNumber, courseId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCourse;
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";
      var rdr = cmd.ExecuteReader()as MySqlDataReader;
      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);
        Course newCourse = new Course(courseName, courseNumber, courseId);
        allCourses.Add(newCourse);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCourses;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses (name, number) VALUES (@name, @number);";
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);
      MySqlParameter number = new MySqlParameter();
      number.ParameterName = "@number";
      number.Value = this._number;
      cmd.Parameters.Add(number);
      cmd.ExecuteNonQuery();
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
      MySqlCommand cmd = new MySqlCommand("DELETE FROM courses WHERE id = @courseId; DELETE FROM students_courses WHERE course_id = @courseId", conn);
      MySqlParameter courseId = new MySqlParameter();
      courseId.ParameterName = "@courseId";
      courseId.Value = "@courseId";
      cmd.Parameters.Add(courseId);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Edit()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand("UPDATE  courses SET name = @name, number = @number WHERE id = @courseId", conn);
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      MySqlParameter number = new MySqlParameter();
      number.ParameterName = "@number";
      number.Value = this._number;
      MySqlParameter courseId = new MySqlParameter();
      courseId.ParameterName = "@courseId";
      courseId.Value = "@courseId";
      cmd.Parameters.Add(name);
      cmd.Parameters.Add(number);
      cmd.Parameters.Add(courseId);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId().Equals(newCourse.GetId());
        bool nameEquality = this.GetName().Equals(newCourse.GetName());
        bool numberEquality = this.GetNumber().Equals(newCourse.GetNumber());
        return (idEquality && nameEquality && numberEquality);
      }
    }
  }
}

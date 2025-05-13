using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;

namespace appCRUD
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=C:\Users\user\Desktop\Software CMETB\SQLiteDatabaseBrowserPortable\SQLiteDatabaseBrowserPortable\college.db;Version=3;";
        //string connectionString = @"Data Source=C:\Users\user\Desktop\Software CMETB\SQLiteDatabaseBrowserPortable\SQLiteDatabaseBrowserPortable\students.db;Version=3;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {


        }

        private DataTable ReadSelectedRecord(string SQLQuery)
        {
            try
            {
                // Create a DBConnection
                SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
                dbConnection.Open();
                string selectQuery = SQLQuery;


                // Create a SQLite Command using our Query
                SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, dbConnection);
                SQLiteDataReader reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
                //reader.Read();

                DataTable dataTable = new DataTable();

                //Load the data into the DataTable
                dataTable.Load(reader);


                return dataTable;

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        private void PopulateCoursesComboBox(DataTable dataTable)
        {
            cboCourses.DataSource = dataTable;
            // What column to display in the dropdown box
            cboCourses.DisplayMember = "course_name";
            cboCourses.ValueMember = "id";
        }

        private void btnReadCourses_Click(object sender, EventArgs e)
        {
            // Read all records for the Courses table
            string SQLQuery = "SELECT * FROM courses";
            DataTable courseDataTable = ReadSelectedRecord(SQLQuery);

            dataGridView1.DataSource = courseDataTable;
            PopulateCoursesComboBox(courseDataTable);

        }

        private void btnReadStudents_Click(object sender, EventArgs e)
        {
            string SQLQuery = "SELECT * FROM students";
            //string SQLQuery = textSQLQuery.Text;

            DataTable studentDataTable = ReadSelectedRecord(SQLQuery);
            dataGridView1.DataSource = studentDataTable;


        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string SQLQuery = textSQLQuery.Text;
            ReadSelectedRecord(SQLQuery);
        }

        private void lblCourse_Click(object sender, EventArgs e)
        {

        }

        private void btnAddCourse_Click(object sender, EventArgs e)
        {
            string courseName = textCourseName.Text;
            double price = Convert.ToDouble(txtPrice.Text);

            // Validate further before adding the course

            //AddNewCourse(courseName, price);

            SQLiteCommand insertCourseCommand = CreateInsertCourseCommand(courseName, price);
            if (AddNewRecord(insertCourseCommand))
            {
                MessageBox.Show("Course added successfully");
            }
            else
            {
                MessageBox.Show("Error adding course");
            }

        }

        private SQLiteCommand CreateInsertCourseCommand(string courseName, double price)
        {

            string insertQuery = @$"INSERT INTO courses (course_name, price) VALUES ($courseName,$price)";

            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, dbConnection);

            // The values must be validated outsde of the method
            insertCommand.Parameters.AddWithValue("$courseName", courseName);
            insertCommand.Parameters.AddWithValue("$price", price);

            return insertCommand;
        }

        private SQLiteCommand CreateInsertStudentCommand(string enrollmentDate, int marks, string grade, string firstName, string lastName, int courseId)
        {
            string insertQuery = @$"INSERT INTO students (enrollment_date, marks, grade, first_name, last_name, course_id) 
                                    VALUES ($enrollmentDate,$marks, $grade, $firstName, $lastName, $courseId)";
            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, dbConnection);
            // The values must be validated outsde of the method
            insertCommand.Parameters.AddWithValue("$enrollmentDate", enrollmentDate);
            insertCommand.Parameters.AddWithValue("$marks", marks);
            insertCommand.Parameters.AddWithValue("$grade", grade);
            insertCommand.Parameters.AddWithValue("$firstName", firstName);
            insertCommand.Parameters.AddWithValue("$lastName", lastName);
            insertCommand.Parameters.AddWithValue("$courseId", courseId);
            return insertCommand;
        }

        private bool AddNewRecord(SQLiteCommand insertCommand)
        {

            bool retValue = false;

            try
            {

                insertCommand.Connection.Open();
                insertCommand.ExecuteNonQuery(CommandBehavior.CloseConnection);

                retValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return retValue;
        }

        private void AddNewCourse(string courseName, double price)
        {

            try
            {
                // Create a Databased Connection based on our connection string
                SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
                dbConnection.Open();

                // Create a SQLite Command using our Query

                string insertQuery = @$"INSERT INTO courses (course_name, price) VALUES ($courseName,$price)";



                // Create a SQLite Command using our Query
                SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, dbConnection);

                // The values must be validated outsde of the method
                insertCommand.Parameters.AddWithValue("$courseName", courseName);
                insertCommand.Parameters.AddWithValue("$price", price);

                int numberRow = insertCommand.ExecuteNonQuery(CommandBehavior.CloseConnection);
                MessageBox.Show($"Number of rows affected: {numberRow}");


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AddNewStudent(string enrollmentDate, int marks, string grade, string firstName, string lastName, int courseId)
        {
            try
            {
                // Create a Databased Connection based on our connection string
                SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
                dbConnection.Open();


                // Create a SQLite Command using our Query

                string insertQuery = @$"INSERT INTO students (enrollment_date, marks, grade, first_name, last_name, course_id) 
                                    VALUES ($enrollmentDate,$marks, $grade, $firstName, $lastName, $courseId)";


                // Create a SQLite Command using our Query
                SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, dbConnection);


                // The values must be validated outsde of the method
                insertCommand.Parameters.AddWithValue("enrollmentDate", enrollmentDate);
                insertCommand.Parameters.AddWithValue("$marks", marks);
                insertCommand.Parameters.AddWithValue("$grade", grade);
                insertCommand.Parameters.AddWithValue("$firstName", firstName);
                insertCommand.Parameters.AddWithValue("$lastName", lastName);
                insertCommand.Parameters.AddWithValue("$courseId", courseId);


                int numberRow = insertCommand.ExecuteNonQuery(CommandBehavior.CloseConnection);
                MessageBox.Show($"Number of rows affected: {numberRow}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            int courseId = Convert.ToInt32(cboCourses.SelectedValue);
            string enrollmentDate = txtStartDate.Text;
            int marks = Convert.ToInt32(txtMarks.Text);
            string grade = txtGrade.Text;


            //AddNewStudent(enrollmentDate, marks, grade, firstName, lastName, courseId);
            SQLiteCommand insertStudent = CreateInsertStudentCommand(enrollmentDate, marks, grade, firstName, lastName, courseId);
            AddNewRecord(insertStudent);


        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void btnDeleteCourse_Click(object sender, EventArgs e)
        {
            // Delete thr selected course
            // What ID is currently selected

            int recordID = Convert.ToInt32(cboCourses.SelectedValue);
            string deleteQuery = $"DELETE FROM courses WHERE id = {recordID}";

            // Create a SQLite Connection
            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);

            // Create a SQLite Command using our Query
            SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, dbConnection);

            dbConnection.Open();
            MessageBox.Show($"Delete course with ID: {recordID}?");
            int rows = deleteCommand.ExecuteNonQuery(CommandBehavior.CloseConnection);

            MessageBox.Show($"Number of deleted records: {rows}");
        }

        private void btnReadRecord_Click(object sender, EventArgs e)
        {
            
        }
    }
}

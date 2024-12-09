using Caliburn.Micro;
using CourseManager.Models;
using CourseManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CourseManager.ViewModels
{
    internal class MainViewModel : Screen //using Caliburn.Micro
    {
        //a collection for a viewmodel which allows notifying changes
        private BindableCollection<EnrollmentModel> _enrollments = new BindableCollection<EnrollmentModel>();
        private BindableCollection<StudentModel> _students = new BindableCollection<StudentModel>();
        private BindableCollection<CourseModel> _courses = new BindableCollection<CourseModel>();

        private readonly string _connectionString = @"Data Source=localhost;Initial Catalog=CourseReport;Integrated Security=True;Encrypt=False";

        private string _appStatus;

        private EnrollmentModel _selectedEnrollment;

        private EnrollmentCommand _enrollmentCommand;

        //constructor
        public MainViewModel() //load the data from db
        {
            SelectedEnrollment = new EnrollmentModel(); //represents the enrollment on which we click on the listview

            try
            {
                _enrollmentCommand = new EnrollmentCommand(_connectionString);
                Enrollments.AddRange(_enrollmentCommand.GetList());

                StudentCommand studentCommand = new StudentCommand(_connectionString);
                Students.AddRange(studentCommand.GetList()); //the list that is coming from the db we're gonna add to Students collection
                                                             //and this list is going to bound to combobox in UI (ItemSource)

                //similarly for courses
                CourseCommand courseCommand = new CourseCommand(_connectionString);
                Courses.AddRange(courseCommand.GetList());
            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message); 
            }
        }

        public CourseModel SelectedEnrollmentCourse
        {
            get
            {
                try
                {
                    //turn _courses into dictionary
                    //enrollment IDs will be the keys by which you can find the rest information
                    var courseDictionary = _courses.ToDictionary(b => b.CourseId);

                    //if there is a selected enrollments (the enrollment on which we click on the listview) and its course is there (if there is a course for that enrollment)
                    if (SelectedEnrollment != null && courseDictionary.ContainsKey(SelectedEnrollment.CourseId))
                    {
                        return courseDictionary[SelectedEnrollment.CourseId];
                    }
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
                return null;
            }
            set
            {
                try
                {
                    var selectedEnrollmentCourse = value;
                    SelectedEnrollment.CourseId = selectedEnrollmentCourse.CourseId;
                    //notify the Ui that selected enrollment just changed
                    NotifyOfPropertyChange(() => SelectedEnrollment);
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
            }
        }

        public StudentModel SelectedEnrollmentStudent
        {
            get
            {
                try
                {
                    //turn _students into dictionary
                    //enrollment IDs will be the keys by which you can find the rest information
                    var studentDictionary = _students.ToDictionary(b => b.StudentId);

                    //if there is a selected enrollments (the enrollment on which we click on the listview) and its student is there (if there is a studentfor that enrollment)
                    if (SelectedEnrollment != null && studentDictionary.ContainsKey(SelectedEnrollment.StudentId))
                    {
                        return studentDictionary[SelectedEnrollment.StudentId];
                    }
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
                return null;
            }
            set
            {
                try
                {
                    var selectedEnrollmentStudent = value;
                    SelectedEnrollment.StudentId = selectedEnrollmentStudent.StudentId;
                    //notify the Ui that selected enrollment just changed
                    NotifyOfPropertyChange(() => SelectedEnrollment);
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
            }
        }

        public BindableCollection<EnrollmentModel> Enrollments
        {
            get { return _enrollments; }
            set { _enrollments = value; }
        }
        public BindableCollection<StudentModel> Students
        {
            get { return _students; }
            set { _students = value; }
        }

        public BindableCollection<CourseModel> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }
        public string AppStatus
        {
            get { return _appStatus;}
            set { _appStatus = value;}
        }

        public EnrollmentModel SelectedEnrollment //telling what is selected in the listview
        {
            get { return _selectedEnrollment;}
            set 
            { 
                _selectedEnrollment = value;
                NotifyOfPropertyChange(() => SelectedEnrollment);
                NotifyOfPropertyChange(() => SelectedEnrollmentCourse);
            }
        }

        //whenever app status changes I want AppStatus property to know and alert a UI (StatusBar)
        private void UpdateAppStatus(string message)
        {
            AppStatus = message;
            NotifyOfPropertyChange(() => AppStatus);
        }
    }
}

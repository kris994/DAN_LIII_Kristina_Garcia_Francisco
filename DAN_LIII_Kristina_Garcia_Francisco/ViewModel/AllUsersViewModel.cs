using DAN_LIII_Kristina_Garcia_Francisco.Commands;
using DAN_LIII_Kristina_Garcia_Francisco.Model;
using DAN_LIII_Kristina_Garcia_Francisco.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace DAN_LIII_Kristina_Garcia_Francisco.ViewModel
{
    /// <summary>
    /// Main Window view model
    /// </summary>
    class AllUsersViewModel : BaseViewModel
    {
        AllUsers allUsers;
        Manager manWindow;
        EnterSalaryValue salaryWindow;
        Service service = new Service();
        /// <summary>
        /// Background worker
        /// </summary>
        private readonly BackgroundWorker bgWorker = new BackgroundWorker();
        /// <summary>
        /// Check if background worker is running
        /// </summary>
        private bool _isRunning = false;

        #region Constructor
        /// <summary>
        /// Constructor with AllUsers param
        /// </summary>
        /// <param name="AllUsers">opens the all uers window</param>
        public AllUsersViewModel(AllUsers usersOpen)
        {
            allUsers = usersOpen;
            AllInfoManagerList = service.GetAllManagersInfo().ToList();
            AllInfoEmployeeList = service.GetAllEmployeesInfo().ToList();
            ManagerList = service.GetAllManagers().ToList();
            EmployeeList = service.GetAllEmployees().ToList();
            UserList = service.GetAllUsers().ToList();
        }

        /// <summary>
        /// Constructor with manager window param
        /// </summary>
        /// <param name="AllUsers">opens the manager window</param>
        public AllUsersViewModel(Manager usersOpen)
        {
            manWindow = usersOpen;
            ManagersEmployees = service.GetAllEmployeesOnSpecificFloor(service.GetManagerFloorNumber(LoggedUser.CurrentUser.UserID));
            EmployeesMonotorReport = service.GetAllEmployeesMonitorReportOnSpecificFloor(service.GetManagerFloorNumber(LoggedUser.CurrentUser.UserID));           
        }

        /// <summary>
        /// Constructor with EnterSalaryValue window param
        /// </summary>
        /// <param name="salaryOpen">opens the salary window</param>
        /// <param name="employeeEdit">gets the employee info that is being edited</param>
        public AllUsersViewModel(EnterSalaryValue salaryOpen, vwEmployee employeeEdit)
        {
            employee = employeeEdit;
            salaryWindow = salaryOpen;
        }

        /// <summary>
        /// Constructor with AllEnterSalaryValue window param
        /// </summary>
        /// <param name="salaryOpen">opens the salary window</param>
        public AllUsersViewModel(EnterSalaryValue salaryOpen)
        {
            ProgressBarVisibility = Visibility.Collapsed;
            salaryWindow = salaryOpen;
            EmployeesMonotorReport = service.GetAllEmployeesMonitorReportOnSpecificFloor(service.GetManagerFloorNumber(LoggedUser.CurrentUser.UserID));
            bgWorker.DoWork += WorkerOnDoWork;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.ProgressChanged += WorkerOnProgressChanged;
            bgWorker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
            InfoLabelBG = "#17a2b8";
            InfoLabel = "Salaries calculation";
        }
        #endregion

        #region Property
        /// <summary>
        /// List of users
        /// </summary>
        private List<tblUser> userList;
        public List<tblUser> UserList
        {
            get
            {
                return userList;
            }
            set
            {
                userList = value;
                OnPropertyChanged("UserList");
            }
        }

        /// <summary>
        /// List of managers
        /// </summary>
        private List<tblManager> managerList;
        public List<tblManager> ManagerList
        {
            get
            {
                return managerList;
            }
            set
            {
                managerList = value;
                OnPropertyChanged("ManagerList");
            }
        }

        /// <summary>
        /// List of employee
        /// </summary>
        private List<tblEmployee> employeeList;
        public List<tblEmployee> EmployeeList
        {
            get
            {
                return employeeList;
            }
            set
            {
                employeeList = value;
                OnPropertyChanged("EmployeeList");
            }
        }

        /// <summary>
        /// List of employees of the manager
        /// </summary>
        private List<vwEmployee> managersEmployees;
        public List<vwEmployee> ManagersEmployees
        {
            get
            {
                return managersEmployees;
            }
            set
            {
                managersEmployees = value;
                OnPropertyChanged("ManagersEmployees");
            }
        }

        /// <summary>
        /// List of specific employees
        /// </summary>
        private List<vwEmployee> employeesMonotorReport;
        public List<vwEmployee> EmployeesMonotorReport
        {
            get
            {
                return employeesMonotorReport;
            }
            set
            {
                employeesMonotorReport = value;
                OnPropertyChanged("EmployeesMonotorReport");
            }
        }

        /// <summary>
        /// List of managers info view
        /// </summary>
        private List<vwManager> allInfoManagerList;
        public List<vwManager> AllInfoManagerList
        {
            get
            {
                return allInfoManagerList;
            }
            set
            {
                allInfoManagerList = value;
                OnPropertyChanged("AllInfoManagerList");
            }
        }

        /// <summary>
        /// List of employee info view
        /// </summary>
        private List<vwEmployee> allInfoEmployeeList;
        public List<vwEmployee> AllInfoEmployeeList
        {
            get
            {
                return allInfoEmployeeList;
            }
            set
            {
                allInfoEmployeeList = value;
                OnPropertyChanged("AllInfoEmployeeList");
            }
        }

        /// <summary>
        /// Specific Manager
        /// </summary>
        private vwManager manager;
        public vwManager Manager
        {
            get
            {
                return manager;
            }
            set
            {
                manager = value;
                OnPropertyChanged("Manager");
            }
        }

        /// <summary>
        /// Specific Employee
        /// </summary>
        private vwEmployee employee;
        public vwEmployee Employee
        {
            get
            {
                return employee;
            }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }

        /// <summary>
        /// Info label
        /// </summary>
        private string infoLabel;
        public string InfoLabel
        {
            get
            {
                return infoLabel;
            }
            set
            {
                infoLabel = value;
                OnPropertyChanged("InfoLabel");
            }
        }

        /// <summary>
        /// Info label background
        /// </summary>
        private string infoLabelBG;
        public string InfoLabelBG
        {
            get
            {
                return infoLabelBG;
            }
            set
            {
                infoLabelBG = value;
                OnPropertyChanged("InfoLabelBG");
            }
        }

        /// <summary>
        /// Salary Info label
        /// </summary>
        private string salaryInfoLabel;
        public string SalaryInfoLabel
        {
            get
            {
                return salaryInfoLabel;
            }
            set
            {
                salaryInfoLabel = value;
                OnPropertyChanged("SalaryInfoLabel");
            }
        }

        /// <summary>
        /// Salary value
        /// </summary>
        private int salaryValue;
        public int SalaryValue
        {
            get
            {
                return salaryValue;
            }
            set
            {
                salaryValue = value;
                OnPropertyChanged("SalaryValue");
            }
        }

        /// <summary>
        /// The progress bar property
        /// </summary>
        private int currentProgress;
        public int CurrentProgress
        {
            get
            {
                return currentProgress;
            }
            set
            {
                if (currentProgress != value)
                {
                    currentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }

        /// <summary>
        /// The progress bar property
        /// </summary>
        private Visibility progressBarVisibility;
        public Visibility ProgressBarVisibility
        {
            get
            {
                return progressBarVisibility;
            }
            set
            {
                progressBarVisibility = value;
                OnPropertyChanged("ProgressBarVisibility");
            }
        }
        #endregion

        #region Background worker
        /// <summary>
        /// Updates the progress bar and prints the value
        /// </summary>
        /// <param name="sender">objecy sender</param>
        /// <param name="e">progress changed event</param>
        private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
            SalaryInfoLabel = CurrentProgress + " %";
        }

        /// <summary>
        /// Method that the background worker executes
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">do work event</param>
        private void WorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            ProgressBarVisibility = Visibility.Visible;
            Random rng = new Random();
            // Restart progress
            bgWorker.ReportProgress(0);
            int counter = EmployeesMonotorReport.Count;
            for (int i = 1; i < counter + 1; i++)
            {
                Thread.Sleep(1000);
                // Calling ReportProgress() method raises ProgressChanged event
                // To this method pass the percentage of processing that is complete

                string salary = service.CalculateSalary(LoggedUser.CurrentUser.UserID, EmployeesMonotorReport[i - 1], SalaryValue).ToString();
                EmployeesMonotorReport[i - 1].Salary = salary;
                service.AddEmployee(EmployeesMonotorReport[i - 1]);

                if (i == counter)
                {
                    // 100% if all reports                   
                    bgWorker.ReportProgress(100);
                }
                else
                {
                    bgWorker.ReportProgress(100 / counter * i);
                }

                SalaryInfoLabel = "";
                _isRunning = false;

                // Cancel the asynchronous operation if still in progress
                if (bgWorker.IsBusy)
                {
                    bgWorker.CancelAsync();
                }
            }
        }

        /// <summary>
        /// Print the appropriate message depending how the worker finished.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">worker completed event</param>
        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                SalaryInfoLabel = e.Error.Message;
                _isRunning = false;
            }
            else
            {
                InfoLabelBG = "#28a745";
                InfoLabel = "Finished updaing the salaries";
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Command that tries to delete the user
        /// </summary>
        private ICommand deleteUser;
        public ICommand DeleteUser
        {
            get
            {
                if (deleteUser == null)
                {
                    deleteUser = new RelayCommand(param => DeleteUserExecute(), param => CanDeleteUserExecute());
                }
                return deleteUser;
            }
        }

        /// <summary>
        /// Executes the delete command
        /// </summary>
        public void DeleteUserExecute()
        {
            // Checks if the user really wants to delete the user
            var result = MessageBox.Show("Are you sure you want to delete the user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (Employee != null)
                    {
                        int userID = Employee.UserID;
                        service.DeleteUserEmployee(userID);
                        EmployeeList = service.GetAllEmployees().ToList();
                        AllInfoEmployeeList = service.GetAllEmployeesInfo().ToList();
                        UserList = service.GetAllUsers().ToList();

                    }
                    if (Manager != null)
                    {
                        int userID = Manager.UserID;
                        service.DeleteUserManager(userID);
                        ManagerList = service.GetAllManagers().ToList();
                        AllInfoManagerList = service.GetAllManagersInfo().ToList();
                        UserList = service.GetAllUsers().ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Checks if the user can be deleted
        /// </summary>
        /// <returns>true if possible</returns>
        public bool CanDeleteUserExecute()
        {
            if (UserList == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Command that tries to open the edit employee window
        /// </summary>
        private ICommand editUser;
        public ICommand EditUser
        {
            get
            {
                if (editUser == null)
                {
                    editUser = new RelayCommand(param => EditUserExecute(), param => CanEditUserExecute());
                }
                return editUser;
            }
        }

        /// <summary>
        /// Executes the edit command
        /// </summary>
        public void EditUserExecute()
        {
            try
            {
                if (Employee != null)
                {
                    AddEmployee addEmployee = new AddEmployee(Employee);
                    addEmployee.ShowDialog();

                    EmployeeList = service.GetAllEmployees().ToList();
                    AllInfoEmployeeList = service.GetAllEmployeesInfo().ToList();
                    UserList = service.GetAllUsers().ToList();
                }
                if (Manager != null)
                {
                    AddManager addManager = new AddManager(Manager);
                    addManager.ShowDialog();

                    ManagerList = service.GetAllManagers().ToList();
                    AllInfoManagerList = service.GetAllManagersInfo().ToList();
                    UserList = service.GetAllUsers().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if the report can be edited
        /// </summary>
        /// <returns>true if possible</returns>
        public bool CanEditUserExecute()
        {
            if (EmployeeList == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Command that tries to add a new employee
        /// </summary>
        private ICommand addNewEmployee;
        public ICommand AddNewEmployee
        {
            get
            {
                if (addNewEmployee == null)
                {
                    addNewEmployee = new RelayCommand(param => AddNewEmpoloyeeExecute(), param => CanAddNewEmployeeExecute());
                }
                return addNewEmployee;
            }
        }

        /// <summary>
        /// Executes the add Employee command
        /// </summary>
        private void AddNewEmpoloyeeExecute()
        {
            try
            {
                AddEmployee addEmployee = new AddEmployee();
                addEmployee.ShowDialog();
                if ((addEmployee.DataContext as AddUserViewModel).IsUpdateEmployee == true)
                {
                    EmployeeList = service.GetAllEmployees().ToList();
                    AllInfoEmployeeList = service.GetAllEmployeesInfo().ToList();
                    UserList = service.GetAllUsers().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to add the new employee
        /// </summary>
        /// <returns>true</returns>
        private bool CanAddNewEmployeeExecute()
        {
            if (ManagerList.Count == 0)
            {
                InfoLabelBG = "#17a2b8";
                InfoLabel = "Currently no Managers in the database";
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Command that tries to add a new manager
        /// </summary>
        private ICommand addNewManager;
        public ICommand AddNewManager
        {
            get
            {
                if (addNewManager == null)
                {
                    addNewManager = new RelayCommand(param => AddNewManagerExecute(), param => CanAddNewManagerExecute());
                }
                return addNewManager;
            }
        }

        /// <summary>
        /// Executes the add Manager command
        /// </summary>
        private void AddNewManagerExecute()
        {
            try
            {
                AddManager addManager = new AddManager();
                addManager.ShowDialog();
                if ((addManager.DataContext as AddUserViewModel).IsUpdateManager == true)
                {
                    ManagerList = service.GetAllManagers().ToList();
                    AllInfoManagerList = service.GetAllManagersInfo().ToList();
                    UserList = service.GetAllUsers().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to add the new manager
        /// </summary>
        /// <returns>true</returns>
        private bool CanAddNewManagerExecute()
        {
            return true;
        }

        /// <summary>
        /// Command that tries to calculate salary
        /// </summary>
        private ICommand calcSalary;
        public ICommand CalcSalary
        {
            get
            {
                if (calcSalary == null)
                {
                    calcSalary = new RelayCommand(param => CalcSalaryExecute(), param => CanCalcSalaryExecute());
                }
                return calcSalary;
            }
        }

        /// <summary>
        /// Executes the calc salary command
        /// </summary>
        private void CalcSalaryExecute()
        {
            try
            {
                EnterSalaryValue salaryValueWindow = new EnterSalaryValue(Employee);
                salaryValueWindow.ShowDialog();
                ManagersEmployees = service.GetAllEmployeesOnSpecificFloor(service.GetManagerFloorNumber(LoggedUser.CurrentUser.UserID));
                EmployeesMonotorReport = service.GetAllEmployeesMonitorReportOnSpecificFloor(service.GetManagerFloorNumber(LoggedUser.CurrentUser.UserID));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to add the new manager
        /// </summary>
        /// <returns>true</returns>
        private bool CanCalcSalaryExecute()
        {
            return true;
        }

        /// <summary>
        /// Command that tries to calculate all salary
        /// </summary>
        private ICommand calcAllSalary;
        public ICommand CalcAllSalary
        {
            get
            {
                if (calcAllSalary == null)
                {
                    calcAllSalary = new RelayCommand(param => CalcAllSalaryExecute(), param => CanCalcAllSalaryExecute());
                }
                return calcAllSalary;
            }
        }

        /// <summary>
        /// Executes the calc all salary command
        /// </summary>
        private void CalcAllSalaryExecute()
        {
            try
            {
                EnterSalaryValue salaryValueWindow = new EnterSalaryValue();
                salaryValueWindow.ShowDialog();
                ManagersEmployees = service.GetAllEmployeesOnSpecificFloor(service.GetManagerFloorNumber(LoggedUser.CurrentUser.UserID));
                EmployeesMonotorReport = service.GetAllEmployeesMonitorReportOnSpecificFloor(service.GetManagerFloorNumber(LoggedUser.CurrentUser.UserID));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to add the new manager
        /// </summary>
        /// <returns>true</returns>
        private bool CanCalcAllSalaryExecute()
        {
            return true;
        }

        /// <summary>
        /// Command that tries to save the salary
        /// </summary>
        private ICommand saveSalary;
        public ICommand SaveSalary
        {
            get
            {
                if (saveSalary == null)
                {
                    saveSalary = new RelayCommand(param => SaveSalaryExecute(), param => CanSaveSalaryeExecute());
                }
                return saveSalary;
            }
        }

        /// <summary>
        /// Tries the execute the save command
        /// </summary>
        private void SaveSalaryExecute()
        {
            try
            {
                if (!bgWorker.IsBusy && Employee == null)
                {
                    InfoLabelBG = "#17a2b8";
                    InfoLabel = "Calculating salary";
                    // This method will start the execution asynchronously in the background
                    bgWorker.RunWorkerAsync();
                    _isRunning = true;
                }
                else if (Employee != null)
                {
                    string salary = service.CalculateSalary(LoggedUser.CurrentUser.UserID, Employee, SalaryValue).ToString();
                    Employee.Salary = salary;
                    service.AddEmployee(Employee);
                    salaryWindow.Close();
                }
                else if (bgWorker.IsBusy)
                {
                    InfoLabelBG = "#ffc107";
                    InfoLabel = "Busy processing the request, please wait.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to save the employee
        /// </summary>
        protected bool CanSaveSalaryeExecute()
        {
            if (SalaryValue < 1 || SalaryValue > 1000)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
       
        /// <summary>
        /// Command that closes the window
        /// </summary>
        private ICommand cancel;
        public ICommand Cancel
        {
            get
            {
                if (cancel == null)
                {
                    cancel = new RelayCommand(param => CancelExecute(), param => CanCancelExecute());
                }
                return cancel;
            }
        }

        /// <summary>
        /// Executes the close command
        /// </summary>
        private void CancelExecute()
        {
            try
            {
                salaryWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to execute the close command
        /// </summary>
        /// <returns>true</returns>
        private bool CanCancelExecute()
        {
            return true;
        }

        /// <summary>
        /// Command that logs off the user
        /// </summary>
        private ICommand logoff;
        public ICommand Logoff
        {
            get
            {
                if (logoff == null)
                {
                    logoff = new RelayCommand(param => LogoffExecute(), param => CanLogoffExecute());
                }
                return logoff;
            }
        }

        /// <summary>
        /// Executes the logoff command
        /// </summary>
        private void LogoffExecute()
        {
            try
            {
                if (Application.Current.Windows.OfType<AllUsers>().FirstOrDefault() != null)
                {
                    allUsers.Close();
                }
                else if(Application.Current.Windows.OfType<Manager>().FirstOrDefault() != null)
                {
                    manWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to logoff
        /// </summary>
        /// <returns>true</returns>
        private bool CanLogoffExecute()
        {
            return true;
        }
        #endregion
    }
}


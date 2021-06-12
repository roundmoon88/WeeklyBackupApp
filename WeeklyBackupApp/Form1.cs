using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Globalization;
namespace WeeklyBackupApp
{
    /// <summary>
/// Revision History
/// Rev 01: Oct 28, enhance performance not log copied info for the first time backup for the current week
/// Rev 02
/// </summary>
/// 
///

    public partial class Form1 : Form
    {
        private string sourceLocation;
        private string destinationLocation;
        //private string commonRelativePath;
        private string exceptionNotice;
        private string weekIndexMappingWithDateDuration;
        private string computerLocation;
        private bool isBackupForNewWeek; //Rev 01
        private int sourceFileNum;
        private string curSourcePath;
        private string curSourceFile;
        private string currentDestinationPath;
        private string curDestinationFile;






        public Form1()
        {
            InitializeComponent();
            isBackupForNewWeek = false; //Rev 01
            GetSourceConfig();
            WeekIndexWithDuration();

            //test
            testTabPage(); //!!??
            //CopyWithProgress();
            this.label1.Text = "";
            this.label2.Text = "";

        }

        /// <summary>
        /// Method to get sources location
        /// </summary>
        private void GetSourceConfig()
        {

            NameValueCollection fileSources = ConfigurationManager.GetSection("fileSources") as NameValueCollection;
            computerLocation = fileSources["computerLocation"];
            if (computerLocation == "Home")
            {
                sourceLocation = fileSources["SourceLocation_Home"];
                destinationLocation = fileSources["DestinationLocation_Home"];
                exceptionNotice = fileSources["ExcepitonNotice_Home"];
                weekIndexMappingWithDateDuration = fileSources["WeekIndexMappingWithDateDuration_Home"];
            }
            else if (computerLocation == "Office")
            {
                //test DbConnection
                //DBConnectionTest(); //for testing

                sourceLocation = fileSources["SourceLocation_Office"];
                destinationLocation = fileSources["DestinationLocation_Office"];
                exceptionNotice = fileSources["ExcepitonNotice_Office"];
                weekIndexMappingWithDateDuration = fileSources["WeekIndexMappingWithDateDuration_Office"];
            }
            //string currentWeekNum = GetCurrentWeekString(9);
            string currentWeekNum = GetCurrentWeekString(GetCurrentWeek());
            destinationLocation = destinationLocation + currentWeekNum + "\\";

            sourceFileNum = GetFileNum();

            //this.progressBar1.Minimum = 1;
            //this.progressBar1.Maximum = sourceFileNum;
            this.progressBar2.Minimum = 1;
            this.progressBar2.Maximum = sourceFileNum;
            //create new folder for the current week
 
            NewFolderCreateForCurrentWeek();

        }
        /// <summary>
        /// create new folder for the current week if the folder does not exist
        /// </summary>
        private void NewFolderCreateForCurrentWeek()
        {
            if (!Directory.Exists(this.destinationLocation))
            {
                isBackupForNewWeek = true; //Rev 01
                Directory.CreateDirectory(this.destinationLocation);
            }
        }

        /// <summary>
        /// method to exceute file copy
        /// </summary>
        /// 
        /// <param name="strSourcePath"></param>
        public int ExecuteFileCopy(string strSourcePath)
        //public async Task<int> ExecuteFileCopyAsynch (string strSourcePath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(strSourcePath);
            if (directoryInfo.Exists)
            {
                //1)process files under current folder
                FileInfo[] fileInfos = directoryInfo.GetFiles("*.*");
                foreach (var file in fileInfos)
                {
                    string fileName = file.Name;
                    curDestinationFile = file.Name;
                    curSourceFile = file.Name;
                    long fileSize = file.Length;
                    DateTime modifiedDatetime = file.LastWriteTime;
                    currentDestinationPath = destinationLocation + GetRelateivePathName(file.FullName);
                    currentDestinationPath = currentDestinationPath.Replace(file.Name, "");

                    string destinationFileFullName = GetCorrepsondingDestinationFullName(file.FullName);

                    bool isChanged = IsFileModified(destinationFileFullName, fileSize, modifiedDatetime);

                    if (isChanged && !FileLogExclude())
                    {
                        //check the destination folder existence

                        //copy file to destination
                        bool overWriteInd = true;
                        if (!this.isFileLocked(file))
                        {
                            file.CopyTo(destinationFileFullName, overWriteInd);

                            //log the file to excel file
                            if (!this.isBackupForNewWeek) //Rev 01
                            {//Rev 01
                                DBConnection db = new DBConnection(this.computerLocation);

                                FileLogDomain fileLog = new FileLogDomain
                                {
                                    FileName = file.Name,
                                    FilePath = currentDestinationPath,
                                    //LogDate = DateTime.Now.Date,
                                    LogDate = DateTime.Now,
                                    TimeStamp = Utility.GetDateTimeStamp(),
                                    NewFileInd = 0

                                };
                                db.InsertLogFile(fileLog);
                            }//Rev 01

  
                        }
                            
                    }

                    //progressBar1.PerformStep();
                    //await ActuallyPerformStep(this.progressBar1);
                    //ActuallyPerformStep(progressBar1);
                    WatchingCopyFile(progress);

                }

                //2) process sub folders under current
                DirectoryInfo[] subfoders = directoryInfo.GetDirectories("*.*");
                foreach (var folder in subfoders)
                {
                    //var subfolderName = folder.Name;
                    var subfolderName = folder.FullName;
                    //var subfolderName = GetCorrepsondingDestinationFullName(folder.FullName);

                    ExecuteFileCopy(subfolderName);
                 //await   ExecuteFileCopyAsynch(subfolderName);
                }
                //3) Clean the temp file(s)
                string strCurrentDestinationFolder = GetCurrentDestinationFolder(strSourcePath);
                this.CleanTempFiles(strCurrentDestinationFolder);
            }
            else
            {
                Directory.CreateDirectory(strSourcePath);
                ExecuteFileCopy(strSourcePath);
               //await ExecuteFileCopyAsynch(strSourcePath);
                //directoryInfo.Create(); //!!!!???
            }
            return 0;
        }
        private string GetCurrentDestinationFolder(string strSourceFolderFullPathName)
        {
            //source path "e:\DailyBackup\10 Electricity"
            //destination path "C:\RiboBackup\42\10 Electricity"
            //generate the destination folder full path
            //a) remove source location
            var strPathName =strSourceFolderFullPathName.Substring(this.sourceLocation.Length, strSourceFolderFullPathName.Length - this.sourceLocation.Length); 
 
            //b) add destination location
            string strDestinationFolderFullPathName = this.destinationLocation + strPathName;

            //throw new NotImplementedException();
            return strDestinationFolderFullPathName;
        }
        /// <summary>
        /// Method to get the related destination file full name
        /// </summary>
        /// <param name="sourceFileFullName"></param>
        /// <returns>aaa dd</returns>
        public string GetCorrepsondingDestinationFullName(string sourceFileFullName)
        {
            string destinationFileFullName = "";
            string strRelativePathName;
            //int pos = sourceFileFullName.IndexOf(this.sourceLocation);
            //int len = sourceFileFullName.Length;

            strRelativePathName = GetRelateivePathName(sourceFileFullName);
            destinationFileFullName = destinationLocation + strRelativePathName;
            return destinationFileFullName;
        }
        private string GetRelateivePathName(string sourceFileFullName)
        {
            return sourceFileFullName.Substring(sourceLocation.Length, sourceFileFullName.Length - sourceLocation.Length);
        }
        /// <summary>
        /// Method to log message
        /// </summary>
        /// <param name="message"></param>
        public void MessageLog(string message)
        {
            using (StreamWriter streamWriter = new StreamWriter(this.exceptionNotice, true))
            {
                if (message != null)
                {
                    DateTime today = DateTime.Now;
                    string strMessage = getDateString(today);
                    //strMessage = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + ":" + message;
                    strMessage = strMessage + " " + message;

                    //streamWriter.WriteAsync(strMessage);
                    streamWriter.WriteLineAsync(strMessage);
                }
                    
            }
        }

        private string getDateString(DateTime dt)
        {
            string retVal = "";
            string year = dt.Year.ToString();

            string month = dt.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;

            string day = dt.Day.ToString();
            if (day.Length == 1)
                day = "0" + day;

            string hour = dt.Hour.ToString();
            if (hour.Length == 1)
                hour = "0" + hour;

            string minute = dt.Minute.ToString();
            if (minute.Length == 1)
                minute = "0" + minute;

            string second = dt.Second.ToString();
            if (second.Length == 1)
                second = "0" + second;

            string millisecond = dt.Millisecond.ToString();

            retVal = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second + " " + millisecond;
            return retVal;
        }
        /// <summary>
        /// Provide a list of Week Index With Duration
        /// </summary>
        public void WeekIndexWithDuration()
        {
            if (!File.Exists(this.weekIndexMappingWithDateDuration))
            //if (1 == 1)
            {//generate file 
                using (StreamWriter streamWriter = new StreamWriter(this.weekIndexMappingWithDateDuration, true))
                {
                    //                    //CultureInfo myCI = new CultureInfo("en-US");
                    //                    //Calendar myCal = myCI.Calendar;
                    //                    //// Gets the DTFI properties required by GetWeekOfYear.
                    //                    //CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                    //                    //DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

                    //                    //myCal.

                    //                    //streamWriter.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + ":" + message);
                    int year = DateTime.Today.Year;
                    int weekIndex = 0;
                    List<WeekIndexDateDuration> weeks = new List<WeekIndexDateDuration>();
                    DateTime firstDayOfYear = new DateTime(year, 1, 1);
                    DateTime firstDayOfNextYear = new DateTime(year + 1, 1, 1);
                    DateTime lastDayOfYear = firstDayOfNextYear.AddDays(-1);

                    DateTime curDate = new DateTime();
                    curDate = firstDayOfYear;
                    while (curDate >= firstDayOfYear && curDate <= lastDayOfYear)
                    {
                        WeekIndexDateDuration weekIndexDateDuration = new WeekIndexDateDuration();

                        weekIndexDateDuration.Index = weekIndex;
                        weekIndexDateDuration.StartDate = curDate;
                        //int indexOfWeek = (int) curDate.DayOfWeek;
                        int offset = 7 - ((int)curDate.DayOfWeek + 1);



                        //weekIndexDateDuration.EndDate = curDate.AddDays(7);
                        weekIndexDateDuration.EndDate = curDate.AddDays(offset);

                        weeks.Add(weekIndexDateDuration);
                        curDate = weekIndexDateDuration.EndDate.AddDays(1);
                        weekIndex++;
                    }


                    //output all the week index and its start and end date
                    for (int i = 0; i < weeks.Count; i++)
                    {
                        int WeekIndex = i;
                        string startDate = weeks[i].StartDate.ToLongDateString();
                        string endDate = weeks[i].EndDate.ToLongDateString();

                        string strWeek = "WeekIn:" + (i + 1).ToString() + ", Start:" + startDate + ", End: " + endDate;
                        streamWriter.WriteLine(strWeek);
                    }

                }
            }
        }
        /// <summary>
        /// Inner class to provide Week entity 
        /// </summary>
        public class WeekIndexDateDuration
        {
            public int Index;
            public DateTime StartDate;
            public DateTime EndDate;
        }

        /// <summary>
        /// Method to get the current week number
        /// </summary>
        /// <returns></returns>
        public int GetCurrentWeek()
        {
            DateTime today = DateTime.Now;
            //int weekIndex = today
            CultureInfo myCI = new CultureInfo("en-US");

            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            int myInt = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);

            //calendar.GetWeekOfYear(today,)
            return myInt;
        }

        /// <summary>
        /// Method to format Current Week number to stirng
        /// </summary>
        /// <param name="intWeekNum"></param>
        /// <returns></returns>

        public string GetCurrentWeekString(int intWeekNum)
        {
            string strWeekNum;
            strWeekNum = intWeekNum.ToString();
            if (strWeekNum.Length == 1)
                strWeekNum = "0" + strWeekNum;

            return strWeekNum;
        }

        /// <summary>
        /// Method to check if file modified
        /// </summary>
        /// <param name="destinationFileFullName"></param>
        /// <param name="fileSize"></param>
        /// <param name="modifiedDateTime"></param>
        /// <returns></returns>
        public bool IsFileModified(string destinationFileFullName, long fileSize, DateTime modifiedDateTime)
        {
            bool blnFileModified = true;

            //if file not exist, regard as the source file changed; then check if its foleder exists, if not, create the folder
            if (!File.Exists(destinationFileFullName))
            {
                //check the its folder existence
                //get the folder path, such as C:\RiboBackup\WeeklyBackupExceptionLog.txt
                string destinationFolderFullName;
                int pos = destinationFileFullName.LastIndexOf("\\");
                destinationFolderFullName = destinationFileFullName.Substring(0, pos);
                //if the folder does not exist, then create folder
                if (!Directory.Exists(destinationFolderFullName))
                    Directory.CreateDirectory(destinationFolderFullName);


                return true;
            }
                
            
            FileInfo destinationFile = new FileInfo(destinationFileFullName);

            long destinationFileSize;
            DateTime destinationFileModifiedDateTime;

            destinationFileSize = destinationFile.Length;
            destinationFileModifiedDateTime = destinationFile.LastWriteTime;

            if (fileSize == destinationFileSize && modifiedDateTime == destinationFileModifiedDateTime)
                blnFileModified = false;
            else
                blnFileModified = true;

            return blnFileModified;
        }

        /// <summary>
        /// Backup event hanvlder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnBackup_Click(object sender, EventArgs e)
        private async void btnBackup_Click(object sender, EventArgs e)
        {
            this.sourceFileNum = GetFileNum(); //call this method, avoid the file numers changed at later time.
            //!!!Test
            StartProgressBar();


            NewFolderCreateForCurrentWeek();  //Rev 01

            MessageLog("File backup began"); 
            //do not allow to click the button during the processing
            this.btnBackup.Enabled = false;

            this.lblMessage.Text = "Backing up is in procedure, plese wait...";
            DateTime startTime = DateTime.Now;


            progressBar2.Value = 1;

            //await ExecuteFileCopy(sourceLocation);

            //ExecuteFileCopy(sourceLocation);
            //var myTask = ExecuteFileCopy(sourceLoacation);
            var myTask2 = Task.Run(() =>ExecuteFileCopy(sourceLocation));
           // var myTask2 = Task.Run(() => ExecuteFileCopyAsynch(sourceLocation));
            await myTask2;

            DateTime endTime = DateTime.Now;


            TimeSpan ts =   endTime - startTime;
            string duration = "";
            if (ts.Hours > 0)
            {
                if (ts.Hours == 1)
                    duration += ts.Hours.ToString() + " hour";
                else
                    duration += ts.Hours.ToString() + " hours";
            }
                
            if (ts.Minutes > 0)
            {
                if (ts.Minutes == 1)
                    duration += ts.Minutes.ToString() + " minutes";
                else
                    duration += ts.Minutes.ToString() + " minutes";
            }
                
            if (ts.Seconds > 0)
            {
                if (ts.Seconds == 1)
                    duration += ts.Seconds.ToString() + " second";
                else
                    duration += ts.Seconds.ToString() + " seconds";
            }

            if (ts.Milliseconds > 0)
            {
                if (ts.Milliseconds == 1)
                    duration += ts.Milliseconds.ToString() + " millisecond";
                else
                    duration += ts.Milliseconds.ToString() + " milliseconds";
            }
                

            string messageEnd = "The backup is done. the begin time is" + startTime.ToLongTimeString() + ". the end time is:" + endTime.ToLongTimeString();
            messageEnd += "\r\n" + "The time duration is:" + duration;
            this.lblMessage.Text = messageEnd;

            this.btnBackup.Enabled = true;
            MessageLog("File backup ended");

            //Once backup done for the first time new folder, the iBakupForNewWeek should be set as false
            if (this.isBackupForNewWeek)
                this.isBackupForNewWeek = false;

            this.label2.Text = "";

        }

        private void testTabPage()
        {
            this.tabPage1.Text = "aa";
        }
        private bool isFileLocked(FileInfo file)
        {

        
            bool retVal = false;
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException e)
            {
                retVal = true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return retVal;
        }
        /// <summary>
        /// Clean the temp files, which begin with 2 digit number, and exists the other naming file begin with the same 2 digit number
        /// </summary>
        /// <param name="path"></param>
        private void CleanTempFiles(string path)
        {

            //check if folder exists, if no existence, return.
            if (!Directory.Exists(path))
                return;

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                string fileName = file.Name; //"01 Based on Faraday's days, electricity should not be a long history, could you still say out some aspects.docx" 
                string extension = file.Extension; //".docx"
                //remove extension
                string fileName2 = fileName.Substring(0, fileName.Length - extension.Length);
                string curFileName;
                curFileName = fileName2;
                //curFileName =  file.Name.Substring(0,2);

                //check if there is the other file(s) 
                if (curFileName.Length == 2)
                {
                    foreach (var file2 in files)
                    {
                        if (file.Name != file2.Name)
                        {
                            if (file2.Name.Substring(0, 2) == curFileName) //it means the short 2-digit tempt file no need to exist
                            {
                                //remove file
                                file.Delete();
                            }
                        }
                    }
                }
                
            }
        }

        private void DBConnectionTest()
        {
            DBConnection db = new DBConnection(this.computerLocation);
            DataSet ds = db.GetDataSet(null);
            //FileLogDomain file = new FileLogDomain
            //{
            //    FileName = "my file",
            //    FilePath = "my file path",
            //    LogDate = DateTime.Now.Date,
            //    NewFileInd = 0
            //};
            //db.InsertLogFile(file);
        }


        private void CopyWithProgress()
        {
            //this.progressBar1.Visible = true;
            //progressBar1.Value = 1;
            //progressBar1.Step = 1;

            //Timer timer = new Timer();
            //timer.Interval = 1000;
            //timer.Enabled = true;

            //for (int i = 1; i < 1000; i++)
            //{
            //    progressBar1.PerformStep();
            //}

        }

        private int GetFileNum()
        {
            string[] sourceFiles = Directory.GetFiles(sourceLocation, "*.*", SearchOption.AllDirectories);
            return sourceFiles.Length;
            //string[] sourceFiles = System.IO.Directory.GetFiles(@"D:\DailyBackup\", "*.*", System.IO.SearchOption.AllDirectories);
            //return sourceFiles.Length;

        }

        //progressBar1.PerformStep()
        //delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        //public delegate void SetControlValueCallback();

        delegate void CallPerformStep(ProgressBar myProgressBar);
        //private void ActuallyPerformStep(ProgressBar myProgressBar)
        //{
        //    if (myProgressBar.InvokeRequired)
        //    {
        //        CallPerformStep del = ActuallyPerformStep;
        //        myProgressBar.Invoke(del, new object[] { myProgressBar });
        //        return;
        //    }

        //    myProgressBar.PerformStep();
        //}

        #region ProgressBar2 associated
        private Progress<int> progress;
        private Action<int> progressTarget;
        private int counter = 0;

        //public int SourceFileNum {
        //    get
        //    {
        //        sourceFileNum = GetFileNum();
        //        return sourceFileNum;
        //    }
        //    set => sourceFileNum = value;
        //}


        //private async Task StartProgressBar()
        //{
        //    progressTarget = ReportProgress;
        //    progress = new Progress<int>(progressTarget);
        //    await WatchingCopyFile(progress);
        //}

        private void StartProgressBar()
        {
            this.progressBar2.Maximum = this.sourceFileNum;
            //this.progressBar2.Maximum = SourceFileNum;
            this.progressBar2.Minimum = 1;
            this.counter = 0;
            progressTarget = ReportProgress;
            progress = new Progress<int>(progressTarget);
        }

        //private async Task<int> WatchingCopyFile(IProgress<int> progress)
        //{
        //    int intRet = 0;
        //    if (progress != null)
        //    {
        //        counter += 1;
        //        progress.Report(counter);
        //    }
        //    return intRet;
        //}

        private void WatchingCopyFile(IProgress<int> progress)
        {
            if (progress != null)
            {
                counter += 1;
                progress.Report(counter);
            }

        }


        /// <summary>
        /// Method to feed the steps for the progress bar
        /// </summary>
        /// <param name="stepIncreased"></param>
        private void ReportProgress(int steps)
        {
            //this.label1.Text = "File numbers:" + counter.ToString() + " of " + this.sourceFileNum.ToString();
            this.label1.Text = "File numbers:" + steps.ToString() + " of " + this.sourceFileNum.ToString();
            //this.label1.Text = "File numbers:" + steps.ToString() + " of " + this.SourceFileNum.ToString();
            this.label2.Text = "copying file: " + this.currentDestinationPath +  this.curDestinationFile; 
            //if (steps <= progressBar2.Maximum)
            progressBar2.Value = steps;

        }
        #endregion (progressBar2)

        private bool FileLogExclude()
        {
            bool retVal = false;
            if (this.currentDestinationPath.Contains("SomePractices"))
                retVal = true;
            return retVal;
        }

        private void btnGeneralCopy_Click(object sender, EventArgs e)
        {
            this.lblTabPage2StateBar.Text = "Copy button is clicked";

        }

        //this will be removed soon.
        //private void btnBackup_Click_1(object sender, EventArgs e)
        //{

        //}

        private void btnSourceBrowse_Click(object sender, EventArgs e)
        {

            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.txtSourcePath.Text = openFileDialog1.FileName;
            }

        }

        private void btnTargetPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK )
                {
                    this.txtTargetPath.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }
    }
}

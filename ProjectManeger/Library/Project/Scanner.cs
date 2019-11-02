using SHUtils.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManager25.Forms;

namespace ProjectManager25.Library.Project
{
    class Scanner
    {
        internal Task<ToolStripMenuItem[]> ScannerProjectsAsync(/*Action<int,int,string> onProgressChanged*/)
        {
            return Task<ToolStripMenuItem[]>.Run(() =>
            {
                //return ProjectScanner(onProgressChanged);
                return ProjectScanner();
            });
        }
        internal ToolStripMenuItem[] ProjectScanner(/*Action<int, int, string> onProgressChanged*/)
        {
            List<ToolStripMenuItem> returned = new List<ToolStripMenuItem>();
            string FilesLocation = Properties.Settings.Default.DirProject;
            if (string.IsNullOrEmpty(FilesLocation))
            {
                Log.System("Project Path not set, no scanning will be done");
                return null;
            }
            Log.System(string.Format("Scanning For projectfiles at : {0}", FilesLocation));
            List<string> Filenames = Directory.EnumerateFiles(FilesLocation, "*" + Properties.Settings.Default.FileExtensionPm2, SearchOption.AllDirectories).ToList();
            Log.System(string.Format("[Scanning Found projectfiles {0} out of {1} files, at : {2}]",
                                      Filenames.Count,
                                      Directory.EnumerateFiles(FilesLocation, "*.*", SearchOption.AllDirectories).Count(),
                                      FilesLocation));
            Log.Spacer();
            //Checking each found project.
            for (int i = 0; i < Filenames.Count; i++)
            {
                string shortfilename = "...\\" + Filenames[i].Substring(FilesLocation.Length + 1);

                string message = string.Format("[Scanning {0} out of {1}] {2}", i + 1, Filenames.Count, shortfilename);
                Log.System(message);
              //  onProgressChanged(i + 1, Filenames.Count, message);

                Stopwatch sw = Stopwatch.StartNew();
                try
                {
                    BinaryFormatter binaryFmt = new BinaryFormatter();
                    // string FileName = string.Format(@"{0}\{1}{2}", Properties.Settings.Default.DirProjectTemp, p.ProjectName, Properties.Settings.Default.FileExtensionPm2);
                    FileStream fs = new FileStream(Filenames[i], FileMode.Open);
                    Project p = (Project)binaryFmt.Deserialize(fs);
                    if (!p.projectDone)
                    {
                        message = string.Format("[Scanning {0} out of {1}] {2}", i + 1, Filenames.Count, "Project not done, will be added to list");
                        try
                        {
                            ToolStripMenuItem tsmi = new ToolStripMenuItem(string.Format("{0}. {1}", i, p.ProjectName));
                            switch (p.CurretPriority)
                            {
                                case 0:
                                    tsmi.Image = Properties.Resources.pmtpGrayIcon16;
                                    break;
                                case 1:
                                    tsmi.Image = Properties.Resources.pmtpGrayIcon16;
                                    break;
                                case 2:
                                    tsmi.Image = Properties.Resources.pmtpBlueIcon16;
                                    break;
                                case 3:
                                    tsmi.Image = Properties.Resources.pmtpYellowIcon16;
                                    break;
                                case 4:
                                    tsmi.Image = Properties.Resources.pmtpRedIcon16;
                                    break;
                                default:
                                    tsmi.Image = Properties.Resources.pmtpGrayIcon16;
                                    break;
                            }
                            tsmi.Click += (sender, ex) =>
                            {
                                ProjectOverview po = new ProjectOverview(p);
                                foreach (Form f in Application.OpenForms)
                                    if (f is Main)
                                    {
                                        po.MdiParent = f;
                                        po.Show();
                                        break;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Could not find main form.");
                                        po.Dispose();
                                    }
                            };
                            returned.Add(tsmi);
                            Log.System(message);
                        }
                        catch (System.Runtime.Serialization.SerializationException e)
                        {
                            Log.Error(string.Format("Following error occured during runtime {0}, with message {1}", e.GetType(), e.Message));
                        }
                    }
                    fs.Close();
                }
                catch
                {
                    Log.Error(string.Format("Coldnot load Project {0} migth be a to old version.", shortfilename[i]));
                }
                sw.Stop();
                Log.LapsTime(sw.Elapsed, DateTime.Now, "Scanning Project Files");
                Log.System(string.Format("[Scanning {0} out of {1}] Scanned in {2} s", i + 1, Filenames.Count, sw.Elapsed.TotalSeconds));
                Log.Spacer();
            }
            return returned.ToArray();
        }
        internal Task<string[]> ScannerForTempProjects()
        {
            return Task<string[]>.Run(() =>
                {
                    //tsmiNotFinishProjects.DropDownItems.Clear();
                    string FilesLocation = Properties.Settings.Default.DirProjectTemp;
                    if (string.IsNullOrEmpty(FilesLocation))
                    {
                        Log.System("Project Temp Path not set, no scanning will be done");
                        return null;
                    }
                    Log.System(string.Format("Scanning For projectfiles at : {0}", FilesLocation));
                    List<string> Filenames = Directory.EnumerateFiles(FilesLocation, "*" + Properties.Settings.Default.FileExtensionPm2, SearchOption.AllDirectories).ToList();
                    Log.System(string.Format("[Scanning Found projectfiles {0} out of {1} files, at : {2}]",
                                              Filenames.Count,
                                              Directory.EnumerateFiles(FilesLocation, "*.*", SearchOption.AllDirectories).Count(),
                                              FilesLocation));

                    Log.Spacer();
                    return Filenames.ToArray();
                });
        }
    }
}

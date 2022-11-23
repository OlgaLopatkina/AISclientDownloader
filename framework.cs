// определяыет версии framework
        private void CheckFWVersion()
        {

            tbAISver.Text = null;
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {
                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();

                        if (install == "")
                        {
                            Trace.WriteLine(versionKeyName + "  " + name);
                            tbAISver.Text += versionKeyName + "  " + name;
                        }
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                Trace.WriteLine(versionKeyName + "  " + name + "  SP" + sp);
                                tbAISver.Text += versionKeyName + "  " + name + "  SP" + sp;
                            }

                        }

                        if (name != "")
                        {
                            continue;
                        }

                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");

                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();

                            install = subKey.GetValue("Install", "").ToString();

                            if (install == "")
                            {
                                Trace.WriteLine(versionKeyName + "  " + name);
                                tbAISver.Text += versionKeyName + "  " + name;
                            }
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    Trace.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
                                    tbAISver.Text += "  " + subKeyName + "  " + name + "  SP" + sp;
                                }
                                else if (install == "1")
                                {
                                    Trace.WriteLine("  " + subKeyName + "  " + name);
                                    tbAISver.Text += "  " + subKeyName + "  " + name;
                                }

                            }

                        }

                    }
                }
            }
        }

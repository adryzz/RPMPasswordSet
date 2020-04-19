using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace RPMPasswordSet
{
    public partial class Form1 : Form
    {
        string Key = @"HKEY_CURRENT_USER\Software\RemotePresentationManager\Passwords";
        string Value = @"Key";
        string Current = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string current = null;
            try
            {
                current = Registry.GetValue(Key, Value, null).ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(this, ex.Message, "RPMPasswordSet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit();
            }
            if (current == null)
            {
                label1.Enabled = false;
                maskedTextBox1.Enabled = false;
            }
            else
            {
                Current = current;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                SHA512 sha = new SHA512Managed();
                if (Current == null)
                {
                    string pass = Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(maskedTextBox2.Text)));
                    string npass = Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(maskedTextBox3.Text)));
                    if (pass == npass)
                    {
                        try
                        {
                            Registry.SetValue(Key, Value, pass, RegistryValueKind.String);//set the hash in the registry
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, ex.Message, "RPMPasswordSet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(maskedTextBox1.Text))).Equals(Current))//the hashes are equals
                {
                    string pass = Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(maskedTextBox2.Text)));
                    string npass = Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(maskedTextBox3.Text)));
                    if (pass == npass)
                    {
                        try
                        {
                            Registry.SetValue(Key, Value, pass, RegistryValueKind.String);//set the hash in the registry
                            MessageBox.Show(this, "Password Set!", "RPMPasswordSet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Current = pass;
                            label1.Enabled = true;
                            maskedTextBox1.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, ex.Message, "RPMPasswordSet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "The new passwords don't match", "RPMPasswordSet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(this, "The old password is incorrect", "RPMPasswordSet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "RPMPasswordSet", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/adryzz/RPMPasswordSet");
        }
    }
}

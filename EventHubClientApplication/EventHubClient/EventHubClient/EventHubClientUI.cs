using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Microsoft.ServiceBus.Messaging;

namespace EventHubClient1
{
    public partial class EventHubClientUI : Form
    {
        static string eventHubName = "Your Event Hub name";
        static string connectionString = "namespace connection string";

        public EventHubClientUI()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        
    }
}

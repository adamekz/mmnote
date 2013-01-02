using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace NETOCR
{
    public partial class Data : System.Web.UI.Page
    {
        public FileClassesDataContext dbfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            dbfile = new FileClassesDataContext();
            Title = "Data";
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            
        }

       

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.CompareTo("Text") == 0)
            {
                int id = (int)GridView1.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;

                var get_data =
                    from file in dbfile.Files
                    where file.FID == id
                    select new { file.Name, file.Path };

                string name = "";
                string Path = "";

                foreach (var file in get_data)
                {
                    name = file.Name;
                    Path = file.Path;
                }
                TitleLabel.Text = name;
                StreamReader str = new StreamReader(Path);
                DataViewBox.Text = str.ReadToEnd();
                str.Close();
                str.Dispose();

                GridPanel.Visible = false;
                TextPanel.Visible = true;
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            TextPanel.Visible = false;
            GridPanel.Visible = true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GMap;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;

namespace GUI_KRTI_BismillahNyoba
{

    public partial class UserControl_Waypoint : UserControl
    {

        private SerialPort port;
     /*   public void setSerialPort(SerialPort port)
        {
            myPort = port;
           // myPort.Write("Hfdsadf");
        }//*/
        //string dataOut1, dataOut2;
        UserControl_Flight ucf = new UserControl_Flight();
        
        GMapOverlay markers = new GMapOverlay("markers");
        GMapOverlay polygons = new GMapOverlay("polygons");
        private List<PointLatLng> _points;
        public UserControl_Waypoint(SerialPort port)
        {
            //InitializeComponent();
            this.port = port; 
        }
        public UserControl_Waypoint(){
            InitializeComponent(); 
            _points = new List<PointLatLng>();
        }

        private void UserControl_Waypoint_Load(object sender, EventArgs e)
        {
            //Map Configuration
            gMapControl1.DragButton = MouseButtons.Left;
            //gMapControl1.CanDragMap = true;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.CacheOnly;
            gMapControl1.ShowCenter = false;
            gMapControl1.Position = new PointLatLng(-7.2778145, 112.7941234);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 9;
            gMapControl1.AutoScroll = true;
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            gMapControl1.IgnoreMarkerOnMouseWheel = true;

            //List View Configuration
            listView1.Columns.Add("Latitude", 150);
            listView1.Columns.Add("Longitude", 150);
            //listView1.Columns.Add("Distance", 150);
        }

        //Add Row
        private void add(String latwp, String lngwp)
        {
            //Array to representation a row
            string[] row = { latwp, lngwp };

            ListViewItem item = new ListViewItem(row);

            //Add it to list view
            listView1.Items.Add(item);
        }

        
        private void gMapControl1_Load(object sender, EventArgs e)
        {
            //var point = new PointLatLng(-7.2778145, 112.7941234);
            //LoadMap(point);
            //Waypoint(point);
            gMapControl1.Refresh();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            if (gMapControl1.Overlays.Count > 0)
            {
                _points.Clear();
                //gMapControl1.Overlays.Remove(markers);
                gMapControl1.Overlays.RemoveAt(0);
                markers.Markers.Clear();
                polygons.Polygons.Clear();
                gMapControl1.Refresh();
                //markers.Markers.Remove(markers);
                listView1.Items.Clear();
            }
        }

        private void Line_Click(object sender, EventArgs e)
        {
            /*
            var polyline = new GMapPolygon(_points, "My Area");
            polyline.Stroke = new Pen(Color.Red, 2);//, Fill = new SolidBrush(false)       
            var polylines = new GMapOverlay("polylines");
            polylines.Polygons.Add(polyline);
            gMapControl1.Overlays.Add(polylines);
            */
            var polygon = new GMapPolygon(_points, "My Area")
            {
                Stroke = new Pen(Color.Red, 2)
            };
            polygon.IsHitTestVisible = true;
            polygons.Polygons.Add(polygon);
            gMapControl1.Overlays.Add(polygons);
            //gMapControl1.Refresh();
            //*/
            
        }

        private async void SaveWP_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (TextWriter tw = new StreamWriter(new FileStream(sfd.FileName, FileMode.Create), Encoding.UTF8))
                    {
                        foreach (ListViewItem item in listView1.Items)
                        {
                            await tw.WriteLineAsync(item.SubItems[0].Text + "\t" + item.SubItems[1].Text);
                        }
                        MessageBox.Show("WP Save Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        
        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            var point = gMapControl1.FromLocalToLatLng(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                double lat = point.Lat;
                double lng = point.Lng;
                txtLatitude.Text = lat + "";
                txtLongitude.Text = lng + "";
                //Load Location
                LoadMap(point);
                //Add marker
                Waypoint(point);
                //add to listview
                ListView(point);
            }
                      
        }

          
        private void LoadMap(PointLatLng point)
        {
            gMapControl1.Position = point;
        }

        private void Waypoint(PointLatLng pointToAdd, GMarkerGoogleType markerType = GMarkerGoogleType.yellow_pushpin)
        {
            var marker = new GMarkerGoogle(pointToAdd, markerType);
            markers.Markers.Add(marker);
            gMapControl1.Overlays.Add(markers);
        }

        private void ListView(PointLatLng point)
        {
            _points.Add(point);
            add(txtLatitude.Text, txtLongitude.Text);
        }
        
        private void SendWP_Click(object sender, EventArgs e)
        {
            //setSerialPort(myPort);
            //if(this.IsOpen){
                //dataOut1 = txtLatitude.Text;
                ///dataOut2 = txtLongitude.Text;
          this.port.WriteLine("ISO");               
                //progressBar1.Value = 100;
            //} 
        }
               
    }
}

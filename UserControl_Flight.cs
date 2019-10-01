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
using AForge.Video;
using AForge.Video.DirectShow;
using Ozeki.Media;
using Ozeki.Camera;

namespace GUI_KRTI_BismillahNyoba
{
    public partial class UserControl_Flight : UserControl
    {
        //Video conf
        private IIPCamera _camera;
        private DrawingImageProvider _imageProvider;
        private MediaConnector _connector;
        private VideoViewerWF _videoViewerWF1;
        private SnapshotHandler _snapshotHandler;
        private MPEG4Recorder _mpeg4Recorder;
        //map conf
        GMarkerGoogle posisi_pesawat;
        int nomer_titik = 0;
        public string RxString;
        
        /// <summary>
        /// //////////////////////////////////////////////
        GMapOverlay markers = new GMapOverlay("markers");
        GMapOverlay polygons = new GMapOverlay("polygons");
        private List<PointLatLng> _points;
        /// </summary>
                
        public UserControl_Flight()
        {
            InitializeComponent();
            getAvailablePorts();

            // Create video viewer UI control
            _imageProvider = new DrawingImageProvider();
            _connector = new MediaConnector();
            _snapshotHandler = new SnapshotHandler();
            _videoViewerWF1 = new VideoViewerWF();
            setVideoViewer();

            buttonDisconnect.Enabled = false;
            _points = new List<PointLatLng>();
        }

        private void setVideoViewer()
        {
            cameraBox.Controls.Add(_videoViewerWF1);
            _videoViewerWF1.Size = new Size(390, 280);
            _videoViewerWF1.BackColor = Color.Silver;
            _videoViewerWF1.BackgroundImage = GUI_KRTI_BismillahNyoba.Properties.Resources.dirgantara;
            _videoViewerWF1.BackgroundImageLayout = ImageLayout.Zoom;
            _videoViewerWF1.TabStop = false;
            _videoViewerWF1.Name = "_videoViewerWf";
        }

        void getAvailablePorts()
        {
            string[] Ports = SerialPort.GetPortNames();
            comboBox_port.Items.AddRange(Ports);
        }

        void NewFrame_event(object send, NewFrameEventArgs e)
        {
            try
            {
                cameraBox.Image = (Image)e.Frame.Clone();
            }
            catch (Exception ) { }
        }

        private void UserControl_Flight_Load(object sender, EventArgs e)
        {
            gMapControl2.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;

            gMapControl2.Position = new PointLatLng(-7.2778145, 112.7941234);

            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
        }

        private void displayingText(object sender, EventArgs e)
        {
            textBoxRx.Text = RxString;
            string[] word = RxString.Split(',');
            int count = word.Length;
            try
            {
                if (count == 6)
                {
                    textBoxRoll.Text = word[0];
                    textBoxPitch.Text = word[1];
                    textBoxYaw.Text = word[2];
                    textBoxLatitude.Text = word[3];
                    textBoxLongtitude.Text = word[4];
                    textBoxAltitude.Text = word[5];

                    attitudeIndicatorInstrumentControl1.SetAttitudeIndicatorParameters(Convert.ToDouble(word[1]), Convert.ToDouble(word[0]));

                    int heading = (Int16)Convert.ToDouble(word[2]) + 90;
                    if (heading > 360) heading = heading - 360;
                    headingIndicatorInstrumentControl1.SetHeadingIndicatorParameters(heading);

                    chart4.Series["PITCH"].Points.AddY(textBoxPitch.Text);
                    chart4.Series["ROLL"].Points.AddY(textBoxRoll.Text);
                    chart4.Series["YAW"].Points.AddY(textBoxYaw.Text);

                    //Program Map
                    double lat = Convert.ToDouble(word[3]);
                    double lng = Convert.ToDouble(word[4]);

                    if (nomer_titik == 0)
                    {
                        GMapOverlay markersOverlay = new GMapOverlay("markers");
                        posisi_pesawat = new GMarkerGoogle(new PointLatLng(lat, lng), new Bitmap(@"C:\Users\Febby Ronaldo\Documents\Visual Studio 2013\Projects\GUI_KRTI_BismillahNyoba\GUI_KRTI_BismillahNyoba\bin\Debug\mdrone.png"));

                        posisi_pesawat.ToolTipText = lat + "," + lng;

                        posisi_pesawat.ToolTipMode = MarkerTooltipMode.Always;
                        markersOverlay.Markers.Add(posisi_pesawat);
                        gMapControl2.Overlays.Add(markersOverlay);
                        gMapControl2.Position = new PointLatLng(lat, lng);
                        nomer_titik++;
                    }
                    else
                    {
                        posisi_pesawat.Position = new PointLatLng(lat, lng);
                        posisi_pesawat.ToolTipText = lat + "," + lng;
                    }
                    gMapControl2.Invalidate();
                }
            }
            catch
            { 
            
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort1.IsOpen) {
                this.RxString = serialPort1.ReadLine();
                this.Invoke(new EventHandler(displayingText));
            }
            
        }

        private void gMapControl2_Load(object sender, EventArgs e)
        {
            //Map Tab Mission
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
            //Map Tab Monitoring
            ///*
            gMapControl2.DragButton = MouseButtons.Left;
            gMapControl2.CanDragMap = true;
            gMapControl2.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.CacheOnly;
            gMapControl2.Position = new PointLatLng(37.983, -1.133);
            gMapControl2.MinZoom = 0;
            gMapControl2.MaxZoom = 24;
            gMapControl2.Zoom = 9;
            gMapControl2.AutoScroll = true;
            //*/
        }

        private void add(String latwp, String lngwp)
        {
            //Array to representation a row
            string[] row = { latwp, lngwp };
           
            ListViewItem item = new ListViewItem(row);

            //Add it to list view
            listView1.Items.Add(item);
        }

        private void buttonGoTo_Click(object sender, EventArgs e)
        {
            double lat = Convert.ToDouble(textBoxLatitude.Text);
            double lng = Convert.ToDouble(textBoxLongtitude.Text);

            if (nomer_titik == 0)
            {
                GMapOverlay markersOverlay = new GMapOverlay("markers");
                posisi_pesawat = new GMarkerGoogle(new PointLatLng(lat, lng), new Bitmap(@"C:\Users\Febby Ronaldo\Documents\Visual Studio 2013\Projects\GUI_KRTI_BismillahNyoba\GUI_KRTI_BismillahNyoba\bin\Debug\mdrone.png"));
                posisi_pesawat.ToolTipText = lat + "," + lng;

                posisi_pesawat.ToolTipMode = MarkerTooltipMode.Always;
                markersOverlay.Markers.Add(posisi_pesawat);
                gMapControl2.Overlays.Add(markersOverlay);
                gMapControl2.Position = new PointLatLng(lat, lng);
                nomer_titik++;
            }
            else
            {
                posisi_pesawat.Position = new PointLatLng(lat, lng);
                posisi_pesawat.ToolTipText = lat + "," + lng;
            }
            gMapControl2.Invalidate();
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxRx.Text = serialPort1.ReadLine();
            }
            catch (TimeoutException)
            {
                textBoxRx.Text = "Timeout Exception";
            }

            string[] word = textBoxRx.Text.Split(',');
            textBoxRoll.Text = word[0];
            textBoxPitch.Text = word[1];
            textBoxYaw.Text = word[2];
            textBoxLatitude.Text = word[3];
            textBoxLongtitude.Text = word[4];
            textBoxAltitude.Text = word[5];
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_port.Text == "" || comboBoxBaudrate.Text == "")
                {
                }
                else
                {
                    serialPort1.PortName = comboBox_port.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBoxBaudrate.Text);
                    serialPort1.Open();
                    textBoxRx.Enabled = true;
                    buttonRead.Enabled = true;
                    buttonConnect.Enabled = true;
                    serialPort1.WriteLine("Connect");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (serialPort1.IsOpen)
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
                UserControl_Waypoint usWP = new UserControl_Waypoint(serialPort1);
                //usWP.setSerialPort(serialPort1);
                usWP.Show();
                MessageBox.Show("Connected Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else 
            {
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
            }
            

        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            textBoxRx.Enabled = false;
            buttonRead.Enabled = false;
            buttonConnect.Enabled = false;

            if (serialPort1.IsOpen)
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
            }
            else
            {
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
            }
        }

        //////////////////////KONFIGURASI VIDEO WEBCAM///////////////////////////////////

        private void button6_Click(object sender, EventArgs e)
        {
            //buttonplay
            var path = textBoxBrowse.Text;
            if (!String.IsNullOrEmpty(path))
            {
                startVideoCapture(path);
            }

        }

        private void startVideoCapture(string path)
        {
            var date = DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                       DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s";

            string currentpath;
            if (String.IsNullOrEmpty(path))
            {
                currentpath = date + ".mp4";
            }
            else
                currentpath = path + "\\" + date + ".mp4";

            _mpeg4Recorder = new MPEG4Recorder(currentpath);
            _mpeg4Recorder.MultiplexFinished += mPEG4Recorder_MultiplexFinished;
            _connector.Connect(_camera.AudioChannel, _mpeg4Recorder.AudioRecorder);
            _connector.Connect(_camera.VideoChannel, _mpeg4Recorder.VideoRecorder);

        }

        private void mPEG4Recorder_MultiplexFinished(object sender, EventArgs e)
        {
            _connector.Disconnect(_camera.AudioChannel, _mpeg4Recorder.AudioRecorder);
            _connector.Disconnect(_camera.VideoChannel, _mpeg4Recorder.VideoRecorder);
            _mpeg4Recorder.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ///BUTTON STOP
            stopVideoCapture();
        }

        private void stopVideoCapture()
        {
            if (_mpeg4Recorder != null)
            {
                _mpeg4Recorder.Multiplex();
                _connector.Disconnect(_camera.AudioChannel, _mpeg4Recorder.AudioRecorder);
                _connector.Disconnect(_camera.VideoChannel, _mpeg4Recorder.VideoRecorder);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ////BUTTON OPEN BROWSER PENYIMPANAN
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxBrowse.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ///BUTTON AMBIL GAMBAR
            var path = textBoxBrowse.Text;
            createSnapshot(path);
        }

        private void createSnapshot(string path)
        {
            var date = DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                       DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s";

            string currentpath;
            if (string.IsNullOrEmpty(path))
                currentpath = date + ".jpg";
            else
                currentpath = path + "\\" + date + ".jpg";

            var snapShotImage = _snapshotHandler.TakeSnapshot().ToImage();
            snapShotImage.Save(currentpath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void buttonOzeki_Click(object sender, EventArgs e)
        {
            _camera = IPCameraFactory.GetCamera("rtsp://192.168.42.1/live", "", "");
            _connector.Connect(_camera.VideoChannel, _imageProvider);
            _connector.Connect(_camera.VideoChannel, _snapshotHandler);
            _videoViewerWF1.SetImageProvider(_imageProvider);
            _videoViewerWF1.Start();
            buttonOzeki.BackgroundImage = GUI_KRTI_BismillahNyoba.Properties.Resources.icons8_wi_fi_filled_50;
            _camera.Start();

        }

        ///////////////////////////////////*Tab Mission*/////////////////////////////////////////////////////////////////////
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
                            await tw.WriteLineAsync("WP" + item.SubItems[0].Text + "\t" + item.SubItems[1].Text + "#");
                        }
                        MessageBox.Show("WP Save Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
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
            var polygon = new GMapPolygon(_points, "My Area")
            {
                Stroke = new Pen(Color.Red, 2)
            };
            polygon.IsHitTestVisible = true;
            polygons.Polygons.Add(polygon);
            gMapControl1.Overlays.Add(polygons);
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

        private void SendWP_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen){
                foreach (ListViewItem item in listView1.Items)
                {
                    serialPort1.WriteLine("WP"+ item.SubItems[0].Text + "\t" + item.SubItems[1].Text + "#");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}

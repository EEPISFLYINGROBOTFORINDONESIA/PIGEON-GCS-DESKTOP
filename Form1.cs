using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Drawing.Imaging;
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

using Ozeki.Media;
using Ozeki.Camera;

namespace GUI_KRTI_BismillahNyoba
{
    public partial class Form1 : Form
    {
        // Global Variables

        short urut = 0, i = 0, flek = 0, cd = 0, de = 0, flagGCS = 0, flag_antena = 1;
        int nomer_titik = 0;

        // Tracker Variables
        int ratusan_yaw, puluhan_yaw, satuan_yaw, ratusan_pitch, puluhan_pitch, satuan_pitch, ribuan_yaw, ribuan_pitch;
        int adc_maksimal_yaw = 0, adc_minimal_yaw = 1023, adc_maksimal_pitch = 355, adc_minimal_pitch = 611;

        double gcslat = 0, gcslong = 0, gslat = 0, gslong = 0;
        double data_adc_yaw = 0, data_adc_pitch = 611, tick = 0, time = 0, r1, r2;

        string[] adc_pitch = new string[4];
        string[] adc_yaw = new string[4];

        public string RxString;
        DataTable tabel = new DataTable();

        int subsp=0;

        // Video Straming Config
        private IIPCamera _camera;
        private DrawingImageProvider _imageProvider;
        private MediaConnector _connector;
        private VideoViewerWF _videoViewerWF1;
        private SnapshotHandler _snapshotHandler;
        private MPEG4Recorder _mpeg4Recorder;

        // Google Map Config
        GMarkerGoogle posisi_pesawat;
        GMapOverlay markers = new GMapOverlay("markers");
        GMapOverlay rute = new GMapOverlay("rute");
        GMapOverlay track = new GMapOverlay("track");
        
        private List<PointLatLng> _points;
        private List<PointLatLng> _jalur;
        private List<PointLatLng> gpollist;

        public Form1()
        {
            InitializeComponent();
            showPowerStatus();
            getAvailablePorts();
            buttonDisconnect.Enabled = false;

            // Create Video Viewer UI Controller
            _imageProvider = new DrawingImageProvider();
            _connector = new MediaConnector();
            _snapshotHandler = new SnapshotHandler();
            _videoViewerWF1 = new VideoViewerWF();
            setVideoViewer();

            _points = new List<PointLatLng>();
            _jalur = new List<PointLatLng>();

            timer1.Start();
        }

        void showPowerStatus()
        {
            PowerStatus status = SystemInformation.PowerStatus;
            label1.Text = status.BatteryLifePercent.ToString("P0");
        }

        void getAvailablePorts() // GET SERIAL
        {
            string[] Ports = SerialPort.GetPortNames();
            comboBox_port.Items.AddRange(Ports);
            comboPort2.Items.AddRange(Ports);
        }

        private void setVideoViewer() // SET VIDEO 
        {
            cameraBox.Controls.Add(_videoViewerWF1);
            _videoViewerWF1.Size = new Size(390, 280);
            _videoViewerWF1.BackColor = Color.Silver;
            _videoViewerWF1.BackgroundImage = GUI_KRTI_BismillahNyoba.Properties.Resources.dirgantara;
            _videoViewerWF1.BackgroundImageLayout = ImageLayout.Zoom;
            _videoViewerWF1.TabStop = false;
            _videoViewerWF1.Name = "_videoViewerWf";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gMapControl2.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;

            gMapControl2.Position = new PointLatLng(-7.2778145, 112.7941234);

            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;

            tabel.Columns.Add("yaw", typeof(string));
            tabel.Columns.Add("pitch", typeof(string));
            tabel.Columns.Add("roll", typeof(string));

            dataGridView1.DataSource = tabel;
            textBoxLintangT.Text = "-7.27585485";
            textBoxBujurT.Text = "112.79432658";

            textBox2.Text = trackBar1.Value.ToString();

            trackBar1.Minimum = 0;
            trackBar1.Maximum = 100;
            trackBar1.TickFrequency = 10;

            trackBar2.Minimum = 0;
            trackBar2.Maximum = 24;
            trackBar2.TickFrequency = 1;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = trackBar1.Value.ToString();
            subsp = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            gMapControl1.Zoom = trackBar2.Value;
        }

        private void ExitButton_Click(object sender, EventArgs e) // CLOSE APPLICATION
        {
            DialogResult exiting = MessageBox.Show("Anda akan keluar aplikasi!\nPastikan Wahana telah kembali dengan selamat!\nTerimaKasih. -EFRISA 2019","Keluar dari PIGEON", MessageBoxButtons.YesNo);
            if (exiting == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void timer1_Tick(object sender, EventArgs e) // SET TIMER
        {
            day.Text = DateTime.Now.ToString("dddd");
            date.Text = DateTime.Now.ToString("MMM dd yyyy");
            times.Text = DateTime.Now.ToString("HH:mm:ss");


        }

        private void button3_Click(object sender, EventArgs e) // MENU FLIGHT
        {
            sidePanel.Height = button3.Height;
            sidePanel.Top = button3.Top;
            tabControl1.SelectedTab = tabPage1;
            panelFlight.Visible = true; panelWaypoint.Visible = false; panelStatistic.Visible = false; panelTracker.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e) // MENU WAYPOINT
        {
            sidePanel.Height = button1.Height;
            sidePanel.Top = button1.Top;
            tabControl1.SelectedTab = tabPage2;
            panelFlight.Visible = false; panelWaypoint.Visible = true; panelStatistic.Visible = false; panelTracker.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e) // MENU STATISTSTIC
        {
            sidePanel.Height = button2.Height;
            sidePanel.Top = button2.Top;
            tabControl1.SelectedTab = tabPage3;
            panelFlight.Visible = false; panelWaypoint.Visible = false; panelStatistic.Visible = true; panelTracker.Visible = false;
        }

        private void buttonTracker_Click(object sender, EventArgs e) // MENU TRACKER
        {
            sidePanel.Height = buttonTracker.Height;
            sidePanel.Top = buttonTracker.Top;
            tabControl1.SelectedTab = tabPage4;
            panelFlight.Visible = false; panelWaypoint.Visible = false; panelStatistic.Visible = false; panelTracker.Visible = true;
        }

        private async void buttonWrite_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (TextWriter tw = new StreamWriter(new FileStream(sfd.FileName, FileMode.Create), Encoding.UTF8))
                    {
                        foreach (ListViewItem item in listView1.Items)
                        {
                            await tw.WriteLineAsync("RPY" + textBoxRoll.Text + "\t" + textBoxPitch.Text + "\t" + textBoxYaw.Text + "#");
                        }
                        MessageBox.Show("WP Save Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void displayingText(object sender, EventArgs e) // LIFE OF MONITORING ===================================================================
        {
            textBoxRx.Text = RxString;
            string[] word = RxString.Split(',');
            int count = word.Length;
            double alti = 0, curlat = 0, curlng = 0, selisihlat = 0, selisihlng = 0, jlat, jlng, jy, jx, horizon, jx_jy, jy_jx;
            double yaw = 0, pitch = 0, tinggiGCS = 1.55;

            try
            {
                // TRACKER MONITORING
                curlat = Convert.ToDouble(word[3]);
                curlng = Convert.ToDouble(word[4]);
                alti = Convert.ToDouble(word[5]);
                if (flagGCS == 1 && flag_antena == 1)
                {
                    //contoh data kuadran I
                    //data awal: 	-7.278985, 112.793415  | latitude  (+)
                    //data akhir:	-7.275577, 112.799019  | longitude (+)

                    //contoh data kuadran IV
                    //data awal:	-7.279581, 112.793845  | latitude  (-)
                    //data akhir:	-7.281858, 112.798542  | longitude (+)

                    //contoh data kuadran III
                    //data awal:	-7.279368, 112.794360  | latitude  (-)
                    //data akhir:	-7.281625, 112.789895  | longitude (-)

                    //contoh data kuadran II
                    //data awal:	-7.279155, 112.794145  | latitude  (+)
                    //data akhir:	-7.276324, 112.789918  | longitude (-)

                    selisihlat = curlat - gcslat;
                    selisihlng = curlng - gcslong;

                    jlat = selisihlat * 3600;
                    jlng = selisihlng * 3600; // konversi ke detik derajat

                    jy = jlat * 30.416;
                    jx = jlng * 30.416; // 1 detik derajat = 30.416 meter [konversi jarak gps ke meter]

                    horizon = Math.Sqrt((jy * jy) + (jx * jx));
                    labeljarak.Text = Convert.ToString(horizon) + " m";
                    jx_jy = Math.Abs(jx) / Math.Abs(jy);
                    jy_jx = Math.Abs(jy) / Math.Abs(jx); // Hitung jarak horizonFmaptal

                    r1 = Math.Sqrt(Math.Pow(jx, 2) + Math.Pow(jy, 2));
                    r2 = Math.Sqrt(Math.Pow(alti, 2) + Math.Pow(r1, 2));
                    labelresultan.Text = Convert.ToString(r2); // hitung resultan

                    if (r2 > 7)
                    { // pergerakan GCS dengan sudut yaw yang dikonversikan dari radian ke derajat
                        if (selisihlat > 0 && selisihlng > 0)                   // kwadran I
                            yaw = (Math.Atan(jx_jy)) * (180 / Math.PI);
                        else if (selisihlat < 0 && selisihlng > 0)              // Kwadran II
                            yaw = 90 + (Math.Atan(jy_jx)) * (180 / Math.PI);
                        else if (selisihlat < 0 && selisihlng < 0)              // Kwadran III
                            yaw = 180 + (Math.Atan(jx_jy)) * (180 / Math.PI);
                        else if (selisihlat > 0 && selisihlng < 0)              // Kwadran IV
                            yaw = 270 + (Math.Atan(jy_jx)) * (180 / Math.PI);
                        else if (selisihlat == 0 && selisihlng > 0)
                            yaw = 90;
                        else if (selisihlat == 0 && selisihlng < 0)
                            yaw = 270;
                        else if (selisihlat > 0 && selisihlng == 0)
                            yaw = 0;
                        else if (selisihlat < 0 && selisihlng == 0)
                            yaw = 180;
                        else if (selisihlat == 0 && selisihlng == 0)
                            yaw = 0;

                        // pergerakan GCS dengan sudut pitch,, ketinggian payload dikurangi dengan ketinggian gcs terlebih dahulu
                        pitch = (Math.Atan(((alti - subsp) - tinggiGCS) / r1)) * (180 / Math.PI);
                        //pitch = (Math.Acos(r1 / r2) * (180 / Math.PI));

                        data_adc_yaw = adc_maksimal_yaw - (((adc_maksimal_yaw - adc_minimal_yaw) * yaw) / 360);
                        data_adc_pitch = adc_maksimal_pitch + (((adc_maksimal_pitch - adc_minimal_pitch) * pitch) / 90);

                        ribuan_yaw = (int)(data_adc_yaw / 1000);
                        ratusan_yaw = (int)(((data_adc_yaw) - (ribuan_yaw * 1000)) / 100);
                        puluhan_yaw = (int)(((data_adc_yaw) - (ribuan_yaw * 1000 + ratusan_yaw * 100)) / 10);
                        satuan_yaw = (int)(data_adc_yaw) - ((ribuan_yaw * 100 + ratusan_yaw * 10 + puluhan_yaw) * 10);

                        ribuan_pitch = (int)(data_adc_pitch / 1000);
                        ratusan_pitch = (int)(((data_adc_pitch) - (ribuan_pitch * 1000)) / 100);
                        puluhan_pitch = (int)(((data_adc_pitch) - (ribuan_pitch * 1000 + ratusan_pitch * 100)) / 10);
                        satuan_pitch = (int)(data_adc_pitch) - ((ribuan_pitch * 100 + ratusan_pitch * 10 + puluhan_pitch) * 10);

                        adc_yaw[0] = Convert.ToString(ribuan_yaw);
                        adc_pitch[0] = Convert.ToString(ribuan_pitch);
                        adc_yaw[1] = Convert.ToString(ratusan_yaw);
                        adc_pitch[1] = Convert.ToString(ratusan_pitch);
                        adc_yaw[2] = Convert.ToString(puluhan_yaw);
                        adc_pitch[2] = Convert.ToString(puluhan_pitch);
                        adc_yaw[3] = Convert.ToString(satuan_yaw);
                        adc_pitch[3] = Convert.ToString(satuan_pitch);
                    }

                    textAngleYaw.Text = Convert.ToString(yaw);
                    textPitchAngle.Text = Convert.ToString(pitch);

                }

                if (count == 12) 
                {
                    // SENSOR MONITORING
                    textBoxRoll.Text = word[0];
                    textBoxPitch.Text = word[1];
                    textBoxYaw.Text = word[2];
                    textBoxLintangP.Text = textBoxLatitude.Text = word[3];
                    textBoxBujurP.Text = textBoxLongtitude.Text = word[4];
                    textBoxAltitude.Text = word[5];

                    attitudeIndicatorInstrumentControl1.SetAttitudeIndicatorParameters(Convert.ToDouble(word[1]), Convert.ToDouble(word[0]));

                    int heading = (Int16)Convert.ToDouble(word[2]) + 90;
                    if (heading > 360) heading = heading - 360;

                    headingIndicatorInstrumentControl1.SetHeadingIndicatorParameters(heading);
                    
                    // CHART MONITORING
                    de++;
                    chart4.Series["PITCH"].Points.AddY(textBoxPitch.Text);
                    chart4.Series["ROLL"].Points.AddY(textBoxRoll.Text);
                    chart4.Series["YAW"].Points.AddY(textBoxYaw.Text);

                    tabel.Rows.Add(textBoxYaw.Text, textBoxPitch.Text, textBoxRoll.Text);      
                    dataGridView1.DataSource = tabel;

                    if (de == 100)
                    {
                        chart4.Series["PITCH"].Points.RemoveAt(0);
                        chart4.Series["ROLL"].Points.RemoveAt(0);
                        chart4.Series["YAW"].Points.RemoveAt(0);

                        tabel.Rows.RemoveAt(0);
                        de = 99;
                    }

                    // WAYPOINTS MONITORING
                    double lat = Convert.ToDouble(word[3]);
                    double lng = Convert.ToDouble(word[4]);

                    //int flag = Convert.ToInt16(word[10]);
                    //if (flag == 1)
                    //{
                    //    var marker = new GMarkerGoogle(_points[flek], GMarkerGoogleType.blue);
                    //    markers.Markers.Add(marker);
                    //    marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    //    gMapControl1.Overlays.Add(markers);
                    //    flek++;
                    //}

                    if (nomer_titik == 0)
                    {
                        GMapOverlay markersOverlay = new GMapOverlay("markers");
                        posisi_pesawat = new GMarkerGoogle(new PointLatLng(lat, lng), new Bitmap(@"C:\Users\Febby Ronaldo\Documents\Visual Studio 2013\Projects\GUI_KRTI_BismillahNyoba\GUI_KRTI_BismillahNyoba\bin\Debug\mdrone.png"));

                        posisi_pesawat.ToolTipText = lat + "," + lng;

                        posisi_pesawat.ToolTipMode = MarkerTooltipMode.Always;
                        markersOverlay.Markers.Add(posisi_pesawat);

                        gMapControl2.Overlays.Add(markersOverlay);
                        gMapControl2.Position = new PointLatLng(lat, lng);

                        gMapControl1.Overlays.Add(markersOverlay);
                        gMapControl1.Position = new PointLatLng(lat, lng);

                        nomer_titik++;
                    }
                    else
                    {
                        posisi_pesawat.Position = new PointLatLng(lat, lng);
                        posisi_pesawat.ToolTipText = lat + "," + lng;

                        _jalur.Add(posisi_pesawat.Position);
                        cd++;
                        var buatjalur = new GMapRoute(_jalur, "My Track")
                        {
                            Stroke = new Pen(Color.Blue, 1)
                        };
                        buatjalur.IsHitTestVisible = true;
                        track.Routes.Add(buatjalur);
                        gMapControl1.Overlays.Add(track);
                        if (cd == 2)
                        {
                            _jalur.RemoveAt(0);
                            cd = 1;
                        }
                    }

                    if (_jalur[0] == _points[flek])
                    {
                        var marker = new GMarkerGoogle(_points[flek], GMarkerGoogleType.blue);
                        markers.Markers.Add(marker);
                        marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                        gMapControl1.Overlays.Add(markers);
                        flek++;
                    }

                    gMapControl2.Invalidate();
                    gMapControl1.Invalidate();


                }
            }
            catch
            {

            }
        }

        // TABPAGE 1 ====================================================================================================================================
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                this.RxString = serialPort1.ReadLine();
                this.Invoke(new EventHandler(displayingText));
            }

        }

        private void gMapControl2_Load_1(object sender, EventArgs e)
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
            gMapControl1.Zoom = 16;
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
            gMapControl2.Zoom = 16;
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
            try
            {
                textBoxRoll.Text = word[0];
                textBoxPitch.Text = word[1];
                textBoxYaw.Text = word[2];
                textBoxLatitude.Text = word[3];
                textBoxLongtitude.Text = word[4];
                textBoxAltitude.Text = word[5];
            } catch (Exception)
            {
                textBoxRx.Text = "Received Data invalid";
            }
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
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (serialPort1.IsOpen)
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
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
            //serialPort1.Close();
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

        private void buttonPlay_Click(object sender, EventArgs e)
        {
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

        private void buttonStop_Click(object sender, EventArgs e)
        {
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

        private void cameraBox_Click(object sender, EventArgs e)
        {

        }

        Point curPoint;
        private void moveWindow_MouseDown(object sender, MouseEventArgs e)
        {
            curPoint = new Point(e.X, e.Y);
        }

        private void moveWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Left += e.X - curPoint.X;
                this.Top += e.Y - curPoint.Y;
            }
        }

        private void comboBox_port_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxBrowse.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        private void buttonShot_Click(object sender, EventArgs e)
        {
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

            var kuy = _snapshotHandler.TakeSnapshot().ToImage();
            kuy.Save(currentpath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void buttonOzeki_Click(object sender, EventArgs e)
        {
            //_camera = IPCameraFactory.GetCamera("rtsp://192.168.42.1/live", "", "");
            _camera = IPCameraFactory.GetCamera("rtsp://192.168.0.3:8554/unicast", "", "");
            //_camera = IPCameraFactory.GetCamera("rtsp://192.168.43.153:8554/unicast", "", "");
            _connector.Connect(_camera.VideoChannel, _imageProvider);
            _connector.Connect(_camera.VideoChannel, _snapshotHandler);
            _videoViewerWF1.SetImageProvider(_imageProvider);
            _videoViewerWF1.Start();
            buttonOzeki.BackgroundImage = GUI_KRTI_BismillahNyoba.Properties.Resources.icons8_wi_fi_filled_50;
            _camera.Start();
        }

        // TABPAGE 2 ====================================================================================================================================
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

        private void button6_Click(object sender, EventArgs e)
        {
            _points.RemoveAt(0);
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            if (gMapControl1.Overlays.Count > 0)
            {
                _points.Clear();
                //gMapControl1.Overlays.Remove(markers);
                gMapControl1.Overlays.RemoveAt(0);
                markers.Markers.Clear();
                rute.Routes.Clear();
                gMapControl1.Refresh();
                //markers.Markers.Remove(markers);
                listView1.Items.Clear();
            }
        }
        private void LoadMap(PointLatLng point)
        {
            gMapControl1.Position = point;
        }

        private void Waypoint(PointLatLng pointToAdd, GMarkerGoogleType markerType = GMarkerGoogleType.yellow)
        {

            var marker = new GMarkerGoogle(pointToAdd, markerType);
            markers.Markers.Add(marker);
            marker.ToolTipText = "WP-" + urut;
            marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

            gMapControl1.Overlays.Add(markers);
            urut++;

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
                //CreateCircle(point, 5);

                //add to listview
                ListView(point);
                var buatrute = new GMapRoute(_points, "My Area")
                {
                    Stroke = new Pen(Color.Red, 2)
                };

                buatrute.IsHitTestVisible = true;
                rute.Routes.Add(buatrute);
                gMapControl1.Overlays.Add(rute);
                
                double distance = buatrute.Distance;
                textDistance.Text = distance.ToString();

            }
        }

        private void SendWP_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    serialPort1.WriteLine("W," + item.SubItems[0].Text + "," + item.SubItems[1].Text);
                }
            }
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }



        private void button4_Click(object sender, EventArgs e)
        {

            if (i == 0)
            {
                gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleChinaSatelliteMapProvider.Instance;
                gMapControl2.MapProvider = GMap.NET.MapProviders.GoogleChinaSatelliteMapProvider.Instance;
                i = 1;
            }
            else if (i == 1)
            {
                gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
                gMapControl2.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
                i = 0;
            }
        }

        // TABPAGE 4 ====================================================================================================================================
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                tick++;
                if (tick % 2 == 0) {
                    time = tick / 2;
                }

                if (r1 > 10) {
                    serialPort2.Write("SD,");
                    serialPort2.Write(adc_yaw[0]);
                    serialPort2.Write(adc_yaw[1]);
                    serialPort2.Write(adc_yaw[2]);
                    serialPort2.Write(adc_yaw[3]);
                    serialPort2.Write(",");
                    serialPort2.Write(adc_pitch[0]);
                    serialPort2.Write(adc_pitch[1]);
                    serialPort2.Write(adc_pitch[2]);
                    serialPort2.Write(adc_pitch[3]);
                    serialPort2.Write(",");
                }
            }
            catch { }
            textBoxRx2.Text = "SD," + adc_yaw[0] + adc_yaw[1] + adc_yaw[2] + adc_yaw[3] + "," + 
                              adc_pitch[0] + adc_pitch[1] + adc_pitch[2] + adc_pitch[3] + ",";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //try
            //{
                flagGCS = 1;
                gslat = gcslat = Convert.ToDouble(textBoxLintangT.Text);
                gslong = gcslong = Convert.ToDouble(textBoxBujurT.Text);
                textBoxLintangT.ReadOnly = true;
                textBoxBujurT.ReadOnly = true;
            //}
            //catch { }
        }

        private void buttonKonek_Click(object sender, EventArgs e)
        {
            if (serialPort2.IsOpen == false)
            {
                try
                {
                    serialPort2.PortName = comboPort2.Text.ToString();
                    serialPort2.BaudRate = Convert.ToInt32(comboBaudrate2.Text);
                    serialPort2.Encoding = Encoding.Default;
                    serialPort2.Open();
                    buttonKonek.BackgroundImage = GUI_KRTI_BismillahNyoba.Properties.Resources.icons8_connected_80;
                    label26.Text = "Connected";
                    //timer2.Start();
                }
                catch { MessageBox.Show("Serial Port Error!!"); }
            }
            else {
                serialPort2.Close();
                buttonKonek.BackgroundImage = GUI_KRTI_BismillahNyoba.Properties.Resources.icons8_disconnected_80;
                label26.Text = "Disconnected";
                //timer2.Stop();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try {
                //serialPort1.Write("sd");
                //serialPort1.Write("sd");
                //serialPort1.Write("sd");
                //serialPort1.Write("sd");
                //serialPort1.Write("sd");
                //serialPort1.Write("sd");
                timer2.Start();
                //serialPort2.WriteLine(textBoxRx2.Text);
            }
            catch { MessageBox.Show("Hubungan Serial Data Error!"); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try {
                //serialPort1.Write("st\r\n");
                timer2.Stop();
            }
            catch { }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBoxLintangT.Text = textBoxLintangP.Text;
            textBoxBujurT.Text = textBoxBujurP.Text;
        }

        private void CreateCircle(PointLatLng point, double radius)
        {
            int segments = 1000;
            gpollist = new List<PointLatLng>();
            for (int i = 0; i < segments; i++)
                gpollist.Add(FindPointAtDistanceFrom(point, i, radius / 1000));

            GMapPolygon gpol = new GMapPolygon(gpollist, "pol");

            markers.Polygons.Add(gpol);
            
        }

        public static PointLatLng FindPointAtDistanceFrom(PointLatLng startPoint, double initialBearingRadians, double distanceKilometres)
        {
            const double radiusEarthKilometres = 6371.01;
            var distRatio = distanceKilometres / radiusEarthKilometres;
            var distRatioSine = Math.Sin(distRatio);
            var distRatioCosine = Math.Cos(distRatio);

            var startLatRad = DegreesToRadians(startPoint.Lat);
            var startLonRad = DegreesToRadians(startPoint.Lng);

            var startLatCos = Math.Cos(startLatRad);
            var startLatSin = Math.Sin(startLatRad);

            var endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(initialBearingRadians)));

            var endLonRads = startLonRad + Math.Atan2(
                          Math.Sin(initialBearingRadians) * distRatioSine * startLatCos,
                          distRatioCosine - startLatSin * Math.Sin(endLatRads));

            return new PointLatLng(RadiansToDegrees(endLatRads), RadiansToDegrees(endLonRads));
        }

        public static double DegreesToRadians(double degrees)
        {
            const double degToRadFactor = Math.PI / 180;
            return degrees * degToRadFactor;
        }

        public static double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }
    }
}

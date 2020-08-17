/*******************************************************************************
 *Author: Kien Truong
 *Program: Falling Apples
 ******************************************************************************/

using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Collections.Generic;

public class FallingApples: Form
{
  private const int maxFormWidth  = 1920;
  private const int maxFormHeight = 1080;
  private const int minFormWidth  = 640;
  private const int minFormHeight = 360;
  Size maxFrameSize = new Size(maxFormWidth,maxFormHeight);
  Size minFrameSize = new Size(minFormWidth,minFormHeight);

  private const int topPanelHeight    = 50;
  private const int bottomPanelHeight = 110;

  //Declare data about the ball
  private const double radius = 8.0;
  private const double angle  = 270.0;    //downward direction
  private const double radian = (Math.PI/180.0) * angle;
  private const double speed  = 150.0;
  private double ball_linear_speed_pix_per_sec;
  private double ball_linear_speed_pix_per_tic;
  private double ball_delta_x;
  private double ball_delta_y;
  private const double ball_center_initial_coord_x = (double)1280*0.5;
  private const double ball_center_initial_coord_y = (double)topPanelHeight+10;
  private double ball_center_current_coord_x;
  private double ball_center_current_coord_y;
  private double appleX;
  private double appleY;

  //For calculating the range for valid clicks
  private int     mouseX;
  private int     mouseY;
  private double  distsq;
  private bool    caught = false;

  private const String welcome_message = "Falling Apples by Kien Truong";
  private System.Drawing.Font welcome_style = new System.Drawing.Font("TimesNewRoman",24,FontStyle.Regular);
  private Brush welcome_paint_brush = new SolidBrush(System.Drawing.Color.Black);
  private Point welcome_location;   //Will be initialized in the constructor.

  private Button startButton = new Button();
  private Button exitButton  = new Button();
  private Button pauseButton = new Button();
  private Button clearButton = new Button();
  private Label  appCaught   = new Label();
  private Label  appMissed   = new Label();
  private Label  sucRatio    = new Label();
  private Label  caughtCount = new Label();
  private Label  missedCount = new Label();
  private Label  sucRatioNum = new Label();

  private Random  newXVal = new Random();

  private int     counterC = 0;
  private int     counterM = 0;
  private double  counterR = 0.0;

  private int formWidth;
  private int formHeight;

  //Declare data about the motion clock
  private static System.Timers.Timer ball_motion_control_clock = new System.Timers.Timer();
  private const double ball_motion_control_clock_rate = 20.0;  //Units are Hz

  //Declare data about the refresh clock;
  private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
  private const double graphic_refresh_rate = 23.3;  //Units are Hz = #refreshes per second

 public FallingApples()
 {//Set the size of the user interface box.
  formWidth   = (maxFormWidth+minFormWidth)/2;
  formHeight  = (maxFormHeight+minFormHeight)/2;
  Size        = new Size(formWidth,formHeight);

  MaximumSize = maxFrameSize;
  MinimumSize = minFrameSize;

  //Initialize text strings
  Text                = "Falling Apples by Kien Truong";
  System.Console.WriteLine("Form_width = {0}, Form_height = {1}.", Width, Height);
  pauseButton.Text    = "Pause";
  startButton.Text    = "Start";
  exitButton.Text     = "Exit";
  clearButton.Text    = "Clear";
  appCaught.Text      = "Apples caught";
  appMissed.Text      = "Apples missed";
  sucRatio.Text       = "Success ratio";
  caughtCount.Text    = counterC.ToString();
  missedCount.Text    = counterM.ToString();
  sucRatioNum.Text    = counterR.ToString();

  //Set sizes
  pauseButton.Size    = new Size(100,40);
  startButton.Size    = new Size(100,40);
  exitButton.Size     = new Size(100,40);
  clearButton.Size    = new Size(100,40);
  appCaught.Size      = new Size(100,40);
  appMissed.Size      = new Size(100,40);
  sucRatio.Size       = new Size(100,40);
  caughtCount.Size    = new Size(100,40);
  missedCount.Size    = new Size(100,40);
  sucRatioNum.Size    = new Size(100,40);

  //Set locations
  pauseButton.Location = new Point(170,600);
  startButton.Location = new Point(50,600);
  exitButton.Location  = new Point(1100,640);
  clearButton.Location = new Point(50,650);
  appCaught.Location   = new Point(400,600);
  appMissed.Location   = new Point(550,600);
  sucRatio.Location    = new Point(700,600);
  caughtCount.Location = new Point(420,650);
  missedCount.Location = new Point(570,650);
  sucRatioNum.Location = new Point(720,650);

  //Set colors
  this.BackColor          = Color.Blue;
  startButton.BackColor   = Color.White;
  pauseButton.BackColor   = Color.White;
  exitButton.BackColor    = Color.White;
  clearButton.BackColor   = Color.White;
  appCaught.BackColor     = Color.Yellow;
  appMissed.BackColor     = Color.Yellow;
  sucRatio.BackColor      = Color.Yellow;
  caughtCount.BackColor   = Color.Yellow;
  missedCount.BackColor   = Color.Yellow;
  sucRatioNum.BackColor   = Color.Yellow;

  //Add controls to the form
  Controls.Add(pauseButton);
  Controls.Add(startButton);
  Controls.Add(exitButton);
  Controls.Add(clearButton);
  Controls.Add(appCaught);
  Controls.Add(appMissed);
  Controls.Add(sucRatio);
  Controls.Add(caughtCount);
  Controls.Add(missedCount);
  Controls.Add(sucRatioNum);

  welcome_location = new Point(Width/2-250,8);

  ball_center_current_coord_x = ball_center_initial_coord_x;
  ball_center_current_coord_y = ball_center_initial_coord_y;

  //Register the event handler.  In this case each button has an event handler, but no other
  //controls have event handlers.
  pauseButton.Enabled     = true;
  startButton.Enabled     = true;
  exitButton.Enabled      = true;
  clearButton.Enabled     = true;

  //Set up the motion clock.  This clock controls the rate of update of the coordinates of the ball.
  ball_motion_control_clock.Enabled = false;
  //Assign a handler to this clock.
  ball_motion_control_clock.Elapsed += new ElapsedEventHandler(Update_ball_position);

  //Set up the refresh clock.
  graphic_area_refresh_clock.Enabled = false;  //Initially the clock controlling the rate of updating the display is stopped.
  //Assign a handler to this clock.
  graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(Update_display);

  //Use extra memory to make a smooth animation.
  DoubleBuffered = true;

  pauseButton.Click += new EventHandler(pause);
  startButton.Click += new EventHandler(start);
  exitButton.Click  += new EventHandler(stoprun);  //The '+' is required.
  clearButton.Click += new EventHandler(refresh);
}//End of constructor FallingApples

 //Method to execute when the exit button receives an event, namely: receives a mouse click

  protected void pause(Object sender, EventArgs events)
  {
   if(pauseButton.Text == "Resume")
   {
     ball_motion_control_clock.Enabled  = true;
     graphic_area_refresh_clock.Enabled = true;
     pauseButton.Text = "Pause";
   }
   else
   {
     ball_motion_control_clock.Enabled  = false;
     graphic_area_refresh_clock.Enabled = false;
     pauseButton.Text = "Resume";
   }

 }//End of pause

   protected void stoprun(Object sender, EventArgs events)
   {
     Close();
   }//End of stoprun

   protected void refresh(Object sender, EventArgs events)
   {
     ball_motion_control_clock.Enabled  = false;
     graphic_area_refresh_clock.Enabled = false;
     caughtCount.Text = "0";
     missedCount.Text = "0";
     sucRatioNum.Text = "0";
     counterC = 0;
     counterM = 0;
     counterR = 0.0;
     ball_center_current_coord_x = (double)1280*0.5;
     ball_center_current_coord_y = (double)topPanelHeight+10;
    Invalidate();
  }//End of refresh

   protected void start(Object sender, EventArgs events)
   {
     ball_linear_speed_pix_per_tic = speed/ball_motion_control_clock_rate;
     ball_delta_y = ball_linear_speed_pix_per_tic*System.Math.Sin(radian);
     ball_delta_x = ball_linear_speed_pix_per_tic*System.Math.Cos(radian);

     Start_graphic_clock(graphic_refresh_rate);
     //The motion clock is started.
     Start_ball_clock(ball_motion_control_clock_rate);
   }//End of start

   protected void Start_graphic_clock(double refresh_rate)
   {   double actual_refresh_rate = 1.0;  //Minimum refresh rate is 1 Hz to avoid a potential division by a number close to zero
       double elapsed_time_between_tics;
       if(refresh_rate > actual_refresh_rate)
       {
         actual_refresh_rate = refresh_rate;
       }
       elapsed_time_between_tics = 1000.0/actual_refresh_rate;  //elapsedtimebetweentics has units milliseconds.
       graphic_area_refresh_clock.Interval = (int)System.Math.Round(elapsed_time_between_tics);
       graphic_area_refresh_clock.Enabled = true;  //Start clock ticking.
   }//End of Start_graphic_clock

   protected void Start_ball_clock(double update_rate)
   {   double elapsed_time_between_ball_moves;
       if(update_rate < 1.0) update_rate = 1.0;  //This program does not allow updates slower than 1 Hz.
       elapsed_time_between_ball_moves = 1000.0/update_rate;  //1000.0ms = 1second.
      //The variable elapsed_time_between_ball_moves has units "milliseconds".
       ball_motion_control_clock.Interval = (int)System.Math.Round(elapsed_time_between_ball_moves);
       ball_motion_control_clock.Enabled = true;   //Start clock ticking.
   }//End of Start_ball_clock

   protected void Update_display(System.Object sender, ElapsedEventArgs evt)
   {
      Invalidate();  //This creates an artificial event so that the graphic area will repaint itself.

      if(!ball_motion_control_clock.Enabled)
      {
        graphic_area_refresh_clock.Enabled = false;
        System.Console.WriteLine("The graphical area is no longer refreshing. You may close the window.");
      }
  }//End of Update_display

  protected void Update_ball_position(System.Object sender, ElapsedEventArgs evt)
   {
      ball_center_current_coord_x += ball_delta_x;
      ball_center_current_coord_y -= ball_delta_y;  //The minus sign is due to the upside down nature of the C# system.

      //Generating a random position when the ball passes a specific range
      if((int)System.Math.Round(ball_center_current_coord_y + radius) >= 590)
      {
        counterM++;
        missedCount.Text = counterM.ToString();
        ball_center_current_coord_x = newXVal.Next(100,1200);
        ball_center_current_coord_y = topPanelHeight+10;
      }

      if(caught)
      {
        counterC++;
        caughtCount.Text  = counterC.ToString();
        ball_center_current_coord_x = newXVal.Next(100,1200);
        ball_center_current_coord_y = topPanelHeight+10;
        caught = false;
      }

      //Updating success ratio
      if(counterC != 0 || counterM != 0)
      {
        counterR = System.Math.Round(((double)counterC)/((double)(counterC+counterM)),3);
        sucRatioNum.Text = ((double)counterR).ToString();
      }
   }//End of method Update_ball_position

   protected override void OnPaint(PaintEventArgs ee)
   {
     Graphics lights = ee.Graphics;
     Pen blackPen    = new Pen(Color.Black, 3);

     lights.FillRectangle(Brushes.Green,0,0,Width,topPanelHeight);
     lights.DrawString(welcome_message,welcome_style,welcome_paint_brush,welcome_location);

     lights.FillRectangle(Brushes.Brown,0,410,Width,topPanelHeight+180);
     lights.FillRectangle(Brushes.Yellow,0,590,Width,topPanelHeight+130);

     appleX = ball_center_current_coord_x - radius;
     appleY = ball_center_current_coord_y - radius;

     lights.FillEllipse(Brushes.Red,(int)appleX,(int)appleY,
                        (float)(2.0*radius),(float)(2.0*radius));

     base.OnPaint(ee);
   }//END protected override void OnPaint(PaintEventArgs ee)

   protected override void OnMouseDown(MouseEventArgs e)
   {
     mouseX = e.X;
     mouseY = e.Y;
     /*************************************************************************
      * Finding distance between 2 points on a plane
      *------------------------------------------------------------------------
      * A: center of the ball (appleX + radius; appleY + radius)
      * B: the position of the mouse click (mouseX and mouseY)
      * distsq(AB) = (xB-xA)^2 + (yB-yA)^2
      *************************************************************************/
     distsq = ((mouseX - (appleX + radius)) * (mouseX - (appleX + radius))) +
              ((mouseY - (appleY + radius)) * (mouseY - (appleY + radius)));
     if(distsq < radius * radius && appleY > 410)
     {
       caught = true;
     }
     else
     {
       caught = false;
     }
     base.OnMouseDown(e);
   }//END OnMouseDown
}//End of clas FallingApples

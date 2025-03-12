using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace One4
{
    public partial class Form1 : Form
    {
        private bool start;     // 游戏是否开始

        private bool ChessCheck = true;     // 白子黑子回合

        private const int size = 15;     // 棋盘大小

        private int[,] CheckBoard = new int[size, size];     // 棋盘点位数组


        private List<Retract> RetractList = new List<Retract>();   // 用于悔棋的列表


        // "窗口"Load事件
        public Form1()
        {
            InitializeComponent();
            initializeGame();                      // 调用初始化游戏
            this.Width = MainSize.FormWidth;       // 设置窗口宽度
            this.Height = MainSize.FormHeight;     // 设置窗口高度
            this.Location = new Point(400, 75);     // 设置窗口位置
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        // 初始化游戏
        private void initializeGame()
        {
            // 棋盘点位数组 重置为0
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    CheckBoard[i, j] = 0;
                }
            }


            RetractList.Clear();                   //清空悔棋列表
            start = false;                         // 未开始
            label1.Text = "游戏未开始";            // 提示文本改为"游戏未开始"
            button1.Visible = true;                // 显示'开始游戏'按钮
            button2.Visible = false;               // 隐藏'重新开始'按钮
            Retract.Visible = false;            //隐藏'悔棋'按钮 
        }

        // 按钮"开始游戏"CLick事件
        private void button1_Click(object sender, EventArgs e)
        {
            start = true;                              // 开始
            ChessCheck = true;                         // 黑子回合
            label1.Text = "黑子回合";                  // 提示文本改为"黑子回合"
            button1.Visible = false;                   // 隐藏'开始游戏'按钮
            button2.Visible = true;                    // 显示'重新开始'按钮
            panel1.Invalidate();                       // 重绘面板"棋盘"
            Retract.Visible = true;                  //显示'悔棋'按钮
        }


        // 确认是否重新开始
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要重新开始？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                initializeGame();                      // 调用初始化游戏方法
                button1_Click(sender, e);              // 调用按钮"开始游戏"Click事件
            }
        }


        //退出按钮
        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }



        // 面板"棋盘"Paint事件
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();      // 创建面板画布
            ChessBoard.DrawCB(g);                      // 调用画棋盘方法
            Chess.ReDrawC(panel1, CheckBoard);         // 调用重新加载棋子方法
        }



        // 面板"控制界面"Paint事件
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            // 设置控制界面的大小
            panel2.Size = new Size(MainSize.FormWidth - MainSize.CBoardWidth -10, MainSize.FormHeight);
        }

        

       
        // 面板"棋盘"MouseDown事件
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            
            // 判断游戏是否开始
            if (start)
            {
                int Judgment = 0;            // 判断数组当前行列，白子还是黑子回合，0表示没有，1表示黑子，2表示白子、用来判断胜利

                int PlacementX = e.X / MainSize.CBoardGap;      // 求鼠标点击的X方向的第几个点位
                int PlacementY = e.Y / MainSize.CBoardGap;      // 求鼠标点击的Y方向的第几个点位

                try
                {
                    // 判断此位置是否为空
                    if (CheckBoard[PlacementX, PlacementY] != 0)
                    {
                        return;                                 // 此位置有棋子
                    }

                    // 黑子回合还是白子回合
                    if (ChessCheck)
                    {
                        CheckBoard[PlacementX, PlacementY] = 1; // 黑子回合
                        Judgment = 1;                           // 切换为判断黑子
                        label2.Text = "白子回合";               // 提示文本改为"白子回合"
                    }
                    else
                    {
                        CheckBoard[PlacementX, PlacementY] = 2; // 白子回合
                        Judgment = 2;                           // 切换为判断白子
                        label2.Text = "黑子回合";               // 提示文本改为"黑子回合"
                    }
                    
                    Chess.DrawC(panel1, ChessCheck, PlacementX, PlacementY);  // 画棋子

                    RetractList.Add(new Retract(PlacementX, PlacementY)); //记录棋子的位置，用于悔棋


                    // 胜利判断
                    if (WinJudgment.ChessJudgment(CheckBoard, Judgment))
                    {
                        // 判断黑子还是白子胜利
                        if (Judgment == 1)
                        {
                            MessageBox.Show("五连珠，黑胜！", "胜利提示");    // 提示黑胜
                        }
                        else
                        {
                            MessageBox.Show("五连珠，白胜！", "胜利提示");    // 提示白胜
                        }
                        initializeGame();                      // 调用初始化游戏
                    }

                    ChessCheck = !ChessCheck;                   // 换棋子
                }
                catch (Exception) { }                            // 防止鼠标点击边界，导致数组越界

            }
            else
            {
                MessageBox.Show("请先开始游戏！", "提示");      // 提示开始游戏
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 获取悔棋列表中，最后一个棋子的索引
            int Index = RetractList.Count - 1;

            // 判断悔棋列表是否为空
            if (Index == -1)
            {
                MessageBox.Show("成功撤回空气！", "提示");
                return;
            }

            // 判断并获取最后一个棋子是白子还是黑子
            string ChessCheckStr = "";
            if (ChessCheck)
            {
                ChessCheckStr = "白方";
            }
            else
            {
                ChessCheckStr = "黑方";
            }

            // 得到最后一个棋子的位置
            Retract retract = RetractList[Index];


            // 确认是否悔棋
            if (MessageBox.Show("确认要撤回\"" + ChessCheckStr + " : 坐标[" + retract.X + ", " + retract.Y + "]\"的棋子吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                // 在悔棋列表中，删除上一个棋子
                RetractList.RemoveAt(Index);

                // 点位数组中，清除上一个棋子
                CheckBoard[retract.X, retract.Y] = 0;

                // 刷新窗体(以达到自动调用重绘棋子的方法)
                this.Refresh();

                // 换棋子
                ChessCheck = !ChessCheck;
            }

        }
    }
    }


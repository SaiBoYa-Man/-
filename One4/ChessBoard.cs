using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One4
{
    class ChessBoard
    {
        // 画棋盘方法
        public static void DrawCB(Graphics g)  //这里的形参，是画布对象
        {
            int GapWidth = MainSize.CBoardGap;                   // 棋格宽度
            int GapNum = MainSize.CBoardWidth / GapWidth - 1;    // 棋格数量
            
           // Bitmap image = new Bitmap("D:\\C语言\\代码\\day11\\day11_01_image\\CheckBoard1.png"); // 棋盘位图路径
            //g.DrawImage(image, new Point[] { new Point(0, 0), new Point(GapWidth * GapNum + 44, 0), new Point(0, GapWidth * GapNum + 44) }); // 画棋盘图片

            
            //这个是 没有用棋盘图片时，用的“画笔”画棋盘方法
            g.Clear(Color.Bisque);                               // 清除画布、并用Bisque颜色填满画布
            Pen pen = new Pen(Color.FromArgb(192, 166, 107));    // 实例化画笔
            // 画棋盘
            for (int i = 0; i < GapNum + 1; i++)
            {
                g.DrawLine(pen, new Point(20, i * GapWidth + 20), new Point(GapWidth * GapNum + 20, i * GapWidth + 20));
                g.DrawLine(pen, new Point(i * GapWidth + 20, 20), new Point(i * GapWidth + 20, GapWidth * GapNum + 20));
            }
            
        }

    }
}

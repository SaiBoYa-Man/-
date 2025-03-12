using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One4
{
    class Retract
    {
        // 构造函数，实例化时传坐标参数
        public Retract(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        // 默认-1，也就是无效坐标
        public int X = -1, Y = -1;
        internal static bool Visible;
    }
}

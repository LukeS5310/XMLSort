using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XMLSort
{
    static class Globs
    {
        public static List<Raion> AllRaions;
        public struct Raion
        {
            public int RNum;
            public int GUNum;
            public string Dis;
            public Raion(int GUNum, string Dis, int RNum)
            {
                this.Dis = Dis;
                this.RNum = RNum;
                this.GUNum = GUNum;
            }
        }
        public static void LoadRA()
        {
            string resource_data = Properties.Resources.RA;
            List<string> stmp = resource_data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            AllRaions = new List<Raion>();
            foreach (string str in stmp)
            {
                string[] data = str.Split(';');
                AllRaions.Add(new Raion(int.Parse(data[0]), data[1],int.Parse(data[2])));
            }
        }


        public enum PrefferedXMLType
        {
            TYPE_UDER,
            TYPE_RAZOV,
            TYPE_UNDEFINED
        }
        public static PrefferedXMLType CurrentXMLPref = PrefferedXMLType.TYPE_UNDEFINED;
        public static int PartNum = 1;
    }
}

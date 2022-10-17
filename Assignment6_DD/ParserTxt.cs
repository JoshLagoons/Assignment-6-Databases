using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Assignment6_DD 
{
    class ParserTxt
    {
        List<MyFile> filestoParser = new List<MyFile>();
        List<Error> errorParse = new List<Error>();

        bool hasError => errorParse.Any();

        public ParserTxt()
        {
            List<string> Insertting = AllFiles();
            foreach(var inserts in Insertting)
            {
                filestoParser.Add(Creating(inserts));
            }
            ErrorCheck1();
            foreach(var pop in filestoParser)
            {
                sqlEngine eng = new sqlEngine(Constants.database, pop);
                errorParse.AddRange(eng.Runsqltasks());
            }
            ErrorCheck2();
        }

        MyFile Creating(string ins)
        {
            MyFile pop = new MyFile();
            if(ins.EndsWith(".txt"))
            {
                pop.Delimiter = "|";
                pop.Extension = ".txt";
            }
            pop.Path = Path.Combine(Constants.dirPath, ins);
            return pop;
        }


        List<string> AllFiles()
        {

            return Directory.GetFiles(Constants.dirPath).Where(x => !x.EndsWith("_out.txt")).ToList();
        }

        public void ErrorCheck1()
        {
            if (hasError)
            {
                WriteLine("The parsing ended with errors");
                foreach (var errDetail in errorParse)
                {
                    WriteLine($"Heres the error: {errDetail.Message} ----{errDetail.Info}");
                }
            }
        }
        public void ErrorCheck2()
        {
            if (hasError)
            {
                foreach (var errDetail2 in errorParse)
                {
                    WriteLine($"Here's the error: {errDetail2.Message} ----{errDetail2.Info}");
                }
            }
            else
            {
                WriteLine("parsing completed!");
            }
        }

    }

}

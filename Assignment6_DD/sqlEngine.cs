using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;
using System.Data.SqlClient;


namespace Assignment6_DD
{
    //this section was where rafael helped me the most to
    class sqlEngine
    {
      

        string table { get; set; }
        MyFile bind { get; set; }

        string sqlConStr { get; set; }

        public sqlEngine(MyFile pop, string database)
        {
            //constructor
            //update just doa normal update 
            //delete dates
            //update $1 
            //export the table into a txt file


        }

        public sqlEngine(string database, MyFile pop)
        {
            table = database;
            bind = pop;

            SqlConnectionStringBuilder mySql = new SqlConnectionStringBuilder();
            mySql["server"] = @"(localdb)\MSSQLLocalDB";
            mySql["Trusted_Connection"] = true;
            mySql["Integrated Security"] = "SSPI";
            mySql["Initial Catalog"] = "PROG260FA22";

            sqlConStr = mySql.ToString();
        }

        public List<Error> Runsqltasks()
        {
                List<Error> head = new List<Error>();
                head.AddRange(Inserttxt());
                head.AddRange(UpdateF2Z());
                head.AddRange(DeleteExpired());
                head.AddRange(PriceIncrease());
                head.AddRange(Exporttxt());

                return head;
        }

        public string SetTitle(string[] title)
        {
            string space = "";
            for (int i = 0; i < title.Length; i++)
            {
                if(!(i == 0))
                {
                    space += $", {title[i]}";
                }
                else
                {
                    space += $"{title[i]}";
                }
            }
            return $"{space}";
        }
    


        public List<Error> Inserttxt()
        {
            List<Error> error = new List<Error>();
            List<string[]> opera = new List<string[]>();
            string titles = "";

            try
            {
                using (StreamReader reading = new StreamReader(bind.Path))
                {
                    int score = 0;
                    while (!reading.EndOfStream)
                    {
                        if (score == 0)
                        {
                            var titleNames = reading.ReadLine()?.Split(",") ?? new string[0];
                            titles = SetTitle(titleNames);
                        }
                        else
                        {
                            var detailRows = reading.ReadLine()?.Split(bind.Delimiter) ?? new string[0];
                            opera.Add(detailRows);
                        }
                    }
                }
                using (SqlConnection con = new SqlConnection(sqlConStr))
                {
                    con.Open();
                    foreach (var item in opera)
                    {
                        string inLineSql = $@"INSERT INTO {table} {titles} VALUES({item[0]}, {item[1]}, {item[2]}, {item[3]}, {item[4]}, {item[5]})";


                        using (var Command = new SqlCommand(inLineSql, con))
                        {
                            var query = Command.ExecuteNonQuery();
                        }
                    }
                    con.Close();
                }
                WriteLine($"{Environment.NewLine}Inserting Data{Environment.NewLine}==========={Environment.NewLine}");
                

            }
            catch(Exception e)
            {
                Write(e);
                error.Add(new Error(e.Message, e.Source));
            }
            return error;


        }

        private List<Error> UpdateF2Z()
        {
            List<Error> error = new List<Error>();
            //this is how I updated my table's data every time I made a build
            //need to be executed twice for the update to be visible.
         
            try
            {
                using (SqlConnection con3 = new SqlConnection(sqlConStr))
                {
                    con3.Open();
                    string InLineSQL2 = $@"UPDATE {table} SET Location = REPLACE(Location, 'F', 'Z')";
                    using (var Command = new SqlCommand(InLineSQL2, con3))
                    {
                        Command.ExecuteNonQuery();
                    }
                    con3.Close();
                }
                WriteLine($"{Environment.NewLine}Updating the locatiion{Environment.NewLine}==========={Environment.NewLine}");
            }
            catch(Exception e)
            {
                error.Add(new Error(e.Message, e.Source));
            }
            return error;

        }

        public List<Error> DeleteExpired()
        {
            List<Error> error = new List<Error>();
            //DELETE
            //deletes a whole row, deleted the rows that were passed their sell date.
            try
            {
                using (SqlConnection con4 = new SqlConnection(sqlConStr))
                {
                    con4.Open();
                    string InLineSQL3 = $@"DELETE FROM {table} WHERE GETDATE() > Sell_By_Date";
                    using (var Command = new SqlCommand(InLineSQL3, con4))
                    {

                        Command.ExecuteNonQuery();
                    }
                    con4.Close();
                }
                WriteLine($"{Environment.NewLine}Deleting the needed rows{Environment.NewLine}==========={Environment.NewLine}");
            }
            catch(Exception e)
            {
                error.Add(new Error(e.Message, e.Source));
            }
            return error;
        }  
        
        private List<Error> PriceIncrease()
        {
            List<Error> error = new List<Error>();
            try
            {
                using (SqlConnection con5 = new SqlConnection(sqlConStr))
                {
                    con5.Open();
                    string InLineSQL4 = $@"UPDATE {table} SET Price = Price + 1";
                    using (var Command = new SqlCommand(InLineSQL4, con5))
                    {
                        Command.ExecuteNonQuery();
                    }
                    con5.Close();
                }
                WriteLine($"{Environment.NewLine}Increasing the price{Environment.NewLine}==========={Environment.NewLine}");
            }
            catch(Exception e)
            {
                error.Add(new Error(e.Message, e.Source));
            }
            return error;
        }  
        private List<Error> Exporttxt()
        {
            //EXPORTING
            List<Error> error = new List<Error>();
            //int index = 6;
            //Dictionary<int, List<string>> opera = new Dictionary<int, List<string>>();
            string Writing = bind.Path.Replace(bind.Extension, $"_out.txt");
            if(File.Exists(Writing))
            {
                File.Delete(Writing);
            }
           try
            {
                using (SqlConnection con5 = new SqlConnection(sqlConStr))
                {
                    con5.Open();
                    string InLineSQL4 = $@"SELECT * FROM Produce INSERT INTO OUTFILE {Constants.output}";

                    using (var Command = new SqlCommand(InLineSQL4, con5))
                    {
                        Command.ExecuteNonQuery();
                    }
                    con5.Close();
                }
                WriteLine($"{Environment.NewLine}Exporting the Data{Environment.NewLine}==========={Environment.NewLine}");
            }
           catch(Exception e)
            {
                error.Add(new Error(e.Message, e.Source));
            }
            return error;
        }


    }
}



//READ TXT FILE
//it will load the txt file everytime you execute it so I comment block to let me not have repeats everytime.
/*    try
    {
        string SourceFolder = @"C:\Users\13129\Desktop\Assignment6_DD\data\";
        string SourceFileName = "Produce.txt";
        string TableName = "[dbo].[Produce]";
        string FileDelimiter = "|";

       using(SqlConnection con2 = new SqlConnection(sqlConStr))
        {
            StreamReader SourceFile = new StreamReader(SourceFolder + SourceFileName);
            string line = "";
            Int32 counter = 0;
            con2.Open();
            while ((line = SourceFile.ReadLine()) != null)
            {
                if (counter > 0)
                {
                    string query = "INSERT INTO" + TableName + "VALUES('" + line.Replace(FileDelimiter, "','") + "')";
                    SqlCommand myCommand = new SqlCommand(query, con2);
                    myCommand.ExecuteNonQuery();
                }
                counter++;
            }
            SourceFile.Close();
            con2.Close();
        }
        WriteLine($"{Environment.NewLine}Inserting txt file{Environment.NewLine}==========={Environment.NewLine}");


    }
    catch (IOException Exception)
    {
        Write(Exception);
    }*/




